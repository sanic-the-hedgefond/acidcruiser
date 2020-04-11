using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    public float speed = 0f;

    private float x_scale;

    // Start is called before the first frame update
    void Start()
    {
        // Some randomness
        float x_pos = Random.Range(-2f, 2f);
        x_pos = Mathf.Round(x_pos * 2f) / 2f;

        x_scale = Random.Range(0.5f, 2f);
        x_scale = Mathf.Round(x_scale * 2f) / 2f;

        Vector3 pos_platform = transform.position;
        Vector3 scale_platform = transform.localScale;

        pos_platform.x = x_pos;
        scale_platform.x = x_scale;
        scale_platform.y = 0.5f;
        scale_platform.z = 0.5f;

        transform.position = pos_platform;
        transform.localScale = scale_platform;

        //FindObjectOfType<playerController>().deathEvent += Kill;
    }

    // Update is called once per frame
    void Update()
    {
        // Move platform down
        Vector3 pos = transform.position;
        pos.y -= speed;
        transform.position = pos;

        // Destroy if platform is out of frame
        if (pos.y < -5f)
        {
            FindObjectOfType<PlayerController>().IncreaseScore(20 * (int)(x_scale*2f));
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
