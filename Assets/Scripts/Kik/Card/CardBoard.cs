using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using Kik.UI;

using UnityEngine;

public class CardBoard : MonoBehaviour
{
    private CardPanel cardPanel;
    private CellFinder cellFinder;
    private CancellationTokenSource holdCancellationTokenSource;

    private Vector3 oldPosition;
    private bool isDragging = false;

    private void Start()
    {
        cardPanel = ComponentExtensions.FindAnyComponent<CardPanel>();
        cellFinder = FindObjectOfType<CellFinder>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    DetectTouch(touchPosition);
                    break;
                case TouchPhase.Moved:
                    Drag(touchPosition);
                    break;
                case TouchPhase.Ended:
                    StopDrag();
                    StopHold();
                    break;
            }
        }

#if UNITY_EDITOR 
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectTouch(mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Drag(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDrag();
            StopHold();
        }
#endif
    }

    private async UniTask HoldDelay(CancellationToken cancellationToken)
    {
        try
        {
            await UniTask.Delay(300, cancellationToken: cancellationToken);
            isDragging = true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Canceled Drag");
        }
    }

    private void DetectTouch(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            cardPanel.gameObject.SetActive(true);
            cardPanel.CurrentSolider = gameObject;
            oldPosition = transform.position;

            holdCancellationTokenSource = new CancellationTokenSource();
            HoldDelay(holdCancellationTokenSource.Token).Forget();
        }
    }

    private void Drag(Vector3 newPosition)
    {
        if (!isDragging) return;

        transform.position = newPosition;
    }

    private void StopDrag()
    {
        isDragging = false;
    }

    private void StopHold()
    {
        FindNewCell();
        if (holdCancellationTokenSource != null)
        {
            holdCancellationTokenSource.Cancel();
            holdCancellationTokenSource.Dispose();
            holdCancellationTokenSource = null;
        }
    }

    private void FindNewCell()
    {
        Cell overlappingCell = cellFinder.FindCellUnderCard(transform);
        if (overlappingCell != null)
        {
            SnapToCell(overlappingCell);
        }
        else
        {
            gameObject.transform.position = oldPosition;
        }
    }

    private void SnapToCell(Cell cell)
    {
        transform.position = cell.transform.position;
        transform.SetParent(cell.transform);
    }
}
