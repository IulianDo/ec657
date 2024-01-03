using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;

public class newCaster : MonoBehaviour
{
    public PlayerInput playerControls;
    private PlayerInput.PlayerActions actions;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] float cooldown;
    private float lastShot;

    private SpellStack spellStack;
    [SerializeField] GameObject sStack;
    private HotBarController hotBarController;
    [SerializeField] GameObject hotBar;

    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();
        actions = playerControls.Player;
        spellStack = sStack.GetComponent<SpellStack>();
        hotBarController = hotBar.GetComponent<HotBarController>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the game is paused
        if (Time.timeScale == 0)
        {
            return;
        }

        //firing code
        if (actions.Shoot.IsPressed()) {
            if (Time.time - lastShot > cooldown)
            {
                cooldown = spellStack.castStack() * playerStats.cdMul;
                lastShot = Time.time;
            }
        }
        //spell combination code
        if (actions.Hotbar.WasPressedThisFrame())
        {
            int slot = (int) actions.Hotbar.ReadValue<float>();
            hotBarController.AddSpell(slot-1);
        }

        if(actions.PopSpell.WasPressedThisFrame())
        {
            if(spellStack.XspellStack.Count > 0) spellStack.removeSpell(0);
        }

        if(actions.ClearQueue.WasPressedThisFrame())
        {
            spellStack.ClearQueue();
        }
    }
}
