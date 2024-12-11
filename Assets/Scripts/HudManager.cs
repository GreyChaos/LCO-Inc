using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text dateText;

    void Start()
    {
        UpdateHud();
    }

   public void UpdateHud()
    {
        moneyText.text = "$" + MoneyManager.GetMoneyCount().ToString("F2");
        timeText.text = TimeManager.GetTimeString();
        dateText.text = TimeManager.GetDateString();
    }
}