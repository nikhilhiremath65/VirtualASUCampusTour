public class Singleton
{
    private static Singleton _instance;

    private string TourName;
    private string ScheduleName;
    private string UserEmail;
    private string UserRole;
    private string PSTourNameEdit;

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
}
