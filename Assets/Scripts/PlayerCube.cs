using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    public GameObject cubeRendered;
    public bool isLeft;

    /*
    private void Update()
    {
        if (isLeft)
        {
            cubeRendered.transform.Rotate(Vector3.up * Mathf.Sin(Time.time * 1f) * 90f);
        }
        else
        {
            cubeRendered.transform.Rotate(Vector3.up * Mathf.Sin(Time.time * 1f + Mathf.PI/4) * -90f);
        }
    }
    */

    public void RotateY(float y)
    {
        cubeRendered.transform.localRotation = Quaternion.Euler(Vector3.up * y);
    }
}
