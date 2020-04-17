using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    public GameObject cubeRendered;
    public bool isLeft;

    public void RotateY(float y)
    {
        cubeRendered.transform.localRotation = Quaternion.Euler(Vector3.up * y);
    }
}
