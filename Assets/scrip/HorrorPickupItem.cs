using UnityEngine;

public class HorrorPickupItem : MonoBehaviour
{
    [Header("획득할 아이템 데이터")]
    public ItemData itemData;

    [Header("획득 후 오브젝트 삭제 여부")]
    public bool destroyAfterPickup = true;

    public void PickUp()
    {
        if (itemData == null)
        {
            Debug.LogWarning(gameObject.name + "에 ItemData가 연결되지 않았습니다.");
            return;
        }

        if (HorrorInventoryManager.Instance == null)
        {
            Debug.LogWarning("씬에 HorrorInventoryManager가 없습니다.");
            return;
        }

        HorrorInventoryManager.Instance.AddItem(itemData);

        Debug.Log("아이템 획득 완료: " + itemData.itemName);

        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
    }
}