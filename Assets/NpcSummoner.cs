using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSummoner : MonoBehaviour
{
    public GameObject npcPrefab; // Assign this in the inspector with your NPC prefab
    public int numberOfNPCs = 5; // The number of NPCs you want to summon
    public Vector2 summonAreaMin = new Vector2(-10, -10); // Min bounds of the summon area
    public Vector2 summonAreaMax = new Vector2(10, 10); // Max bounds of the summon area


    private void Awake()
    {
        SummonNPCs();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SummonNPCs()
    {
        for (int i = 0; i < numberOfNPCs; i++)
        {
            // Generate a random position within the specified bounds
            Vector2 spawnPosition = new Vector2(
                Random.Range(summonAreaMin.x, summonAreaMax.x),
                Random.Range(summonAreaMin.y, summonAreaMax.y)
            );

            // Instantiate the NPC at the generated position
            Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
