using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Logger
{
    private static List<string> messages;

    static Logger()
    {
        messages = new List<string>();
    }

    public static void IngameDebug(string debugMsg)
    {
        // if (messages.Count > 0 && messages.Last() == debugMsg)
        // {
        //     return;
        // }
        messages.Add(debugMsg);

        Debug.Log($"LOGMSG: {debugMsg}");

        GameObject.Find("DebugText")?.GetComponent<TextMeshProUGUI>()?.SetText(string.Join("\n", messages));
    }

    public static void Clear()
    {
        messages.Clear();
        GameObject.Find("DebugText")?.GetComponent<TextMeshProUGUI>()?.SetText("");
    }
}
