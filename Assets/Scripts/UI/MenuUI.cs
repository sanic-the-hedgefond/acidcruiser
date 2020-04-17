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

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetPlayerName()
    {
        playerNameText.text = gameManager.GetPlayerName();
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
}
