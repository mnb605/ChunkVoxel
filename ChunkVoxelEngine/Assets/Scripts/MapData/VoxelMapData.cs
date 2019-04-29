using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelChunkPositionData
{
    public byte[] x;
    public byte[] y;
    public byte[] z;

    public VoxelChunkPositionData(Vector3 ChunkPos)
    {
        x = new byte[2];
        y = new byte[2];
        z = new byte[2];

        x[1] = (byte)((ChunkPos.x + 0.5d) / 255);
        x[0] = (byte)((ChunkPos.x + 0.5d) % 255);

        y[1] = (byte)((ChunkPos.y - 0.5d) / 255);
        y[0] = (byte)((ChunkPos.y - 0.5d) % 255);

        z[1] = (byte)((ChunkPos.z + 0.5d) / 255);
        z[0] = (byte)((ChunkPos.z + 0.5d) % 255);
    }

    ~VoxelChunkPositionData()
    {

    }
}

public class VoxelChunkMeshData {
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] tris;

    public VoxelChunkMeshData(Mesh meshData)
    {
        vertices = new Vector3[meshData.vertexCount];
        uvs = new Vector2[meshData.uv.Length];
        tris = new int[meshData.triangles.Length];
    }

    ~VoxelChunkMeshData()
    {

    }
}
