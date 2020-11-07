using UnityEngine;

public class TourLocation : MonoBehaviour
{

    public string Name;
    public string Latitute;
    public string Longitude;
    public int index;

    public bool Drag;

    public TourLocation(string name, int index)
    {
        this.Name = name;
        this.Drag = false;
        this.index = index;
    }
}
