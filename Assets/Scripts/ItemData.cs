using System;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Deployable,
}

public enum ConsumableType
{
    SizeMultiplier,
    JumpPowerMultiplier,
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 
    public string displayName;
    public ItemType type;
    public Sprite icon;

    [Header("Consumable")]
    public float duration;
    public ItemDataConsumable[] consumables;
}
