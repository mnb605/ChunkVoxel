using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSText : MonoBehaviour
{
    Text fpsText;
    float deltaTime = 0.0f;

    private void Start()
    {
        fpsText = this.GetComponent<Text>();
    }

    private void LateUpdate()
    {
        int w = Screen.width, h = Screen.height;
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = text;
    }

    //private void OnGUI()
    //{
    //    int w = Screen.width, h = Screen.height;
    //    deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    //    GUIStyle style = new GUIStyle();

    //    Rect rect = new Rect(w / 2, h / 2, w, h);
    //    style.alignment = TextAnchor.UpperLeft;
    //    style.fontSize = h * 2 / 50;
    //    style.normal.textColor = new Color(0.0f, 0.9f, 0.0f, 1.0f);
    //    float msec = deltaTime * 1000.0f;
    //    float fps = 1.0f / deltaTime;
    //    //if (fps > 60) fps = 60;
    //    string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
    //    //fpsText.text = text;
    //    GUI.Label(rect, text, style);
    //    Debug.Log(text);
    //}
}
