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
    public GameObject cube_shattered;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
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

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().DecreaseHealth(20);
            Debug.Log("-20HP Time: " + Time.time);
            Destroy(gameObject);
            //gameObject.AddComponent<TriangleExplosion>();
            //StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
        }
    }
    */

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Pos: " + transform.position + " Scale: " + transform.localScale);
            GameObject tmp = Instantiate(cube_shattered, transform.position, transform.rotation);
            tmp.transform.localScale = transform.localScale/2f;
            //tmp.transform.position = transform.position;
            //tmp.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }
    */

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(transform.localScale);
            for (int i = 0; i < (int)(transform.localScale[0]*2); i++)
            {
                for (int j = 0; j < (int)(transform.localScale[1]*2); j++)
                {
                    for (int k = 0; k < (int)(transform.localScale[2]*2); k++)
                    {
                        //Debug.Log("i" + i + " j" + j + " k" + k);
                        Vector3 relative_pos = new Vector3(i - transform.localScale[0] / 2f, j - transform.localScale[1] / 2f, k)/2f;
                        Instantiate(cube_shattered, transform.position+relative_pos, transform.rotation);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(!isDestroyed)
            {
                StartCoroutine(DeathAnimation());
                FindObjectOfType<PlayerController>().DecreaseHealth(20);
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
            FindObjectOfType<PlayerController>().IncreaseScore(20 * ((int)transform.localScale.x + 1) *
                                                                    ((int)transform.localScale.y + 1));
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
