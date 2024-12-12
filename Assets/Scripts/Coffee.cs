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
    private MoneyManager moneyManager;

    void Awake(){
        
    }

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
        moneyManager = FindFirstObjectByType<MoneyManager>();
    }

    // Needs to run twice to ensure everything is set.
    int updateCoffeeAmount = 2;
    int coffeeUpdatesDone = 0;
    void Update(){
        if (coffeeUpdatesDone < updateCoffeeAmount){
            UpdateCoffeeCost();
            coffeeUpdatesDone++;
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
        moneyManager.Sale(salePrice, expenseCost);
    }


    public static Coffee generateRandomCoffee(){
        return CoffeeObjectsOrderableOnly[Random.Range(0, CoffeeObjectsOrderableOnly.Count)];;
    }

}
