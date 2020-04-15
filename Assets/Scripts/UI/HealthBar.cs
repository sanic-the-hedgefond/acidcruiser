using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Color color_start;
    public Color color_finish;

    private float x_scale;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerController>().healthEvent += UpdateHealth;
        x_scale = transform.localScale.x;
    }

    void UpdateHealth(int h)
    {
        if (h < 0) { h = 0; }

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(SetWidth((float)h));
            StartCoroutine(ColorBlink());
        }

    }

    public IEnumerator SetWidth(float w)
    {
        int frames = 120;
        Vector3 scale = transform.localScale;

        for (int i = 0; i < frames; i++)
        {
            scale.x = Mathf.SmoothStep(scale.x, x_scale * w / 100f, i / (float)frames);
            transform.localScale = scale;
            yield return null;
        }
    }

    public IEnumerator ColorBlink()
    {
        Image img = GetComponent<Image>();
        int frames = 60;

        for (int i = 0; i < frames; i++)
        {
            img.color = new Color(  Mathf.SmoothStep(color_start.r, color_finish.r, i / (float)frames),
                                    Mathf.SmoothStep(color_start.g, color_finish.g, i / (float)frames),
                                    Mathf.SmoothStep(color_start.b, color_finish.b, i / (float)frames));
            yield return null;
        }

        for (int i = 0; i < frames; i++)
        {
            img.color = new Color(Mathf.SmoothStep(color_finish.r, color_start.r, i / (float)frames),
                                    Mathf.SmoothStep(color_finish.g, color_start.g, i / (float)frames),
                                    Mathf.SmoothStep(color_finish.b, color_start.b, i / (float)frames));
            yield return null;
        }
    }
}
