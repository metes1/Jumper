using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    // An array of the different types of challenges
    public GameObject[] challenges;
    // An array of the portals
    public GameObject[] portals;
    public float spawnRate = 0.5f;
    public float minSpawnRate = 0.5f;
    public float upsideDownLength = 50.0f;
    public float upsideDownTime = 100.0f;

    // Current number of obstacle types that can be spawned
    // Will increase as highscore gets higher
    public int obsType = 1;

    private Vector2 spawnPoint = new Vector2(10f, -1.91f);
    private float floorY = -1.91f;
    private float ceilY = 3.34f;
    private Vector2 floorPortalPoint = new Vector2(10f, -1.549626f);
    private Vector2 ceilPortalPoint = new Vector2(10f, -1.549626f);
    private float counter = 0.0f;
    private int rateUp = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gameOver == false && GameController.instance.titleScreen == false)
        {
            if (GameController.instance.reversedGravity == false)
            {
                spawnPoint.y = floorY;
            } else
            {
                spawnPoint.y = ceilY;
            }

            if (obsType < challenges.Length && GameController.instance.score >= 25.0f*obsType)
            {
                // Add a new object type after player's score increases by 30
                obsType++;
            }
            if (spawnRate < 2.5f && GameController.instance.score >= 20.0f * rateUp)
            {
                // Add a new object type after player's score increases by 20
                spawnRate += 0.2f;
                rateUp++;
            }
            if (GameController.instance.score >= upsideDownTime)
            {
                GeneratePortal();
            } else
            {
                if (counter <= 0.0f)
                {
                    GenerateRandomChallenge();
                }
                else
                {
                    // Pick a random spawn rate within the specified range
                    // Range of spawn rate will increase as score increases
                    counter -= Time.deltaTime * Random.Range(minSpawnRate, spawnRate);
                }
            }

            GameObject currentChild;
            for (int i = 0; i < transform.childCount; i++)
            {
                currentChild = transform.GetChild(i).gameObject;
                if (currentChild.transform.position.x <= -20.0f)
                {
                    // Destroy obstacle after it moves off screen
                    Destroy(currentChild);
                }
            }

        }
    }

    void GenerateRandomChallenge()
    {
        GameObject newChallenge = Instantiate(challenges[Random.Range(0, obsType)], spawnPoint, Quaternion.identity) as GameObject;
        // Make this object a child of the GameController object
        newChallenge.transform.parent = transform;
        if (GameController.instance.reversedGravity == true)
        {
            newChallenge.transform.localScale = new Vector3(1, -1, 1);
        } else
        {
            newChallenge.transform.localScale = new Vector3(1, 1, 1);
        }
        counter = 1.0f;
    }

    void GeneratePortal()
    {
        GameObject newPortal;
        if (GameController.instance.reversedGravity == false)
        {
            newPortal = Instantiate(portals[0], floorPortalPoint, Quaternion.identity) as GameObject;
            // Set length of time player will be in upside down mode
            upsideDownTime += upsideDownLength;
            counter = 4.0f;
        } else
        {
            newPortal = Instantiate(portals[1], ceilPortalPoint, Quaternion.identity) as GameObject;
            // Set how far next reverse gravity portal will be, when player will go in upside down mode again
            upsideDownTime += 100.0f;
            counter = 4.0f;
        }
        newPortal.transform.parent = transform;
    }

}
