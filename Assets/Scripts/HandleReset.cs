using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().startGameEvent += Reset;
    }

    // Update is called once per frame
    void Reset()
    {
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - 150f, 0f);
    }
}
