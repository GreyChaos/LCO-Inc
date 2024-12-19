using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    public Sprite coffeeSprite;
    public Coffee PreReqCoffee;
    public Machines Addition;
    static List<GameObject> coffeeGameObjects;
    public static List<Coffee> CoffeeObjects;
    public static List<Coffee> CoffeeObjectsOrderableOnly = new();
    public bool Orderable = true;
    public float salePrice;
    float expenseCost;
    [SerializeField] NewUnlock newUnlock;

    // Unlock System Stuff, add items to the enum to make them conditions
    public enum UnlockCondition{
        None,
        CustomerServed,
        DayReached,
        TotalProfit
    }

    public UnlockCondition unlockCondition;
    public int unlockAmount;

    void Start(){
        CoffeeObjects = new List<Coffee>();
        coffeeGameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coffee"));
        foreach (GameObject coffeeObject in coffeeGameObjects)
        {
            Coffee coffeeComponent = coffeeObject.GetComponent<Coffee>();
            if(coffeeComponent.Orderable){
                CoffeeObjectsOrderableOnly.Add(coffeeComponent);
            }
            CoffeeObjects.Add(coffeeComponent);
        }
    }

    // Needs to run twice to ensure everything is set.
    int updateCoffeeAmount = 2;
    int coffeeUpdatesDone = 0;
    void Update(){
        if (coffeeUpdatesDone < updateCoffeeAmount){
            UpdateCoffeeCost();
            coffeeUpdatesDone++;
        }
        EnableOrder();
    }

    // Checks to see if conditions have been met, and if so unlock that coffee.
    void EnableOrder(){
        if(Orderable) return;
        if(!CoffeeObjectsOrderableOnly.Contains(PreReqCoffee)) return;
        switch(unlockCondition){
            case UnlockCondition.None :
                Orderable = false;
                break;
            case UnlockCondition.CustomerServed:
                if(CustomerManager.TotalCustomersServed >= unlockAmount)
                    Orderable = true;
                break;
            case UnlockCondition.DayReached:
                if(TimeManager.GetDay() >= unlockAmount)
                    Orderable = true;
                break;
            case UnlockCondition.TotalProfit:
                if(MoneyManager.GetTotalProfits() >= unlockAmount)
                    Orderable = true;
                break;
        }
        if(Orderable){
            newUnlock.gameObject.SetActive(true);
            CoffeeObjectsOrderableOnly.Add(this);
            newUnlock.EnableNewUnlock(coffeeSprite, name);
        }
    }

    public void UpdateCoffeeCost(){
        expenseCost = 0;
        expenseCost += Addition.expenseCostPerUse;
        if(PreReqCoffee != null){
            expenseCost += PreReqCoffee.expenseCost;
        }
    }

    public void SellCoffee(){
        MoneyManager.Sale(salePrice, expenseCost);
    }


    public static Coffee generateRandomCoffee(){
        return CoffeeObjectsOrderableOnly[Random.Range(0, CoffeeObjectsOrderableOnly.Count)];;
    }

}
