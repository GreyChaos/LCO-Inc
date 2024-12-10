using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public SpriteRenderer backGround;
    public float darknessSpeed;
    public float timePerDark;
    public float currentTime = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DarkenTheWorld(darknessSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime == 3000){
            CustomerManager.CustomerCap = 8;
        }
        if(currentTime == 4500){
            CustomerManager.CustomerCap = 10;
        }
        if(currentTime == 5000){
            CustomerManager.CustomerCap = 12;
        }
        if(currentTime == 6000){
            CustomerManager.CustomerCap = 6;
        }

    }

    IEnumerator DarkenTheWorld(float time){
        while (true)
        {
            if(backGround.color.r < .2f){
                CustomerManager.CustomerCap = 0;
            break;
            }
            yield return new WaitForSeconds(time);
            currentTime += timePerDark;
            backGround.color = new Color(backGround.color.r - darknessSpeed, backGround.color.g - darknessSpeed, backGround.color.b - darknessSpeed);
        }
    }
}
