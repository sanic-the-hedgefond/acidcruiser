using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotate : MonoBehaviour
{
    public float x_speed;
    public float y_speed;

    public float z_rot;
    public float y_rot;
    public float x_rot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x += x_speed;
        pos.y -= y_speed;
        transform.position = pos;
        transform.Rotate(new Vector3(x_rot, y_rot, z_rot));
    }
}
