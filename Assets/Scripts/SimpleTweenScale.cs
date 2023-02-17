using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleTweenScale : MonoBehaviour
{
    public float scale = 1f;
    public float duration = 1f;

    private void OnEnable()
    {
        transform.DOScale(scale, duration);
    }
}
