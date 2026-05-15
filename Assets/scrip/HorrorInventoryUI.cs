using System.Text;
using TMPro;
using UnityEngine;

public class HorrorInventoryUI : MonoBehaviour
{
    public GameObject detailPanel;
    public TextMeshProUGUI detailTitleText;
    public TextMeshProUGUI detailContentText;

    [Header("Battery Use")]
    public BatteryManager batteryManager;
    public float batteryRechargeAmount = 40f;

    public void ShowKey()
    {
        detailPanel.SetActive(true);
        detailTitleText.text = "Key";

        StringBuilder sb = new StringBuilder();

        foreach (var item in HorrorInventoryManager.Instance.keyItems)
        {
            sb.AppendLine("- " + item.itemName);
        }

        detailContentText.text = sb.ToString();
    }

    public void ShowClue()
    {
        detailPanel.SetActive(true);
        detailTitleText.text = "Clue";

        StringBuilder sb = new StringBuilder();

        foreach (var item in HorrorInventoryManager.Instance.clueItems)
        {
            sb.AppendLine(item.itemDescription);
            sb.AppendLine();
        }

        detailContentText.text = sb.ToString();
    }

    public void ShowBattery()
    {
        detailPanel.SetActive(true);
        detailTitleText.text = "Battery";

        detailContentText.text = "보유 개수 : " + HorrorInventoryManager.Instance.batteryCount
                               + "\n\n사용 버튼을 누르면 캠코더 배터리가 회복됩니다.";
    }

    public void ShowCardKey()
    {
        detailPanel.SetActive(true);
        detailTitleText.text = "CardKey";

        StringBuilder sb = new StringBuilder();

        foreach (var item in HorrorInventoryManager.Instance.cardKeyItems)
        {
            sb.AppendLine("- " + item.itemName);
        }

        detailContentText.text = sb.ToString();
    }

    public void UseBatteryButton()
    {
        if (HorrorInventoryManager.Instance == null)
        {
            Debug.LogWarning("HorrorInventoryManager가 없습니다.");
            return;
        }

        if (batteryManager == null)
        {
            Debug.LogWarning("BatteryManager가 연결되지 않았습니다.");
            return;
        }

        bool used = HorrorInventoryManager.Instance.UseBattery();

        if (!used)
        {
            detailPanel.SetActive(true);
            detailTitleText.text = "Battery";
            detailContentText.text = "보유 개수 : 0\n\n사용할 배터리가 없습니다.";
            return;
        }

        batteryManager.RechargeBattery(batteryRechargeAmount);

        detailPanel.SetActive(true);
        detailTitleText.text = "Battery";
        detailContentText.text = "보유 개수 : " + HorrorInventoryManager.Instance.batteryCount
                               + "\n\n배터리를 사용했습니다.";
    }

    public void CloseDetail()
    {
        detailPanel.SetActive(false);
    }
}