using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;

public class UINetworkManager : MonoBehaviour
{
    public TMP_InputField nicknameInputField;
    public TMP_InputField joinCodeInputField;
    
    public void BtnClick_StartHost()
    {
        Debug.Log("Starting Host....");

        StartHostWithRelay(10, "udp");
    }

    public void BtnClick_StartClient()
    {
        if (joinCodeInputField.text == "")
        {
            Debug.Log("Join code is empty");
            return;
        }

        Debug.Log("Starting Client....");
        StartClientWithRelay(joinCodeInputField.text, "udp");
    }

    public async Task<string> StartHostWithRelay(int maxConnections, string connectionType)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, connectionType));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);


        if (joinCode != "")
        {
            joinCodeInputField.text = joinCode;
            joinCodeInputField.interactable = false;
        }


        return NetworkManager.Singleton.StartHost() ? joinCode : null;


    }

    public async Task<bool> StartClientWithRelay(string joinCode, string connectionType)
    {
        await UnityServices.InitializeAsync();        
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, connectionType));
        
        if (NetworkManager.Singleton.StartClient())
        {
            joinCodeInputField.gameObject.SetActive(false);
        }

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}
