using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellList : MonoBehaviour
{
    /* Explanation of purpose:
     * Because of how Unity works, actual OOP doesn't really work, that's where this comes in
     * The SpellList object houses instances of the spells as components, and those are stored in this components to be retrieved by the other objects
     * It also means spells for the hotbar and for combinations are possible here
     */
    public ScriptableSpell[] scriptableSpellList;
    public Spell[] spellList;
    public ScriptableSpell[] scriptableHotbarList;
    public Spell[] hotbarList;
    [SerializeField] HotBarController hotbar;
    [SerializeField] SpellStack spellStack;


    void Awake()
    {
        hotbarList = new Spell[scriptableHotbarList.Length];
        spellList = new Spell[scriptableSpellList.Length];

        int counter = 0;
        
        for (int i = 0; i < scriptableSpellList.Length; i++)
        {
            spellList[i] = gameObject.AddComponent<Spell>();
            spellList[i].spellType = scriptableSpellList[i];
            spellList[i].InitialiseSpell();
            if (spellList[i].spellType == scriptableHotbarList[counter])
            {
                hotbarList[counter] = spellList[i];
                counter ++;
            }
        }
        spellStack.WhenToStart();
        hotbar.WhenToStart();

    }

}
