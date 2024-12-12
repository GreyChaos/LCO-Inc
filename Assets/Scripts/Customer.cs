using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Customer : MonoBehaviour
{

    // Mostly Sprite stuff that could be simplified.
    public Tilemap tilemap;
    public TileBase walkable;
    public bool OrderRecieved = false;
    public Vector3 enterTarget;
    public Vector3 exitTarget;
    public Vector3 spawningPoint;
    public SpriteRenderer spriteRenderer;
    public Sprite[] customerSprites;
    public GameObject infoCircle;
    public CustomerManager CustomerManager;

    // Path Finding Stuff
    public float moveSpeed = 3f;
    public Pathfinding pathfinding;
    private List<Vector3Int> path;
    private int currentPathIndex = 0;
    public Coffee coffeOrder;
    public bool StayingCustomer;
    Vector3 originalExit;

    // A value that goes from 1 (Full) to 0 (Empty)
    float patience = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalExit = new Vector3(exitTarget.x, exitTarget.y, 0);;
        spriteRenderer.sprite = customerSprites[Random.Range(0, customerSprites.Length)];
        GenerateOrder();
    }

    bool OrderFinished = false;
    
    // Update is called once per frame
    void Update()
    {
        
        if(!OrderRecieved && patience > 0){
            waitInLine();
            if(waitingOnSpot()){
                CustomerManager.checkIfFirstInLine(this);
                patience -= .0005f;
            }
            infoCircle.transform.Find("Colored Circle").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Progress", patience);
        }else if (OrderRecieved){
            // Makes sure this section is only run once after the order is done.
            if(!OrderFinished){
                Destroy(infoCircle.transform.Find("Info Circle Sprite").gameObject);
                Destroy(infoCircle.transform.Find("Colored Circle").gameObject);
                infoCircle.transform.Find("Coffee Icon").transform.localPosition = new Vector3(0.134f,0.572f,0);
                infoCircle.transform.Find("Coffee Icon").transform.localScale = new Vector3(0.2f,0.2f,1);
                coffeOrder.SellCoffee();
                OrderFinished = true;
            }
            
            if(enterTarget != null){
                CustomerManager.updateCustomerQueueing(this);
            }
            if(StayingCustomer){
                sitDownAndWait();
            }
            Pathfinding(exitTarget);
        // Order Failed
        }else{
            StayingCustomer = false;
            customerLeaves();
        }
    }

    void customerLeaves(){
        CustomerManager.updateCustomerQueueing(this);
        Pathfinding(exitTarget);
    }

    Vector3 seatSpot;
    bool hasSeat = false;
    void sitDownAndWait(){
        if(!hasSeat){
            seatSpot = CustomerManager.FindValidSeat();
            if(seatSpot == new Vector3(0,0,0)){
                StayingCustomer = false;
                return;
            }
            hasSeat = true;
        }
        if(seatSpot != new Vector3(0,0,0)){
            exitTarget = seatSpot;
            StartCoroutine(StartTimer());
        }
    }

    bool isWaiting;
    IEnumerator StartTimer()
    {
        if (isWaiting)
            yield break;

        isWaiting = true;

        float randomTime = Random.Range(3f, 8f);
        yield return new WaitForSeconds(randomTime);
        CustomerManager.ReturnSeat(exitTarget);
        exitTarget = originalExit;
        StayingCustomer = false;
    }
    void waitInLine(){
        Pathfinding(enterTarget);
    }

    // When the customer is ready for a new order, this is called. Making a random order and setting the icon above their head
    void GenerateOrder(){
        coffeOrder = Coffee.generateRandomCoffee();
        infoCircle.transform.Find("Coffee Icon").GetComponent<SpriteRenderer>().sprite = coffeOrder.coffeeSprite;
    }

    bool waitingOnSpot(){
        Vector3Int start = tilemap.WorldToCell(transform.position);
        Vector3Int goal = tilemap.WorldToCell(enterTarget);
        return start == goal && goal == tilemap.WorldToCell(enterTarget);
    }

    // Path finding, it's a mess and barely works. Needs to be rewritten. The entire system, not just this part.
    void Pathfinding(Vector3 target){
        Vector3Int start = tilemap.WorldToCell(transform.position);
        Vector3Int goal = tilemap.WorldToCell(target);
        if(start == goal && goal == tilemap.WorldToCell(exitTarget) && !StayingCustomer){
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
