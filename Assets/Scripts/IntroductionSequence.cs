using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntroductionSequence : MonoBehaviour
{
    public AudioSource audioSource;
    public float initialAudioDelay = 1.5f;
    public float postAudioDelay = 4f;
    public GameObject[] introObjects;

    public Action<IntroductionSequence> IntroductionSequenceFinished;

    private void Update()
    {
        bool leftControllerConnected = OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);

        if (leftControllerConnected && OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            StopAllCoroutines();
            IntroductionSequenceFinished?.Invoke(this);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(IntroSequence());
    }

    private void OnDisable()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        foreach (GameObject obj in introObjects)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    private IEnumerator IntroSequence()
    {
        if (audioSource != null)
        {
            yield return new WaitForSeconds(initialAudioDelay);

            audioSource.Play();

            foreach (GameObject obj in introObjects)
            {
                if (obj != null) obj.SetActive(true);
            }

            yield return new WaitForSeconds(audioSource.clip.length + postAudioDelay);

            IntroductionSequenceFinished?.Invoke(this);
        }
    }
}
