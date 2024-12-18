using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;
using System.IO;
using UnityEngine.AI;
using UnityEditor.Animations;
using UnityEditor;

public class TimeManager : MonoBehaviour
{
    // Creates HudManager object and static value for currentTime. Allows TimeManager to update the Hud text whenever time passes.
    [SerializeField] HudManager hudManager;
    [SerializeField] Light2D sun;
    private static string currentTime;
    private static string currentDate;

    // Creates const variables for starting day, month, and year.
    public const int STARTING_DAY = 1;
    public const int STARTING_MONTH = 1;
    public const int STARTING_YEAR = 1980;

    // Integers for year, month, day.
    private static int day = STARTING_DAY;
    private static int month = STARTING_MONTH;
    private static int year = STARTING_YEAR;

    // Integers for hour, minute, boolean to check if AM or PM, and static integer for adjusting openingHour.
    public static int openingHour = 9;
    public static int closingHour = 17;
    private static int hour = openingHour;
    private static Boolean morningTime = true;
    private static int minute = 0;

    // Determines the number of game minutes that pass per second of real time.
    private float realSecondsToGameMinutes = 0.5f;

    // Creates timeFactor variable, initially equal to 1.
    private static float timeFactor = 1f;

    // Sets speed the sun sets.
    [SerializeField] float sunSetSpeed = .0014f;
    
    [SerializeField] SeasonManager seasonManager;

    // Keeps track of the Coroutine, incase it needs broken
    private Coroutine timeCoroutine;
    private float previousTimeFactor = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        timeCoroutine = StartCoroutine("UpdateTime");
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            timeFactor = .000001f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeFactor = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            timeFactor = 2f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            timeFactor = 3f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            timeFactor = 10f;
        }

        // Restart the coroutine if timeFactor changes
        if (Mathf.Abs(timeFactor - previousTimeFactor) > Mathf.Epsilon)
        {
            previousTimeFactor = timeFactor;
            StopCoroutine(timeCoroutine);
            timeCoroutine = StartCoroutine(UpdateTime());
        }
    }

    IEnumerator UpdateTime()
    {
        while(true)
        {
            if (timeFactor == 0)
                yield return new WaitUntil(() => timeFactor != 0);
            else
                yield return new WaitForSecondsRealtime(realSecondsToGameMinutes / timeFactor);

            minute++;
            
            // There's probably a better way to do this, but these if statements essentially increment time at each variable (minute, hour, day, month, year).
            if (minute == 60)
                HourChanged();
            if (hour == 12 && minute == 0)
                TimeOfDayChange();
            if ((hour >= closingHour && GameObject.FindWithTag("Customer") == null) || hour == 24)
                EndOfDay();
            if (day == 30)
                MonthChanged();
            if (month == 12)
                YearChanged();

            // Updates the currentTime and currentDate string values every game minute
            updateTime();
            updateDate();

            // Calls to update the HUD every game minute.
            hudManager.UpdateHud();

            //Updates the suns brightness
            UpdateSun();
        }
    }

    // Updates the sun positioning.
    void UpdateSun(){
        if(hour >= 15){
            sun.color = new Color(sun.color.r - sunSetSpeed, sun.color.g - sunSetSpeed, sun.color.b - sunSetSpeed);
        }
    }

    void HourChanged()
    {
        minute = 0;
        hour++;
    }

    void TimeOfDayChange()
    {
        morningTime = !morningTime;
    }

    void EndOfDay()
    {
        SceneManager.LoadScene(2);
    }

    public static void StartNextDay()
    {
        hour = openingHour;
        morningTime = true;
        day++;
    }

    // Changes the month on day 31, and calls the season manager to Update the season if applicable.
    void MonthChanged()
    {
        day = 1;
        month++;

        // If month is divisible by 3, calls on the season manager to update the season.
        if (month % 3 == 0)
            seasonManager.UpdateSeason(month);

    }

    void YearChanged()
    {
        month = 1;
        year++;
    }

    // Updates currentTime string.
    void updateTime()
    {
        if(hour > 12)
            currentTime = (hour - 12).ToString();
        else
            currentTime = hour.ToString();

        currentTime += ":" + minute.ToString("00");

        if (morningTime)
            currentTime += "am";
        else
            currentTime += "pm";
    }
    // Returns a string value of time, used in HUD.
    public static string GetTimeString()
    {
        return currentTime;
    }

    // Updates currentDate string.
    void updateDate()
    {
        currentDate = month + "/" + day + "/" + year;
    }

    // Updates the Time Factor value (Bigger number => Time goes faster)
    public static void UpdateTimeFactor(float inputTimeFactor)
    {
        timeFactor = inputTimeFactor;
    }

    // Returns the time factor value.
    public static float GetTimeFactor()
    {
        return timeFactor;
    }
    // Returns a string value of the current date, used in HUD.
    public static string GetDateString()
    {
        return currentDate;
    }

    // Returns current year.
    public static int GetYear()
    {
        return year;
    }

    // Returns current day, used to unlock things
    public static int GetMonth(){
        return month;
    }
    // Returns current day, used to unlock things
    public static int GetDay(){
        return day;
    }

    // Returns integer value of current hour, used by CustomerManager to determine if Customers should spawn.
    public static int GetHour()
    {
        return hour;
    }
    // Returns integer value of current minute, not sure if needed but included just in case.
    public static int GetMinute()
    {
        return minute;
    }
}
