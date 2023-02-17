//	CameraFacing.cs 
//	original by Neil Carter (NCarter)
//	modified by Hayden Scott-Baron (Dock) - http://starfruitgames.com
//  allows specified orientation axis


using UnityEngine;
using System.Collections;

public class CameraFacing : MonoBehaviour
{
	[SerializeField]
	public Transform cameraToLookAt;

	private bool cameraAssigned = true;

    private void OnEnable()
    {
        if (cameraToLookAt == null)
			StartCoroutine(DelayedLookatAssignment());
    }

    private void Update() 
	{
		if (!cameraAssigned) return;

		Vector3 v = cameraToLookAt.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(cameraToLookAt.position - v); 
	}

	private IEnumerator DelayedLookatAssignment()
    {
		cameraAssigned = false;
		yield return new WaitForSeconds(0.2f);
		cameraToLookAt = MonsterGameManager.Instance.playerHead;
		cameraAssigned = true;
	}
}