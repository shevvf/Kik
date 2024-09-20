using Unity.Netcode;

using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CellFinder cellFinder;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    private void Start()
    {
        cellFinder = FindObjectOfType<CellFinder>();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Cell overlappingCell = cellFinder.FindCellUnderCard(rectTransform);
        if (overlappingCell != null)
        {
            SnapToCell(overlappingCell);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private void SnapToCell(Cell cell)
    {
        Vector3 cellPosition = cell.transform.localPosition;

        if (IsServer)
        {
            SpawnCard(cellPosition);
        }
        else
        {
            SpawnCardServerRpc(cellPosition);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCardServerRpc(Vector3 position, ServerRpcParams rpcParams = default)
    {
        SpawnCard(position, rpcParams);
    }

    public void SpawnCard(Vector3 position, ServerRpcParams rpcParams = default)
    {
        CardUI cardUI = gameObject.GetComponent<CardUI>();
        GameObject newBoardCard = Instantiate(cardUI.Card.CardReference, position, Quaternion.identity);
        if (newBoardCard.TryGetComponent(out NetworkObject networkObject))
        {
            networkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);
            Debug.Log($"GameObject {newBoardCard.name} spawned, Owner : {rpcParams.Receive.SenderClientId}");
        }

        DeleteClientCardClientRpc();
    }

    [ClientRpc]
    private void DeleteClientCardClientRpc()
    {
        Destroy(gameObject);
    }
}