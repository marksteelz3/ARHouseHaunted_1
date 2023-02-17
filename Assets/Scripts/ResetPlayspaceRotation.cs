using UnityEngine;
using System;
using System.Collections;

public class ResetPlayspaceRotation : MonoBehaviour
{
    public Transform playSpace;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public float rotationSequenceEndDelay = 3f;
    public AudioSource audioSource;
    public float audioReminderDelay = 10f;
    public GameObject[] objectsEnabledOnSequenceStart;
    public GameObject[] objectsEnabledOnReset;
    public GameObject[] objectsDisabledOnReset;

    public Action<ResetPlayspaceRotation> PlayspaceRotationReset;

    private bool rotationReset = false;

    private void OnEnable()
    {
        //NOTE: disabling for AR House halloween event
        //StartCoroutine(ResetRotationAudioReminder());

        foreach (GameObject obj in objectsEnabledOnSequenceStart)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    private void Update()
    {
        ResetRotationFromControllers();
    }

    private void ResetRotationFromControllers()
    {
        if (MonsterGameManager.Instance.ResetConfirmed) Destroy(this);

        bool rightControllerConnected = OVRInput.IsControllerConnected(rightController);
        bool leftControllerConnected = OVRInput.IsControllerConnected(leftController);

        if (rightControllerConnected && leftControllerConnected && IsRightTriggerPressed() && IsLeftTriggerPressed())
        {
            Vector3 controllersLine = MonsterGameManager.Instance.leftController.position - MonsterGameManager.Instance.rightController.position;
            Vector3 wallDirection = Vector3.ProjectOnPlane(controllersLine, Vector3.up);

            playSpace.rotation = Quaternion.LookRotation(wallDirection);
            if (!rotationReset)
            {
                StopAllCoroutines();
                StartCoroutine(DelayedRotationSequenceEnd());
            }
        }
    }

    private IEnumerator ResetRotationAudioReminder()
    {
        if (audioSource != null)
        {
            while (enabled)
            {
                audioSource.Play();

                yield return new WaitForSeconds(audioSource.clip.length + audioReminderDelay);
            }
        }
    }

    private IEnumerator DelayedRotationSequenceEnd()
    {
        rotationReset = true;

        yield return new WaitForSeconds(rotationSequenceEndDelay);

        foreach (GameObject obj in objectsEnabledOnReset)
        {
            if (obj != null) obj.SetActive(true);
        }

        foreach (GameObject obj in objectsDisabledOnReset)
        {
            if (obj != null) obj.SetActive(false);
        }

        PlayspaceRotationReset?.Invoke(this);
    }

    private bool IsRightTriggerPressed()
    {
        return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, rightController);
    }

    private bool IsLeftTriggerPressed()
    {
        return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, leftController);
    }
}
