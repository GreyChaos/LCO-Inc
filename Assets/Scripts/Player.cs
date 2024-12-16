using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    // Okay so this class is a mess, and needs to be organized and split up.
    public Tilemap tilemap;
    public TileBase walkable;
    Vector3 MousePOS;
    public bool mouseMove = false;
    Customer nearbyCustomer;
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
        CoffeeSprite = transform.Find("Player Coffee").GetComponent<SpriteRenderer>();
        foreach (Coffee coffeeType in Coffee.CoffeeObjects){
            if(coffeeType.PreReqCoffee == null){
                HeldCoffee = coffeeType;
            }
        }
    }

    void Update() {
        // Checks who the nearest customer is, so you can serve them
        nearbyCustomer = FindClosestCustomer();
        // Mouse move can be ignored at the moment as it's disabled.
        if(mouseMove){
            MousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePOS = new Vector3(MousePOS.x , MousePOS.y, 0);
            if (Input.GetMouseButton(0) && ConvertWorldPOSToTile(MousePOS) == walkable){
                Vector3Int start = tilemap.WorldToCell(transform.position);
                Vector3Int goal = tilemap.WorldToCell(MousePOS);
                path = pathfinding.FindPath(start, goal);
                // Start moving if path is found
                if (path != null && path.Count > 0)
                {
                    currentPathIndex = 0;
                }
            }
        }
        if (path != null && path.Count > 0)
        {
            MoveAlongPath();
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

            float distance = Vector3.Distance(transform.position, customer.transform.position);

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
    public bool setTarget(GameObject target){
        
        Vector3Int start = tilemap.WorldToCell(transform.position);
        Vector3Int goal = tilemap.WorldToCell(target.transform.position);
        if(start == goal){
            if(!nearbyCustomer.OrderRecieved){
                if(standingSpot == RegisterStandingSpot){
                    if(nearbyCustomer.coffeOrder == HeldCoffee){
                        ResetCoffee();
                        nearbyCustomer.OrderRecieved = true;
                        CustomerManager.TotalCustomersServed += 1;
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

    // The tiles in the grid are not the same cords as objects, so this just converts a place, like where you click, to a tile spot.

    TileBase ConvertWorldPOSToTile(Vector3 worldPosition){
        Vector3 localPosition = new Vector3(worldPosition.x, worldPosition.y, 0);
        Vector3Int cellPosition = tilemap.WorldToCell(localPosition);
        TileBase tile = tilemap.GetTile(cellPosition);
        return tile;
    }
}
