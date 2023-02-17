using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenOscillator : MonoBehaviour
{
    public float cycleLength = 12f;
    public float startDelay = 0f;
    public float shootUpTime = 0.5f;
    public Transform top;
    public Transform bottom;
    public Transform shootUpLocation;
    public AudioSource audioSource;

    private bool shootingUp = false;

    private Tween currentTween;

    private void OnEnable()
    {
        StartCoroutine(Oscillate());
    }

    private IEnumerator Oscillate()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            float randomTime = (cycleLength / 2) + Random.Range(0, cycleLength);

            currentTween = transform.DOMove(top.position, randomTime);
            yield return new WaitForSeconds(randomTime);
            currentTween = transform.DOMove(bottom.position, randomTime);
            yield return new WaitForSeconds(randomTime); 
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void QuickShootUp()
    {
        if (shootingUp) return;

        StopAllCoroutines();
        StartCoroutine(ShootUp());
    }

    private IEnumerator ShootUp()
    {
        shootingUp = true;
        currentTween.Kill();
        currentTween = transform.DOMove(shootUpLocation.position, shootUpTime);
        audioSource.Play();

        yield return new WaitForSeconds(shootUpTime);

        StartCoroutine(Oscillate());

        yield return new WaitForSeconds(2.5f);
        shootingUp = false;
    }
}
