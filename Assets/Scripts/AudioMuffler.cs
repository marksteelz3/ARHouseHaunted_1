using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioMuffler : MonoBehaviour
{
    public AudioSource audioSource;
    public float targetVolume = 0.5f;
    public float transitionTime = 1f;

    private void OnEnable()
    {
        if (audioSource != null)
        {
            audioSource.DOFade(targetVolume, transitionTime);
        }
    }
}
