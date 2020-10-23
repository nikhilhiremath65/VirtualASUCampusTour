public class Singleton
{
    private static Singleton _instance;

    private string TourName;
    private string ScheduleName;

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
}
