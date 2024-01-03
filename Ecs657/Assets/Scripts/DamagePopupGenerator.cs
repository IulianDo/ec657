using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public static DamagePopupGenerator current;
    public GameObject prefab; 
	void Awake()
	{
        current = this;
	}

    public void CreatePopUp(Vector3 position, string text)
	{
        Vector3 offset = new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f));
        var popup = Instantiate(prefab, position + offset, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popup, 1f);
	}
}
