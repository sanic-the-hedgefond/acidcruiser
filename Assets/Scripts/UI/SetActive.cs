using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public List<GameObject> gameObjectsToActivate;
    public List<GameObject> gameObjectsToDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gameObject in gameObjectsToActivate)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in gameObjectsToDeactivate)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
