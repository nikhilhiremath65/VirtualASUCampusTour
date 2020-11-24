using Models;
using System.Collections.Generic;
public class Singleton
{
    private static Singleton _instance;

    private string TourName;
    private string ScheduleName;
    private string UserEmail;
    private string UserRole;
    private string PSTourNameEdit;
    private string BuildingInfo;
    private string ARType;


    private Dictionary<string, Coordinates> sharedTourLocations;
    private bool IsDrag; 
    private bool MapMode; 

    private string UserName = "nhiremat";
    protected Singleton()
    {
        sharedTourLocations = new Dictionary<string, Coordinates>();
        MapMode = true;
    }

    public static Singleton Instance()
    {

        if (_instance == null)
        {
            _instance = new Singleton();
        }

        return _instance;
    }

    public void setTourName(string TourName)
    {
        this.TourName = TourName;
    }

    public string getTourName()
    {
        return this.TourName;
    }
    public void setScheduleName(string ScheduleName)
    {
        this.ScheduleName = ScheduleName;
    }

    public string getScheduleName()
    {
        return this.ScheduleName;
    }

    public void setUserName(string UserName)
    {
        this.UserName = UserName;
    }
    public string getUserName()
    {
        return this.UserName;
    }

    public void setUserEmail(string UserEmail)
    {
        this.UserEmail = UserEmail;
    }

    public string getUserEmail()
    {
        return this.UserEmail;
    }

    public void setUserRole(string UserRole)
    {
        this.UserRole = UserRole;
    }

    public string getUserRole()
    {
        return this.UserRole;
    }

    public string getPSTourNameEdit()
    {
        return this.PSTourNameEdit;
    }

    public void setPSTourNameEdit(string PSTourName)
    {
        this.PSTourNameEdit = PSTourName;
    }

    public Dictionary<string, Coordinates> getSharedTourLocations()
    {
        return sharedTourLocations;
    }

    public void setSharedLocation(Dictionary<string, Coordinates> sharedLocations)
    {
        sharedTourLocations = sharedLocations;
    }

    public void addSharedLocation(string name, Coordinates coordinates){
        
        sharedTourLocations[name] = coordinates;
    }

    public void setISDrag(bool IsDrag)
    {
        this.IsDrag = IsDrag;
    }

    public bool getIsDrag()
    {
        return this.IsDrag;
    }

    public void setMapMode(bool MapMode)
    {
        this.MapMode = MapMode;
    }

    public bool isMapMode()
    {
        return this.MapMode;
    }
    public void setBuildingInfo(string buildingInfo)
    {
        this.BuildingInfo = buildingInfo;
    }

    public string getBuildingInfo()
    {
        return this.BuildingInfo;
    }
    public void setARType(string ARType)
    {
        this.ARType = ARType;
    }

    public string getARType()
    {
        return this.ARType;
    }

}
