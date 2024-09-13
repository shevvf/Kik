using System;

using UnityEngine;

[Serializable]
public class Card
{
    [field: SerializeField] public GameObject CardReference { get; private set; }
    [field: SerializeField] public CardModel CardModel { get; private set; }
}
