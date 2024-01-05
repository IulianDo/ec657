using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ScriptableSpell;

[CreateAssetMenu(fileName = "New Test Spell", menuName = "Test Spell")]
public class NewScriptableSpell : ScriptableObject
{
    [System.Serializable] public enum spellClasses { Projectile, Particles, AoE, Self, Secret_Third_Option };
    public spellClasses spellClass;
    [SerializeReference] public Item data = new Item();
    [SerializeReference] public SubItem subdata = null;

    #region data classes
    [System.Serializable]
    public class Item
    {
        [SerializeField] public string component;
        [SerializeField] public string description;
        [SerializeField] public float cooldown = 0;
        [SerializeField] public int damage;
        [SerializeField] public int duration = 0;
        [SerializeField] public float interval = 0;
        [SerializeField] public int ammo = 0;
        [SerializeField] public Sprite image;
        [SerializeField] public NewScriptableSpell[] combinations = new NewScriptableSpell[0];
    }

    #endregion

    #region subdata classes

    [System.Serializable]
    public class SubItem
    {

    }

    [System.Serializable]
    public class ProjectileItem : SubItem
    {
        [SerializeField] public Transform projectile;
        [SerializeField] public float projSpeed;
        public ProjectileItem() { }
    }

    public class ParticleItem : SubItem
    {
        [SerializeField] public Transform gameObj;
        [SerializeField] public float dmgInterval;
        [SerializeField] public int chipDmg;
        [SerializeField] public bool canDamage = true;
        public ParticleItem() { }
    }

    public class AOEItem : SubItem
    {
        [SerializeField] public float radius;
        [SerializeField] public Transform fieldObj;
    }

    public class SelfItem : SubItem
    {
        [SerializeField] public float effectFactor;
    }
    #endregion


    private void OnValidate()
    {
        switch (spellClass)
        {
            case spellClasses.Projectile:
                if (subdata == null || subdata.GetType() != typeof(ProjectileItem))
                {
                    subdata = new ProjectileItem();
                }
                break;
            case spellClasses.Particles:
                if (subdata == null || subdata.GetType() != typeof(ParticleItem))
                {
                    subdata = new ParticleItem();
                }
                break;
            case spellClasses.AoE:
                if (subdata == null || subdata.GetType() != typeof(AOEItem))
                {
                    subdata = new AOEItem();
                }
                break;
            case spellClasses.Self:
                if (subdata == null || subdata.GetType() != typeof(SelfItem))
                {
                    subdata = new SelfItem();
                }
                break;
            default:
                break;
        }
    }
}