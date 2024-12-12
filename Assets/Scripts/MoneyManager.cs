using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    // Creates moneyCount variable to track player money. Assigns arbitrary starting amount of $100, editable in inspector for testing purposes.
    [SerializeField] private static float moneyCount = 100f;

    // Allows MoneyManager to call HudManager updates.
    [SerializeField] HudManager hudManager;

    // Creates float variables to track total Revenues, Costs, Profits, Money, and profit per sale.
    private float totalRevenues = 0f;
    private float totalCosts = 0f;
    private float profit = 0f;
    private static float dailyProfits = 0f;
    private float totalProfits = 0f;

    // Records a sale, taking in the price the item is sold at and the cost of ingredients for the item. Adds the profit to totalProfits and moneyCount variables.
    public void Sale(float salePrice, float ingredientCost)
    {
        totalRevenues += salePrice;
        totalCosts -= ingredientCost;

        profit = salePrice - ingredientCost;

        dailyProfits += profit;
        totalProfits += profit;
        moneyCount += profit;

        hudManager.UpdateHud();
    }

    // Subtracts a given amount from the total moneyCount variable. 
    public void Expense(float expenseAmount)
    {
        moneyCount -= expenseAmount;

        hudManager.UpdateHud();
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

    // Resets the daily profits value, called by Time Manager at start of new day.
    public static void ResetDailyProfits()
    {
        dailyProfits = 0f;
    }
}
