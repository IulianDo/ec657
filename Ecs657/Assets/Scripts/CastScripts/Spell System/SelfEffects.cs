using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfEffects : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    [SerializeField] private Transform shield;
    [SerializeField] private Transform fShield;
    [SerializeField] private Transform wShield; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startEffect(float duration, string effect, float factor, float interval)
    {
        switch(effect)
        {
            case "Focus":
                StartCoroutine(Focus(duration, factor));
                break;
            case "Blaze":
                StartCoroutine(Blaze(duration, factor));
                break;
            case "Shield":
                Shield(duration, factor, interval);
                break;
            case "Flame Shield":
                FlameShield(duration, factor, interval);
                break;
            case "Water Shield":
                WaterShield(duration, factor, interval);
                break;
            case "Earth Armor":
                StartCoroutine(EarthArmor(duration, factor));
                break;
            default:
                break;
        }
    }

    IEnumerator Focus(float duration, float factor)
    {
        stats.cdMul /= factor;
        yield return new WaitForSeconds(duration);
        stats.cdMul *= factor;
    }

    IEnumerator Blaze(float duration, float factor)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.speed *= factor;
        yield return new WaitForSeconds(duration);
        playerMovement.speed /= factor;
    }

    IEnumerator EarthArmor(float duration, float factor)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.speed /= 2;
        stats.defMul/= factor;
        yield return new WaitForSeconds(duration);
        playerMovement.speed *= 2;
        stats.defMul*= factor;

    }

    void Shield(float duration, float factor, float interval)
    {
        ShieldController shieldController = Instantiate(shield, player.transform).gameObject.GetComponent<ShieldController>();
        shieldController.Init(Mathf.RoundToInt(15 * stats.hpMul), duration, interval, factor, stats.dmgMul);
    }

    void FlameShield(float duration, float factor, float interval)
    {
        ShieldController shieldController = Instantiate(fShield, player.transform).gameObject.GetComponent<ShieldController>();
        shieldController.Init(Mathf.RoundToInt(15 * stats.hpMul), duration, interval, factor, stats.dmgMul);
    }

    void WaterShield(float duration, float factor, float interval)
    {
        ShieldController shieldController = Instantiate(wShield, player.transform).gameObject.GetComponent<ShieldController>();
        shieldController.Init(Mathf.RoundToInt(30 * stats.hpMul), duration, interval, factor, stats.dmgMul);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
