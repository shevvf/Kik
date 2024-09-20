using TMPro;

using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

using UnityEngine;
using UnityEngine.UI;

public class RelayManager : MonoBehaviour
{
    [SerializeField] private Button host;
    [SerializeField] private Button join;
    [SerializeField] private TMP_Text code;
    [SerializeField] private TMP_InputField inputCode;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        host.onClick.AddListener(CreateRelay);
        join.onClick.AddListener(() => JoinRelay(inputCode.text));
    }

    private async void CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2, "europe-west2");
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        code.SetText($"Code: {joinCode}");
        Debug.Log($"Join Code: {joinCode}");

        RelayServerData relayServerData = new(allocation, "wss");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();
    }

    private async void JoinRelay(string code)
    {
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);
        RelayServerData relayServerData = new(joinAllocation, "wss");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
    }
}