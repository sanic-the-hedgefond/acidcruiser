using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float x_speed;
    public float y_speed;

    public float y_acc;

    public int score;

    public bool isDestroyed;

    public string message = "";

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        score = 100;
        gameManager = FindObjectOfType<GameManager>();
        isDestroyed = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move platform
        Vector3 pos = transform.position;
        pos.x += x_speed;
        pos.y -= y_speed;
        transform.position = pos;

        //transform.Rotate(new Vector3(0.2f, 4f, 0.1f));
        Vector3 locScale = transform.localScale;
        locScale.x = 0.5f + Mathf.Sin(Time.time * 3f) * 0.05f;
        locScale.y = 0.5f + Mathf.Cos(Time.time + 0.2f) * 0.05f;
        transform.localScale = locScale;


        // Destroy if platform is out of frame
        if (pos.y < -6f)
        {
            Kill();
        }

        else if (pos.y < 8f)
        {
            y_speed += y_acc;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!isDestroyed)
            {
                gameManager.audioCoin.Play();
                StartCoroutine(DeathAnimation());

                GameObject gameUI = GameObject.Find("GameUI");
                if (gameUI != null)
                {
                    gameUI.GetComponent<GameUI>().DisplayText("+" + score, 200f, 20, new Vector2(0f, 250f), new Color(0f, 255f/255f, 0f));
                }

                if(FindObjectOfType<PlayerController>() != null)
                {
                    FindObjectOfType<PlayerController>().IncreaseScore(score);
                }
            }

            isDestroyed = true;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public IEnumerator DeathAnimation()
    {
        int frames_part1 = 5;
        int frames_part2 = 5;

        float scale_factor = 1.75f;

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
