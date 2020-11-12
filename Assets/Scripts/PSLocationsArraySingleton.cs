using System.Collections;
using System.Collections.Generic;

public class PSLocationArraySingleton
{

    private static PSLocationArraySingleton instance = null;

    private static Dictionary<string, ArrayList> toursLocations;

    private static bool tourLocationsObjectStatus = false;

    private static ArrayList Tours;
    private static int updateStatus = 0;

    public static PSLocationArraySingleton Instance()
    {
       
            if (instance == null)
                instance = new PSLocationArraySingleton();

            return instance;
        
    }

    private PSLocationArraySingleton()
    {
    }

    public void setLocations(ArrayList tours)
    {
        Tours = tours;
    }

    public ArrayList getLocations()
    {
        return Tours;
    }

    public void setUpdateStatus(int update)
    {
        updateStatus = update;
    }

    public int getUpdateStatus()
    {
        return updateStatus;
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
}