using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TMP_FontAsset acidFont;

    List<GameObject> texts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerController>().deathEvent += OnDeath;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDeath()
    {
        foreach (GameObject text in texts)
        {
            Destroy(text);
        }
        texts.Clear();

        gameObject.SetActive(false);
    }

    public void DisplayText(string txt, float size, int duration, Vector2 position, Color color)
    {
        StartCoroutine(DisplayTextCoroutine(txt, size, duration, position, color));
    }

    public IEnumerator DisplayTextCoroutine(string txt, float size, int duration, Vector2 position, Color color)
    {
        GameObject textGO = new GameObject("gameUItext");
        texts.Add(textGO);
        textGO.transform.SetParent(this.transform);

        RectTransform rect = textGO.AddComponent<RectTransform>();
        textGO.AddComponent<CanvasRenderer>();
        TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();

        text.text = txt;
        text.fontSize = size;
        text.color = color;
        text.alignment = TextAlignmentOptions.Center;
        text.font = acidFont;

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0f, 0f);
        rect.sizeDelta = new Vector2(1000f, 200f);

        Vector3 pos = textGO.transform.position;
        pos.x += position.x;
        pos.y += position.y;
        textGO.transform.position = pos;

        textGO.transform.localScale = Vector3.zero;

        int blendtime = 20;
        float y_speed = 2.5f;

        for (int i = 0; i < blendtime; i++)
        {
            pos.y += y_speed;
            textGO.transform.position = pos;
            textGO.transform.localScale = Vector3.one * Mathf.SmoothStep(0f, 1f, i / (float)blendtime);
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < duration; i++)
        {
            pos.y += y_speed;
            textGO.transform.position = pos;
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < blendtime; i++)
        {
            pos.y += y_speed;
            textGO.transform.position = pos;
            text.color = new Color(color.r, color.g, color.b, Mathf.SmoothStep(1f, 0f, i / (float)blendtime));
            yield return new WaitForFixedUpdate();
        }

        Destroy(textGO);
    }
}
