public class ScheduleNameTransfer
{

    private static ScheduleNameTransfer instance = null;

    private string Name;

    public static ScheduleNameTransfer Instance
    {
        get
        {
            if (instance == null)
                instance = new ScheduleNameTransfer();

            return instance;
        }
    }

    private ScheduleNameTransfer()
    {
    }

    public void setScheduleName(string name)
    {
        Name = name;
    }

    public string getScheduleName()
    {
        return Name;
    }
}