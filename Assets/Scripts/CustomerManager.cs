using System.Collections.Generic;
using System.Linq;
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

    List<Customer> customersWaiting = new();
    public static List<Customer> Customers = new();

    // How many customers are allowed to be spawned in at a time.
    public int customerCap = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            newCustomer.exitTarget = customerLeavingPoint;
            newCustomer.tilemap = tilemap;
            newCustomer.pathfinding = pathFinding;
            newCustomer.CustomerManager = this;
            Customers.Add(newCustomer);
            customersWaiting.Add(newCustomer);
        }
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
