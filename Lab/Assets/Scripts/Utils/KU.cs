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

    private enum ConsoleState
    {
        Closed,
        Partial,
        Opened
    }

    public static KU Sys { get; private set; }

    public Text DefaultLogText;
    public bool UseLogPanel;
    public KeyCode OpenConsoleKey;

    private static Transform KUTransform;
    private static Canvas KUCanvas;
    private static Transform logPanelTransform;
    private static Image logPanelImage;
    private static GameObject logPanelObject;
    private static int LogNumber = 0;
    private static Color defaultColor = Color.white;
    private static ConsoleState currentConsoleState = ConsoleState.Closed;

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
        logPanelImage = logPanelTransform.GetComponent<Image>();
        logPanelObject = logPanelTransform.gameObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(OpenConsoleKey))
        {
            ToggleConsoleState();
        }
    }

    private void ToggleConsoleState()
    {
        int stateNumber = Enum.GetNames(typeof(ConsoleState)).Length;
        currentConsoleState = (ConsoleState)(((int)currentConsoleState + 1) % stateNumber);

        
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
        if (Sys.UseLogPanel && !logPanelImage.enabled)
            logPanelImage.enabled = true;
        var text = Instantiate(Sys.DefaultLogText, logPanelTransform);
        text.transform.SetAsFirstSibling();
        text.text = log.Message.ToString();
        text.color = log.LogColor;
        Sys.StartCoroutine(RemoveFromScreen(text, log.Duration));
    }

    private static IEnumerator RemoveFromScreen(Text logText, float duration)
    {
        yield return new WaitForSeconds(duration);
        LogNumber--;
        if (LogNumber < 1 || !Sys.UseLogPanel)
            logPanelImage.enabled = false;
        Destroy(logText.gameObject);
    }
}

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