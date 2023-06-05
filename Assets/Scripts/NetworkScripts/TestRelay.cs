using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine.UI;
using TMPro;
public class TestRelay : MonoBehaviour
{
    public Button join;
    public TextMeshProUGUI theCode;
    public TMP_InputField codeInputField;
   
    // Start is called before the first frame update
    async void  Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    private async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string JoinCode= await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            visualDebugger.AddMessage(JoinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }catch(RelayServiceException e){
            Debug.Log(e);
            visualDebugger.AddMessage(e.ToString());
        }
    }

    private async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Join relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
            visualDebugger.AddMessage(e.ToString());
        }
                
    }
    public void StartRelay()
    {
        CreateRelay();
    }

    public void SetText(string text)
    {

    }
    public void setCode()
    {
        theCode.text = codeInputField.text;
    }
    public void JoinWithCode()
    {

        Debug.Log("THE CODE IS: " + theCode);
        JoinRelay("NRW98F");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
