using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using System.Threading;

public class ModifyTerrain : MonoBehaviour {

    VoxelMapGenerator voxelMapGenerator;
	GameObject cameraGO;
    ControllerLaserPoint controllerPointer;
    PlayerInputAction playerAction;

    // Use this for initialization
    void Start () {

        voxelMapGenerator = gameObject.GetComponent("VoxelMapGenerator") as VoxelMapGenerator;
		cameraGO=GameObject.FindGameObjectWithTag("MainCamera");
        controllerPointer = FindObjectOfType<ControllerLaserPoint>();
        playerAction = FindObjectOfType<PlayerInputAction>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && controllerPointer.GetIsPointerHover)
        {
            if(playerAction.GetCurrentMode == InputMode.AddBlock)
            {
                AddBlockCursor(255);
            }
            else
            {
                ReplaceBlockCursor(0);
            }
        }
        StartCoroutine(LoadChunks(GameObject.FindGameObjectWithTag("Player").transform.position, 128, 164));
    }
	
	public IEnumerator LoadChunks(Vector3 playerPos, float distToLoad, float distToUnload){
		for(int x=0;x<voxelMapGenerator.chunks.GetLength(0);x++){
            ZYChunkLoad(playerPos, distToLoad, distToUnload, x);

            yield return new WaitForEndOfFrame();
        }
    }

    public void ZYChunkLoad(Vector3 playerPos, float distToLoad, float distToUnload, int x)
    {
        for (int z = 0; z < voxelMapGenerator.chunks.GetLength(2); z++)
        {
            float dist = Vector2.Distance(new Vector2(x * voxelMapGenerator.chunkSize, z * voxelMapGenerator.chunkSize), new Vector2(playerPos.x, playerPos.z));

            if (dist < distToLoad)
            {
                if (voxelMapGenerator.chunks[x, 0, z] == null)
                {
                    voxelMapGenerator.GenColumn(x, z);
                }
            }
            else if (dist > distToUnload)
            {
                if (voxelMapGenerator.chunks[x, 0, z] != null)
                {
                    voxelMapGenerator.UnloadColumn(x, z);
                }
            }
        }
    }
	
	public void ReplaceBlockCenter(float range, byte block)
    { 
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.distance < range)
            {
                ReplaceBlockAt(hit, block);
            }
        }
    }

	public void AddBlockCenter(float range, byte block)
    {
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.distance < range)
            {
                AddBlockAt(hit, block);
            }
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }
	
	public void ReplaceBlockCursor(byte block)
    {
        //Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast (ray, out hit))
        //      {

        //	ReplaceBlockAt(hit, block);

        //}
        ReplaceBlockAt(controllerPointer.GetRaycast, block);
    }
	
	public void AddBlockCursor( byte block)
    {
        //Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast (ray, out hit))
        //      {

        //	AddBlockAt(hit, block);
        //	Debug.DrawLine(ray.origin,ray.origin+( ray.direction*hit.distance),Color.green,2);
        //}
        AddBlockAt(controllerPointer.GetRaycast, block);
    }
	
	public void ReplaceBlockAt(RaycastHit hit, byte block)
    {
			Vector3 position = hit.point;
			position+=(hit.normal*-0.5f);
			
			SetBlockAt(position, block);
	}
	
	public void AddBlockAt(RaycastHit hit, byte block)
    {
			Vector3 position = hit.point;
			position+=(hit.normal*0.5f);
			
			SetBlockAt(position,block);
	}
	
	public void SetBlockAt(Vector3 position, byte block)
    {
		int x= Mathf.RoundToInt( position.x );
		int y= Mathf.RoundToInt( position.y );
		int z= Mathf.RoundToInt( position.z );
		
		SetBlockAt(x,y,z,block);
	}
	
	public void SetBlockAt(int x, int y, int z, byte block)
    {
		print("### SetBlockAt :: " + x + ", " + y + ", " + z);
		
		if(voxelMapGenerator.data[x+1,y,z]==254){
			voxelMapGenerator.data[x+1,y,z]=255;
		}
		if(voxelMapGenerator.data[x-1,y,z]==254){
			voxelMapGenerator.data[x-1,y,z]=255;
		}
		if(voxelMapGenerator.data[x,y,z+1]==254){
			voxelMapGenerator.data[x,y,z+1]=255;
		}
		if(voxelMapGenerator.data[x,y,z-1]==254){
			voxelMapGenerator.data[x,y,z-1]=255;
		}
		if(voxelMapGenerator.data[x,y+1,z]==254){
			voxelMapGenerator.data[x,y+1,z]=255;
		}
		voxelMapGenerator.data[x,y,z]=block;
		
		UpdateChunkAt(x,y,z,block);
	
	}
	/// <summary>
    /// 해당 위치의 Chunk의 Update flag를 True로 바꿔주는 동시에 인근 Chunk들의 보이게 되는 면에 대한 처리도 동시에 진행.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="block"></param>
	public void UpdateChunkAt(int x, int y, int z, byte block)
    {		
        //To do: LateUpdate에서 해당 위치 기준의 Chunk업데이트 플래그 구현
		
		int updateX= Mathf.FloorToInt( x/voxelMapGenerator.chunkSize);
		int updateY= Mathf.FloorToInt( y/voxelMapGenerator.chunkSize);
		int updateZ= Mathf.FloorToInt( z/voxelMapGenerator.chunkSize);
		
		print("Updating: " + updateX + ", " + updateY + ", " + updateZ);
		
		
		voxelMapGenerator.chunks[updateX,updateY, updateZ].update=true;
		
		if(x-(voxelMapGenerator.chunkSize*updateX)==0 && updateX!=0)
        {
			voxelMapGenerator.chunks[updateX-1,updateY, updateZ].update=true;
		}
		
		if(x-(voxelMapGenerator.chunkSize*updateX)==15 && updateX!=voxelMapGenerator.chunks.GetLength(0)-1)
        {
			voxelMapGenerator.chunks[updateX+1,updateY, updateZ].update=true;
		}
		
		if(y-(voxelMapGenerator.chunkSize*updateY)==0 && updateY!=0)
        {
			voxelMapGenerator.chunks[updateX,updateY-1, updateZ].update=true;
		}
		
		if(y-(voxelMapGenerator.chunkSize*updateY)==15 && updateY!=voxelMapGenerator.chunks.GetLength(1)-1)
        {
			voxelMapGenerator.chunks[updateX,updateY+1, updateZ].update=true;
		}
		
		if(z-(voxelMapGenerator.chunkSize*updateZ)==0 && updateZ!=0)
        {
			voxelMapGenerator.chunks[updateX,updateY, updateZ-1].update=true;
		}
		
		if(z-(voxelMapGenerator.chunkSize*updateZ)==15 && updateZ!=voxelMapGenerator.chunks.GetLength(2)-1)
        {
			voxelMapGenerator.chunks[updateX,updateY, updateZ+1].update=true;
		}
		
	}
	
}
