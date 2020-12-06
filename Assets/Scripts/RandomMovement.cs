using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float y_move;
    public float y_time;
    public float y_phase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.y += Mathf.Sin(Time.time * y_time + y_phase) * y_move;
        transform.position = pos;
    }
}
