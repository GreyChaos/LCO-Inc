using UnityEngine;

public class Coffee : MonoBehaviour
{

    public Sprite coffeeSprite;
    public Sprite coffeeMilkSprite;
    public Sprite defaultSprite;
    public SpriteRenderer playerSprite;

    public enum CoffeeOption
    {
        HasCoffee,
        HasMilk,

        Trash
    }

    public bool hasCoffee;
    public bool hasMilk;

    // Resets the Coffee in the players hand back to nothing
    public void ResetCoffee(){
        hasCoffee = false;
        hasMilk = false;
        playerSprite.sprite = defaultSprite;
    }

    // Toggles between having coffee, milk, and whatever else we add
    public void ToggleOption(CoffeeOption option)
    {
        switch (option)
        {
            case CoffeeOption.Trash:
                ResetCoffee();
                break;
            case CoffeeOption.HasCoffee:
                hasCoffee = true;
                playerSprite.sprite = coffeeSprite;
                break;

            case CoffeeOption.HasMilk:
                if(hasCoffee){
                    hasMilk = true;
                    playerSprite.sprite = coffeeMilkSprite;
                    break;
                }
            break;
        }
    }
}
