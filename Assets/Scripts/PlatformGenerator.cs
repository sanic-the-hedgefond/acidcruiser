using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    //public GameObject platform;
    private GameObject[] stages;
    private int current_stage;

    private List<GameObject> level;

    void Start()
    {
        FindObjectOfType<GameManager>().startGameEvent += Activate;
        FindObjectOfType<PlayerController>().deathEvent += Deactivate;

        level = new List<GameObject>();

        gameObject.SetActive(false);

        stages = Resources.LoadAll<GameObject>("Levels");
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

        foreach (GameObject stage in level)
        {
            Destroy(stage);
        }
        level.Clear();
        current_stage = 0;
        GenerateStage(current_stage);
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
