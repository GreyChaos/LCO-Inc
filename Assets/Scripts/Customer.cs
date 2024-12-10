using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Customer : MonoBehaviour
{

    // Mostly Sprite stuff that could be simplified.
    public Tilemap tilemap;
    public TileBase walkable;
    public bool OrderComplete = false;
    public GameObject enterTarget;
    public GameObject exitTarget;
    public SpriteRenderer spriteRenderer;
    public Sprite[] customerSprites;
    public SpriteRenderer customerWantSprite;
    public Sprite CoffeeBlackIcon;
    public Sprite CoffeeMilkIcon;
    public CustomerManager CustomerManager;

    // Path Finding Stuff
    public float moveSpeed = 3f;
    public Pathfinding pathfinding;
    private List<Vector3Int> path;
    private int currentPathIndex = 0;

    // Types of Coffee we offer
    public enum CoffeeOption
    {
        CoffeeBlack,
        CoffeeWithMilk
    }
    public CoffeeOption order;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = customerSprites[Random.Range(0, customerSprites.Length)];
        GenerateOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if(!OrderComplete){
            waitInLine();
            
        }else{
            customerWantSprite.transform.localPosition = new Vector3(0.134f,0.572f,0);
            customerWantSprite.transform.localScale = new Vector3(0.2f,0.2f,1);
            if(enterTarget != null){
                CustomerManager.updateCustomerQueueing(this);
            }
            Pathfinding(exitTarget);
        }
    }

    void waitInLine(){
        Pathfinding(enterTarget);
    }

    // When the customer is ready for a new order, this is called. Making a random order and setting the icon above their head
    void GenerateOrder(){
        order = (CoffeeOption)Random.Range(0, System.Enum.GetValues(typeof(CoffeeOption)).Length);
        if(order == CoffeeOption.CoffeeWithMilk){
            customerWantSprite.sprite = CoffeeMilkIcon;
        }
        if(order == CoffeeOption.CoffeeBlack){
            customerWantSprite.sprite = CoffeeBlackIcon;
        }
    }

    // Path finding, it's a mess and barely works. Needs to be rewritten. The entire system, not just this part.
    void Pathfinding(GameObject target){
        Vector3Int start = tilemap.WorldToCell(transform.position);
        Vector3Int goal = tilemap.WorldToCell(target.transform.position);
        if(start == goal && goal == tilemap.WorldToCell(exitTarget.transform.position)){
            CustomerManager.destroyCustomer(this);
        }
        path = pathfinding.FindPath(start, goal);
        // Start moving if path is found
        if (path != null && path.Count > 0)
        {
            currentPathIndex = 0;
        }
        
        if (path != null && path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    // The 2nd part to path finding, the walking part
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
