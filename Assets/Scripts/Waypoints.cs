using System;

[Serializable]
public class Waypoints
{

	public string BuildingName;
	public string Location;
	public string Time;


	public Waypoints(){}

	public Waypoints(string buildingname, string location, string time){
		BuildingName = buildingname;
		Location = location;
		Time = time;
	}
}
