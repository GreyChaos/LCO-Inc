using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.Events;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject salePopUpPrefab;

    // Creates listener that triggers on sales, passing through the profit value.
    void Start()
    {
        MoneyManager.saleEventProfit.AddListener(GenerateSalePopUp);
    }

    // Generates salePopUpPrefab gameobject into the scene.
    private void GenerateSalePopUp(float popUpAmount)
    {
        Vector3 salePopUpPos = new Vector3(CustomerManager.registerPosition.x, CustomerManager.registerPosition.y + 0.5f, 0f);
        GameObject salePopUP = Instantiate(salePopUpPrefab, salePopUpPos, Quaternion.identity);
        salePopUP.GetComponentInChildren<TMP_Text>().text = "$" + popUpAmount.ToString("F2");
    }
}
