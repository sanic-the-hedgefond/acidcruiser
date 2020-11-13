using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int> healthEvent;
    public Action<int> scoreEvent;
    public Action deathEvent;
    public Action restartEvent;
    public Action pauseEvent;

    public float rotation_speed;
    public float rotation_smoothness;
    public float translation_speed;
    public float inner_roation;

    private float sensitivity;

    public GameObject go_cube_L, go_cube_R;

    public AudioSource snd_hit;
    public AudioSource snd_dead;
    public AudioSource audioNearPlatform;

    private Vector2 position_start;
    private Vector2 position_delta;
    private float rotation_interp = 0f;
    private float const_inner_rotation = 0f;

    private float nearPlatformRadius = 2.2f;
    private float volume = 0.1f;

    private bool isDead;
    private const int start_health = 100;
    private int health;
    private int score;

    public int Score { get => score; set => score = value; }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().startGameEvent += Reset;
        isDead = true;
        score = 0;

        go_cube_L.SetActive(false);
        go_cube_R.SetActive(false);

        audioNearPlatform.volume = volume;
        audioNearPlatform.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Input handling
        if (Input.touchCount > 0 && !isDead)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("Touch Begin");
                position_start = touch.position * sensitivity - position_delta;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                // Sliding distance
                position_delta = touch.position * sensitivity - position_start;
            }
        }

        if (IsDoubleTap())
        {
            if (pauseEvent != null)
            {
                pauseEvent();
                FindObjectOfType<GameManager>().Pause();
            }
        }
    }

    private void FixedUpdate()
    {
        // Change rotation of player(cubes)
        rotation_interp = Mathf.Lerp(rotation_interp, position_delta.x, rotation_smoothness);

        transform.rotation = Quaternion.Euler(Vector3.back * rotation_interp * rotation_speed);

        const_inner_rotation += 0.5f;

        go_cube_L.GetComponent<PlayerCube>().RotateY(rotation_interp * inner_roation + const_inner_rotation);
        go_cube_R.GetComponent<PlayerCube>().RotateY(-rotation_interp * inner_roation - const_inner_rotation);

        //SetBGHue(Mathf.Abs(rotation_interp) % 1000 * 0.001f);

        // Check for platforms in radius and play sound
        Collider[] nearPlatforms = Physics.OverlapSphere(transform.position, 15);

        //Debug.Log(string.Format("nearPlatforms: {0}", nearPlatforms.Length));

        if (nearPlatforms.Length != 0)
        {
            float delta = 0f;
            float minDistance = nearPlatformRadius;

            foreach (var platform in nearPlatforms)
            {
                if (platform.gameObject.tag == "Platform")
                {
                    //vol = 1f;
                    Vector3 closestL = platform.ClosestPointOnBounds(go_cube_L.transform.position + new Vector3(0.25f, 0f, 2.2f));
                    Vector3 closestR = platform.ClosestPointOnBounds(go_cube_R.transform.position + new Vector3(-0.25f, 0f, 2.2f));

                    float distL = Vector3.Distance(go_cube_L.transform.position, closestL);
                    float distR = Vector3.Distance(go_cube_R.transform.position, closestR);

                    //Debug.Log(string.Format("DistL: {0}, DistR: {1}", distL, distR));
                    if (distL < minDistance || distR < minDistance)
                    {
                        minDistance = Mathf.Min(distL, distR);
                        delta = nearPlatformRadius - minDistance;
                    }
                }
            }

            audioNearPlatform.volume = volume + delta / 6f;
            audioNearPlatform.pitch = 1f - delta / 15f;
        }
        else
        {
            audioNearPlatform.volume = volume;
        }
    }

    public void SetHealth(int h)
    {
        health = h;
    }

    public void DecreaseHealth(int h)
    {
        health -= h;

        if (healthEvent != null && !isDead)
        {
            snd_hit.pitch = 1.0f + health / 100.0f;
            snd_hit.Play();
            healthEvent(health);
        }

        if (health <= 0f)
        {
            snd_dead.Play();
            PlayerDead();
        }
    }

    public void IncreaseScore(int s)
    {
        score += s;

        if (scoreEvent != null && !isDead)
        {
            scoreEvent(score);
        }
    }

    public void DecreaseScore(int s)
    {
        score -= s;
        if(score < 0) { score = 0; }

        if (scoreEvent != null && !isDead)
        {
            scoreEvent(score);
        }
    }

    public void PlayerDead()
    {
        isDead = true;
        //audioNearPlatform.volume = volume;
        gameObject.SetActive(false);

        if (deathEvent != null)
        {
            deathEvent();
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        go_cube_L.SetActive(true);
        go_cube_R.SetActive(true);
        isDead = false;
        score = 0;
        health = start_health;
        transform.rotation = Quaternion.identity;
        sensitivity = FindObjectOfType<GameManager>().GetSensitivity();

        if (restartEvent != null)
        {
            restartEvent();
        }

        if (scoreEvent != null)
        {
            scoreEvent(score);
        }

        if (healthEvent != null)
        {
            healthEvent(health);
        }
    }

    private void SetBGHue(float h)
    {
        Color bg_col = Camera.main.GetComponent<Camera>().backgroundColor;
        float H, S, V;
        Color.RGBToHSV(bg_col, out H, out S, out V);
        H = h;
        Camera.main.GetComponent<Camera>().backgroundColor = Color.HSVToRGB(H, S, V);
    }

    public bool IsDoubleTap()
    {
        bool result = false;

        if (Input.touchCount <= 0)
        {
            return false;
        }
        foreach (var touch in Input.touches)
        {
            if (touch.tapCount == 2)
            {
                result = true;
            }
        }

        return result;
    }
}