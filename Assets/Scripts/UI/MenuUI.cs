using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI playerNameText;
    public Slider sliderSensitivity;

    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        gameManager = FindObjectOfType<GameManager>();
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn(2));
    }

    public void SetPlayerName()
    {
        playerNameText.text = gameManager.GetPlayerName();
        playerNameText.SetText(gameManager.GetPlayerName());
    }

    public void ChangePlayerName()
    {
        if (playerNameText.text != "")
        {
            gameManager.SetPlayerName(playerNameText.text);
        }
    }

    public void ChangeSensitivity()
    {
        gameManager.SetSensitivity(sliderSensitivity.value);
    }

    public void SetSlider()
    {
        sliderSensitivity.value = gameManager.GetSensitivity();
    }

    IEnumerator FadeIn(float fadeTime)
    {
        float elapsedTime = 0f;

        while (canvasGroup.alpha < 1)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
            yield return null;
        }

        yield return null;
    }
}
