using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEnded : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private int score;
    [SerializeField] private List<Image> crunchies;
    [SerializeField] private GameObject GameEndedScreen;
    [SerializeField] private Button returnToMenu;
    [SerializeField] private TextMeshProUGUI totalScore;
    // Start is called before the first frame update

    private void Awake()
    {
        returnToMenu.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
        foreach(Image crunch in crunchies)
        {
            crunch.enabled = false;
        }
    }
    private void Start()
    {
        Hide();

        GameManagerStates.Instance.OnStateChanged += GameManagerStates_OnStateChanged;
    }

    private void GameManagerStates_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManagerStates.Instance.IsGameOver())
        {
            Debug.Log("GAMEOVER");
            SaveScore();
            Show();
        }
    }

    private void Update()
    {

    }

    private void SaveScore() {
        int catnipAmount = gm.GetCatnip();
        int effectiveScore = catnipAmount * 10;

        LevelProperties props = GameObject.Find("Level Properties").GetComponent<LevelProperties>();
        if (props != null) {
            SaveManager.RegisterHighscore(props.GetLevelName(), effectiveScore);
            SaveManager.RegisterAchievement(props.GetLevelName(), effectiveScore);
        } else {
            Debug.Log("Level properties not found.");
        }
    }

    private void Show()
    {
        GameEndedScreen.SetActive(true);
        int numberCatnip = gm.GetCatnip();
        score = numberCatnip * 10;
        totalScore.text = $"Score: {score}";
       
        if (score >= 50 && score < 100)
        {
            Debug.Log(crunchies[0]);
            crunchies[0].enabled = true;
        }
        else if (score >= 100 && score < 200)
        {
            crunchies[0].enabled = true;
            crunchies[1].enabled = true;
        }
        else if (score >= 200)
        {
            crunchies[0].enabled = true;
            crunchies[1].enabled = true;
            crunchies[2].enabled=true;
         
        }
    }

    private void Hide()
    {
        GameEndedScreen.SetActive(false);
    }
}
