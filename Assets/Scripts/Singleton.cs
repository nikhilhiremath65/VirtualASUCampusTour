public class Singleton
{
    private static Singleton _instance;

    private string TourName;

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
}
