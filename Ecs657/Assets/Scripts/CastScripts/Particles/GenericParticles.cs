using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericParticles : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particles;
    protected int damage = 4;
    protected float damageInterval = 0.2f; // The time interval for damage in seconds
    protected float time;

    protected bool canDamage = true;

    void Start()
    {
    }

    public void init(float time, float dmgInterval)
    {
        this.time = time;
        damageInterval = dmgInterval;

    }

    public IEnumerator StartParticles()
    {
        //note: currently set to turn off automatically, will later add a check to turn off when player lets go
        particles = GetComponent<ParticleSystem>();
        particles.Play();
        yield return new WaitForSeconds(time);
        particles.Stop();
        Destroy(gameObject);
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
                ParticleEffect(enemy);
                // Start the damage cooldown coroutine
                StartCoroutine(DamageCooldown());
            }
        }
    }

    protected abstract void ParticleEffect(Enemy enemy);

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDamage = true;
    }

}
