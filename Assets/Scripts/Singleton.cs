public class Singleton
{
    private static Singleton _instance;

    private string TourName;
    private string ScheduleName;

    private string UserName = "nhiremat";
    protected Singleton()
    {

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
        this.ScheduleName = UserName;
    }
    public string getUserName()
    {
        return this.UserName;
    }
}
