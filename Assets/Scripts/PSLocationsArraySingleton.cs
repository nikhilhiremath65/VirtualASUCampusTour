using System.Collections;
using System.Collections.Generic;

public class PSLocationArraySingleton
{

    private static PSLocationArraySingleton instance = null;
    private static Dictionary<string, ArrayList> toursLocations;
    private static Dictionary<string, int> toursLocationsUpdateStatusDictionary;
    private static bool tourLocationsObjectStatus = false;
   

    public static PSLocationArraySingleton Instance()
    {
       
            if (instance == null)
                instance = new PSLocationArraySingleton();

            return instance;
        
    }

    private PSLocationArraySingleton()
    {
    }

    public void setToursLocationsDictionary(Dictionary<string, ArrayList> toursLocationsObject)
    {
        toursLocations = toursLocationsObject;
    }

    public Dictionary<string, ArrayList> getToursLocationDictionary()
    {
        return toursLocations;
    }

    public void setToursLocationsObjectStatus(bool status)
    {
        tourLocationsObjectStatus = status;
    }

    public bool getToursLocationsObjectStatus()
    {
        return tourLocationsObjectStatus;
    }

    public void setToursLocationsUpdateStatusDictionary(Dictionary<string, int> toursLocationsStatusUpdateObject)
    {
        toursLocationsUpdateStatusDictionary = toursLocationsStatusUpdateObject;
    }

    public Dictionary<string, int> getToursLocationsUpdateStatusDictionary()
    {
        return toursLocationsUpdateStatusDictionary;
    }

}