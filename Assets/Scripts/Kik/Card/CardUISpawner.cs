using System.Collections.Generic;

using UnityEngine;

public class CardUISpawner : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> Cards { get; private set; }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            GameObject randomCard = Cards[Random.Range(0, Cards.Count)];
            Instantiate(randomCard, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        }
    }
}
