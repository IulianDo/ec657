using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    [SerializeField] private GameObject self;
    [SerializeField] private int healFor;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().Heal(healFor);
            Destroy(self);
        }
    }
}
