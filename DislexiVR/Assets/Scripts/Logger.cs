using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Logger
{
    private static string lastMsg;

    public static void IngameDebug(string debugMsg)
    {
        if (lastMsg == debugMsg)
        {
            return;
        }
        lastMsg = debugMsg;
        Debug.Log(debugMsg);
        TextMeshProUGUI textyBit = GameObject.Find("DebugText")?.GetComponent<TextMeshProUGUI>();
        if (textyBit != null)
        {
            textyBit.SetText(textyBit.text + $"\n{debugMsg}");
        }
    }
}
