using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float speedX;
    public float speedY;

    public float accY;

    public float rotZ;
    public float rotY;
    public float rotX;

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

        SetScore();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move platform
        Vector3 pos = transform.position;
        
        pos.y -= speedY;

        if (pos.y < -(2.5f + longestSide) && !soundPlayed)
        {
            OutOfFrame();
        }

        // Destroy if platform is out of frame
        if (pos.y < -6f)
        {
            Kill();
        }

        else if (pos.y < 8f)
        {
            speedY += accY;
            pos.x += speedX;
            transform.Rotate(new Vector3(rotX, rotY, rotZ));
        }

        transform.position = pos;
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

        if (gameManager && !isLast && message == "")
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
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();

        if (message != "" && gameUI != null)
        {
            gameUI.GetComponent<GameUI>().DisplayText(message, 200f, 120, new Vector2(0f, -100f), new Color(255f, 255f, 255f));
        }

        if (isLast)
        {
            gameManager.NextStage();
        }

        if (levelGenerator)
        {
            Debug.Log("Remove Platform");
            levelGenerator.removePlatform(gameObject);
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
        Kill();
    }

    public void SetScore()
    {
        score = 20 * ((int)transform.localScale.x + 1) * ((int)transform.localScale.y + 1) * ((int)transform.localScale.z + 1);
        longestSide = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void SetSpeed(float x, float y)
    {
        speedX = x;
        speedY = y;
    }

    public void SetAcceleration(float y)
    {
        accY = y;
    }

    public void SetRotation(float x, float y, float z)
    {
        rotX = x;
        rotY = y;
        rotZ = z;
    }
}
