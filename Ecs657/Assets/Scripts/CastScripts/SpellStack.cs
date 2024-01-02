using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellStack : MonoBehaviour
{
    //If possible later, replace the arrays for stackSpells and stackSlots with Lists
    [SerializeField] private Spell[] stackSpells = new Spell[5];
    [SerializeField] private GameObject[] stackSlots = new GameObject[5];
    [SerializeField] private Transform slotFab;
    [SerializeField] private GameObject grid;
    private int stackIndex = 0;
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
        //if spell is added twice, it'll cancel out
        if (!checkDuplicates(spell))
        {
            //reloads the spell, making it have maximum amunition
            spell.Reload();
            //otherwise, its added to the stack
            XspellStack.Add(spell);
            XstackSlots.Add(Instantiate(slotFab, grid.transform).gameObject);
            XstackSlots[XstackSlots.Count - 1].GetComponent<SlotController>().SetSpellInit(spell,spell.GetMaxAmmo());
            Debug.Log(XspellStack[XspellStack.Count-1].spellName);
        }
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

    bool checkDuplicates(Spell spell)
    {
        if (XspellStack.Contains(spell))
        {
            removeSpell(spell);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Removes a spell from the stack at the specified index.
    void removeSpell(int index)
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
    //cast all spells in stack and removes them
    public float castStack()
    {
        try
        {
            if (XspellStack.Count>0)
            {
                float cooldown = XspellStack[0].spellType.data.cooldown;
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