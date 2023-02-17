using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayspace : MonoBehaviour
{
    public Transform playSpace;
    public float initialControllerRotationOffset;
    public float dualControllerRotationOffset;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;

    private bool positionReset = false;

    private void Update()
    {
        ResetFromControllerPosition();
    }

    private void ResetFromControllerPosition()
    {
        if (MonsterGameManager.Instance.ResetConfirmed) Destroy(this);

        bool rightControllerConnected = OVRInput.IsControllerConnected(rightController);
        bool leftControllerConnected = OVRInput.IsControllerConnected(leftController);

        if (positionReset && rightControllerConnected && leftControllerConnected && IsRightTriggerPressed() && IsLeftTriggerPressed())
        {
            //Debug.Log("Resetting playspace rotation!");

            //Vector3 newPosition = new Vector3(MonsterGameManager.Instance.playerController.position.x,
            //                                    playSpace.position.y,
            //                                    MonsterGameManager.Instance.playerController.position.z);

            //Vector3 newForward = playSpace.position - newPosition;
            //float rotationDifference = Vector3.Angle(newForward, Vector3.forward);
            //float direction = newPosition.x < playSpace.position.x ? 1 : -1;
            //playSpace.rotation = Quaternion.Euler(0f, rotationDifference * direction, 0f);

            Vector3 controllersLine = MonsterGameManager.Instance.leftController.position - MonsterGameManager.Instance.rightController.position;
            Vector3 wallDirection = Vector3.ProjectOnPlane(controllersLine, Vector3.up);

            //float rotationDifference = Vector3.Angle(wallDirection, Vector3.forward);
            ////float direction = newPosition.x < playSpace.position.x ? 1 : -1;
            //playSpace.rotation = Quaternion.Euler(0f, dualControllerRotationOffset - rotationDifference, 0f);

            playSpace.rotation = Quaternion.LookRotation(wallDirection);
        }

        if (!positionReset && rightControllerConnected && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, rightController))
        {
            Debug.Log("Resetting playspace position!");
            //playSpace.rotation = Quaternion.Euler(0f, MonsterGameManager.Instance.playerController.rotation.eulerAngles.y, 0f);

            Vector3 newPosition = new Vector3(MonsterGameManager.Instance.rightController.position.x, 
                                                playSpace.position.y,
                                                MonsterGameManager.Instance.rightController.position.z);
            playSpace.position = newPosition;

            //Initial rotation to line up the fire pit roughly
            //float rotationDifference = Vector3.Angle(MonsterGameManager.Instance.rightController.forward, Vector3.forward);
            float controllerYRotation = MonsterGameManager.Instance.rightController.rotation.eulerAngles.y;
            playSpace.rotation = Quaternion.Euler(0f, controllerYRotation + initialControllerRotationOffset, 0f);

            positionReset = true;
        }
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
