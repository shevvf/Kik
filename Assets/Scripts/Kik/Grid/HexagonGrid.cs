using UnityEngine;

public class HexagonGrid : MonoBehaviour
{
    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private int gridRadius = 4;
    [SerializeField] private bool randomColor = false;

    private float hexWidth;
    private float hexHeight;

    private void Start()
    {
        hexWidth = hexPrefab.transform.localScale.x;
        hexHeight = hexPrefab.transform.localScale.y;

        CreateHexagonGrid();
    }

    private void CreateHexagonGrid()
    {
        for (int q = -gridRadius; q <= gridRadius; q++)
        {
            for (int r = Mathf.Max(-gridRadius, -q - gridRadius); r <= Mathf.Min(gridRadius, -q + gridRadius); r++)
            {
                Vector3 position = HexToWorldPosition(q, r);
                GameObject hex = Instantiate(hexPrefab, position, Quaternion.identity);
                hex.transform.SetParent(transform);

                if (!randomColor) continue;

                SpriteRenderer hexRenderer = hex.GetComponent<SpriteRenderer>();
                hexRenderer.color = GetRandomColor();
            }
        }
    }

    private Vector3 HexToWorldPosition(int q, int r)
    {
        float x = hexWidth * 0.75f * q;
        float y = hexHeight * (Mathf.Sqrt(3) / 2) * (r + q / 2f);
        return new Vector3(x, y, 0);
    }

    Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        return new Color(r, g, b, 1f);
    }
}