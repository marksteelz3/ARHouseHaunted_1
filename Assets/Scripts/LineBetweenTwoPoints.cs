using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBetweenTwoPoints : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform start;
    public Transform end;
    public float lineWidth = 0.01f;

    private Vector3 lastStartPos;
    private Vector3 lastEndPos;

    private void OnEnable()
    {
        if (lineRenderer == null || start == null || end == null)
        {
            Destroy(this);
            return;
        }

        Vector3[] path = {start.position, end.position};
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(path);
        lastStartPos = start.position;
        lastEndPos = end.position;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startWidth = lineWidth;
    }

    private void Update()
    {
        if (lastStartPos == start.position && lastEndPos == end.position) return;

        Vector3[] path = { start.position, end.position };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(path);
        lastStartPos = start.position;
        lastEndPos = end.position;
    }
}
