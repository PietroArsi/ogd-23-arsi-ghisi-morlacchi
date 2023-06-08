using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeAIResponse : MonoBehaviour
{
    public TMPro.TextMeshProUGUI responseText;
    public TMPro.TMP_InputField questionField;
    public SimpleRotatingImage loadingIcon;
    public Button generateButton;

    void Start() {
        responseText.text = "...";
    }

    public void GenerateResponse() {
        string question = questionField.text;

        if (question == "") {
            return;
        }

        generateButton.interactable = false;

        if (CheckInput(question) != 0) {

        }
        else {
            loadingIcon.SetRotate(true);
            string response = "Meow ";
            int limit = Random.Range(0, 10);
            for (int i = 0; i<limit; i++) {
                response += "meow ";
            }
            StartCoroutine(DelayAnswer(response));
        }
    }

    IEnumerator DelayAnswer(string response) {
        yield return new WaitForSeconds(2);
        responseText.text = response;
        loadingIcon.SetRotate(false);
        generateButton.interactable = true;
    }

    private int CheckInput(string inputString) {
        return 0;
    }
}
