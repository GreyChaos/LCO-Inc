using UnityEngine;

public class Bean : MonoBehaviour
{
    private float timer = 0f;
    private const float interval = 0.01f; 

    void Update()
    {
        timer += Time.deltaTime; 

        if (timer >= interval)
        {
            MoveBean(); 
            timer = 0f; 
        }
    }

    void MoveBean()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, 0);

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }
}
