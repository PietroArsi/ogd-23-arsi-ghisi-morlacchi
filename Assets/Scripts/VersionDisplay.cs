using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.GetComponent<TMPro.TextMeshProUGUI>().text = $"v {Application.version}";
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<TMPro.TextMeshProUGUI>().text = $"v {Application.version}";
    }
}
