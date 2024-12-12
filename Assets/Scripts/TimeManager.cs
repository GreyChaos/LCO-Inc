using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class TimeManager : MonoBehaviour
{
    // Creates HudManager object and static value for currentTime. Allows TimeManager to update the Hud text whenever time passes.
    [SerializeField] HudManager hudManager;
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
    [SerializeField] private float realSecondsToGameMinutes = 0.1f;

    // Public boolean to turn time on/off.
    private static Boolean timeRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created.
    void Start()
    {
        timeRunning = true;

        StartCoroutine("UpdateTime");
    }

    IEnumerator UpdateTime()
    {
        while(timeRunning)
        {
            // Waits to update every number of real seconds equivalent to one game minute (currently set 1:10).
            yield return new WaitForSecondsRealtime(realSecondsToGameMinutes);
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
        }
    }

    private void HourChanged()
    {
        minute = 0;
        hour++;
    }

    private void TimeOfDayChange()
    {
        morningTime = !morningTime;
    }

    private void EndOfDay()
    {
        timeRunning = false;
        SceneManager.LoadScene(1);
    }

    public static void StartNextDay()
    {
        hour = openingHour;
        morningTime = true;
        day++;
    }

    private void MonthChanged()
    {
        day = 1;
        month++;
    }

    private void YearChanged()
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
    private void updateDate()
    {
        currentDate = month + "/" + day + "/" + year;
    }
    // Returns a string value of the current date, used in HUD.
    public static string GetDateString()
    {
        return currentDate;
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
