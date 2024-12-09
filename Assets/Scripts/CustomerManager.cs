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
            newCustomer.enterTarget = customerWaitingPoint;
            newCustomer.exitTarget = customerLeavingPoint;
            newCustomer.tilemap = tilemap;
            newCustomer.pathfinding = pathFinding;
            newCustomer.CustomerManager = this;
            Customers.Add(newCustomer);
        }
    }

    public void destroyCustomer(Customer customer){
        Customers.Remove(customer);
        Destroy(customer.gameObject);
        Debug.Log(Customers.Count);
    }

}
