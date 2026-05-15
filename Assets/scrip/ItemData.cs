using UnityEngine;

public enum InventoryItemType
{
    Key,
    Clue,
    Battery,
    CardKey
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public string itemName;

    [TextArea(3, 8)]
    public string itemDescription;

    public Sprite itemIcon;

    public InventoryItemType itemType;
}