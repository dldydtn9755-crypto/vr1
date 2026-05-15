using System.Collections.Generic;
using UnityEngine;

public class HorrorInventoryManager : MonoBehaviour
{
    public static HorrorInventoryManager Instance;

    [Header("보유 아이템")]
    public List<ItemData> keyItems = new List<ItemData>();
    public List<ItemData> clueItems = new List<ItemData>();
    public List<ItemData> cardKeyItems = new List<ItemData>();

    [Header("배터리 개수")]
    public int batteryCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogWarning("추가할 ItemData가 없습니다.");
            return;
        }

        switch (itemData.itemType)
        {
            case InventoryItemType.Key:
                keyItems.Add(itemData);
                Debug.Log("열쇠 획득: " + itemData.itemName);
                break;

            case InventoryItemType.Clue:
                clueItems.Add(itemData);
                Debug.Log("단서 획득: " + itemData.itemName);
                break;

            case InventoryItemType.Battery:
                batteryCount++;
                Debug.Log("배터리 획득. 현재 개수: " + batteryCount);
                break;

            case InventoryItemType.CardKey:
                cardKeyItems.Add(itemData);
                Debug.Log("카드키 획득: " + itemData.itemName);
                break;
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (ItemData item in keyItems)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }

        foreach (ItemData item in cardKeyItems)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasKey(string keyName)
    {
        foreach (ItemData item in keyItems)
        {
            if (item.itemName == keyName)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasCardKey(string cardKeyName)
    {
        foreach (ItemData item in cardKeyItems)
        {
            if (item.itemName == cardKeyName)
            {
                return true;
            }
        }

        return false;
    }

    public bool UseBattery()
    {
        if (batteryCount <= 0)
        {
            Debug.Log("배터리가 없습니다.");
            return false;
        }

        batteryCount--;
        Debug.Log("배터리 사용. 남은 개수: " + batteryCount);
        return true;
    }
}