using System.Collections;

public class PSLocationArraySingleton
{

    private static PSLocationArraySingleton instance = null;

    private static ArrayList Tours;
    private static int updateStatus = 0;

    public static PSLocationArraySingleton Instance
    {
        get
        {
            if (instance == null)
                instance = new PSLocationArraySingleton();

            return instance;
        }
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
}