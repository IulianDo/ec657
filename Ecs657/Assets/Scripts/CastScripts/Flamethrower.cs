using UnityEngine;
using System.Collections;

public class FlamethrowerController : MonoBehaviour
{
    public ParticleSystem flameParticles;
    public int damage = 4;
    public float damageInterval = 0.2f; // The time interval for damage in seconds

    private bool canDamage = true;

    void Update()
    {
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