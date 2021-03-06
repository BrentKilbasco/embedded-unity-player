﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

	
  private enum Cubeside {BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK};

  // Public data
	public  bool        isSolid;
	
  // Private data
  private BlockType   bType;
  private GameObject  parent;
	private Vector3     position;
	private Material    cubeMaterial;
  private int         x;
	private int         y;
	private int         z;
	

	Vector2[,] blockUVs = { 
		/*GRASS TOP*/		
      {new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
       new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		
      {new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
       new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			
      {new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
       new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*STONE*/			
      {new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
       new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )}
	}; 



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     Constructor
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	public Block(BlockType pBlockType, Vector3 pPos, GameObject pParent, Material pCubeMaterial){

		bType         = pBlockType;
		parent        = pParent;
		position      = pPos;
		cubeMaterial  = pCubeMaterial;

		if ( bType == BlockType.AIR )
			isSolid = false;
		else
			isSolid = true;

		x = (int)pPos.x;
		y = (int)pPos.y;
		z = (int)pPos.z;

	}//END Constructor



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     CreateQuad
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	void CreateQuad(Cubeside side,List<Vector3> pVerts, List<Vector3> pNormals, List<Vector2> pUVs, List<int> pTris){

		Vector3[] vertices  = new Vector3[4];
		Vector3[] normals   = new Vector3[4];
		Vector2[] uvs       = new Vector2[4];
		int[]     triangles = new int[6];

		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		if ( bType == BlockType.GRASS && side == Cubeside.TOP ){

			uv00 = blockUVs[0, 0];
			uv10 = blockUVs[0, 1];
			uv01 = blockUVs[0, 2];
			uv11 = blockUVs[0, 3];
		
    } else if ( bType == BlockType.GRASS && side == Cubeside.BOTTOM ) {

			uv00 = blockUVs[(int)(BlockType.DIRT+1),0];
			uv10 = blockUVs[(int)(BlockType.DIRT+1),1];
			uv01 = blockUVs[(int)(BlockType.DIRT+1),2];
			uv11 = blockUVs[(int)(BlockType.DIRT+1),3];
		
    } else {

			uv00 = blockUVs[(int)(bType+1),0];
			uv10 = blockUVs[(int)(bType+1),1];
			uv01 = blockUVs[(int)(bType+1),2];
			uv11 = blockUVs[(int)(bType+1),3];
		
    }//END if/else

		//all possible vertices 
		Vector3 p0 = World.allVertices[x,y,z+1];
		Vector3 p1 = World.allVertices[x+1,y,z+1];
		Vector3 p2 = World.allVertices[x+1,y,z];
		Vector3 p3 = World.allVertices[x,y,z];		 
		Vector3 p4 = World.allVertices[x,y+1,z+1];
		Vector3 p5 = World.allVertices[x+1,y+1,z+1];
		Vector3 p6 = World.allVertices[x+1,y+1,z];
		Vector3 p7 = World.allVertices[x,y+1,z];
		
		int trioffset = 0;

		switch( side ){

			case Cubeside.BOTTOM:
				trioffset = pVerts.Count;
				pVerts.Add(p0); pVerts.Add(p1); pVerts.Add(p2); pVerts.Add(p3);
				pNormals.Add(World.allNormals[(int)World.NDIR.DOWN]);
				pNormals.Add(World.allNormals[(int)World.NDIR.DOWN]);
				pNormals.Add(World.allNormals[(int)World.NDIR.DOWN]);
				pNormals.Add(World.allNormals[(int)World.NDIR.DOWN]);
				pUVs.Add(uv11); pUVs.Add(uv01); pUVs.Add(uv00); pUVs.Add(uv10);
				pTris.Add(3 + trioffset); pTris.Add(1 + trioffset); pTris.Add(0 + trioffset); 
        pTris.Add(3 + trioffset); pTris.Add(2 + trioffset); pTris.Add(1 + trioffset);
			break;

			case Cubeside.TOP:
				trioffset = pVerts.Count;
				pVerts.Add(p7); pVerts.Add(p6); pVerts.Add(p5); pVerts.Add(p4);
				pNormals.Add(World.allNormals[(int)World.NDIR.UP]);
				pNormals.Add(World.allNormals[(int)World.NDIR.UP]);
				pNormals.Add(World.allNormals[(int)World.NDIR.UP]);
				pNormals.Add(World.allNormals[(int)World.NDIR.UP]);
				pUVs.Add(uv11); pUVs.Add(uv01); pUVs.Add(uv00); pUVs.Add(uv10);
				pTris.Add(3 + trioffset); pTris.Add(1 + trioffset); pTris.Add(0 + trioffset); 
        pTris.Add(3 + trioffset); pTris.Add(2 + trioffset); pTris.Add(1 + trioffset);
			break;

			case Cubeside.LEFT:
				trioffset = pVerts.Count;
				pVerts.Add(p7); pVerts.Add(p4); pVerts.Add(p0); pVerts.Add(p3);
				pNormals.Add(World.allNormals[(int)World.NDIR.LEFT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.LEFT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.LEFT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.LEFT]);
				pUVs.Add(uv11); pUVs.Add(uv01); pUVs.Add(uv00); pUVs.Add(uv10);
				pTris.Add(3 + trioffset); pTris.Add(1 + trioffset); pTris.Add(0 + trioffset); 
        pTris.Add(3 + trioffset); pTris.Add(2 + trioffset); pTris.Add(1 + trioffset);
			break;

			case Cubeside.RIGHT:
				trioffset = pVerts.Count;
				pVerts.Add(p5); pVerts.Add(p6); pVerts.Add(p2); pVerts.Add(p1);
				pNormals.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				pUVs.Add(uv11); pUVs.Add(uv01); pUVs.Add(uv00); pUVs.Add(uv10);
				pTris.Add(3 + trioffset); pTris.Add(1 + trioffset); pTris.Add(0 + trioffset); 
        pTris.Add(3 + trioffset); pTris.Add(2 + trioffset); pTris.Add(1 + trioffset);
			break;

			case Cubeside.FRONT:
				trioffset = pVerts.Count;
				pVerts.Add(p4); pVerts.Add(p5); pVerts.Add(p1); pVerts.Add(p0);
				pNormals.Add(World.allNormals[(int)World.NDIR.FRONT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.FRONT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.FRONT]);
				pNormals.Add(World.allNormals[(int)World.NDIR.FRONT]);
				pUVs.Add(uv11); pUVs.Add(uv01); pUVs.Add(uv00); pUVs.Add(uv10);
				pTris.Add(3 + trioffset); pTris.Add(1 + trioffset); pTris.Add(0 + trioffset); 
        pTris.Add(3 + trioffset); pTris.Add(2 + trioffset); pTris.Add(1 + trioffset);
			break;

			case Cubeside.BACK:
				trioffset = pVerts.Count;
				pVerts.Add(p6); pVerts.Add(p7); pVerts.Add(p3); pVerts.Add(p2);
				pNormals.Add(World.allNormals[(int)World.NDIR.BACK]);
				pNormals.Add(World.allNormals[(int)World.NDIR.BACK]);
				pNormals.Add(World.allNormals[(int)World.NDIR.BACK]);
				pNormals.Add(World.allNormals[(int)World.NDIR.BACK]);
				pUVs.Add(uv11); pUVs.Add(uv01); pUVs.Add(uv00); pUVs.Add(uv10);
				pTris.Add(3 + trioffset); pTris.Add(1 + trioffset); pTris.Add(0 + trioffset); 
        pTris.Add(3 + trioffset); pTris.Add(2 + trioffset); pTris.Add(1 + trioffset);
			break;
		
    }//END switch

	}//END CreateQuad



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     HasSolidNeighbour
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	public bool HasSolidNeighbour(int x, int y, int z){

		Block[,,] chunks = parent.GetComponent<Chunk>().chunkData;
		try {
			return chunks[x,y,z].isSolid;

		} catch(System.IndexOutOfRangeException){}

		return false;

	}//END HasSolidNeighbour


  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     Draw
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	public void Draw(List<Vector3> pVerts, List<Vector3> pNormals, List<Vector2> pUVs, List<int> pTris){

		if ( bType == BlockType.AIR ) 
      return;

		if ( !HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z + 1) )
			CreateQuad( Cubeside.FRONT, pVerts, pNormals, pUVs, pTris );

		if ( !HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z - 1) )
			CreateQuad( Cubeside.BACK, pVerts, pNormals, pUVs, pTris );

    if ( !HasSolidNeighbour((int)position.x,(int)position.y + 1,(int)position.z) )
			CreateQuad( Cubeside.TOP, pVerts, pNormals, pUVs, pTris );
		
    if ( !HasSolidNeighbour((int)position.x,(int)position.y - 1,(int)position.z) )
			CreateQuad( Cubeside.BOTTOM, pVerts, pNormals, pUVs, pTris );
		
    if ( !HasSolidNeighbour((int)position.x - 1,(int)position.y,(int)position.z) )
			CreateQuad( Cubeside.LEFT, pVerts, pNormals, pUVs, pTris );
		
    if ( !HasSolidNeighbour((int)position.x + 1,(int)position.y,(int)position.z) )
			CreateQuad( Cubeside.RIGHT, pVerts, pNormals, pUVs, pTris );
	
  }//END Draw

}//END class Block

