using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour
{
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer, playerLayer;
    [SerializeField] private Timer timer;
    //_________________________________________________________//
    //Attack variables
    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackRange, sightRange;
    [SerializeField] private int baseProjSpeed;
    [SerializeField] private float attackTiming;
    [SerializeField] private int baseDamage;
    private int projSpeed;
    private int damage;
    private bool hasAttacked = false;
    private bool canSeePlayer = false;
    private bool canAttackPlayer = false;
    //_________________________________________________________//
    //Boss variables
    [SerializeField] private GameObject bigProjectile;
    [SerializeField] private int bigProjSpeed;
    [SerializeField] private float bigProjectileTiming = 5f; // Adjust the interval as needed
    private bool specialAttack = false;
    [SerializeField] private GameObject shieldPrefab;
    private GameObject shield;
    private bool isShieldActive = false;
    [SerializeField] private float shieldTiming = 10f; // Adjust the interval as needed



    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks to see if the object can see or attack the player
        canSeePlayer = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        canAttackPlayer = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (canSeePlayer)
        {

            if (canAttackPlayer)
            {
                SpecialAttackPlayer();
                AttackPlayer();
                if (!isShieldActive)
                {
                    ActivateShield();
                }
            }
        }
    }
    //--------------------------------------------------------//
    #region attacking code
    //fire projectile towards player
    private void AttackPlayer()
    {
        enemy.SetDestination(transform.position);
        transform.LookAt(player.position);

        if (!hasAttacked)
        {
            Attack();

            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackTiming);
        }
    }

    private void SpecialAttackPlayer()
    {
        enemy.SetDestination(transform.position);
        transform.LookAt(player.position);

        if (!specialAttack)
        {
            BigAttack();

            specialAttack = true;
            Invoke(nameof(ResetBigAttack), bigProjectileTiming);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    private void ResetBigAttack()
    {
        specialAttack = false;
    }

    // Creates a projectile to send towards the player
    private void Attack()
    {
        Vector3 attackDirection = (player.position - transform.position).normalized;

        // Calculate the rotation with a 90-degree offset along the x-axis
        Quaternion projectileRotation = Quaternion.LookRotation(attackDirection) * Quaternion.Euler(90f, 0f, 0f);

        // Offset the position from where the projectile is instantiated
        Vector3 projectileOffset = new Vector3(0f, 1f, 0f); // Adjust the offset values as needed

        GameObject currentprojectile = Instantiate(projectile, transform.position + projectileOffset, projectileRotation);
        currentprojectile.GetComponent<Rigidbody>().AddForce(attackDirection * projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<Projectile>().SetDamage(damage);
    }
    private void BigAttack()
    {
        Vector3 attackDirection = (player.position - transform.position).normalized;
    
        // Calculate the rotation with a 90-degree offset along the x-axis
        Quaternion bigProjectileRotation = Quaternion.LookRotation(attackDirection) * Quaternion.Euler(90f, 0f, 0f);

        // Offset the position from where the projectile is instantiated
        Vector3 projectileOffset = new Vector3(0f, 1f, 0f); // Adjust the offset values as needed


        GameObject currentBigProjectile = Instantiate(bigProjectile, transform.position + projectileOffset, bigProjectileRotation);
        currentBigProjectile.GetComponent<Rigidbody>().AddForce(attackDirection * bigProjSpeed, ForceMode.Impulse);
        currentBigProjectile.GetComponent<Projectile>().SetDamage(baseDamage * 3);
    }

    private void ActivateShield()
    {
        // Calculate a position around the boss for the shield to spawn
        Vector3 shieldSpawnPosition = transform.position + transform.forward * 3f + Vector3.up * 3f; 

        shield = Instantiate(shieldPrefab, shieldSpawnPosition, Quaternion.identity);
        StartCoroutine(SpinShield());
        isShieldActive = true;
    }

    private IEnumerator SpinShield()
    {
        float elapsedTime = 0f;
        float orbitDuration = 5f;  // Adjust the orbit duration as needed
        float orbitSpeed = 360f / orbitDuration;
        float orbitRadius = 4f;  // Adjust the orbit radius as needed
        float orbitHeight = 3f;  // Adjust the orbit height as needed

        while (true)
        {
            // Calculate the new position of the shield in a circular orbit around the boss
            float orbitAngle = elapsedTime * orbitSpeed;
            Vector3 orbitPosition = transform.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * orbitAngle), 0f, Mathf.Sin(Mathf.Deg2Rad * orbitAngle)) * orbitRadius;

            // Adjust the orbit height
            orbitPosition.y = transform.position.y + orbitHeight;

            shield.transform.position = orbitPosition;

            // Calculate the direction from the shield to the boss
            Vector3 lookAtDirection = transform.position - shield.transform.position;

            // Rotate the shield to face away from the boss, then add a 90-degree rotation
            Quaternion lookRotation = Quaternion.LookRotation(-lookAtDirection, Vector3.up) * Quaternion.Euler(180f, 90f, -35f);
            shield.transform.rotation = lookRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    
    //For ramping the difficulty of the enemy over time
    private void SetStats()
    {
        float currentTime = timer.timeValue;

        //increase projectile speed ever 60 seconds by 10% up to 30% increase, linear
        float projectileSpeedIncrementer = Mathf.FloorToInt(currentTime / 60);
        projectileSpeedIncrementer = projectileSpeedIncrementer / 10;
        float projectileSpeedMultiplier = Mathf.Clamp(projectileSpeedIncrementer, 0f, 0.3f);
        projSpeed = baseProjSpeed + Mathf.CeilToInt(baseProjSpeed * projectileSpeedMultiplier);

        //increase damage by 1 every 30 seconds up to 5, linear
        int DamageIncrementer = Mathf.FloorToInt(currentTime / 30);
        DamageIncrementer = (int)Mathf.Clamp(DamageIncrementer, 0f, 5f);
        damage = baseDamage + DamageIncrementer;
    }

    #endregion
    //--------------------------------------------------------//
}

