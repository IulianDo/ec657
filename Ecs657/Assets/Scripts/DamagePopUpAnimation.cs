using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve heightCurve;
    public AnimationCurve opacityCurve;
    private TextMeshProUGUI tmp;

    private Vector3 origin;
    private float time = 0;
    // Start is called before the first frame update
    void Awake()
	{
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
	}
    // Update is called once per frame
    void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
