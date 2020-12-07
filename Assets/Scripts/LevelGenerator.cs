using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;

    [Header("Level Settings")]
    public float levelStartHeight;
    public float levelEndHeight;

    public float ratioVerticalPlatforms;

    [Header("Coin Settings")]
    public float ratioCoins;

    public int minCoinsInRow;
    public int maxCoinsInRow;

    public float minCoinX;
    public float maxCoinX;

    [Header("Distance between Platforms")]
    public float minPlatformDist;

    [Header("Platform Size")]
    public float minPlatformWidth;
    public float maxPlatformWidth;

    public float minPlatformHeight;
    public float maxPlatformHeight;

    [Header("Platform Position")]
    public float minPlatformX;
    public float maxPlatformX;

    [Header("Platform Speed")]
    public float minPlatformSpeedX;
    public float maxPlatformSpeedX;
    public float platformSpeedY;
    public float platformAccY;

    [Header("Platform Rotation")]
    public float minPlatformRotationX;
    public float maxPlatformRotationX;
    public float minPlatformRotationY;
    public float maxPlatformRotationY;
    public float minPlatformRotationZ;
    public float maxPlatformRotationZ;

    private List<GameObject> platforms;
    private List<GameObject> coins;

    private float height;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().startGameEvent += Activate;
        FindObjectOfType<PlayerController>().deathEvent += Deactivate;

        platforms = new List<GameObject>();
        coins = new List<GameObject>();
        gameObject.SetActive(false);
        //generateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (platforms.Count == 0)
        {
            GenerateLevel();
        }
    }

    void Activate()
    {
        gameObject.SetActive(true);
        foreach (GameObject obj in platforms)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in coins)
        {
            Destroy(obj);
        }
        platforms.Clear();
    }

    public void Deactivate()
    {
        foreach (GameObject obj in platforms)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in coins)
        {
            Destroy(obj);
        }
        platforms.Clear();
        gameObject.SetActive(false);
    }

    public void GenerateLevel()
    {
        height = levelStartHeight;
        bool isPlatformVertical = false;
        bool isCoin = false;
        //platforms = new List<PlatformController>();

        int curPlatform = 0;
        int curCoin = 0;

        while (height < levelEndHeight)
        {

            isCoin = Random.Range(0f, 1f) < ratioCoins;
            if (isCoin)
            {
                height += 2f;
                int coinsInRow = Random.Range(minCoinsInRow, maxCoinsInRow);
                for (int i = 0; i < coinsInRow; i++)
                {
                    Vector3 position = new Vector3(Random.Range(minCoinX, maxCoinX), height, 0);
                    coins.Add(Instantiate(coinPrefab, position, Quaternion.identity));
                    coins[curCoin].GetComponent<CoinController>().SetSpeed(platformSpeedY);
                    curCoin++;
                    height += 1f;
                }
                height += 2f;
            }
            else
            {
                Vector3 scale = new Vector3(Random.Range(minPlatformWidth, maxPlatformWidth), Random.Range(minPlatformHeight, maxPlatformHeight), Random.Range(0.3f, 0.6f));
                Vector3 position = new Vector3(Random.Range(minPlatformX, maxPlatformX), Random.Range(-1f, 1f) + height, 0);
                Vector3 rotation = new Vector3(Random.Range(-30f, 30f), Random.Range(-5f, 5f), Random.Range(-10f, 10f));

                if (isPlatformVertical)
                {
                    float tmp = scale.x;
                    scale.x = scale.y;
                    scale.y = tmp;
                }

                if (platforms.Count >= 1)
                {
                    float distance = Vector3.Distance(platforms[curPlatform - 1].GetComponent<Collider>().ClosestPointOnBounds(position), position);
                    if (distance > minPlatformDist)
                    {
                        platforms.Add(Instantiate(platformPrefab, position, Quaternion.Euler(rotation)));
                    }
                    else
                    {
                        height += 1f;
                        continue;
                    }
                }
                else
                {
                    platforms.Add(Instantiate(platformPrefab, position, Quaternion.Euler(rotation)));
                }

                platforms[curPlatform].transform.localScale = scale;

                PlatformController platformController = platforms[curPlatform].GetComponent<PlatformController>();

                platformController.SetScore();
                platformController.SetSpeed(Random.Range(minPlatformSpeedX, maxPlatformSpeedX), platformSpeedY);
                platformController.SetAcceleration(platformAccY);

                if (isPlatformVertical)
                {
                    platformController.SetRotation(Random.Range(minPlatformRotationY, maxPlatformRotationY),
                                                   Random.Range(minPlatformRotationX, maxPlatformRotationX),
                                                   Random.Range(minPlatformRotationZ, maxPlatformRotationZ));
                }
                else
                {
                    platformController.SetRotation(Random.Range(minPlatformRotationX, maxPlatformRotationX),
                                                   Random.Range(minPlatformRotationY, maxPlatformRotationY),
                                                   Random.Range(minPlatformRotationZ, maxPlatformRotationZ));
                }

                curPlatform++;
                height += 1f;
                isPlatformVertical = Random.Range(0f, 1f) < ratioVerticalPlatforms;
            }
        }
    }

    public void removePlatform(GameObject platform)
    {
        platforms.Remove(platform);
    }
}
