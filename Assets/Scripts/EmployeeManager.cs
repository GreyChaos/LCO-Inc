using UnityEngine;
using UnityEngine.Tilemaps;

public class EmployeeManager : MonoBehaviour
{

    // A temp class that just spawns an employee
    public Tilemap tilemap;
    public TileBase walkable;
    Customer servingCustomer;
    public GameObject standingSpot;
    public GameObject RegisterStandingSpot;
    public Machines machines;
    public CustomerManager customerManager;
    public Coffee HeldCoffee;
    public SpriteRenderer CoffeeSprite;
    public Pathfinding pathfinding;

    public Employee Employee;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        Employee.enabled = true;
        Employee.tilemap = tilemap;
        Employee.walkable = walkable;
        Employee.standingSpot = standingSpot;
        Employee.RegisterStandingSpot = RegisterStandingSpot;
        Employee.customerManager = customerManager;
        Employee.pathfinding = pathfinding;
        Instantiate(Employee, new Vector3(-.154f, .919f,0), Quaternion.identity);
    }
}
