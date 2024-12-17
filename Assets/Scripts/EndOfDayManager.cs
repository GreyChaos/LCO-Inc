using TMPro;
using UnityEngine;

public class EndOfDayManager : MonoBehaviour
{
    [SerializeField] TMP_Text endOfDayRevenue;
    [SerializeField] TMP_Text endOfDayCosts;
    [SerializeField] TMP_Text endOfDayProfits;
    [SerializeField] TMP_Text endOfDayBills;
    [SerializeField] TMP_Text endOfDayNetProfits;
    [SerializeField] TMP_Text endOfDayMoneyBalance;
    [SerializeField] TMP_Text endOfDayCustomerSatisfaction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateProfits();
        UpdateBills();
        UpdateNetProfits();
        UpdateMoneyBalance();
        UpdateCustomerSatisfaction();
        CustomerManager.Customers.Clear();
        TimeManager.UpdateTimeFactor(1f);
    }

    private void UpdateProfits()
    {
        endOfDayProfits.text = " Today's Profit: $" + MoneyManager.GetDailyProfits().ToString("F2");
    }

    private void UpdateBills()
    {
        endOfDayBills.text = " Today's Bills: $" + MoneyManager.GetDailyBills().ToString("F2");
    }

    private void UpdateNetProfits()
    {
        endOfDayNetProfits.text = " Net Profits: $" + (MoneyManager.GetDailyProfits() - MoneyManager.GetDailyBills()).ToString("F2");
    }

    private void UpdateMoneyBalance()
    {
        endOfDayMoneyBalance.text = " Ending Bank Balance: $" + MoneyManager.GetMoneyCount().ToString("F2");
    }

    private void UpdateCustomerSatisfaction()
    {
        endOfDayCustomerSatisfaction.text = " Today's Customer Happiness: ";
    }
}
