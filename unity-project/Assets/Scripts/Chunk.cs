using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

  public  Mesh      myMesh;
	public  Material  cubeMaterial;
	public  Block[,,] chunkData;
	private int       cw;
	private int       ch;
	private int       cd;


	private List<Vector3> verts   = new List<Vector3>();
	private List<Vector3> norms   = new List<Vector3>();
	private List<Vector2> uvs     = new List<Vector2>();
	private List<int>     tris    = new List<int>();



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     BuildChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	void BuildChunk(int pSizeX, int pSizeY, int pSizeZ){

		chunkData = new Block[pSizeX, pSizeY, pSizeZ];
		cw = pSizeX;
		ch = pSizeY;
		cd = pSizeZ;

		//create blocks
		for( int z = 0; z < pSizeZ; z++ )
			for( int y = 0; y < pSizeY; y++ )
				for( int x = 0; x < pSizeX; x++ ){

					Vector3 pos = new Vector3( x, y, z );

					if ( Random.Range(0,100) < 50 )
						chunkData[x,y,z] = new Block( BlockType.DIRT, pos, this.gameObject, cubeMaterial );

					else
						chunkData[x,y,z] = new Block( BlockType.AIR, pos, this.gameObject, cubeMaterial );
				
        }//END for

	}//END BuildChunk



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     DrawChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	public void DrawChunk(){

		//draw blocks
		verts.Clear();
		norms.Clear();
		uvs.Clear();
		tris.Clear();

		for(int z = 0; z < cd; z++)
			for(int y = 0; y < ch; y++)
				for(int x = 0; x < cw; x++)
					chunkData[x,y,z].Draw( verts, norms, uvs, tris );   

		Mesh mesh = new Mesh();
	  mesh.name = "ScriptedMesh"; 

		mesh.vertices   = verts.ToArray();
		mesh.normals    = norms.ToArray();
		mesh.uv         = uvs.ToArray();
		mesh.triangles  = tris.ToArray();
		 
		mesh.RecalculateBounds();

		MeshFilter meshFilter = (MeshFilter) this.gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

    // Keep a cache of the mesh so we can destroy it later
    myMesh = mesh;

		MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
		renderer.material = cubeMaterial;
	
  }//END DrawChunk


	
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     CreateChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	public void CreateChunk(int pWidth, int pHeight, int pDepth) {

		BuildChunk( pWidth, pHeight, pDepth );

    DrawChunk();

	}//END CreateChunk
	

}//END class Chunk

