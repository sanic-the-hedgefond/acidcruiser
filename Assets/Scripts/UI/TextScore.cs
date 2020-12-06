using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PlayerController>())
        {
            FindObjectOfType<PlayerController>().scoreEvent += UpdateScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScore(int s)
    {
        string txt = s.ToString();
        GetComponent<TextMeshProUGUI>().text = txt;
    }
}
