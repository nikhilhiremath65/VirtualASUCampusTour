public class DeptTournametransfer
{

    private static DeptTournametransfer instance = null;

    private string Name;

    public static DeptTournametransfer Instance
    {
        get
        {
            if (instance == null)
                instance = new DeptTournametransfer();

            return instance;
        }
    }

    private DeptTournametransfer()
    {
    }

    public void setDeptTourName(string name)
    {
        Name = name;
    }

    public string getDeptTourName()
    {
        return Name;
    }
}