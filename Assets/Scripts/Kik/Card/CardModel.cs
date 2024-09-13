using System;

using UnityEngine;

[Serializable]
public class CardModel
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
}
