using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private static float totalProfits = 0f;

    private static float dailyRevenues = 0f;
    private static float dailyCosts = 0f;
    private static float dailyProfits = 0f;
    
    [SerializeField] private GameObject salePopUpPrefab;
    
    public void Start()
    {
        dailyRevenues = 0;
        dailyCosts = 0;
        dailyProfits = 0;
    }
    // Records a sale, taking in the price the item is sold at and the cost of ingredients for the item. Adds the profit to totalProfits and moneyCount variables.
    public void Sale(float salePrice, float ingredientCost)
    {
        dailyRevenues += salePrice;
        dailyCosts += ingredientCost;

        totalRevenues += salePrice;
        totalCosts -= ingredientCost;

        profit = salePrice - ingredientCost;

        dailyProfits += profit;
        totalProfits += profit;
        moneyCount += profit;

        GenerateSalePopUp(profit);
        SoundEffectManager.Instance.PlayAudioClip(saleSoundEffect, transform, .1f);
        
        if (SceneManager.GetActiveScene().name == "GameplayScene")
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

        // Checks if the active scene is the gameplay scene before updating the Hud, prevents call to update during End of Day Recap.
        if (SceneManager.GetActiveScene().name == "GameplayScene")
            hudManager.UpdateHud();
    }

    public static void EndOfDayAdjustment(float expenseAmount)
    {
        moneyCount -= expenseAmount;
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
