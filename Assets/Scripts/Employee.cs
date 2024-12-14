using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Employee : MonoBehaviour
{
    // Okay so this class is a mess, and needs to be organized and split up.
    public Tilemap tilemap;
    public TileBase walkable;
    Customer servingCustomer;
    public GameObject standingSpot;
    public GameObject RegisterStandingSpot;
    public Machines machines;
    public CustomerManager customerManager;
    public Coffee HeldCoffee;
    public SpriteRenderer CoffeeSprite;
    

    // Path Finding Stuff
    public float moveSpeed = 3f;
    public Pathfinding pathfinding;
    private List<Vector3Int> path;
    private int currentPathIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoffeeSprite = transform.Find("Employee Coffee").GetComponent<SpriteRenderer>();
        ResetCoffee();
    }

    void Update() {
        // Checks who the nearest customer is, so you can serve them
        servingCustomer = FindClosestCustomer();
        ServeCustomer();
        if (path != null && path.Count > 0)
        {
            MoveAlongPath();
        }
    }


    List<Coffee> coffeeChain = new();
    bool makingCoffee = false;
    Coffee coffeeCurrentlyBeingMade;
    void ServeCustomer(){
        if(servingCustomer == null){
            return;
        }
        // Get the Customers Order
        if(!makingCoffee){
            makingCoffee = true;
            coffeeChain.Clear();
            coffeeCurrentlyBeingMade = servingCustomer.coffeOrder;
            if(coffeeCurrentlyBeingMade != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
            if(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee != null){
                coffeeChain.Add(coffeeCurrentlyBeingMade.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee.PreReqCoffee);
            }else{
                return;
            }
        }
        // Check if current order is that
        if(HeldCoffee == coffeeCurrentlyBeingMade){
            // Serve Customer
            setTarget(RegisterStandingSpot.transform.position);
            standingSpot = RegisterStandingSpot;
        }else{
            foreach(Coffee coffee in coffeeChain){
                if(coffee == HeldCoffee){
                    machines = coffeeChain[coffeeChain.IndexOf(coffee) - 1].Addition;
                    Vector3 machineSpot = machines.transform.Find("Standing Spot").gameObject.transform.position;
                    setTarget(machineSpot);
                    Coffee newCoffee = machines.UseMachine(transform.position, HeldCoffee);
                    if(newCoffee != null){
                        HeldCoffee = newCoffee;
                        CoffeeSprite.sprite = newCoffee.coffeeSprite;
                        makingCoffee = false;
                    }
                    break;
                }
            }
        }
    }

    // Does what it says, finds and returns the nearest customer.
    Customer FindClosestCustomer()
    {
        Customer closestCustomer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Customer customer in CustomerManager.Customers)
        {
            if (customer == null) continue;

            float distance = Vector3.Distance(RegisterStandingSpot.transform.position, customer.transform.position);

            if (distance > 2) continue;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCustomer = customer;
            }

            
        }

        return closestCustomer;
    }

    public void ResetCoffee(){
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
                if(coffeeType.PreReqCoffee == null){
                    HeldCoffee = coffeeType;
                    CoffeeSprite.sprite = HeldCoffee.coffeeSprite;
                }
        }
    }

    // So this one, is the one that actually says hey the customer wants this order, do you have it? And if so removes it and tells the customer to go away, but it also a part of the players pathfinding.
    public bool setTarget(Vector3 target){
        Vector3Int start = tilemap.WorldToCell(transform.position);
        Vector3Int goal = tilemap.WorldToCell(target);
        if(start == goal){
            if(!servingCustomer.OrderRecieved){
                if(standingSpot == RegisterStandingSpot){
                    if(servingCustomer.coffeOrder == HeldCoffee){
                        ResetCoffee();
                        servingCustomer.OrderRecieved = true;
                    }
                    if(servingCustomer.coffeOrder != HeldCoffee){
                        makingCoffee = false;
                    }
                }
            }   
            return false;
        }
        path = pathfinding.FindPath(start, goal);
        // Start moving if path is found
        if (path != null && path.Count > 0)
        {
            currentPathIndex = 0;
        }
        return true;
    }

    // More pathfinding stuff
    void MoveAlongPath()
    {
        if (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = tilemap.CellToWorld(path[currentPathIndex]);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                currentPathIndex++;
            }
        }
    }
}
