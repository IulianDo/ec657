using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class GenericProjectile : MonoBehaviour
{
    private int damage;
    protected float duration;
    protected float interval;
    protected float dmgMul;
    protected Enemy enemy;
    protected GameObject enemyObj;
    protected bool hit=false;
    protected bool effectEnd=false;

    public void setData(int damage, int duration, float interval, float dmgMul)
    {
        this.damage = damage;
        this.duration = duration;
        this.interval = interval;
        this.dmgMul = dmgMul;
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
        if ((effectEnd || enemyObj == null) && hit)
        {
            StopCoroutine(projEffect());
            Destroy(gameObject);
        }
    }
}
