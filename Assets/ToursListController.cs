using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourListController : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    ArrayList schedules;
    // Start is called before the first frame update
    void Start()
    {
        schedules = new ArrayList();
        for (int i = 0; i < 20; i++)
        {
            schedules.Add(new Schedule("Tour" + i));
        }


        foreach (Schedule s in schedules)
        {
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = s.Name;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;


        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
