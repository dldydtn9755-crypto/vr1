using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private List<string> items = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            Debug.Log(itemName + " 을(를) 인벤토리에 추가했습니다.");
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
            Debug.Log(itemName + " 을(를) 인벤토리에서 제거했습니다.");
        }
    }

    public void ShowInventory()
    {
        string allItems = "현재 인벤토리: ";

        if (items.Count == 0)
        {
            allItems += "비어 있음";
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                allItems += items[i];

                if (i < items.Count - 1)
                {
                    allItems += ", ";
                }
            }
        }

        Debug.Log(allItems);
    }
}