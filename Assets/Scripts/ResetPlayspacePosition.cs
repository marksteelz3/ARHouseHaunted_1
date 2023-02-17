using UnityEngine;
using System;
using System.Collections;

public class ResetPlayspacePosition : MonoBehaviour
{
    public Transform playSpace;
    public float initialControllerRotationOffset;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public AudioSource resetAudioSFX;
    public GameObject[] enabledObjectsOnReset;
    //public float audioReminderDelay = 10f;

    public Action<ResetPlayspacePosition> PlayspacePositionReset;

    private void OnEnable()
    {
        //StartCoroutine(ResetPositionAudioReminder());
    }

    private void Update()
    {
        ResetFromControllerPosition();
    }

    private void ResetFromControllerPosition()
    {
        if (MonsterGameManager.Instance.ResetConfirmed) Destroy(this);

        bool rightControllerConnected = OVRInput.IsControllerConnected(rightController);

        if (rightControllerConnected && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, rightController))
        {
            Debug.Log("Resetting playspace position!");

            Vector3 newPosition = new Vector3(MonsterGameManager.Instance.rightController.position.x,
                                                playSpace.position.y,
                                                MonsterGameManager.Instance.rightController.position.z);
            playSpace.position = newPosition;

            //Initial rotation to line up the fire pit roughly
            float controllerYRotation = MonsterGameManager.Instance.rightController.rotation.eulerAngles.y;
            playSpace.rotation = Quaternion.Euler(0f, controllerYRotation + initialControllerRotationOffset, 0f);

            if (resetAudioSFX != null) resetAudioSFX.Play();

            foreach (GameObject obj in enabledObjectsOnReset)
            {
                if (obj != null) obj.SetActive(true);
            }

            PlayspacePositionReset?.Invoke(this);
        }
    }

    //private IEnumerator ResetPositionAudioReminder()
    //{
    //    if (audioSource != null)
    //    {
    //        while (enabled)
    //        {
    //            audioSource.Play();

    //            yield return new WaitForSeconds(audioSource.clip.length + audioReminderDelay);
    //        }
    //    }
    //}
}
