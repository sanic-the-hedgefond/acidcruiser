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

    public AudioSource audioHit;
    public AudioSource audioDead;
    public AudioSource audioNearPlatform;

    private Vector2 position_start;
    private Vector2 position_delta;
    private float rotation_interp = 0f;
    private float const_inner_rotation = 0f;

    public float nearPlatformRadius = 1.5f;
    public float gravityPlatform = -0.5f;
    public float volumePlatform = 0.05f;

    public float minDistanceCoin = 1.0f;
    public float gravityCoin = 0.15f;

    private bool isDead;
    private const int start_health = 100;
    private int health;
    private int score;

    public int Score { get => score; set => score = value; }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().startGameEvent += Reset;
        FindObjectOfType<GameManager>().pauseGameEvent += Mute;
        isDead = true;
        score = 0;

        go_cube_L.SetActive(false);
        go_cube_R.SetActive(false);
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

        if (IsDoubleTap() && !isDead)
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
        Collider[] nearObjects = Physics.OverlapSphere(transform.position, 10);

        //Debug.Log(string.Format("nearPlatforms: {0}", nearPlatforms.Length));

        if (nearObjects.Length != 0)
        {
            float delta = 0f;
            float minDistancePlatform = nearPlatformRadius;

            foreach (var nearObject in nearObjects)
            {
                if (nearObject.gameObject.tag == "Platform")
                {
                    //vol = 1f;
                    Vector3 closestL = nearObject.ClosestPointOnBounds(go_cube_L.transform.position + new Vector3(0.25f, 0f, 2.2f));
                    Vector3 closestR = nearObject.ClosestPointOnBounds(go_cube_R.transform.position + new Vector3(-0.25f, 0f, 2.2f));

                    float distL = Vector3.Distance(go_cube_L.transform.position, closestL);
                    float distR = Vector3.Distance(go_cube_R.transform.position, closestR);

                    //Debug.Log(string.Format("DistL: {0}, DistR: {1}", distL, distR));
                    /*if (distL < minDistancePlatform || distR < minDistancePlatform)
                    {
                        minDistancePlatform = Mathf.Min(distL, distR);
                        delta = nearPlatformRadius - minDistancePlatform;
                    }*/

                    if (distL < minDistancePlatform)
                    {
                        delta = nearPlatformRadius - distL;

                        Vector3 direction = (go_cube_L.transform.position - nearObject.gameObject.transform.position).normalized;
                        //Debug.Log(direction);
                        nearObject.GetComponent<Rigidbody>().AddForce(direction * gravityPlatform);
                    }
                    else if (distR < minDistancePlatform)
                    {
                        delta = nearPlatformRadius - distR;

                        Vector3 direction = (go_cube_R.transform.position - nearObject.gameObject.transform.position).normalized;
                        //Debug.Log(direction);
                        nearObject.GetComponent<Rigidbody>().AddForce(direction * gravityPlatform);
                    }
                }

                if (nearObject.gameObject.tag == "Coin")
                {
                    Vector3 closestL = nearObject.ClosestPointOnBounds(go_cube_L.transform.position + new Vector3(0.25f, 0f, 2.2f));
                    Vector3 closestR = nearObject.ClosestPointOnBounds(go_cube_R.transform.position + new Vector3(-0.25f, 0f, 2.2f));

                    float distL = Vector3.Distance(go_cube_L.transform.position, closestL);
                    float distR = Vector3.Distance(go_cube_R.transform.position, closestR);

                    if (distL < minDistanceCoin)
                    {
                        float smoothFactor = 1 + minDistanceCoin - distL;
                        Vector3 newPos = Vector3.Lerp(nearObject.gameObject.transform.position, go_cube_L.transform.position, gravityCoin * smoothFactor);
                        nearObject.gameObject.transform.position = newPos;
                    }
                    else if (distR < minDistanceCoin)
                    {
                        float smoothFactor = 1 + minDistanceCoin - distR;
                        Vector3 newPos = Vector3.Lerp(nearObject.gameObject.transform.position, go_cube_R.transform.position, gravityCoin * smoothFactor);
                        nearObject.gameObject.transform.position = newPos;
                    }
                }

            }
            audioNearPlatform.volume = volumePlatform + delta / 10f;
            audioNearPlatform.pitch = 1f - delta / 15f;
        }
        else
        {
            audioNearPlatform.volume = volumePlatform;
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
            audioHit.pitch = 1.0f + health / 100.0f;
            audioHit.Play();
            healthEvent(health);
        }

        if (health <= 0f)
        {
            audioDead.Play();
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
        Mute();

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

        audioNearPlatform.volume = volumePlatform;
        audioNearPlatform.Play();

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

    public void Mute()
    {
        audioNearPlatform.volume = 0;
    }
}