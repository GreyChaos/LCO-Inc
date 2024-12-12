using UnityEngine;

public class Machines : MonoBehaviour
{
    public bool trashCan;
    public float expenseCostPerUse = 1;

    public static void ResetCoffee(){
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == null){
                    PlayerMovement.PlayerCoffee = coffeeType;
                    PlayerMovement.PlayerCoffeeSprite.sprite = PlayerMovement.PlayerCoffee.coffeeSprite;
                }
        }
    }

    public void UseMachine(){
        if(PlayerMovement.PlayerCoffee == null || this == trashCan){
            ResetCoffee();
        }
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
            if(coffeeType.PreReqCoffee == PlayerMovement.PlayerCoffee && coffeeType.Addition == this){
                PlayerMovement.PlayerCoffee = coffeeType;
                PlayerMovement.PlayerCoffeeSprite.sprite = PlayerMovement.PlayerCoffee.coffeeSprite;
                return;
            }
        }
    }
}
