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

    public static Coffee generateRandomCoffee(){
        return CoffeeObjectsOrderableOnly[Random.Range(0, CoffeeObjectsOrderableOnly.Count)];;
    }

}
