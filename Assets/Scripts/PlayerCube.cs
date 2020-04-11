using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour
{
    public GameObject cubeRendered;
    public bool isLeft;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            FindObjectOfType<PlayerController>().DecreaseHealth(20);
            //Destroy(collision.gameObject);
            collision.gameObject.AddComponent<TriangleExplosion>();
            StartCoroutine(collision.gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
        }
    }

    public void RotateY(float y)
    {
        //cubeRendered.transform.Rotate(new Vector3(0f, y, 0f));
        //cubeRendered.transform.rotation = Quaternion.Euler(cubeRendered.transform.up * y);
        cubeRendered.transform.localRotation = Quaternion.Euler(Vector3.up * y);
    }
}
