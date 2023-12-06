using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float expiration;
    private int damage;

    public void SetDamage( int amount)
    {
        damage = amount;
    }

	private void Start()
	{
        Invoke(nameof(DestroyProjectile), expiration);
    }

    // Detects collision with enemy or player so that they can take damage
    void OnCollisionEnter(Collision collide){
        if (collide.gameObject.GetComponent<Enemy>() != null)
        {
            collide.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        if (collide.gameObject.GetComponent<PlayerStats>() != null)
        {
            collide.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
        }
        
        Destroy(gameObject);
    } 

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
