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

    public Coffee UseMachine(Vector3 employeePosition, Coffee coffee){
        if(!PlayerOnMachine(employeePosition)){
            return coffee;
        }
        if((coffee == null || this == trashCan)){
            foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == null){
                    return coffeeType;
                }
            }
        }
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
            if(coffeeType.PreReqCoffee == coffee && coffeeType.Addition == this){
                coffee = coffeeType;
                return coffeeType;
            }
        }
        return coffee;
    }

    bool PlayerOnMachine(Vector3 employeePosition){
        if(employeePosition == standingSpot)
        {
            return true;
        }
        return false;
    }
}
