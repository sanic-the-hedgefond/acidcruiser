using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platform;
    public GameObject[] stages;
    private int current_stage;

    private List<GameObject> level;

    void Start()
    {
        FindObjectOfType<GameManager>().startGameEvent += Activate;
        FindObjectOfType<PlayerController>().deathEvent += Deactivate;

        level = new List<GameObject>();

        current_stage = 0;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (current_stage <= stages.Length) { current_stage++; }
            else { current_stage = 0; }
            GenerateStage(current_stage);

            timer = time_interval;
        }
        */
    }

    void Deactivate()
    {
        foreach (GameObject stage in level)
        {
            Destroy(stage);
        }

        level.Clear();
        gameObject.SetActive(false);
    }

    void Activate()
    {
        gameObject.SetActive(true);
        current_stage = 0;
        GenerateStage(current_stage);
    }

    public void SetDifficulty(int d)
    {

    }

    private void GenerateStage(int n)
    {
        level.Add(Instantiate(stages[n], transform));
    }

    public void NextStage()
    {
        //Debug.Log("Next Stage: " + current_stage + " of " + stages.Length);
        if (current_stage < stages.Length - 1) { current_stage++; }
        else { current_stage = 0; }
        GenerateStage(current_stage);
    }
}
