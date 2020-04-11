using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI playerNamePro;
    public Slider difficulty;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetPlayerName()
    {
        playerNamePro.text = gameManager.GetPlayerName();
    }

    public void ChangePlayerName()
    {
        if (playerNamePro.text != "")
        {
            gameManager.SetPlayerName(playerNamePro.text);
        }
    }

    public void changeDifficulty()
    {
        gameManager.SetDifficulty((int)difficulty.value);
    }
}
