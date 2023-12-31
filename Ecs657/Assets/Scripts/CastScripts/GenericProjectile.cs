using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class GenericProjectile : MonoBehaviour
{
    private int damage;
    protected int duration;
    protected float interval;
    protected Enemy enemy;
    protected GameObject enemyObj;
    protected bool hit=false;

    public void setData(int damage, int duration, float interval)
    {
        this.damage = damage;
        this.duration = duration;
        this.interval = interval;
        Invoke("cleanup", 5);
    }

    private void cleanup()
    {
        Destroy(gameObject);
    }

    // Triggers when it collides with an enemy
    private void OnTriggerEnter(Collider other)
    {
        enemyObj = other.gameObject;
        enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            CancelInvoke("cleanup");
            enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(projEffect());
            hit = true;
        }
    }

    protected abstract IEnumerator projEffect();

    // Update is called once per frame
    // Allows for damage over time
    void Update()
    {
        if ((duration <= 0 || enemyObj == null) && hit)
        {
            StopCoroutine(projEffect());
            Destroy(gameObject);
        }
    }
}
