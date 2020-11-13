using System;
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

    public bool isDestroyed;
    public bool isLast;

    public string message = "";

    private GameManager gameManager;
    private int score;

    private bool soundPlayed;
    private float longestSide;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        isDestroyed = false;
        soundPlayed = false;

        score = 20 * ((int)transform.localScale.x + 1) * ((int)transform.localScale.y + 1) * ((int)transform.localScale.z + 1);

        longestSide = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move platform
        Vector3 pos = transform.position;
        pos.x += x_speed;
        pos.y -= y_speed;
        transform.position = pos;
        transform.Rotate(new Vector3(x_rot, y_rot, z_rot));

        if (pos.y < -(2.5f + longestSide) && !soundPlayed)
        {
            OutOfFrame();
        }

        // Destroy if platform is out of frame
        if (pos.y < -6f)
        {
            Kill();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!isDestroyed)
            {
                StartCoroutine(DeathAnimation());
                FindObjectOfType<PlayerController>().DecreaseHealth(100 / gameManager.hitsTilDeath);

                GameObject gameUI = GameObject.Find("GameUI");
                if (gameUI != null)
                {
                    gameUI.GetComponent<GameUI>().DisplayText("-" + score, 200f, 20, new Vector2(0f, 250f), new Color(255f/255f, 68f/255f, 195f/255f));
                }

                if(FindObjectOfType<PlayerController>() != null)
                {
                    FindObjectOfType<PlayerController>().DecreaseScore(score);
                }
            }

            isDestroyed = true;
        }
    }

    public void OutOfFrame()
    {
        GameObject gameUI = GameObject.Find("GameUI");

        if (!isLast)
        {
            gameManager.audioPlatformOutOfFrame.Play();
            soundPlayed = true;

            if (gameUI != null)
            {
                gameUI.GetComponent<GameUI>().DisplayText("+" + score, 200f, 20, new Vector2(0f, 150f), new Color(255f, 255f, 255f));
            }

            FindObjectOfType<PlayerController>().IncreaseScore(score);
        }
    }

    public void Kill()
    {
        GameObject gameUI = GameObject.Find("GameUI");

        if (message != "" && gameUI != null)
        {
            gameUI.GetComponent<GameUI>().DisplayText(message, 200f, 120, new Vector2(0f, -100f), new Color(255f, 255f, 255f));
        }

        if (isLast)
        {
            gameManager.NextStage();
        }
        Destroy(gameObject);
    }

    public IEnumerator DeathAnimation()
    {
        int frames_part1 = 10;
        int frames_part2 = 7;

        float scale_factor = 1.25f;

        Vector3 scale = transform.localScale;

        for (int i = 0; i < frames_part1; i++)
        {
            transform.localScale = new Vector3(Mathf.SmoothStep(scale.x, scale.x * scale_factor, i / (float)frames_part1),
                                                Mathf.SmoothStep(scale.y, scale.y * scale_factor, i / (float)frames_part1),
                                                Mathf.SmoothStep(scale.z, scale.z * scale_factor, i / (float)frames_part1));
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < frames_part2; i++)
        {
            transform.localScale = new Vector3( Mathf.SmoothStep(scale.x, 0f, i/(float)frames_part2),
                                                Mathf.SmoothStep(scale.y, 0f, i/(float)frames_part2),
                                                Mathf.SmoothStep(scale.z, 0f, i/(float)frames_part2));
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
