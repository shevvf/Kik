using System.Collections.Generic;

using Unity.Netcode;

using UnityEngine;

public class CardManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> player1Cards;
    [SerializeField] private List<GameObject> player2Cards;

    private Dictionary<int, GameObject> cardDictionary;

    private void Start()
    {
        cardDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < player1Cards.Count; i++)
        {
            cardDictionary.Add(i, player1Cards[i]);
        }
        for (int i = 0; i < player2Cards.Count; i++)
        {
            cardDictionary.Add(i + player1Cards.Count, player2Cards[i]);
        }
    }

    public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            NetworkObject.CheckObjectVisibility += SetObserver;
            NetworkManager.OnClientConnectedCallback += AssignRandomCardsOnPlayerJoin;
        }
    }

    public override void OnDestroy()
    {
        if (IsServer)
        {
            try
            {
                NetworkObject.CheckObjectVisibility -= SetObserver;
                NetworkManager.OnClientConnectedCallback -= AssignRandomCardsOnPlayerJoin;
            }
            catch
            {
            }
        }
    }

    private bool SetObserver(ulong clientId)
    {
        return clientId == OwnerClientId;
    }

    //public bool CheckObjectVisibility(NetworkConnection connection)
    //{
    //    if (NetworkManager.Singleton.ConnectedClients.ContainsKey(connection.Cli) && connection.ClientId == OwnerClientId)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private void AssignRandomCardsOnPlayerJoin(ulong clientId)
    {
        int[] randomCardIndices = GetRandomCardIndices(clientId);
        SendCardsClientRpc(randomCardIndices, clientId);
    }

    private int[] GetRandomCardIndices(ulong clientId)
    {
        List<int> availableCards = clientId == 0
            ? new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            : new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

        int cardsToSpawn = 7;
        int[] randomCards = new int[cardsToSpawn];

        for (int i = 0; i < cardsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            randomCards[i] = availableCards[randomIndex];
            availableCards.RemoveAt(randomIndex);
        }

        return randomCards;
    }

    [ClientRpc]
    private void SendCardsClientRpc(int[] cardIndices, ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId != clientId)
        {
            return;
        }

        if (IsServer)
        {
            SpawnCards(cardIndices, clientId);
        }
        else
        {
            SpawnCardServerRpc(cardIndices, clientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCardServerRpc(int[] cardIndices, ulong clientId)
    {
        SpawnCards(cardIndices, clientId);
    }

    private void SpawnCards(int[] cardIndices, ulong ownerClientId)
    {
        foreach (int index in cardIndices)
        {
            if (cardDictionary.ContainsKey(index))
            {
                GameObject card = Instantiate(cardDictionary[index], transform.position, Quaternion.identity);

                if (card.TryGetComponent(out NetworkObject networkObject))
                {
                    networkObject.SpawnWithOwnership(ownerClientId);
                }
                card.transform.SetParent(transform);
            }
        }
    }
}
