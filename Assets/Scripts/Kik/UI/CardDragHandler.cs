using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        CardUI cardUI = gameObject.GetComponent<CardUI>();
        GameObject newBoardCard = Instantiate(cardUI.Card.CardReference, cell.transform.localPosition, Quaternion.identity);
        newBoardCard.transform.SetParent(cell.transform);
        Destroy(gameObject);
    }
}