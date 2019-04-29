using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMapDataManager : GameObjectSingleton<VoxelMapDataManager> {
    private Dictionary<VoxelChunkPositionData, VoxelChunkMeshData> VoxelMapDictionaryData = new Dictionary<VoxelChunkPositionData, VoxelChunkMeshData>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public static void SaveDataProcess(Chunk saveChunk)
    {
        if(saveChunk.GetChunkMesh.vertexCount > 0)
        {
            VoxelChunkPositionData saveKey = new VoxelChunkPositionData(saveChunk.transform.position);
            VoxelChunkMeshData saveValue = new VoxelChunkMeshData(saveChunk.GetChunkMesh);
        }
    }

    IEnumerator SaveProcessCoroutine()
    {
        yield return new WaitForEndOfFrame();
    }

    public static void LoadDataProcess()
    {

    }
}
