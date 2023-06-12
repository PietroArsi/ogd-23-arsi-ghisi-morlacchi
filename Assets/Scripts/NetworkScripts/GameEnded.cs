using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameEnded : MonoBehaviour
{
    [SerializeField] private GameManager _gm;
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
            Show();
        }
    }

    private void Update()
    {

    }


    private void Show()
    {
        GameEndedScreen.SetActive(true);
        int numberCatnip= _gm.GetCatnip();
        score = numberCatnip * 10;
        totalScore.text = "Scroe: " + score;
        for(int index=0; index < crunchies.Count ;index++)
        {
            Debug.Log(index);
            if (score > 50)
            {
                Debug.Log(crunchies[index]);
                crunchies[index].enabled = true;
            }
            else if (score < 100)
            {
                crunchies[index].enabled = true;
            }
            else if (score > 150)
            {
                crunchies[index].enabled=true;
                break;
            }
        }
    }
    private void Hide()
    {
        GameEndedScreen.SetActive(false);
    }
}
