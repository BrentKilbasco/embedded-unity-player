using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

  public Mesh myMesh;
	public Material cubeMaterial;
	public Block[,,] chunkData;
	int cw;
	int ch;
	int cd;

	List<Vector3> Verts = new List<Vector3>();
	List<Vector3> Norms = new List<Vector3>();
	List<Vector2> UVs = new List<Vector2>();
	List<int> Tris = new List<int>();



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     BuildChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	void BuildChunk(int sizeX, int sizeY, int sizeZ){

		chunkData = new Block[sizeX, sizeY, sizeZ];
		cw = sizeX;
		ch = sizeY;
		cd = sizeZ;

		//create blocks
		for( int z = 0; z < sizeZ; z++ )
			for( int y = 0; y < sizeY; y++ )
				for( int x = 0; x < sizeX; x++ ){

					Vector3 pos = new Vector3( x, y, z );

					if ( Random.Range(0,100) < 50 )
						chunkData[x,y,z] = new Block( Block.BlockType.DIRT, pos, this.gameObject, cubeMaterial );

					else
						chunkData[x,y,z] = new Block( Block.BlockType.AIR, pos, this.gameObject, cubeMaterial );
				
        }//END for

	}//END BuildChunk



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     DrawChunk
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	public void DrawChunk(){

		//draw blocks
		Verts.Clear();
		Norms.Clear();
		UVs.Clear();
		Tris.Clear();

		for(int z = 0; z < cd; z++)
			for(int y = 0; y < ch; y++)
				for(int x = 0; x < cw; x++)
					chunkData[x,y,z].Draw(Verts, Norms, UVs, Tris);   

		Mesh mesh = new Mesh();
	  mesh.name = "ScriptedMesh"; 

		mesh.vertices = Verts.ToArray();
		mesh.normals = Norms.ToArray();
		mesh.uv = UVs.ToArray();
		mesh.triangles = Tris.ToArray();
		 
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
	public void CreateChunk(int w, int h, int d) {

		BuildChunk( w, h, d );

    DrawChunk();

	}//END CreateChunk
	

}//END class Chunk

