using Unity.Netcode;

using UnityEngine;

public class PlayerConnectionHandler : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDestroy()
    {
        try
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
        catch
        {
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            HandlePlayerConnected();
        }
        else
        {
            HandleEnemyConnected();
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            HandlePlayerDisconnected();
        }
        else
        {
            HandleEnemyDisconnected();
        }
    }

    private void HandlePlayerConnected()
    {
        Debug.Log("����� �����������.");
    }

    private void HandleEnemyConnected()
    {
        Debug.Log("���� �����������.");
    }

    private void HandlePlayerDisconnected()
    {
        Debug.Log("����� ����������.");
    }

    private void HandleEnemyDisconnected()
    {
        Debug.Log("���� ����������.");
    }
}
