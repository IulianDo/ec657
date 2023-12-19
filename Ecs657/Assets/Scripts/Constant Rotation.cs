using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpRotation : MonoBehaviour
{
    // Update is called once per frame
    //rotates gameobject
    [SerializeField] private Vector3 rotation;
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
}
