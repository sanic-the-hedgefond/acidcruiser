using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float rotation_time;
    public float rotation_amplitude;

    Material mat_BG;

    // Start is called before the first frame update
    void Start()
    {
        mat_BG = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.back * Mathf.Sin(Time.time * rotation_time) * rotation_amplitude);
        transform.Translate(Vector3.up * Mathf.Sin(Time.time) * 0.01f);
        //SetSpeed(new Vector2(0f, Mathf.Sin(Time.time * 0.5f) * 0.5f + 2.0f));
    }

    void SetSpeed(Vector2 speed)
    {
        mat_BG.SetVector("Vector2_71705CE4", speed);
    }

    public IEnumerator SetGridColor(Color color)
    {
        int duration = 60;

        Color color_current = mat_BG.GetColor("Color_CE87F6DE");
        Debug.Log(color_current);

        for (int i = 0; i < duration; i++)
        {
            Color color_transition = new Color(Mathf.SmoothStep(color_current.r, color.r, i / (float)duration),
                                                Mathf.SmoothStep(color_current.g, color.g, i / (float)duration),
                                                Mathf.SmoothStep(color_current.b, color.b, i / (float)duration),
                                                Mathf.SmoothStep(color_current.a, color.a, i / (float)duration));
            mat_BG.SetColor("Color_CE87F6DE", color_transition);

            yield return new WaitForFixedUpdate();
        }

        Debug.Log(mat_BG.GetColor("Color_CE87F6DE"));
    }
}