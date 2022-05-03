using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandler : MonoBehaviour
{
    public int nXPerChunk;
    public int nYPerChunk;
    public int nZPerChunk;
    public float chunkGridSize;

    public Material chunkMaterial;
    public float thresholdValue;
    
    float chunkXDimension;
    float chunkYDimension;

    float chunkZDimension;

    Dictionary<Vector3, Chunk> chunkHashMap = new Dictionary<Vector3, Chunk>();

    void Start()
    {
        chunkXDimension = nXPerChunk * chunkGridSize;
        chunkYDimension = nYPerChunk * chunkGridSize;
        chunkZDimension = nZPerChunk * chunkGridSize;
    }

    public void AddChunk(Vector3 positionChunkCenter)
    {
        chunkHashMap.Add(positionChunkCenter, new Chunk(positionChunkCenter, nXPerChunk, nYPerChunk, nZPerChunk, chunkGridSize, thresholdValue, chunkMaterial));
    }

    public void AddChunkFromPoint(Vector3 position)
    {
        
        chunkHashMap.Add(positionChunkCenter, new Chunk(chunkGetNearestChunkCenter(position), nXPerChunk, nYPerChunk, nZPerChunk, chunkGridSize, thresholdValue, chunkMaterial));
    }


    public Chunk GetChunkFromPosition(Vector3 position)
    {
        Vector3 nearestChunkCenter = GetNearestChunkCenter(position);

        if (chunkHashMap.ContainsKey(nearestChunkCenter)) {return chunkHashMap[chunkHashMap]; }

        return null;
    }

    public Vector3 GetNearestChunkCenter(Vector3 position)
    {
        Vector3 returnVector = new Vector3(0f,0f,0f);

        returnVector.x = Mathf.Floor(position.x, chunkXDimension) * chunkXDimension;
        returnVector.y = Mathf.Floor(position.y, chunkYDimension) * chunkYDimension;
        returnVector.z = Mathf.Floor(position.z, chunkZDimension) * chunkZDimension;

        return returnVector;
    }


}

public class Chunk
{
    public Vector3 positionChunkCenter;
    public Mesh mesh;

    public ScalarFieldPoint[] scalarField;

    public float thresholdValue;

    GameObject chunkGameObject;
    GameObject marchingTerrain;

    int nX;
    int nY;
    int nZ;
    float gridSize;

    public bool chunkVisible;

    public Chunk(Vector3 positionChunkCenter, int nX, int nY, int nZ, float gridSize, float thresholdValue, Material material)
    {
        this.positionChunkCenter = positionChunkCenter;
        this.nX = nX ;
        this.nY = nY ;
        this.nZ = nZ ;
        this.gridSize = gridSize;

        this.chunkVisible = true;

        /*
        scalarField = new ScalarFieldPoint[this.nX * this.nY * this.nZ];

        for (int i = 0; i < this.nX * this.nY * this.nZ; i++)
        {
            scalarField[i] = new ScalarFieldPoint();
        }
        */

        chunkGameObject = new GameObject("Marching Cubes Chunk");
        chunkGameObject.AddComponent<MeshFilter>();
        chunkGameObject.AddComponent<MeshRenderer>();
        chunkGameObject.GetComponent<Renderer>().material = material;

        marchingTerrain = GameObject.Find("MarchingTerrain");

        InitializeScalarField();
        RebuildChunkMesh();

        chunkGameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void RebuildChunkMesh()
    {
        mesh = marchingTerrain.GetComponent<MarchingCubes>().GetMeshFromField(scalarField, thresholdValue);
    }

    public void InitializeScalarField()
    {
        scalarField = marchingTerrain.GetComponent<NoiseTerrain>().InitializeScalarField(nX, nY, nZ, gridSize, positionChunkCenter);
    }

    public void HideChunk()
    {
        chunkGameObject.SetActive(false);
        chunkVisible = false;
    }

    public void ShowChunk()
    {
        chunkGameObject.SetActive(true);
        chunkVisible = true;
    }

}
