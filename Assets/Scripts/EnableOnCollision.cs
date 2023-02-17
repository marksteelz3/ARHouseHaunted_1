using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnableOnCollision : MonoBehaviour
{
    public GameObject target;

    private bool didEnable = false;

    private void OnTriggerEnter(Collider other)
    {
        //If the collider has touched a player
        if (other.gameObject.layer == 9 && !didEnable)
        {
            target.SetActive(true);
            didEnable = true;
        }
    }
}
