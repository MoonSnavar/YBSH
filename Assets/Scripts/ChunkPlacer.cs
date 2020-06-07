using System.Collections.Generic;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public Transform player;
    public Chunk[] chunkPrefabs;
    public Chunk firstChunk;

    private int lastChunk;
    private List<Chunk> spawnedChunks = new List<Chunk>();
    
    public void Start()
    {
        lastChunk = 2;
        SetDestroySettings();
        spawnedChunks.Add(firstChunk);       
        SpawnChunk();       
    }
    private void Update()
    {
        if (player.transform.position.y > spawnedChunks[lastChunk].End.position.y - 5)
        {
            if (lastChunk + 1 < spawnedChunks.Count)
                lastChunk += 1;
            for (int i = 0; i <= spawnedChunks.Count-1; i++)
            {
                if (i != lastChunk-1 && i != lastChunk && i != lastChunk + 1)
                    spawnedChunks[i].gameObject.SetActive(false);
                else
                    spawnedChunks[i].gameObject.SetActive(true);
            }
        }
        if (player.transform.position.y < spawnedChunks[lastChunk-1].End.position.y)
        {
            if (lastChunk - 1 != 0)
                lastChunk -= 1;
            for (int i = 0; i <= spawnedChunks.Count - 1; i++)
            {
                if (i != lastChunk - 1 && i != lastChunk && i != lastChunk + 1)
                    spawnedChunks[i].gameObject.SetActive(false);
                else
                    spawnedChunks[i].gameObject.SetActive(true);
            }
        }
    }
    private void SetDestroySettings()
    {
        int progress = PlayerPrefs.GetInt("LevelProgress");
        float difficulty;
        difficulty = ((float)progress / 10) + 1;

        DestroyWithChance.difficulty = difficulty;
        print("progress " + progress);
        print("difficulty " + difficulty);
    }

    public void ClearLevel()
    {        
        for (int i = 1; i < spawnedChunks.Count; i++)
        {
            Destroy(spawnedChunks[i].gameObject);              
        }
        spawnedChunks.RemoveRange(1, spawnedChunks.Count - 1);
    }

    public void SpawnChunk()
    {
        for (int i = 0; i < 9; i++)
        {
            Chunk newChunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]);
            newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position - newChunk.Begin.localPosition;

            if (i > 2 && !PlayerManager.isGetProducts)
                newChunk.gameObject.SetActive(false); //первые 3 чанка оставляю, остальные отключаю

            if (i < 7 && PlayerManager.isGetProducts)
                newChunk.gameObject.SetActive(false); //последние 3 чанка оставляю, остальные отключаю

            spawnedChunks.Add(newChunk);
        }
    }
}
