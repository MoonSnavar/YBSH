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
        //через дебаг проверить почему у магазина прогружается только 1 чанк, а прошлые не хотят
    }
    private void SetDestroySettings()
    {
        int progress = PlayerPrefs.GetInt("Progress");
        if (progress < 2)
        {
            DestroyWithChance.chanceOfStayingCE = 0.2f;
            DestroyWithChance.chanceOfStayingAI = 0.05f;
            DestroyWithChance.chanceOfStayingHor = 0.1f;
            DestroyWithChance.chanceOfStayingSt = 0.3f;
            DestroyWithChance.chanceOfStayingHeal = 0.6f;
            DestroyWithChance.chanceOfStayingCP = 0.6f;
        }
        else if (progress < 4)
        {
            DestroyWithChance.chanceOfStayingCE = 0.3f;
            DestroyWithChance.chanceOfStayingAI = 0.1f;
            DestroyWithChance.chanceOfStayingHor = 0.2f;
            DestroyWithChance.chanceOfStayingSt = 0.4f;
            DestroyWithChance.chanceOfStayingHeal = 0.5f;
            DestroyWithChance.chanceOfStayingCP = 0.55f;
        }
        else if (progress < 6)
        {
            DestroyWithChance.chanceOfStayingCE = 0.4f;
            DestroyWithChance.chanceOfStayingAI = 0.2f;
            DestroyWithChance.chanceOfStayingHor = 0.3f;
            DestroyWithChance.chanceOfStayingSt = 0.5f;
            DestroyWithChance.chanceOfStayingHeal = 0.4f;
            DestroyWithChance.chanceOfStayingCP = 0.5f;
        }
        else if (progress < 8)
        {
            DestroyWithChance.chanceOfStayingCE = 0.5f;
            DestroyWithChance.chanceOfStayingAI = 0.3f;
            DestroyWithChance.chanceOfStayingHor = 0.4f;
            DestroyWithChance.chanceOfStayingSt = 0.6f;
            DestroyWithChance.chanceOfStayingHeal = 0.3f;
            DestroyWithChance.chanceOfStayingCP = 0.4f;
        }
        else if (progress < 10)
        {
            DestroyWithChance.chanceOfStayingCE = 0.55f;
            DestroyWithChance.chanceOfStayingAI = 0.35f;
            DestroyWithChance.chanceOfStayingHor = 0.45f;
            DestroyWithChance.chanceOfStayingSt = 0.65f;
            DestroyWithChance.chanceOfStayingHeal = 0.25f;
            DestroyWithChance.chanceOfStayingCP = 0.35f;
        }
        else
        {
            DestroyWithChance.chanceOfStayingCE = 0.6f;
            DestroyWithChance.chanceOfStayingAI = 0.4f;
            DestroyWithChance.chanceOfStayingHor = 0.55f;
            DestroyWithChance.chanceOfStayingSt = 0.7f;
            DestroyWithChance.chanceOfStayingHeal = 0.2f;
            DestroyWithChance.chanceOfStayingCP = 0.3f;
        }        
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
