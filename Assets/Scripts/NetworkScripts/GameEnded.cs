using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEnded : NetworkBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private int score;
    [SerializeField] private List<Image> crunchies;
    [SerializeField] private GameObject GameEndedScreen;
    [SerializeField] private Button returnToMenu;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI EndTitle;
    //public GenericSceneManager genericSceneManager;
    // Start is called before the first frame update

    private void Awake()
    {
        //returnToMenu.onClick.AddListener(() =>
        //{
        //    NetworkManager.Singleton.Shutdown();
        //    //if (genericSceneManager != null)
        //    //{
        //    //    genericSceneManager.ChangeScene("MainMenu");
        //    //}
        //    //SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        //});
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

    public void ReturnToMenu()
    {
        NetworkManager.Singleton.Shutdown();
        //if (genericSceneManager != null)
        //{
        //    genericSceneManager.ChangeScene("MainMenu");
        //}
        //SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
    }
    private void GameManagerStates_OnStateChanged(object sender, System.EventArgs e)
    {

        if (GameManagerStates.Instance.IsGameOver())
        {
            if (GameManagerStates.Instance.GetComponent<GameManager>().GetEvidence() >= 5)
            {
                if (IsHost)
                {
                   
                    ShowGameOverClientRpc();
                    HideCookiesClientRpc();
                }
            }
            else
            {
                ShowLevelCompleteClientRpc();
                //Debug.Log("Level Complete");
                //SaveScore();
                //Show(GameManagerStates.Instance.GetComponent<GameManager>().GetEvidence() < 5);
            }
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

    private void Show(bool withCookies)
    {
        GameEndedScreen.SetActive(true);
        int numberCatnip = gm.GetCatnip();
        score = numberCatnip * 10;
        totalScore.text = $"Score: {score}";
        if (withCookies)
        {
            if (score >= 0 && score < 10)
            {
                Debug.Log(crunchies[0]);
                crunchies[0].enabled = true;
            }
            else if (score >= 10 && score < 30)
            {
                crunchies[0].enabled = true;
                crunchies[1].enabled = true;
            }
            else if (score >= 30)
            {
                crunchies[0].enabled = true;
                crunchies[1].enabled = true;
                crunchies[2].enabled = true;

            }
        } else
        {
            crunchies[0].enabled = false;
            crunchies[1].enabled = false;
            crunchies[2].enabled = false;
            totalScore.enabled = false;
        }
        
    }

    private void Hide()
    {
        GameEndedScreen.SetActive(false);
    }

    [ClientRpc]
    private void ShowGameOverClientRpc()
    {
        EndTitle.text = "Game Over (Dog Strike)";
        totalScore.gameObject.SetActive(false);
        Show(false);
       // NetworkManager.Singleton.Shutdown();
    }

    [ClientRpc] 
    void HideCookiesClientRpc()
    {
        foreach (Image crunch in crunchies)
        {
            crunch.enabled = false;
        }
      
    }
    [ClientRpc]
    private void ShowLevelCompleteClientRpc()
    {
        EndTitle.text = "Winner Winner Mouse Dinner";
        totalScore.gameObject.SetActive(true);
        SaveScore();
        Show(true);
        // NetworkManager.Singleton.Shutdown();
    }

   
}
