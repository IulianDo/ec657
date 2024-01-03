using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    //Slot is purely visual, the actual spells are stored in the hotbar
    private Image icon;
    private Image slot;
    private TextMeshProUGUI ammo;
    private int ammoval;
    private bool hotbar;
    [SerializeField] private Transform iconPrefab;
    [SerializeField] private Transform ammoPrefab;
    private Spell slotSpell;
    

    void Start()
    {
        icon = Instantiate(iconPrefab, transform).gameObject.GetComponent<Image>();
        slot = GetComponent<Image>();
        ammo = Instantiate(ammoPrefab, transform).gameObject.GetComponent<TextMeshProUGUI>();
        SetSpell(slotSpell);
        if(hotbar)
        {
            ammo.enabled = false;
        }
        
    }


    public void SetSpell(Spell spell)
    {
        slotSpell = spell;
        icon.sprite=spell.icon;
        icon.enabled=true;
        ammo.text=ammoval.ToString();
    }

    public void Cast()
    {
        ammoval--;
        ammo.text = ammoval.ToString();
        CoolDown(slotSpell.spellType.data.cooldown);
    }

    public void HotBarSlot()
    {
        hotbar = true;
    }

    //seems strange to have these two, but it's because adding the spell right after instantiating, Start is still running
    //so this only assigns the spell, then the slot runs the set itself
    //doing it after instantiation requires setspel
    //because instantiate doesn't allow parameters/constructors for some reason
    public void SetSpellInit(Spell spell, int ammo)
    {
        this.ammoval = ammo;
        slotSpell = spell;
    }

    public void ClearSlot()
    {
        icon.sprite=null;
        icon.enabled = false;
    }

    public void CoolDown(float time)
    {
        slot.fillAmount = 0;
        StartCoroutine(CDFill(time));
    }

    private IEnumerator CDFill(float time)
    {
        float factor = (1.0f / time) * Time.deltaTime;
        while(slot.fillAmount < 1)
        {
            slot.fillAmount += factor;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }


}
