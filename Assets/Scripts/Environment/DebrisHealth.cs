using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float curreHealth;

    private void Start()
    {
        curreHealth = maxHealth;
    }

}