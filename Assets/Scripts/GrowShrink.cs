using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrowShrink : MonoBehaviour
{
    public float cycleTime = 2.0f;
    public float newScaleMultiplier = 2.0f;

    private bool currentlyScaling = false;

    public void AnimateScale()
    {
        if (!currentlyScaling) Animate();
    }

    private IEnumerator Animate()
    {
        currentlyScaling = true;

        float originalScale = transform.localScale.x;

        transform.DOScale(newScaleMultiplier * originalScale, cycleTime / 2f);
        yield return new WaitForSeconds(cycleTime / 2f);
        transform.DOScale(originalScale, cycleTime / 2f);
        yield return new WaitForSeconds(cycleTime / 2f);

        currentlyScaling = false;
    }
}
