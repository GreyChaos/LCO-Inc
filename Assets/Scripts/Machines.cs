using UnityEngine;
using UnityEngine.Tilemaps;

public class Machines : MonoBehaviour
{
    public bool trashCan;
    public float expenseCostPerUse = 1;
    Tilemap lowTileMap;
    Transform standingSpotObject;
    Vector3Int standingSpotTile;
    Vector3 standingSpot;
    GameObject player;

    void Start(){
        lowTileMap = GameObject.Find("LowTileMap").GetComponent<Tilemap>();
        standingSpotObject = transform.Find("Standing Spot");
        standingSpotTile = lowTileMap.WorldToCell(standingSpotObject.position);
        standingSpot = lowTileMap.CellToWorld(new Vector3Int(standingSpotTile.x, standingSpotTile.y, 0));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public static void ResetCoffee(){
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == null){
                    PlayerMovement.PlayerCoffee = coffeeType;
                    PlayerMovement.PlayerCoffeeSprite.sprite = PlayerMovement.PlayerCoffee.coffeeSprite;
                }
        }
    }

    public void UseMachine(){
        if(!PlayerOnMachine()){
            return;
        }
        if((PlayerMovement.PlayerCoffee == null || this == trashCan)){
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

    bool PlayerOnMachine(){
        if(player.transform.position == standingSpot)
        {
            return true;
        }
        return false;
    }
}
