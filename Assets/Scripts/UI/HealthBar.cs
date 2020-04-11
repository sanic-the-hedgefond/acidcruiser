using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float x_scale;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerController>().healthEvent += UpdateHealth;
        x_scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth(int h)
    {
        if (h < 0) { h = 0; }
        Vector3 scale = transform.localScale;
        scale.x = x_scale * (float)h / 100f;
        transform.localScale = scale;
    }
}
