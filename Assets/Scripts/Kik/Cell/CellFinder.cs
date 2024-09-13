using UnityEngine;

public class CellFinder : MonoBehaviour
{
    public Cell FindCellUnderCard(Transform cardTransform)
    {
        Vector3 cardPosition;
        if (cardTransform is RectTransform)
        {
            cardPosition = Camera.main.ScreenToWorldPoint(cardTransform.transform.position);
            cardPosition.z = 0;
        }
        else
        {
            cardPosition = cardTransform.position;
        }

        Debug.Log($"Card Center in World: {cardPosition}");

        foreach (Cell cell in FindObjectsOfType<Cell>())
        {
            Vector2 cellSize = cell.GetComponent<SpriteRenderer>().bounds.size;
            Rect cellRectWorld = new(cell.transform.position - (Vector3)cellSize / 2, cellSize);

            if (IsCardOverlappingCell(cardPosition, cellRectWorld))
            {
                return cell;
            }
        }

        Debug.Log("No Cell found under the Card");
        return null;
    }

    private bool IsCardOverlappingCell(Vector3 cardCenterWorld, Rect cellRectWorld)
    {
        return cellRectWorld.Contains(cardCenterWorld);
    }
}
