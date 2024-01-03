using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class Enemy : MonoBehaviour
{
    //External Variables
    [Header("External Variables")]
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask groundLayer, playerLayer;
    [SerializeField] private Timer timer;
    //_________________________________________________________//
    //Movement variables
    [Header("Movement Variables")]
    [SerializeField] private float waitAtPoint;
    [SerializeField] private float walkRange, attackRange, sightRange;
    bool isWalkPointSet = false;
    Vector3 walkPoint;
    //_________________________________________________________//
    //Attack variables
    [Header("Attack Variables")]
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
    [Header("HP Variables")]
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private int baseMaxHp;
    [SerializeField] private bool isDead;
    private int maxHP;
    private int currentHP;
    //_________________________________________________________//
    //XP Variables 
    [Header("XP Variables")]
    [SerializeField] private float xpValue;
    [SerializeField] private GameObject EXP;
	//_________________________________________________________//
	[Header("Misc Variables")]
	[SerializeField] private string nextLevel;
    [SerializeField] bool isBoss;
	//_________________________________________________________//
	//Status Effects
	public enum status {Normal, Stunned};
    private status effect = status.Normal;
    //_________________________________________________________//

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        isDead = false;

        maxHP = (int)(baseMaxHp * SetStat(15f, 0.1f, 0f, 2f));
        currentHP = maxHP;
        healthbar.setMaxHealth(maxHP);

        projSpeed = (int)(baseProjSpeed * SetStat(60f, 0.1f, 0f, 0.3f));
        damage = (int)(baseDamage * SetStat(30f, 1f, 0f, 5f));

    }

    // Update is called once per frame
    void Update()
    {
        // Checking status effects for behaviour changes
        switch (effect)
        {
            case status.Normal:
                break;
            case status.Stunned:
                return;
            default:
                break;
        }

        // Checks to see if the object can see or attack the player
        canSeePlayer = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        canAttackPlayer = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (canSeePlayer)
        {
            if (canAttackPlayer)
            {
                AttackPlayer();
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
    private void ResetAttack()
    {
        hasAttacked = false;
    }
    // Creates a projectile to send towards the player
    private void Attack()
    {
        GameObject currentprojectile = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
        currentprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<Projectile>().SetDamage(damage);
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
        healthbar.setHealth(currentHP);
        currentHP -= amount;
        DamagePopupGenerator.current.CreatePopUp(transform.position, amount.ToString());
        if (currentHP <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }
    //functions needed when dying
    private void Die()
    {
        DropXP(xpValue);
        Destroy(gameObject);
        Debug.Log("This is a simple log message.");
        SceneManager.LoadScene(nextLevel);
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
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 10f, groundLayer);
            Vector3 randomPosition = new Vector3(transform.position.x + randomRangeX,
                                                 transform.position.y - hit.distance + 1f + randomRangeY, // makes sure players can grab xp from tall enemies
                                                 transform.position.z + randomRangeZ);
            GameObject XPDrop = Instantiate(EXP, randomPosition, Quaternion.identity);
            XPDrop.GetComponent<ExperienceController>().SetXp(value / numberOfXP);
		}
	}
	#endregion
	//--------------------------------------------------------//
	#region StatusEffects
	public void SetEffect(status effect)
	{
		this.effect = effect;
	}
	#endregion
	//--------------------------------------------------------//
	#region misc code

	private float SetStat(float increaseEvery, float changeBy, float min, float max)
	{
		float multiplier = Mathf.FloorToInt(timer.totalTimePassed / increaseEvery);
		float percentageIncrease = Mathf.Clamp(multiplier * changeBy, min, max);
		return 1 + percentageIncrease;
	}
    public void AddForce(Vector3 direction, int force)
    {
        direction = direction.normalized;
        this.transform.position = this.transform.position + direction * force * Time.deltaTime;
        isWalkPointSet = false;
        enemy.SetDestination(this.transform.position);
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
