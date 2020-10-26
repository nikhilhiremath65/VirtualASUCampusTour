public class DB_Details
{

    private string URL = "https://asu-ar-app.firebaseio.com/";
    private string buildingDBName = "buildingDataBase";
    private string scheduleDBName = "scheduleDataBase";
    private string sharedDBName = "sharedLocationsDataBase";
    private string tourDBName = "tourDataBase";

    public string getDBUrl()
    {
        return URL;
    }

    public string getBuildingDBname()
    {
        return buildingDBName;
    }

    public string getScheduleDBName()
    {
        return scheduleDBName;
    }

    public string getTourDBName()
    {
        return tourDBName;
    }

    public string getSharedDBName()
    {
        return sharedDBName;
    }
}
