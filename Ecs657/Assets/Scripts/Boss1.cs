using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss1 : MonoBehaviour
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

            if(canAttackPlayer)
            {
                SpecialAttackPlayer();
                AttackPlayer();
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
        GameObject currentprojectile = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
        currentprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<Projectile>().SetDamage(damage);
    }
    private void BigAttack()
    {
        GameObject currentprojectile = Instantiate(bigProjectile, transform.position + transform.forward, Quaternion.identity);
        currentprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<Projectile>().SetDamage(baseDamage*3);

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
        DamageIncrementer = (int) Mathf.Clamp(DamageIncrementer, 0f, 5f);
        damage = baseDamage + DamageIncrementer;
    }
    #endregion
    //--------------------------------------------------------//
}

