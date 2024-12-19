using TMPro;
using UnityEngine;

public class EndOfDayManager : MonoBehaviour
{
    // Allows attaching TMP_Text objects for each result in unity inspector.
    [SerializeField] TMP_Text endOfDayProfits;
    [SerializeField] TMP_Text endOfDayBills;
    [SerializeField] TMP_Text endOfDayNetProfits;
    [SerializeField] TMP_Text endOfDayMoneyBalance;
    [SerializeField] TMP_Text endOfDayCustomerSatisfaction;
    
    // Allows connecting Rent System through unity inspector.
    [SerializeField] RentSystem rentSystem;

    // Creates local variables to track values printed in the recap.
    private float endProfitsFloat;
    private float endBillsFloat;
    private float endNetProfitsFloat;
    private float endMoneyBalanceFloat;

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
        endProfitsFloat = MoneyManager.GetDailyProfits();
        endOfDayProfits.text = " Today's Profit: $" + endProfitsFloat.ToString("F2");
    }

    private void UpdateBills()
    {
        endBillsFloat = rentSystem.GetRent();
        MoneyManager.Expense(endBillsFloat);
        endOfDayBills.text = " Today's Bills: $" + endBillsFloat.ToString("F2");
    }

    private void UpdateNetProfits()
    {
        endNetProfitsFloat = endProfitsFloat - endBillsFloat;
        endOfDayNetProfits.text = " Net Profits: $" + endNetProfitsFloat.ToString("F2");
    }

    private void UpdateMoneyBalance()
    {
        endMoneyBalanceFloat = MoneyManager.GetMoneyCount();
        endOfDayMoneyBalance.text = " Ending Bank Balance: $" + endMoneyBalanceFloat.ToString("F2");
    }

    private void UpdateCustomerSatisfaction()
    {
        endOfDayCustomerSatisfaction.text = " Today's Customer Happiness: ";
    }
}
