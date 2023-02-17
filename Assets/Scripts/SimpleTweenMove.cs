using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleTweenMove : MonoBehaviour
{
    public Transform target;
    public float duration = 3f;
    public bool moveToHeadYPos = false;

    private void OnEnable()
    {
        Vector3 playerHeadY = new Vector3(transform.position.x, MonsterGameManager.Instance.playerHead.position.y, transform.position.z);
        Vector3 position = moveToHeadYPos ? playerHeadY : target.position;
        transform.DOMove(position, duration);
    }
}
