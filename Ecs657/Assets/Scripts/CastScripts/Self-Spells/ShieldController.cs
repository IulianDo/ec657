using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShieldController : MonoBehaviour
{
    protected int hp;
    protected float duration;
    protected float dmgMul;
    protected float factor;
    protected float interval;
    protected Enemy enemy;
    protected GameObject enemyObj;

    public void Init(int hp, float duration, float interval, float factor, float dmgMul)
    {
        this.hp = Mathf.RoundToInt(hp*dmgMul);
        this.duration = duration;
        this.dmgMul = dmgMul;
        this.factor = factor;
        this.interval = interval;
        StartCoroutine(ShieldLifetime(duration));
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void projEffect();

    private IEnumerator ShieldLifetime(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyObj = other.gameObject;
        enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            TakeDamage(enemy.GetDamage());
            enemy.AddForce(enemyObj.transform.forward*-1, 100);
            projEffect();
            return;
        }
        else
        {
            Projectile proj = enemyObj.GetComponent<Projectile>();
            if (proj != null)
            {
                TakeDamage(proj.GetDamage());
                Destroy(enemyObj);
            }
        }
    }
}
