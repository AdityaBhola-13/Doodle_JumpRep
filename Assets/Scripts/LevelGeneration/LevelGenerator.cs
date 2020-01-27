using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class LevelGenerator : MonoBehaviour
{
    public GameObject doodlePrefab;
    GameManager gManager;
    public List<GameObject> platformsList = new List<GameObject>();
    public List<GameObject> pickUpList = new List<GameObject>();
    public List<GameObject> monstersList = new List<GameObject>();
    float xPos = 2.2f;
    float maxGapY = 0.4f;
    public float minGapY = 0.9f;
    public GameObject lastPlt;
    public GameObject lastMonsterSpawned;
    public bool monsterSpawned;

    bool startSpawning;
    Vector3 camPosition;
    Vector3 spawningPos;

    int upToDown;
    float lastUpToDownXPos;
    float lastUpToDownYPos;

    int randomNumbersPlatform;
    public int maxRandomNumberPlatform = 17;
    // Start is called before the first frame update
    void Start()
    {
       
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SpawningFirstPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        //Spawing First Platform or Spawning Platform for first time :- Done By Aditya
        camPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane / 2));
        Vector2 lastPlatform_YPos = new Vector2(0, lastPlt.transform.position.y);
        if (lastPlatform_YPos.y < camPosition.y + 2f)
        {
            if(gManager.score > 50000 || gManager.isGameOver == false)
            {
                SpawningPlatforms();
            }
        }

    }
    //Spawning Platforms or First Time :- Done by Aditya 
    void SpawningFirstPlatform()
    {
        camPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane / 2));
        spawningPos = new Vector3(Random.Range(-xPos, xPos), -camPosition.y, 0);
        GameObject firstSpawnedPlatform = Instantiate(platformsList[0], spawningPos, Quaternion.identity);
        Instantiate(doodlePrefab, new Vector2(firstSpawnedPlatform.transform.position.x, firstSpawnedPlatform.transform.position.y + 0.7f),Quaternion.identity);
        lastPlt = firstSpawnedPlatform;
        for (int i = 0; lastPlt.transform.position.y < camPosition.y + 2f; i++)
        {
            spawningPos.y += Random.Range(maxGapY, minGapY);
            spawningPos.x = Random.Range(-xPos, xPos);
            GameObject spawnedPlatforms = Instantiate(platformsList[0], spawningPos, Quaternion.identity);
            lastPlt = spawnedPlatforms;
        }
        changingRandomNumbers();
    }
    //Generated or Spawning Platforms, Pickups , Monsters Procedurally :- Done By Aditya
    void SpawningPlatforms()
    {
        randomNumbersPlatform = Random.Range(0, maxRandomNumberPlatform);
        spawningPos.y += Random.Range(maxGapY, minGapY);
        spawningPos.x = Random.Range(-xPos, xPos);
        float distBtwnUDplatform = Vector3.Distance(new Vector2(lastUpToDownXPos, 0), new Vector2(spawningPos.x, 0));
        if (upToDown >= 5)
        {
            upToDown = 0;
        }
        if (upToDown > 0 && upToDown <= 4)
        {
            if (distBtwnUDplatform < 1)
            {
                if (lastUpToDownXPos <= 0)
                {
                    spawningPos.x = lastUpToDownXPos + 1.7f;
                }
                if (lastUpToDownXPos >= 0)
                {
                    spawningPos.x = lastUpToDownXPos - 1.7f;
                }
            }
            if (randomNumbersPlatform == 15 || randomNumbersPlatform == 15)
            {
                spawningPos.y = spawningPos.y + 1.75f;
            }
            upToDown++;
        }
        if(monsterSpawned == true)
        {
            spawningPos.y = lastMonsterSpawned.transform.position.y + 0.2f;
            monsterSpawned = false;
        }
        GameObject lastSpawnedPlatform = null;
        switch (randomNumbersPlatform)
        {
            case 6:
            case 10:
                lastSpawnedPlatform = Instantiate(platformsList[1], spawningPos, Quaternion.identity);
                break;
            case 9:
                lastSpawnedPlatform = Instantiate(platformsList[3], spawningPos, Quaternion.identity);
                lastUpToDownXPos = lastSpawnedPlatform.transform.position.x;
                lastUpToDownYPos = lastSpawnedPlatform.transform.position.y;
                upToDown = 1;
                break;
            case 15:
            case 20:
            case 33:
            case 43:
                lastSpawnedPlatform = Instantiate(platformsList[2], spawningPos, Quaternion.identity);
                break;
            case 35:
            case 40:
                lastSpawnedPlatform = Instantiate(platformsList[4], spawningPos, Quaternion.identity);
                break;
            case 44:
            case 50:
                lastSpawnedPlatform = Instantiate(platformsList[5], spawningPos, Quaternion.identity);
                break;
            case 7:
            case 14:
                lastSpawnedPlatform = Instantiate(platformsList[0], spawningPos, Quaternion.identity);
                int randomSpawanedObjects = Random.Range(0, 11);
                if(randomSpawanedObjects == Random.Range(1,5))
                {
                    GameObject pickUp = Instantiate(pickUpList[0], new Vector2(Random.Range(lastSpawnedPlatform.transform.position.x + 0.3f, lastSpawnedPlatform.transform.position.x - 0.3f), lastSpawnedPlatform.transform.position.y +0.088f ), Quaternion.identity);
                }
                if (randomSpawanedObjects == Random.Range(9,10))
                {
                    GameObject pickUp = Instantiate(pickUpList[1], new Vector2(Random.Range(lastSpawnedPlatform.transform.position.x + 0.3f, lastSpawnedPlatform.transform.position.x - 0.3f), lastSpawnedPlatform.transform.position.y + 0.22f), Quaternion.identity);
                }
                if(randomSpawanedObjects == Random.Range(6, 8))
                {
                    GameObject monster = Instantiate(monstersList[Random.Range(0,monstersList.Count)], new Vector2(lastSpawnedPlatform.transform.position.x, lastSpawnedPlatform.transform.position.y + 0.3f), Quaternion.identity);
                    lastMonsterSpawned = monster;
                    monsterSpawned = true;
                }
                break;
            default:
                lastSpawnedPlatform = Instantiate(platformsList[0], spawningPos, Quaternion.identity);
                break;
        }
        lastPlt = lastSpawnedPlatform;
    }

    //Increasing Difficulties in each 5000 increment :- Done By Aditya
    void changingRandomNumbers()
    {
        if (gManager.score % 5000 == 0)
        {
            maxRandomNumberPlatform += 3;
            minGapY += 0.1f;
            Mathf.Clamp(minGapY, 1, 1.5f);
        }
    }
    

}
