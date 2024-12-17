using System.Collections;
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
    public Coffee coffeeInMachine;
    Coffee startingCoffee;
    public float machineTimer = 2f;
    float timerProgress = 0;
    bool machineReady = false;
    GameObject infoCircle;


    void Start(){
        if(transform.Find("Info Circle") != null){
            infoCircle = transform.Find("Info Circle").gameObject;
            infoCircle.SetActive(false);
        } 
        lowTileMap = GameObject.Find("LowTileMap").GetComponent<Tilemap>();
        standingSpotObject = transform.Find("Standing Spot");
        standingSpotTile = lowTileMap.WorldToCell(standingSpotObject.position);
        standingSpot = lowTileMap.CellToWorld(new Vector3Int(standingSpotTile.x, standingSpotTile.y, 0));
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
            if(coffeeType.PreReqCoffee == null){
                startingCoffee = coffeeType;
            }
        }
        if(infoCircle != null)
            infoCircle.transform.Find("Colored Circle").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Progress", 0);
    }

    public Coffee UseMachine(Vector3 employeePosition, Coffee coffee){
        // Make sure user is at the machine
        if(!PlayerOnMachine(employeePosition)){
            return coffee;
        }
        // Set coffee to cup if no coffee, or at a trashcan
        if((coffee == null || this == trashCan)){
            foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == null){
                    return coffeeType;
                }
            }
        }
        // Check what coffee uses this machine, if a valid recipe is found, start making the coffee only if machine is empty
        if(coffeeInMachine == null){
            foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == coffee && coffeeType.Addition == this){
                    if(infoCircle != null){
                        infoCircle.SetActive(true);
                        infoCircle.transform.Find("Coffee Icon").GetComponent<SpriteRenderer>().sprite = coffee.coffeeSprite;
                    }
                    coffeeInMachine = coffeeType;
                    // Start timer to get coffee ready
                    if(machineTimer > 0){
                        StartCoroutine(CoffeeWaitingTime());
                        return startingCoffee;
                    }else{
                        coffeeInMachine = null;
                        return coffeeType;
                    }
                }
            }
        }
        // Take the coffee back out of the machine
        if(machineReady){
            Coffee returnedCoffee = coffeeInMachine;
            coffeeInMachine = null;
            machineReady = false;
            foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == null){
                    infoCircle.transform.Find("Coffee Icon").GetComponent<SpriteRenderer>().sprite = coffeeType.coffeeSprite;
                }
            }
            if(infoCircle != null){
                infoCircle.transform.Find("Colored Circle").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Progress", 0);
                infoCircle.SetActive(false);
            }
            return returnedCoffee;
        }
        // Do nothing, return orignial coffee
        return coffee;
    }

    private IEnumerator CoffeeWaitingTime(){
        float elapsedTime = 0f;

        // Updates the circle with the time
        while (elapsedTime * TimeManager.GetTimeFactor() < machineTimer)
        {
            elapsedTime += Time.deltaTime * TimeManager.GetTimeFactor();
            timerProgress = elapsedTime * TimeManager.GetTimeFactor() / machineTimer;
            infoCircle.transform.Find("Colored Circle").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Progress", timerProgress);
            yield return null;
        }
        infoCircle.transform.Find("Coffee Icon").GetComponent<SpriteRenderer>().sprite = coffeeInMachine.coffeeSprite;
        machineReady = true;
        
    }

    bool PlayerOnMachine(Vector3 employeePosition){
        if(employeePosition == standingSpot)
        {
            return true;
        }
        return false;
    }
}
