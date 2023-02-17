using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProximityEvent : MonoBehaviour
{
    public float proximity = 0.1f;
    public float minimumEventInterval = 2.0f;
    public UnityEvent onProximityReached;

    private bool eventEnabled = true;

    private void Update()
    {
        if (eventEnabled) CheckTorchDistance();
    }

    private void CheckTorchDistance()
    {
        Vector3 rightDistanceVector = transform.position - MonsterGameManager.Instance.rightTorch.position;
        Vector3 leftDistanceVector = transform.position - MonsterGameManager.Instance.leftTorch.position;

        if (rightDistanceVector.magnitude < proximity || leftDistanceVector.magnitude < proximity)
        {
            onProximityReached?.Invoke();
            StartCoroutine(EventInterval());
        }
    }

    private IEnumerator EventInterval()
    {
        eventEnabled = false;
        yield return new WaitForSeconds(minimumEventInterval);
        eventEnabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, proximity);
    }
}
