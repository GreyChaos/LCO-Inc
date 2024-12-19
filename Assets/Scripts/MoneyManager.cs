using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    // Creates moneyCount variable to track player money. Assigns arbitrary starting amount of $100, editable in inspector for testing purposes.
    [SerializeField] private static float moneyCount = 100f;

    // Creates float variables to track total Revenues, Costs, Profits, Money, and profit per sale.
    private static float totalRevenues = 0f;
    private static float totalCosts = 0f;
    private static float profit = 0f;
    private static float totalProfits = 0f;

    private static float dailyRevenues = 0f;
    private static float dailyCosts = 0f;
    private static float dailyProfits = 0f;

    // Creates static events for sales and expenses. Used by Hudmanager to know when money is changed so the Hud can be updated, and by SoundEffectManager and PopUpManager to know when to instantiate respective gameObjects.
    public static UnityEvent saleEvent;
    public static UnityEvent<float> saleEventProfit;
    public static UnityEvent expenseEvent;
    
    void Awake()
    {
        if (saleEvent == null)
            saleEvent = new UnityEvent();
        if (saleEventProfit == null)
            saleEventProfit = new UnityEvent<float>();
        if (expenseEvent == null)
            expenseEvent = new UnityEvent();
    }
    
    void Start()
    {
        dailyRevenues = 0;
        dailyCosts = 0;
        dailyProfits = 0;
    }
    // Records a sale, taking in the price the item is sold at and the cost of ingredients for the item. Adds the profit to totalProfits and moneyCount variables.
    public static void Sale(float salePrice, float ingredientCost)
    {
        dailyRevenues += salePrice;
        dailyCosts += ingredientCost;

        totalRevenues += salePrice;
        totalCosts -= ingredientCost;

        profit = salePrice - ingredientCost;

        dailyProfits += profit;
        totalProfits += profit;
        moneyCount += profit;
        
        // Invokes moneyChange event, HudManager and SoundEffectManager listen for first event and PopUpManager listens for second event (passing through profit value).
        saleEvent.Invoke();
        saleEventProfit.Invoke(profit);
    }

    // Subtracts a given amount from the total moneyCount variable. 
    public static void Expense(float expenseAmount)
    {
        moneyCount -= expenseAmount;

        // Invokes moneyChange event, hudManager listens for this to update hud.
        expenseEvent.Invoke();
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

    public static float GetTotalProfits(){
        return totalProfits;
    }
}
