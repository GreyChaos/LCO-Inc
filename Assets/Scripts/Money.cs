using UnityEngine;

public class Money : MonoBehaviour
{
    // Creates moneyCount variable to track player money. Assigns arbitrary starting amount of $100, editable in inspector for testing purposes.
    [SerializeField] private float moneyCount = 100;

    // Creates float variables to track total Revenues, Costs, Profits, Money, and profit per sale.
    private float totalRevenues;
    private float totalCosts;
    private float profit;
    private float totalProfits;

    // Records a sale, taking in the price the item is sold at and the cost of ingredients for the item. Adds the profit to totalProfits and moneyCount variables.
    void Sale(float salePrice, float ingredientCost)
    {
        totalRevenues += salePrice;
        totalCosts -= ingredientCost;

        profit = salePrice - ingredientCost;

        totalProfits += profit;
        moneyCount += profit;
    }

    // Subtracts a given amount from the total moneyCount variable. 
    void Expense(float expenseAmount)
    {
        moneyCount -= expenseAmount;
    }
}
