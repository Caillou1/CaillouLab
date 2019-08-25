using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimit : MonoBehaviour
{
    public bool ShowFPS = true;
    public int FrameLimit = 60;

    private void Awake() {
        //Application.targetFrameRate = FrameLimit;
    }

    private void Start() {
        KU.StartTimer(() => LogFPS(.1f), .1f, true);
    }

    private void LogFPS(float duration) {
        float fps = 1.0f/Time.deltaTime;
        KU.LogPermanent("Framerate", fps, Color.red, false);
    }
}
