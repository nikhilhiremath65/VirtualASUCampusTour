using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 speed; 
    void Start(){
        speed = new Vector2(10, 10);
    }
    // Update is called once per frame
    void Update()
    {
        float inputX  = Input.GetAxis("Horizontal");
        float inputY  = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, 0, speed.y * inputY);

        movement *= Time.deltaTime;

        gameObject.transform.Translate(movement);
    }
}
