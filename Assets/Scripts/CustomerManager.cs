using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerManager : MonoBehaviour
{

    [SerializeField] Customer defaultCustomer;
    [SerializeField] Pathfinding pathFinding;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap midTilemap;
    [SerializeField] Tilemap highTilemapInside;

    List<Customer> customersWaiting = new();
    public static List<Customer> Customers = new();
    List<Vector3> Seats = new();
    List<Vector3> spawnTiles = new();
    Vector3 registerWaitingSpot;

    // How many customers are allowed to be spawned in at a time.
    public int customerCap = 6;

    public Tile stoolTile;
    public Tile spawnTile;
    public Tile register;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find stool pos
        BoundsInt bounds = midTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = midTilemap.GetTile(position);
                if(tile == null){
                    continue;
                }
                if(tile == stoolTile){
                    // Mid tile -3.5, .25 = Low tile -3.5, -.05
                    Vector3 midLayerCords = midTilemap.CellToWorld(position);
                    Seats.Add(new Vector3(midLayerCords.x, midLayerCords.y - .2f, 0));
                }
            }
        }
        // Find Spawn/Exit pos
        bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(position);
                if(tile == null){
                    continue;
                }
                if(tile == spawnTile){
                    Vector3 layerCords = tilemap.CellToWorld(position);
                    spawnTiles.Add(new Vector3(layerCords.x, layerCords.y, 0));
                }
            }
        }
        // Find Register Waiting spot
        bounds = highTilemapInside.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                TileBase tile = highTilemapInside.GetTile(position);
                if(tile == null){
                    continue;
                }
                if(tile == register){
                    Vector3 layerCords = highTilemapInside.CellToWorld(position);
                    Vector3Int tileCords = tilemap.WorldToCell(layerCords);
                    // -3, -2 is the ground tile in front of the register. The Customer side
                    Vector3Int convertedCords = new Vector3Int(tileCords.x - 3, tileCords.y - 2, 0);
                    registerWaitingSpot = tilemap.CellToWorld(convertedCords);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnCustomer();
    }

    // checks to see if a customer can be spawned, and spawns them at the starting point
    void spawnCustomer(){
        if((Customers.Count < customerCap) && (TimeManager.GetHour() < TimeManager.closingHour)){

            // Random Spawn
            Vector3 position = spawnTiles[Random.Range(0, spawnTiles.Count)];
            Vector3 spawningPoint = position;
            // Random Exit
            position = spawnTiles[Random.Range(0, spawnTiles.Count)];
            Vector3 exitTarget = position;

            Customer newCustomer = Instantiate(defaultCustomer, spawningPoint, Quaternion.identity);
            newCustomer.enterTarget = waitingSpotPosition(registerWaitingSpot);
            newCustomer.exitTarget = exitTarget;
            newCustomer.spawningPoint = spawningPoint;
            newCustomer.tilemap = tilemap;
            newCustomer.pathfinding = pathFinding;
            newCustomer.CustomerManager = this;
            newCustomer.StayingCustomer = Random.Range(0, 2) == 0;
            Customers.Add(newCustomer);
            customersWaiting.Add(newCustomer);
        }
    }

    public Vector3 FindValidSeat(){
        if(Seats.Count == 0){
            return new Vector3(0,0,0);
        }
        int seatNumber = Random.Range(0, Seats.Count);
        Vector3 temp = new Vector3(Seats[seatNumber].x, Seats[seatNumber].y, 0);
        Seats.RemoveAt(seatNumber);
        return temp;
    }

    public void ReturnSeat(Vector3 seat){
        Seats.Add(seat);
    }

    Vector3 waitingSpotPosition(Vector3 customerWaitingPoint){
        Vector3 newPosition = customerWaitingPoint;

        Vector3 localPosition = new Vector3(newPosition.x, newPosition.y, 0);
        Vector3Int cellPosition = tilemap.WorldToCell(localPosition);

        cellPosition.x -= (1 * customersWaiting.Count);
        newPosition = tilemap.CellToWorld(cellPosition);

        return newPosition;
    }

    Vector3 waitingSpotPositionUpdate(Vector3 customerWaitingPoint, Customer finishedCustomer){
        if(finishedCustomer.gameObject.transform.position.x < customerWaitingPoint.x){
            return customerWaitingPoint;
        }
        Vector3 newPosition = customerWaitingPoint;

        Vector3 localPosition = new Vector3(newPosition.x, newPosition.y, 0);
        Vector3Int cellPosition = tilemap.WorldToCell(localPosition);

        cellPosition.x += 1;
        newPosition = tilemap.CellToWorld(cellPosition);

        return newPosition;
    }

    public void destroyCustomer(Customer customer){
        Customers.Remove(customer);
        Destroy(customer.gameObject);
    }

    public void updateCustomerQueueing(Customer finishedCustomer){
        if(!customersWaiting.Contains(finishedCustomer)){
            return;
        }
        customersWaiting.Remove(finishedCustomer);
        foreach (Customer customer in customersWaiting){
            customer.enterTarget = waitingSpotPositionUpdate(customer.enterTarget, finishedCustomer);
        }

    }

    public void checkIfFirstInLine(Customer customerChecking){
        foreach (Customer customer in customersWaiting){
            // Check first to see if that customer spot is behind, if so ignore
            if(customer.enterTarget.x < customerChecking.enterTarget.x){
                continue;
            }
            // Check if spot in front of them actually has somebody on it, or about to be on it
            if(Vector3.Distance(customer.transform.position, customer.enterTarget) < 1f){
                continue;
            }
            // Finally, steal the spot, swapping spots with the customer
            Vector3 tempPosition = customerChecking.enterTarget;
            customerChecking.enterTarget = customer.enterTarget;
            customer.enterTarget = tempPosition;
            return;

        }
    }
}


