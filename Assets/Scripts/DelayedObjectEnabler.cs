using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedObjectEnabler : MonoBehaviour
{
    public GameObject target;
    public float delay;

    private void OnEnable()
    {
        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        target.SetActive(true);
    }
}
