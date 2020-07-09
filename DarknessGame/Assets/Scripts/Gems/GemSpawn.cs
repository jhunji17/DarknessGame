using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawn : MonoBehaviour
{
    public GameObject Gem;
    public int initialNumberOfGems = 10;
    Vector2 spawnLocation;

    private void Start()
    {
        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        

        for (int i = 0; i < initialNumberOfGems; i++)
        {
            float RandomX = Random.Range(-width, width);
            float RandomY = Random.Range(-height, height);

            spawnLocation = new Vector2(RandomX, RandomY);

            Instantiate(Gem, spawnLocation, Quaternion.identity);

        }
    }
}
