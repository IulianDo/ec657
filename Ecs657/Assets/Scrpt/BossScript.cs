using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : Enemy
{

    
    [SerializeField] bool cooldown;
    [SerializeField] float turnSpeed;
    [SerializeField] float y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int rangvar = Random.Range(1, 5);
        if (rangvar == 5 && !cooldown)
        {
            y = ((y + turnSpeed) * Time.deltaTime) % 360;
            transform.rotate(0f,y,0f);
        }
    }

    private void Attack()
    {
        GameObject currentprojectile = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
        currentprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.Impulse);
        currentprojectile.GetComponent<Projectile>().SetDamage(damage);
    }
}
