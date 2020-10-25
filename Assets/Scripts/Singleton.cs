public class Singleton
{
    private static Singleton _instance;

    private string TourName;
    private string ScheduleName;
    private string DeptTourName;

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

    public void SetTourName(string TourName)
    {
        this.TourName = TourName;
    }

    public string GetTourName()
    {
        return this.TourName;
    }
    public void SetScheduleName(string ScheduleName)
    {
        this.ScheduleName = ScheduleName;
    }

    public string GetScheduleName()
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
    public string GetDeptTourName()
    {
        return this.DeptTourName;
    }
}
