using UnityEngine;
using System.Collections;

public class FlamethrowerController : MonoBehaviour
{
    [SerializeField]private ParticleSystem flameParticles;
    private int damage = 4;
    private float damageInterval = 0.2f; // The time interval for damage in seconds
    private float time;

    private bool canDamage = true;

    void Start()
    {
    }

    public void init(float time, float dmgInterval)
    {
        this.time = time;
        damageInterval = dmgInterval;

    }

    public IEnumerator StartFlamethrower()
    {
        //note: currently set to turn off automatically, will later add a check to turn off when player lets go
        flameParticles = GetComponent<ParticleSystem>();
        flameParticles.Play();
        yield return new WaitForSeconds(time);
        flameParticles.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (canDamage)
        {
            // Check if the collided object has an "Enemy" component
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeDamage(damage);

                // Start the damage cooldown coroutine
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDamage = true;
    }
}