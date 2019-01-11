using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    public static IEnumerator Timer(Action action, float duration, bool loop)
    {
        do
        {
            yield return new WaitForSeconds(duration);
            action.Invoke();
        } while (loop);
    }
}