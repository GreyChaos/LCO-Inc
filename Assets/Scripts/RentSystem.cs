using UnityEngine;

public class RentSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float dailyRent;
    // Sets how many days rent is free to start players off.
    private const int DAYS_FREE_RENT = 5;

    public float GetRent()
    {
        // Sets and returns a free daily rent if the date is before given number of free starting days.
        if (TimeManager.GetDay() <= DAYS_FREE_RENT && TimeManager.GetMonth() == TimeManager.STARTING_MONTH && TimeManager.GetYear() == TimeManager.STARTING_YEAR)
        {
            dailyRent = 0;
            return dailyRent;
        }
        // Uodates and returns the daily rent.
        else
            UpdateRent();
            return dailyRent;
    }

    // Would like to incorporate store size, date, other features into this hence the separate method. For now it just returns rent of $100.
    void UpdateRent()
    {
        dailyRent = 100;
    }

}
