using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedObjectDisabler : MonoBehaviour
{
    public GameObject target;
    public float delay;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        target.SetActive(false);
    }
}
