
using UnityEngine;

public class MoveToObject : MonoBehaviour
{

    public GameObject standingSpot;
    public PlayerMovement player;
    Machines machine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        machine = GetComponentInChildren<Machines>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When clicked a player walks to a thing, if clicked again is toggles whatever option that is for the coffee, so milk adds milk.
    void OnMouseDown(){
        if(player.standingSpot == standingSpot){
            machine.UseMachine();
        }
        if(player.setTarget(standingSpot)){
            player.standingSpot = standingSpot;
        }
    }
}
