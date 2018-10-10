using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class KU : MonoBehaviour
{
    private class LogInfo
    {
        public object Message;
        public float Duration;
        public Color LogColor;

        public LogInfo(object m, float d, Color? c)
        {
            Message = m;
            Duration = d;
            LogColor = c ?? defaultColor;
        }
    }

    public static KU Instance { get; private set; }

    public Text DefaultLogText;
    public bool UseLogPanel;

    private Transform KUTransform;
    private Canvas KUCanvas;
    private Transform logPanelTransform;
    private Image logPanelImage;

    private static int LogNumber = 0;
    private static Color defaultColor = Color.white;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        KUTransform = transform;
        KUCanvas = transform.GetComponentInChildren<Canvas>();
        logPanelTransform = KUCanvas.transform.Find("LogPanel");
        logPanelImage = logPanelTransform.GetComponent<Image>();
    }
    
    public static void Log(object message, float duration = 5.0f, Color? color = null, bool logToScreen = true, bool logToConsole = true, Object context = null)
    {
        if(logToConsole)
        {
            Debug.Log(message, context);
        }

        if(logToScreen)
        {
            LogToScreen(new LogInfo(message, duration, color));
        }
    }

    private static void LogToScreen(LogInfo log)
    {
        LogNumber++;
        if (Instance.UseLogPanel && !Instance.logPanelImage.enabled)
            Instance.logPanelImage.enabled = true;
        var text = Instantiate(Instance.DefaultLogText, Instance.logPanelTransform);
        text.transform.SetAsFirstSibling();
        text.text = log.Message.ToString();
        text.color = log.LogColor;
        Instance.StartCoroutine(RemoveFromScreen(text, log.Duration));
    }

    private static IEnumerator RemoveFromScreen(Text logText, float duration)
    {
        yield return new WaitForSeconds(duration);
        LogNumber--;
        if (LogNumber < 1 || !Instance.UseLogPanel)
            Instance.logPanelImage.enabled = false;
        Destroy(logText.gameObject);
    }
}

public static class KUExtensions
{
    public static Transform FindDeep(this Transform parent, string name)
    {
        var result = parent.Find(name);
        if (result != null)
            return result;
        foreach (Transform child in parent)
        {
            result = child.FindDeep(name);
            if (result != null)
                return result;
        }
        return null;
    }
}