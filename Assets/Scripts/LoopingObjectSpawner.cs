using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public float spawnInterval = 3.0f;
    public float randomSpawnForce = 0f;

    private void OnEnable()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (enabled)
        {
            Rigidbody rb = Instantiate(objectPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();

            if (randomSpawnForce > 0f)
            {
                rb.transform.RotateAround(rb.transform.position, Vector3.up, Random.Range(-30, 30f));
                rb.AddForce(rb.transform.forward * randomSpawnForce); 
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
