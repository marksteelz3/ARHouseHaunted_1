using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimedPopup : MonoBehaviour
{
    public float popupDuration = 10f;
    public bool animateIn = true;

    public void OnEnable()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        gameObject.SetActive(true);

        if (animateIn)
        {
            float startScale = transform.localScale.x;
            transform.localScale = Vector3.zero;
            transform.DOScale(startScale, 1f);
        }

        yield return new WaitForSeconds(popupDuration);

        if (animateIn)
        {
            transform.DOScale(0f, 1f);
        }

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
