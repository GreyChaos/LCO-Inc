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

public class TimeManager : MonoBehaviour
{
    // Creates HudManager object and static value for currentTime. Allows TimeManager to update the Hud text whenever time passes.
    [SerializeField] HudManager hudManager;
    [SerializeField] Light2D sun;
    private static string currentTime;
    private static string currentDate;

    // Integers for year, month, day.
    private int year = 1980;
    private int month = 1;
    private static int day = 1;

    // Integers for hour, minute, boolean to check if AM or PM, and static integer for adjusting openingHour.
    public static int openingHour = 9;
    public static int closingHour = 17;
    private static int hour = openingHour;
    private static Boolean morningTime = true;
    private static int minute = 0;

    // Determines the number of game minutes that pass per second of real time.
    [SerializeField] private float realSecondsToGameMinutes = 0.5f;

    // Creates timeFactor variable, initially equal to 1.
    private static float timeFactor = 1f;
    // float used to record previous time factor in case the game is paused.
    private static float prevTimeFactor = 1f;

    // Sets speed the sun sets.
    [SerializeField] float sunSetSpeed = .0014f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        StartCoroutine("UpdateTime");
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
            if ((hour > closingHour && GameObject.FindWithTag("Customer") == null) || hour == 24)
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

    // Allows for "pausing" the game by adjusting the timeFactor to 0. Allows for easier integration than using timeRunning boolean.
    public static void toggleTimePause(bool gamePaused)
    {
        if (gamePaused)
        {
            prevTimeFactor = timeFactor;
            timeFactor = 0;
        }
        else
            timeFactor = prevTimeFactor;
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

    void MonthChanged()
    {
        day = 1;
        month++;
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

    // Returns current day, used to unlock things
    public static int getDay(){
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
