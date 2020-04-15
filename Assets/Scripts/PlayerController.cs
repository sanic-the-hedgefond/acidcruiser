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

    public float rotation_speed;
    public float rotation_smoothness;
    public float translation_speed;

    public GameObject go_cube_L, go_cube_R;
    public FixedJoystick joystick;

    private Vector2 position_start;
    private Vector2 position_delta;
    private float rotation_interp = 0f;

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

        joystick.AxisOptions = AxisOptions.Horizontal;
        joystick.SnapX = true;
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
                position_start = touch.position - position_delta;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                // Sliding distance
                position_delta = touch.position - position_start;

                /*
                // Change distance from between cubes
                Vector3 pos_cube_L = go_cube_L.transform.TransformPoint(new Vector3(position_delta.y * translation_speed, 0.0f, 0.0f));
                Vector3 pos_cube_R = go_cube_R.transform.TransformPoint(new Vector3(position_delta.y * -translation_speed, 0.0f, 0.0f));

                if ((pos_cube_L - pos_cube_R).magnitude > 0.495f &&
                    (pos_cube_L - pos_cube_R).magnitude < 4.5f)
                {  
                    go_cube_L.transform.position = pos_cube_L;
                    go_cube_R.transform.position = pos_cube_R;
                }
                */
            }

            //Debug.Log("Delta: " + position_delta.x + " || Interp: " + rotation_interp + " || Touch:" + touch.position.x);
            // Change background color depending on player rotation
            /*
            Color bg_col = Camera.main.GetComponent<Camera>().backgroundColor;

            float H, S, V;
            Color.RGBToHSV(bg_col, out H, out S, out V);
            H = (Mathf.Abs(position_delta.x) % 1000) * 0.001f;
            Camera.main.GetComponent<Camera>().backgroundColor = Color.HSVToRGB(H, S, V);
            Debug.Log("H:" + H + "Delta: " + position_delta.x);
            */
        }

        // Change rotation of player(cubes)
        rotation_interp = Mathf.Lerp(rotation_interp, position_delta.x, rotation_smoothness);
        transform.rotation = Quaternion.Euler(Vector3.back * rotation_interp * rotation_speed);
        SetBGHue(Mathf.Abs(rotation_interp) % 1000 * 0.001f);
        go_cube_L.GetComponent<PlayerCube>().RotateY(rotation_interp);
        go_cube_R.GetComponent<PlayerCube>().RotateY(-rotation_interp);

        /*
        if (joystick.Direction.x != 0 && !isDead)
        {
            transform.Rotate(Vector3.back, joystick.Direction.x * rotation_speed);
        }
        */
    }

    public void OnCollisionEnterChild(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            DecreaseHealth(20);

            /*
            if (health > 0)
            {
                Debug.Log(health);
                collision.gameObject.AddComponent<TriangleExplosion>();
                StartCoroutine(collision.gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
            }
            */
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
            healthEvent(health);
        }

        if (health <= 0f)
        {
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

    public void PlayerDead()
    {
        isDead = true;
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
        joystick.Reset();

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
        //Debug.Log("H:" + H);
    }
}
