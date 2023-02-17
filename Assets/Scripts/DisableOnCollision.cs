using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DisableOnCollision : MonoBehaviour
{
    public GameObject target;

    private bool didDisable = false;

    private void OnTriggerEnter(Collider other)
    {
        //If the collider has touched a player
        if (other.gameObject.layer == 9 && !didDisable)
        {
            target.SetActive(false);
            didDisable = true;
        }
    }
}
