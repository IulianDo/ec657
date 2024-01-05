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
    //Movement variables
    [SerializeField] private float waitAtPoint;
    [SerializeField] private float walkRange, attackRange, sightRange;
    bool isWalkPointSet = false;
    Vector3 walkPoint;
    //_________________________________________________________//
    //Attack variables
    [SerializeField] private GameObject projectile;
    [SerializeField] private int baseProjSpeed;
    [SerializeField] private float attackTiming;
    [SerializeField] private int baseDamage;
    private int projSpeed;
    private int damage;
    private bool hasAttacked = false;
    private bool canSeePlayer = false;
    private bool canAttackPlayer = false;
    //_________________________________________________________//
    //HP variables
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private int baseMaxHp;
    [SerializeField] private bool isDead;
    private int maxHP;
    private int currentHP;
    //_________________________________________________________//
    //Boss decleration variable
    [SerializeField] bool isBoss;
    //_________________________________________________________//
    //will be used for win screen
    [SerializeField] private Menus menus;
    //_________________________________________________________//
    //xpVariables
    [SerializeField] private float xpValue;
    [SerializeField] private GameObject EXP;
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
        isDead = false;
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
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Wonder();
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
        float orbitRadius = 3.5f;  // Adjust the orbit radius as needed
        float orbitHeight = 3f;  // Adjust the orbit height as needed

        while (elapsedTime < orbitDuration)
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

        DeactivateShield();
    }

    private void DeactivateShield()
    {
        if (isShieldActive && shield != null)
        {
            Destroy(shield);
            isShieldActive = false;

            // Start the cooldown coroutine
            Invoke(nameof(ResetShield), shieldTiming);
        }
    }

    private void ResetShield()
    {
        isShieldActive = false;
    }

    #endregion
    //--------------------------------------------------------//
    #region movement code
    //go towards player
    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    // Controls enemy movement by setting and guiding them to walk points.
    private void Wonder()
    {
        if (isWalkPointSet)
        {
            enemy.SetDestination(walkPoint);
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                isWalkPointSet = false;
            }
        }
        else
        {
            Invoke(nameof(SetWalkingPoint), Random.Range(0f, waitAtPoint));
        }
    }

    // Sets location to head to
    private void SetWalkingPoint()
    {
        float randomZ = Random.Range(-walkRange, walkRange);
        float randomX = Random.Range(-walkRange, walkRange);

        walkPoint = new Vector3(transform.position.x + randomX,
                                transform.position.y,
                                transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            isWalkPointSet = true;
        }
    }
    #endregion
    //--------------------------------------------------------//
    #region takeDamage code
    //reduce current hp by x
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        healthbar.setHealth(currentHP);
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    //functions needed when dying
    private void Die()
    {
        //cheaks if the enemy is the final boss
        if (isBoss)
        {
            menus.Win();
            Destroy(gameObject);
            return;
        }
        TempEnemy();
        DropXP(xpValue);
        Destroy(gameObject);
        Debug.Log("This is a simple log message.");
        SceneManager.LoadScene("level2");
    }
    //Drops x number of orbs which gives you xpValue worth of xp in total
    //orbs drop randomly when enemy dies within a certain range
    private void DropXP(float value)
    {
        int numberOfXP = Random.Range(1, 5);
        for (int i = 0; i < numberOfXP; i++)
        {
            float randomRangeX = Random.Range(0.5f, -0.5f);
            float randomRangeY = Random.Range(0.5f, -0.5f);
            float randomRangeZ = Random.Range(0.5f, -0.5f);
            Vector3 randomPosition = new Vector3(transform.position.x + randomRangeX,
                                                 transform.position.y + randomRangeY,
                                                 transform.position.z + randomRangeZ);

            GameObject XPDrop = Instantiate(EXP, randomPosition, Quaternion.identity);
            XPDrop.GetComponent<ExperienceController>().SetXp(value / numberOfXP);
        }
    }

    #endregion
    //--------------------------------------------------------//
    #region misc code
    //spawn 2 enemies every time an enemy dies
    private void TempEnemy()
    {
        float randomZ = Random.Range(-walkRange, walkRange);
        float randomX = Random.Range(-walkRange, walkRange);

        Vector3 spawnLocation = new Vector3(200f + randomX,
                                            5f,
                                            200f + randomZ);
        Instantiate(gameObject, spawnLocation, Quaternion.identity);
        Instantiate(gameObject, spawnLocation + Vector3.one * 2, Quaternion.identity);
    }

    //For ramping the difficulty of the enemy over time
    private void SetStats()
    {
        float currentTime = timer.timeValue;

        //increase hp every 15 seconds by 10% up to 200% original hp, linear
        float HPIncrementer = Mathf.FloorToInt(currentTime / 15);
        HPIncrementer = HPIncrementer / 10;
        float HPMultiplier = Mathf.Clamp(HPIncrementer, 0f, 2f);
        maxHP = baseMaxHp + Mathf.CeilToInt(baseMaxHp * HPMultiplier);
        currentHP = maxHP;
        healthbar.setMaxHealth(maxHP);

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
    //for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion
    //--------------------------------------------------------//
}

