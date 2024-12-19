using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class HudManager : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text dateText;

    private UnityAction updateAction;

    void Start()
    {
        UpdateHud();

        MoneyManager.expenseEvent.AddListener(UpdateHud);
    }

   public void UpdateHud()
    {
        moneyText.text = "$" + MoneyManager.GetMoneyCount().ToString("F2");
        timeText.text = TimeManager.GetTimeString();
        dateText.text = TimeManager.GetDateString();
    }
}