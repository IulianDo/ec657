using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss3 : MonoBehaviour
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
    [SerializeField] private GameObject spike;
    [SerializeField] private float summonCooldown = 10f; // Adjust the cooldown as needed
    private bool canSummon = true;



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
                SummonObjectAtPlayer();
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

    private IEnumerator DelayedSummon()
    {
        // Add a delay here (for example, 2 seconds)

        // Offset the spike spawn position below the player
        Vector3 summonPosition = player.position - new Vector3(-2f, 5f, 6f); // Adjust the offset values as needed

        // Rotate the spike object to be perpendicular to the ground (90 degrees around the x-axis)
        Quaternion spikeRotation = Quaternion.Euler(-90f, 0f, 0f);

        yield return new WaitForSeconds(0.4f);

        // Instantiate the spike object at the modified player's position with the desired rotation
        GameObject currentSpike = Instantiate(spike, summonPosition, spikeRotation);

        // Apply a force to the Rigidbody of the spike to make it shoot straight up
        Vector3 spikeForce = Vector3.up * 1; // Adjust the upward force as needed
        currentSpike.GetComponent<Rigidbody>().AddForce(spikeForce, ForceMode.Impulse);
        currentSpike.GetComponent<Projectile>().SetDamage(damage * 1);

    }

    private void SummonObjectAtPlayer()
    {
        if (canSummon)
        {
            StartCoroutine(DelayedSummon());
            StartCoroutine(SummonCooldown());
        }
    }

    private IEnumerator SummonCooldown()
    {
        canSummon = false;
        yield return new WaitForSeconds(summonCooldown);
        canSummon = true;
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
        Vector3 projectileOffset = new Vector3(0f, 3f, 0f); // Adjust the offset values as needed


        GameObject currentBigProjectile = Instantiate(bigProjectile, transform.position + projectileOffset, bigProjectileRotation);
        currentBigProjectile.GetComponent<Rigidbody>().AddForce(attackDirection * bigProjSpeed, ForceMode.Impulse);
        currentBigProjectile.GetComponent<Projectile>().SetDamage(baseDamage * 3);
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

