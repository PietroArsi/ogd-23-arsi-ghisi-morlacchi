using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> words = new List<string>();
        string textFile = Resources.Load<TextAsset>("tutorial").ToString();

        //foreach (string s in textFile.Split("\n")) {
        //    words.Add(s.Replace("\r", ""));
        //}

        GetComponent<TMPro.TextMeshProUGUI>().text = textFile;
    }
}
