using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HotBarController : MonoBehaviour
{
    //here the inputtable spells are stored, and they're added to SpellStack on input
    [SerializeField] private Transform slotFab;
    private SlotController[] slots = new SlotController[10];
    private Spell[] spells = new Spell[10];
    [SerializeField] GameObject spellList;
    [SerializeField] private SpellStack spellStack;
    [SerializeField] private GameObject grid;

    public void WhenToStart()
    {
        spells=spellList.GetComponent<SpellList>().hotbarList; //retrieves list of spells for the hotbar from spellList
        //sets all slot icons to spells stored on launch if not blank
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i].spellName != null)
            {
                slots[i] = Instantiate(slotFab, grid.transform).GetComponent<SlotController>(); //if the slot has a spell, create a SpellSlot
                slots[i].SetSpellInit(spells[i],0); //assign the new slot its corresponding spell
                slots[i].HotBarSlot();
            }
        }
    }

    //add spell into a specific slot
    public void AddSpell(int slot)
    {
        //add spell to spellstack
        if (spells[slot].spellName != null)
        {
            spellStack.addSpell(spells[slot]);
        }
    }
}
