using System.Collections.Generic;
using UnityEngine;

public class Coffee : MonoBehaviour
{
    public Sprite coffeeSprite;

    public Coffee PreReqCoffee;

    public Machines Addition;

    static List<GameObject> coffeeGameObjects;
    public static List<Coffee> CoffeeObjects;

    void Start(){
        CoffeeObjects = new List<Coffee>();
        coffeeGameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Coffee"));
        foreach (GameObject coffeeObject in coffeeGameObjects)
        {
            Coffee coffeeComponent = coffeeObject.GetComponent<Coffee>();
            CoffeeObjects.Add(coffeeComponent);
        }
    }

    public static Coffee generateRandomCoffee(){
        return CoffeeObjects[Random.Range(1, CoffeeObjects.Count)];;
    }

}
