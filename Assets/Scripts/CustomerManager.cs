using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    public static Customer[] customers;
    public GameObject[] waitingSpots;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        customers = FindObjectsByType<Customer>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // When a customer leaves their spot in the queue, this moves all the positions around in order. So 1 2 3 4 is now 2 3 4 1
    public void UpdateWaitingSpots()
    {
    Vector3[] positions = new Vector3[waitingSpots.Length];
    for (int i = 0; i < waitingSpots.Length; i++)
    {
        positions[i] = waitingSpots[i].transform.position;
    }
    for (int i = 1; i < waitingSpots.Length; i++)
    {
        waitingSpots[i].transform.position = positions[i - 1];
    }
    waitingSpots[0].transform.position = positions[waitingSpots.Length - 1];
}



}
