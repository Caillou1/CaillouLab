using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class KU : MonoBehaviour
{
    public static KU Sys { get; private set; }

    public Text DefaultLogText;
    public bool UseLogPanel;

    private static Transform KUTransform;
    private static Canvas KUCanvas;
    private static Transform logPanelTransform;
    private static Transform permanentLogTransform;
    private static Transform punctualLogTransform;
    private static RectTransform logPanelRectTransform;
    private static Image logPanelImage;
    private static GameObject logPanelObject;
    private static int LogNumber = 0;
    private static Color defaultColor = Color.white;
    private static Dictionary<string, Text> permanentLogs = new Dictionary<string, Text>();

    private void Awake()
    {
        if (Sys == null)
        {
            Sys = this;
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
        KUCanvas = KUTransform.GetComponentInChildren<Canvas>();
        logPanelTransform = KUCanvas.transform.Find("LogPanel");
        permanentLogTransform = KUCanvas.transform.KUFindDeep("PermanentLogs");
        punctualLogTransform = KUCanvas.transform.KUFindDeep("PunctualLogs");
        logPanelRectTransform = logPanelTransform.GetComponent<RectTransform>();
        logPanelImage = logPanelTransform.GetComponent<Image>();
        logPanelObject = logPanelTransform.gameObject;
    }

    // LOG TO SCREEN

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

    public static void LogPermanent(string logKey, object message, Color? color = null, bool logToConsole = true, Object context = null) {
        if(logToConsole) {
            Debug.Log(message, context);
        }

        if(!permanentLogs.ContainsKey(logKey)) {
            permanentLogs.Add(logKey, Instantiate(Sys.DefaultLogText, permanentLogTransform));
        }

        permanentLogs[logKey].text = logKey + " : " + message.ToString();
        permanentLogs[logKey].color = color ?? defaultColor;
        LayoutRebuilder.ForceRebuildLayoutImmediate(logPanelRectTransform);
    }

    public static void UnlogPermanent(string logKey) {
        if(permanentLogs.ContainsKey(logKey)) {
            Destroy(permanentLogs[logKey].gameObject);
            permanentLogs.Remove(logKey);
        }
    }

    private static void LogToScreen(LogInfo log)
    {
        LogNumber++;
        if (Sys.UseLogPanel && !logPanelImage.enabled)
            logPanelImage.enabled = true;
        var text = Instantiate(Sys.DefaultLogText, punctualLogTransform);
        text.transform.SetAsFirstSibling();
        text.text = log.Message.ToString();
        text.color = log.LogColor;
        LayoutRebuilder.ForceRebuildLayoutImmediate(logPanelRectTransform);
        StartTimer(() => RemoveFromScreen(text, log.Duration), log.Duration);
    }

    private static void RemoveFromScreen(Text logText, float duration)
    {
        LogNumber--;
        if (LogNumber < 1 || !Sys.UseLogPanel)
            logPanelImage.enabled = false;
        Destroy(logText.gameObject);
    }

    // TIMER

    public static IEnumerator StartTimer(Action action, float duration, bool loop = false)
    {
        IEnumerator timer = Timer(action, duration, loop);
        Sys.StartCoroutine(timer);
        return timer;
    }

    public static void StopTimer(IEnumerator timer)
    {
        Sys.StopCoroutine(timer);
    }

    private static IEnumerator Timer(Action action, float duration, bool loop)
    {
        do
        {
            yield return new WaitForSeconds(duration);
            action.Invoke();
        } while (loop);
    }

    // DEBUG SPHERE

    static float sphereT = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
    static List<Vector3> sphereVertices = new List<Vector3>
    {
        new Vector3(-1,  sphereT,  0),
        new Vector3(1, sphereT, 0),
        new Vector3(-1, -sphereT, 0),
        new Vector3(1, -sphereT, 0),
        new Vector3(0, -1, sphereT),
        new Vector3(0, 1, sphereT),
        new Vector3(0, -1, -sphereT),
        new Vector3(0, 1, -sphereT),
        new Vector3(sphereT, 0, -1),
        new Vector3(sphereT, 0, 1),
        new Vector3(-sphereT, 0, -1),
        new Vector3(-sphereT, 0, 1)
    };
    static List<int> sphereTriangles = new List<int>
    {
        0,11,5,0,5,1,0,1,7,0,7,10,0,10,11,1,5,9,5,11,4,11,10,2,10,7,6,7,1,8,3,9,4,3,4,2,3,2,6,3,6,8,3,8,9,4,9,5,2,4,11,6,2,10,8,6,7,9,8,1
    };

    public static void DebugSphere(Vector3 center, float radius, Color color, float duration)
    {
        radius /= 2f;
        for (int i = 0; i < sphereTriangles.Count - 3; i += 3)
        {
            Debug.DrawLine(center + sphereVertices[sphereTriangles[i]] * radius, center + sphereVertices[sphereTriangles[i + 1]] * radius, color, duration);
            Debug.DrawLine(center + sphereVertices[sphereTriangles[i + 1]] * radius, center + sphereVertices[sphereTriangles[i + 2]] * radius, color, duration);
            Debug.DrawLine(center + sphereVertices[sphereTriangles[i + 2]] * radius, center + sphereVertices[sphereTriangles[i]] * radius, color, duration);
        }
    }
}

// EXTENSIONS
public static class KUExtensions
{
    /// <summary>
    /// Find child transform with corresponding name in all chidren and their children
    /// </summary>
    /// <param name="parent">Parent to search in</param>
    /// <param name="name">Name of child to find</param>
    /// <returns></returns>
    public static Transform KUFindDeep(this Transform parent, string name)
    {
        var result = parent.Find(name);
        if (result != null)
            return result;
        foreach (Transform child in parent)
        {
            result = child.KUFindDeep(name);
            if (result != null)
                return result;
        }
        return null;
    }

    /// <summary>
    /// Set transform scale x, y and z to globalScale
    /// </summary>
    /// <param name="tf">Transform to rescale</param>
    /// <param name="globalScale">Global scale</param>
    public static void KUSetGlobalScale(this Transform tf, float globalScale)
    {
        tf.localScale = new Vector3(globalScale, globalScale, globalScale);
    }
}