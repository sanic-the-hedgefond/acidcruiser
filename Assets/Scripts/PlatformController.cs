﻿using System;
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

    private GameManager gameManager;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        isDestroyed = false;

        score = 20 * ((int)transform.localScale.x + 1) * ((int)transform.localScale.y + 1);
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
                FindObjectOfType<PlayerController>().DecreaseHealth(20);

                GameObject gameUI = GameObject.Find("GameUI");
                if (gameUI != null)
                {
                    gameUI.GetComponent<GameUI>().DisplayText("-" + score, 200f, 40, new Vector2(0f, 250f), new Color(255f/255f, 68f/255f, 195f/255f));
                }

                if(FindObjectOfType<PlayerController>() != null)
                {
                    FindObjectOfType<PlayerController>().DecreaseScore(score);
                }
            }

            isDestroyed = true;
        }
    }

    public void Kill()
    {
        if (isLast)
        {
            gameManager.NextStage();
        }
        else
        {
            GameObject gameUI = GameObject.Find("GameUI");
            if (gameUI != null)
            {
                gameUI.GetComponent<GameUI>().DisplayText("+" + score, 200f, 40, new Vector2(0f, 250f), new Color(0f, 0f, 0f));
            }

            FindObjectOfType<PlayerController>().IncreaseScore(score);
        }
        Destroy(gameObject);
    }

    public IEnumerator DeathAnimation()
    {
        int frames_part1 = 20;
        int frames_part2 = 10;

        float scale_factor = 1.25f;

        Vector3 scale = transform.localScale;

        for (int i = 0; i < frames_part1; i++)
        {
            transform.localScale = new Vector3(Mathf.SmoothStep(scale.x, scale.x * scale_factor, i / (float)frames_part1),
                                                Mathf.SmoothStep(scale.y, scale.y * scale_factor, i / (float)frames_part1),
                                                Mathf.SmoothStep(scale.z, scale.z * scale_factor, i / (float)frames_part1));
            yield return null;
        }

        for (int i = 0; i < frames_part2; i++)
        {
            transform.localScale = new Vector3( Mathf.SmoothStep(scale.x, 0f, i/(float)frames_part2),
                                                Mathf.SmoothStep(scale.y, 0f, i/(float)frames_part2),
                                                Mathf.SmoothStep(scale.z, 0f, i/(float)frames_part2));
            yield return null;
        }
        Destroy(gameObject);
    }
}
