using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuUI : MonoBehaviour
{
    // [Header("Movement")]
    public static LobbyManagementUI LocalInstance { get; private set; }
    [SerializeField] private Button HostLobby;
    [SerializeField] private Button ReturnMenu;
    //[SerializeField] private Button JoinGameButton;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Button CreateLobby;
    [SerializeField] private Button cancleCreation;
    [SerializeField] private Image createLobbyImage;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;

    [SerializeField] private TMP_InputField nameLobbyInputs;

    public bool isActive = false;

    private void Awake()
    {
        isActive = true;
        Debug.Log("ADD LISTENERS");
        HostLobby.onClick.AddListener(() =>
        {
            createLobbyImage.gameObject.SetActive(true);
        });
        CreateLobby.onClick.AddListener(() =>
        {

            CatnipLobby.Instance.CreateLobby(nameLobbyInputs.text);
        });
        cancleCreation.onClick.AddListener(()=>{
            createLobbyImage.gameObject.SetActive(false);
        });
        ReturnMenu.onClick.AddListener(() =>
        {

            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
        //}

        // JoinGameButton.onClick.AddListener(() =>
        //  {
        //      ConnectionManager.Instance.StartClient();
        //      CatnipLobby.Instance.QuickJoin();
        //  });



        playerName.text = GenerateRandomName();

        lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
       playerName.text = "Cat0" + Random.Range(0, 100);
       ConnectionManager.Instance.SetPlayerName(playerName.text);

        CatnipLobby.Instance.OnLobbyListChanged += CatnipLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void CatnipLobby_OnLobbyListChanged(object sender, CatnipLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        //clean the lobby list
        //clean the lobby list
        
            foreach (Transform child in lobbyContainer)
            {
                if (child == lobbyTemplate) continue;
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbyList)
            {
                //Debug.Log(lobby.Name);
                Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
                lobbyTransform.gameObject.SetActive(true);
                lobbyTransform.GetComponent<lobbyTemplateUi>().setLobby(lobby);
            }
        
    }

    private void OnDestroy()
    {
        CatnipLobby.Instance.OnLobbyListChanged -= CatnipLobby_OnLobbyListChanged;
    }

    private string GenerateRandomName() {
        
        return "Cat0" + Random.Range(0, 100);
    }

    //temporary solution with keyboard
   
}
