using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float x_speed;
    public float y_speed;

    public float z_rot;
    public float y_rot;
    public float x_rot;

    public bool isLast;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move platform
        Vector3 pos = transform.position;
        pos.x += x_speed;
        pos.y -= y_speed;

        // Additional y speed relative to difficulty
        pos.y -= gameManager.GetDifficultyFactor() / 200f;

        transform.position = pos;

        transform.Rotate(new Vector3(x_rot, y_rot, z_rot));

        // Destroy if platform is out of frame
        if (pos.y < -5f)
        {
            FindObjectOfType<PlayerController>().IncreaseScore(20 * ((int)transform.localScale.x+1) * ((int)transform.localScale.y+1));
            Kill();
        }
    }

    public void Kill()
    {
        if (isLast)
        {
            gameManager.NextStage();
        }
        Destroy(gameObject);
    }
}
