using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

  public enum NDIR {UP, DOWN, LEFT, RIGHT, FRONT, BACK}

  


	static int        cWidth        = 16;
	static int        cHeight       = 16;
	static int        cDepth        = 16;
	public bool       isCubeShowing = false;
	public GameObject chunkPrefab   = null;
  public Chunk      chunkInstance = null;


  public static Vector3[,,] allVertices = new Vector3[cWidth+1, cHeight+1, cDepth+1];
	public static Vector3[]   allNormals  = new Vector3[6];




  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     BtnAction_ToggleChunkVisibility
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void BtnAction_ToggleChunkVisibility(){

    isCubeShowing = !isCubeShowing;

    if ( isCubeShowing )
      SpawnBlockChunk();

    else
      RemoveBlockChunk();

  }//END BtnAction_ToggleChunkVisibility



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     Start
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void Start(){

    SpawnBlockChunk();

  }//END Start



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     SpawnBlockChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void SpawnBlockChunk(){

		//generate all vertices
		for(int z = 0; z <= cDepth; z++)
			for(int y = 0; y <= cHeight; y++)
				for(int x = 0; x <= cWidth; x++){

					allVertices[x,y,z] = new Vector3(x,y,z);	 
				
        }//END for

		allNormals[(int) NDIR.UP] = Vector3.up;
		allNormals[(int) NDIR.DOWN] = Vector3.down;
		allNormals[(int) NDIR.LEFT] = Vector3.left;
		allNormals[(int) NDIR.RIGHT] = Vector3.right;
		allNormals[(int) NDIR.FRONT] = Vector3.forward;
		allNormals[(int) NDIR.BACK] = Vector3.back;

		//build chunk here
    GameObject newChunk = Instantiate( chunkPrefab,this.transform.position,this.transform.rotation );
		chunkInstance = newChunk.GetComponent<Chunk>();
    chunkInstance.CreateChunk( cWidth, cHeight, cDepth );

  }//END SpawnBlockChunk



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     RemoveBlockChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void RemoveBlockChunk(){

    if ( chunkInstance == null )
      return;

    GameObject  tempObj   = chunkInstance.gameObject;
    Mesh        tempMesh  = chunkInstance.myMesh;

    chunkInstance.myMesh  = null;
    chunkInstance         = null;

    Destroy( tempMesh );
    Destroy( tempObj );

  }//END RemoveBlockChunk
  

}//END class World
