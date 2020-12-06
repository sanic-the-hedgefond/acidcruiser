using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : FadeIn
{
    private GameManager gameManager;

    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerNamePlaceholder;
    public Slider sliderSensitivity;
    public Slider sliderMusicVolume;

    //public float fadeTime = 1.5f;

    //private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    new void Start()
    {
        //base.Start();
        gameObject.SetActive(false);
    }

    public new void OnEnable()
    {
        gameManager = FindObjectOfType<GameManager>();
        //canvasGroup = GetComponent<CanvasGroup>();

        playerNamePlaceholder.text = gameManager.GetPlayerName();
        playerNamePlaceholder.SetText(gameManager.GetPlayerName());

        base.OnEnable();
        //StartCoroutine(DoFadeIn(fadeTime, canvasGroup));
    }

    /*
    public new void OnDisable()
    {
        base.OnDisable();

        //canvasGroup.alpha = 0;
    }
    */

    public void SetPlayerName()
    {
        playerNameText.text = gameManager.GetPlayerName();
        playerNameText.SetText(gameManager.GetPlayerName());
    }

    public void ChangePlayerName()
    {
        if (playerNamePlaceholder.text != "")
        {
            gameManager.SetPlayerName(playerNamePlaceholder.text);
        }
    }

    public void ChangeSensitivity()
    {
        gameManager.SetSensitivity(sliderSensitivity.value);
    }

    public void SetSliderSensitivity()
    {
        sliderSensitivity.value = gameManager.GetSensitivity();
    }

    public void ChangeMusicVolume()
    {
        gameManager.SetMusicVolume(sliderMusicVolume.value);
    }

    public void SetSliderMusicVolume()
    {
        sliderMusicVolume.value = gameManager.GetMusicVolume();
    }
}
