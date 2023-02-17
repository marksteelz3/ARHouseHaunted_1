using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnable : MonoBehaviour
{
    public GameObject target;
    public float enabledTime = 2.0f;

    public void TimedEnabled()
    {
        if (target != null) StartCoroutine(EnableTimer());
    }

    private IEnumerator EnableTimer()
    {
        target.SetActive(true);
        yield return new WaitForSeconds(enabledTime);
        target.SetActive(false);
    }
}
