  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToursListController : MonoBehaviour
{

    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

    ArrayList tours;
    // Start is called before the first frame update
    void Start()
    {
        tours = new ArrayList();
        for (int i = 0; i < 20; i++)
        {
            tours.Add(new Schedule("Tour " + i));
        }


        foreach (Schedule t in tours)
        {
            GameObject newSchedule = Instantiate(ListItemPrefab) as GameObject;

            ListItemController controller = newSchedule.GetComponent<ListItemController>();
            controller.Name.text = t.Name;

            newSchedule.transform.parent = ContentPanel.transform;
            newSchedule.transform.localScale = Vector3.one;


        }
    }
}
