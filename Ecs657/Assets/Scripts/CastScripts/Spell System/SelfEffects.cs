using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfEffects : MonoBehaviour
{
    PlayerStats stats;
    GameObject player;
    GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(GameObject player, GameObject camera, PlayerStats stats)
    {
        this.player = player;
        this.camera = camera;
        this.stats = stats;
    }

    public void startEffect(float duration, string effect, float factor)
    {
        switch(effect)
        {
            case "Focus":
                Focus(duration, factor);
                break;
            case "Blaze":
                Blaze(duration, factor);
                break;
            case "Shield":
                Shield(duration, factor);
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

    IEnumerator Shield(float duration, float factor)
    {
        stats.defMul*=factor;
        yield return new WaitForSeconds(duration);
        stats.defMul/=factor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
