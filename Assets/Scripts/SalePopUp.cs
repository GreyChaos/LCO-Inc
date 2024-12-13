using TMPro;
using UnityEngine;

public class SalePopUp : MonoBehaviour
{
    [SerializeField] TMP_Text saleProfits;

    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    public static void CreatePopUp(Vector3 position, float saleProfit)
    {
        
    }
}