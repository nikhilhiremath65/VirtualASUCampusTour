using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDropDown : MonoBehaviour
{
    public Dropdown HoursDropdown;
    public Dropdown MinutesDropdown;

    int[] hoursValues;
    int[] minutesValues;

    void Start()
    {
        PopulateList();
    }

    void PopulateList()
    {
        List<string> hours = new List<string>() { "hour" };
        for (int i = 0; i < 24; i++)
        {
            hours.Add(i.ToString("00"));
        }

        List<string> minutes = new List<string>() { "minute" };
        for (int i = 0; i < 12; i++)
        {
            minutes.Add((i*5).ToString("00"));
        }

        HoursDropdown.AddOptions(hours);
        MinutesDropdown.AddOptions(minutes);
    }
}
