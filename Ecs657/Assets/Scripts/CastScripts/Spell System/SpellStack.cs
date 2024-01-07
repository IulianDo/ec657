using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellStack : MonoBehaviour
{
    //If possible later, replace the arrays for stackSpells and stackSlots with Lists
    [SerializeField] private Transform slotFab;
    [SerializeField] private GameObject grid;
    [SerializeField] private Spell[] spellList;
    [SerializeField] private GameObject spellListObj;

    [SerializeField] public List<Spell> XspellStack;
    [SerializeField] public List<GameObject> XstackSlots;

    // Start is called before the first frame update
    public void WhenToStart()
    {
        spellList = spellListObj.GetComponent<SpellList>().spellList;
    }

    public void addSpell(Spell spell)
    {
        if(XspellStack.Count >= 3)
        {
            return;
        }
        //reloads the spell, making it have maximum amunition
        spell.Reload();
        //otherwise, its added to the stack
        XspellStack.Add(spell);
        XstackSlots.Add(Instantiate(slotFab, grid.transform).gameObject);
        XstackSlots[XstackSlots.Count - 1].GetComponent<SlotController>().SetSpellInit(spell,spell.GetMaxAmmo());
        Debug.Log(XspellStack[XspellStack.Count-1].spellName);
        //check if any new combos are possible
        checkCombos();
    }

    public void checkCombos()
    {
        //checks every spell in spellList for combination match
        foreach (Spell newSpell in spellList)
        {
            //if match found, remove each spell in stack and add the new spell
            int[] indices = newSpell.checkCombination(XspellStack);
            if (indices != null)
            {
                foreach (int index in indices)
                {
                    removeSpell(index);
                }
                addSpell(newSpell);
            }
        }
    }

    // Removes a spell from the stack at the specified index.
    public void removeSpell(int index)
    {
        XspellStack.RemoveAt(index);
        Destroy(XstackSlots[index]);
        XstackSlots.RemoveAt(index);
    }

    void removeSpell(Spell spell)
    {
        Destroy(XstackSlots[XspellStack.IndexOf(spell)]);
        XstackSlots.RemoveAt(XspellStack.IndexOf(spell));
        XspellStack.Remove(spell);
    }

    public void ClearQueue()
    {
        XspellStack.Clear();
        for(int i = 0; i < XstackSlots.Count; i++)
        {
            Destroy(XstackSlots[i]);
        }
        XstackSlots.Clear();
    }

    //cast first spell in queue and consume ammo
    public float castStack()
    {
        try
        {
            if (XspellStack.Count>0)
            {
                float cooldown = XspellStack[0].GetCooldown();
                //checks to see if there is any ammunition left
                if (XspellStack[0].Cast() <= 0)
                {
                    removeSpell(0);
                }
                else //consumes ammo for slot
                {
                    XstackSlots[0].GetComponent<SlotController>().Cast();
                }
                return cooldown;
            }
            return 0f;
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}