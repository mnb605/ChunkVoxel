using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMapGenerator : GameObjectSingleton<VoxelMapGenerator> {
    public GameObject chunkPrefab;
    public Chunk[,,] chunks;  //worldX,Y,Z변수가 배열의 크기.

    [HideInInspector]
    public int chunkSize = 16;
    [HideInInspector]
    public byte[,,] data;
    [HideInInspector]
    public byte[,,] TextureIddata;
    public int worldX = 128;
    public int worldY = 128;
    public int worldZ = 128;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    
    void Start()
    {
        data = new byte[worldX, worldY, worldZ];
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 6, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 2, 0) + 1;
                
                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        data[x, y, z] = 1;
                    }
                    else if (y <= dirt + stone)
                    {
                        data[x, y, z] = 2;
                    }
                }
            }
        }

        chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(worldZ / chunkSize)];
    }

    public void GenColumn(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            GameObject newChunk = Instantiate(chunkPrefab, new Vector3(x * chunkSize - 0.5f,
            y * chunkSize + 0.5f, z * chunkSize - 0.5f),
            new Quaternion(0, 0, 0, 0),
            this.transform) as GameObject;

            chunks[x, y, z] = newChunk.GetComponent<Chunk>();
            
            chunks[x, y, z].VoxelMapGeneratorGO = gameObject;
            chunks[x, y, z].chunkSize = chunkSize;
            chunks[x, y, z].chunkX = x * chunkSize;
            chunks[x, y, z].chunkY = y * chunkSize;
            chunks[x, y, z].chunkZ = z * chunkSize;
        }
    }

    public void UnloadColumn(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            Destroy(chunks[x, y, z].gameObject);
        }
    }

    int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = SimplexNoise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }

    public byte Block(int x, int y, int z)
    {

        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return (byte)1;
        }

        return data[x, y, z];
    }
}
