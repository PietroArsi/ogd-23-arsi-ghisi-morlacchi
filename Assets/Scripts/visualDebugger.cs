using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class visualDebugger : MonoBehaviour
{
    public static visualDebugger LocalInstance { get; private set; }
    public static int maxMessages = 10;
    static TMPro.TextMeshProUGUI console;
    int messageCounter;
    static List<string> messages;

    private void Awake()
    {
        //if (LocalInstance != null) Destroy(this.transform.parent);
        //DontDestroyOnLoad(this.transform.parent);
    }
    void Start() {
        console = transform.GetComponent<TMPro.TextMeshProUGUI>();
        messages = new List<string>();
        UpdateConsole();
    }

    public static void AddMessage(string message)
    {
        messages.Insert(0, message);
        while(messages.Count > maxMessages)
        {
            messages = messages.Take(maxMessages).ToList();
        }

        UpdateConsole();
    }

    static void UpdateConsole()
    {
        string text = "";
        foreach(string s in messages)
        {
            text += $"{s}\n";
        }

        console.text = text;
    }
}
