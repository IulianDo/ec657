using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScriptableSpell;

[CreateAssetMenu(fileName = "New Test Spell", menuName = "Test Spell")]
public class NewScriptableSpell : ScriptableObject
{
    [System.Serializable] public enum spellClasses { Projectile, Particles, Secret_Third_Option };
    public spellClasses spellClass;
    [SerializeReference] public Item data = new Item();

    #region data classes
    [System.Serializable]
    public class Item
    {
        [SerializeField] public string component;
        [SerializeField] public string description;
        [SerializeField] public float cooldown = 0;
        [SerializeField] public int damage;
        [SerializeField] public int duration = 0;
        [SerializeField] public int interval = 0;
        [SerializeField] public Sprite image;
        [SerializeField] public NewScriptableSpell[] combinations = new NewScriptableSpell[0];
    }

    [System.Serializable]
    public class ProjectileItem : Item
    {
        [SerializeField] public Transform projectile;
        [SerializeField] public float projSpeed;
        public ProjectileItem() { }
    }

    public class ParticleItem : Item
    {
        [SerializeField] public ParticleSystem particles;
        [SerializeField] public float dmgInterval;
        [SerializeField] public bool canDamage = true;
        public ParticleItem() { }
    }

    #endregion


    private void OnValidate()
    {
        switch (spellClass)
        {
            case spellClasses.Projectile:
                if (data == null || data.GetType() != typeof(ProjectileItem))
                {
                    data = new ProjectileItem();
                }
                break;
            case spellClasses.Particles:
                if (data == null || data.GetType() != typeof(ParticleItem))
                {
                    data = new ParticleItem();
                }
                break;
            default:
                break;
        }
    }
}