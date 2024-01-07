using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //Other variables
    [SerializeField] private PlayerInput playerControls;
    private PlayerInput.PlayerActions actions;

    //Projetile variables
    [SerializeField] GameObject projectile;
    [SerializeField] float projSpeed;
    [SerializeField] float cooldown;
    [SerializeField] int damage;
    private float lastShot;

    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();
        actions = playerControls.Player;
    }
	// Update is called once per frame
	void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (actions.Shoot.IsPressed())
        {
            if (Time.time - lastShot > cooldown)
            {
                GameObject currentprojectile = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity);
                currentprojectile.GetComponent<Rigidbody>().AddForce(transform.forward * projSpeed, ForceMode.Impulse);
                currentprojectile.GetComponent<Projectile>().SetDamage(damage);
                lastShot = Time.time;
            }
        }
    }
}
