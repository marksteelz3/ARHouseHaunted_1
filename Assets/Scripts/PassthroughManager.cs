using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
    public OVRPassthroughLayer passthrough;
    public OVRInput.Button button;
    public OVRInput.Controller controller;
    public List<Gradient> colorMapGradient;

    private Coroutine colorTweenRoutine;

    // Update is called once per frame
    void Update()
    {
        //if(OVRInput.GetDown(button,controller))
        //{
        //    passthrough.hidden = !passthrough.hidden;
        //}
    }

    public void SetOpacity(float value)
    {
        passthrough.textureOpacity = value;
    }

    public void SetColorMapGradient(int index)
    {
        passthrough.colorMapEditorGradient = colorMapGradient[index];
    }

    public void SetBrightness(float value)
    {
        passthrough.colorMapEditorBrightness = value;
    }
    public void SetContrast(float value)
    {
        passthrough.colorMapEditorContrast = value;
    }
    public void SetPosterize(float value)
    {
        passthrough.colorMapEditorPosterize = value;
    }

    public void SetEdgeRendering(bool value)
    {
        passthrough.edgeRenderingEnabled = value;
    }

    public void SetEdgeRed(float value)
    {
        Color newColor = new Color(value, passthrough.edgeColor.g, passthrough.edgeColor.b);
        passthrough.edgeColor = newColor;
    }

    public void SetEdgeGreen(float value)
    {
        Color newColor = new Color(passthrough.edgeColor.r, value, passthrough.edgeColor.b);
        passthrough.edgeColor = newColor;
    }
    public void SetEdgeBlue(float value)
    {
        Color newColor = new Color(passthrough.edgeColor.r, passthrough.edgeColor.g, value);
        passthrough.edgeColor = newColor;
    }

    public void SetEdgeColor(Color color)
    {
        passthrough.edgeColor = color;
    }

    public void TweenEdgeColor(Color color, float infectionDuration = 15f)
    {
        SetEdgeRendering(true);
        if (colorTweenRoutine != null) StopCoroutine(colorTweenRoutine);
        colorTweenRoutine = StartCoroutine(TweenEdgeColorCoroutine(color, infectionDuration));
    }

    private IEnumerator TweenEdgeColorCoroutine(Color color, float infectionDuration = 15f)
    {
        float alpha = 0f;
        float tweenTime = 2.5f;
        int iterations = 100;
        float alphaDelta = 1f / iterations;
        float timeDelta = tweenTime / iterations;
        passthrough.edgeColor = Color.clear;

        for (int i = 0; i < iterations; i++)
        {
            alpha += alphaDelta;
            Color currentColor = new Color(color.r, color.g, color.b, alpha);
            passthrough.edgeColor = currentColor;
            yield return new WaitForSeconds(timeDelta);
        }

        yield return new WaitForSeconds(infectionDuration);

        for (int i = 0; i < iterations; i++)
        {
            alpha -= alphaDelta;
            Color currentColor = new Color(color.r, color.g, color.b, alpha);
            passthrough.edgeColor = currentColor;
            yield return new WaitForSeconds(timeDelta);
        }

        SetEdgeRendering(false);
    }
}
