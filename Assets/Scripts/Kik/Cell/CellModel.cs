using System;

using UnityEngine;


[Serializable]
public class CellModel
{
    [field: SerializeField] public int CellId { get; set; }
    [field: SerializeField] public bool IsEmpty { get; set; }
}
