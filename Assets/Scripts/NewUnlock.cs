using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewUnlock : MonoBehaviour
{

    [SerializeField] SpriteRenderer unlockedItem;
    [SerializeField] TextMeshProUGUI unlockedName;
    public GameObject bean;
    List<Bean> beans = new();
    private float rainTimer = 0f;  // Timer to track the time for bean rain interval
    private float rainInterval = 0.1f;  // Time between each bean drop

    private float timer = 0f; // Timer to track the time for popup close
    private const float interval = 5f; // Time until menu closes

    void Start()
    {
        // Initial setup if needed
    }

    void Update()
    {
        unlockedItem.transform.eulerAngles += new Vector3(0, .5f, 0);

        // Only run rainBeans every 0.2 seconds (5 times per second)
        rainTimer += Time.deltaTime;
        if (rainTimer >= rainInterval)
        {
            rainBeans();
            rainTimer = 0f;  // Reset the timer
        }

        timer += Time.deltaTime; 

        if (timer >= interval)
        {
            gameObject.SetActive(false);
            timer = 0f; 
        }
    }

    // Make it rain beans, but only 5 times per second
    void rainBeans()
    {
        // Instantiate the bean with a random position and random rotation
        GameObject newBean = Instantiate(bean, new Vector3(Random.Range(-8.8f, 8.8f), 6.25f, 0), Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

        // Add the Bean script to the instantiated object (if not already attached)
        if (newBean.GetComponent<Bean>() == null)
        {
            newBean.AddComponent<Bean>();
        }
    }

    public void EnableNewUnlock(Sprite sprite, string name){
        unlockedItem.sprite = sprite;
        unlockedName.SetText(name);
    }
}



