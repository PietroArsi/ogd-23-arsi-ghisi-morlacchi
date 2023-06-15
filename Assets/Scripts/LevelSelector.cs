using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject levelLabel;
    public GameObject scoreLabel;
    public Transform interactionCollider;
    public LayerMask interactionLayer;

    private TMPro.TextMeshProUGUI levelText;
    private TMPro.TextMeshProUGUI scoreText;
    private string levelSelected = null;

    void Start()
    {
        levelLabel.SetActive(false);
        scoreLabel.SetActive(false);
        levelText = levelLabel.transform.Find("title").GetComponent<TMPro.TextMeshProUGUI>();
        scoreText = scoreLabel.transform.Find("title").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapBox(interactionCollider.transform.position, interactionCollider.localScale / 2, Quaternion.identity, interactionLayer);

        bool found = false;
        foreach (Collider c in hitColliders) {
            if (hitColliders.Length > 0 && c.GetComponent<LevelPointManager>()) {
                string levelName = c.GetComponent<LevelPointManager>().GetLabel();
                levelText.text = levelName;
                scoreText.text = $"HS: {SaveManager.GetHighscore(c.GetComponent<LevelPointManager>().GetName())}";
                if (c.GetComponent<LevelPointManager>().IsUnlocked()) {
                    levelSelected = c.GetComponent<LevelPointManager>().GetName();
                }
                found = true;
                levelLabel.SetActive(true);
                scoreLabel.SetActive(true);
                break;
            }
        }

        if (!found) {
            levelSelected = null;
            levelLabel.SetActive(false);
            scoreLabel.SetActive(false);
        }
    }

    public string GetSelectedLevel() {
        if (levelSelected != null) {
            return levelSelected;
        } else {
            return "";
        }
    }
}
