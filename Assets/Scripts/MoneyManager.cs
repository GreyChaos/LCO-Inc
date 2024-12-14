using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    // Creates moneyCount variable to track player money. Assigns arbitrary starting amount of $100, editable in inspector for testing purposes.
    [SerializeField] private static float moneyCount = 100f;

    // Allows MoneyManager to call HudManager updates.
    [SerializeField] HudManager hudManager;

    // Importing sound effects in the unity inspector.
    [SerializeField] private AudioClip saleSoundEffect;

    // Creates float variables to track total Revenues, Costs, Profits, Money, and profit per sale.
    private float totalRevenues = 0f;
    private float totalCosts = 0f;
    private float profit = 0f;
    private static float dailyProfits = 0f;
    private float totalProfits = 0f;
    
    [SerializeField] private GameObject salePopUpPrefab;

    // Records a sale, taking in the price the item is sold at and the cost of ingredients for the item. Adds the profit to totalProfits and moneyCount variables.
    public void Sale(float salePrice, float ingredientCost)
    {
        totalRevenues += salePrice;
        totalCosts -= ingredientCost;

        profit = salePrice - ingredientCost;

        dailyProfits += profit;
        totalProfits += profit;
        moneyCount += profit;

        GenerateSalePopUp(profit);
        SoundEffectManager.instance.PlayAudioClip(saleSoundEffect, transform, .5f);
        hudManager.UpdateHud();
    }

    // Generates a SalePopUp gameobject with text equivalent to $(Float Amount Input) at slightly above the register position on the y-axis.
    private void GenerateSalePopUp(float popUpAmount)
    {
        Vector3 salePopUpPos = new Vector3(CustomerManager.registerPosition.x, CustomerManager.registerPosition.y + 0.5f, 0f);
        GameObject salePopUP = Instantiate(salePopUpPrefab, salePopUpPos, Quaternion.identity);
        salePopUP.GetComponentInChildren<TMP_Text>().text = "$" + popUpAmount.ToString("F2");
    }

    // Subtracts a given amount from the total moneyCount variable. 
    public void Expense(float expenseAmount)
    {
        moneyCount -= expenseAmount;

        hudManager.UpdateHud();
    }

    // Creates little pop ups displaying profit over the register whenever a sale is made.
    void PopUpSaleText()
    {

    }

    public static float GetMoneyCount()
    {
        return moneyCount;
    }

    // Returns float of daily profits value.
    public static float GetDailyProfits()
    {
        return dailyProfits;
    }

    public static float GetDailyBills()
    {
        return 0.00f;
    }

    // Resets the daily profits value, called by Time Manager at start of new day.
    public static void ResetDailyProfits()
    {
        dailyProfits = 0f;
    }
}
