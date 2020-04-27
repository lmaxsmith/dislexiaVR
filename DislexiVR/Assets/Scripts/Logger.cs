using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Logger
{
    private static List<string> messages;
    private static TextMeshProUGUI guiText;

    static Logger()
    {
        messages = new List<string>();
    }

    private static bool EnsureText()
    {
        if (guiText == null)
        {
            guiText = GameObject.Find("DebugText")?.GetComponent<TextMeshProUGUI>();
        }

        if (guiText == null)
        {
            return false;
        }

        return true;
    }

    public static void IngameDebug(string debugMsg)
    {
        if (!EnsureText()) return;

        Debug.Log($"LOGMSG: {debugMsg}");
        messages.Add(debugMsg);
        guiText.SetText(string.Join("\n", messages));
    }

    public static void Clear()
    {
        if (!EnsureText()) return;

        Debug.Log($"LOGMSG: CLEARING");

        messages.Clear();
        guiText.SetText(string.Join("\n", messages));
    }
}
