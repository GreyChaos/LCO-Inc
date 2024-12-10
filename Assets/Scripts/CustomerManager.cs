using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomerManager : MonoBehaviour
{

    [SerializeField] Customer defaultCustomer;
    [SerializeField] GameObject customerSpawningPoint;
    [SerializeField] GameObject customerLeavingPoint;
    [SerializeField] GameObject customerWaitingPoint;

    [SerializeField] Pathfinding pathFinding;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap midTilemap;

    List<Customer> customersWaiting = new();
    public static List<Customer> Customers = new();
    List<Vector3> Seats = new();

    // How many customers are allowed to be spawned in at a time.
    public int customerCap = 1;

    public Tile stoolTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        spawnCustomer();
    }

    // checks to see if a customer can be spawned, and spawns them at the starting point
    void spawnCustomer(){
        if(Customers.Count < customerCap){
            Customer newCustomer = Instantiate(defaultCustomer, customerSpawningPoint.transform.position, Quaternion.identity);
            newCustomer.enterTarget = Instantiate(customerWaitingPoint, waitingSpotPosition(customerWaitingPoint), Quaternion.identity);
            newCustomer.exitTarget = Instantiate(customerLeavingPoint, customerLeavingPoint.transform.position, Quaternion.identity);
            newCustomer.tilemap = tilemap;
            newCustomer.pathfinding = pathFinding;
            newCustomer.CustomerManager = this;
            newCustomer.StayingCustomer = Random.Range(0, 2) == 0;
            newCustomer.StayingCustomer = true;
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

    Vector3 waitingSpotPosition(GameObject customerWaitingPoint){
        Vector3 newPosition = customerWaitingPoint.transform.position;

        Vector3 localPosition = new Vector3(newPosition.x, newPosition.y, 0);
        Vector3Int cellPosition = tilemap.WorldToCell(localPosition);

        cellPosition.x -= (1 * customersWaiting.Count);
        newPosition = tilemap.CellToWorld(cellPosition);

        return newPosition;
    }

    Vector3 waitingSpotPositionUpdate(GameObject customerWaitingPoint){
        Vector3 newPosition = customerWaitingPoint.transform.position;

        Vector3 localPosition = new Vector3(newPosition.x, newPosition.y, 0);
        Vector3Int cellPosition = tilemap.WorldToCell(localPosition);

        cellPosition.x += 1;
        newPosition = tilemap.CellToWorld(cellPosition);

        return newPosition;
    }

    public void destroyCustomer(Customer customer){
        Customers.Remove(customer);
        Destroy(customer.enterTarget);
        Destroy(customer.gameObject);
    }

    public void updateCustomerQueueing(Customer finishedCustomer){
        if(!customersWaiting.Contains(finishedCustomer)){
            return;
        }
        customersWaiting.Remove(finishedCustomer);
        foreach (Customer customer in customersWaiting){
            customer.enterTarget.transform.position = waitingSpotPositionUpdate(customer.enterTarget);
        }

    }

}
