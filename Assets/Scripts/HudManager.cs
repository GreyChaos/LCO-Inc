using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;

    void Start()
    {
        UpdateHud();
    }

   public void UpdateHud()
    {
        moneyText.text = "$" + MoneyManager.GetMoneyCount().ToString("F2");
    }
}