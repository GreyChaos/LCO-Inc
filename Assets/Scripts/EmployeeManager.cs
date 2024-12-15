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
    Employee newEmployee;
    public GameObject canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        canvas.SetActive(true);
    }

    public void DisableUI(){
        canvas.SetActive(false);
    }


    bool employeeToggle = true;
    public void toggleEmployee(){
        employeeToggle = !employeeToggle;
        if(employeeToggle){
            Destroy(newEmployee.gameObject);
        }else{
            newEmployee = Instantiate(Employee, new Vector3(-.154f, .919f,0), Quaternion.identity);
            newEmployee.enabled = true;
            newEmployee.tilemap = tilemap;
            newEmployee.walkable = walkable;
            newEmployee.standingSpot = standingSpot;
            newEmployee.RegisterStandingSpot = RegisterStandingSpot;
            newEmployee.customerManager = customerManager;
            newEmployee.pathfinding = pathfinding;
            
        }
    }
}
