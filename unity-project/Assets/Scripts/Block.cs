using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

	enum Cubeside {BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK};
	public enum BlockType {GRASS, DIRT, STONE, AIR};

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
	public Block(BlockType b, Vector3 pos, GameObject p, Material c){

		bType = b;
		parent = p;
		position = pos;
		cubeMaterial = c;

		if(bType == BlockType.AIR)
			isSolid = false;
		else
			isSolid = true;

		x = (int) pos.x;
		y = (int) pos.y;
		z = (int) pos.z;

	}//END Constructor



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     CreateQuad
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
	void CreateQuad(Cubeside side,List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t){

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
				trioffset = v.Count;
				v.Add(p0); v.Add(p1); v.Add(p2); v.Add(p3);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);
			break;

			case Cubeside.TOP:
				trioffset = v.Count;
				v.Add(p7); v.Add(p6); v.Add(p5); v.Add(p4);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);
			break;

			case Cubeside.LEFT:
				trioffset = v.Count;
				v.Add(p7); v.Add(p4); v.Add(p0); v.Add(p3);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);
			break;

			case Cubeside.RIGHT:
				trioffset = v.Count;
				v.Add(p5); v.Add(p6); v.Add(p2); v.Add(p1);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);
			break;

			case Cubeside.FRONT:
				trioffset = v.Count;
				v.Add(p4); v.Add(p5); v.Add(p1); v.Add(p0);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);
			break;

			case Cubeside.BACK:
				trioffset = v.Count;
				v.Add(p6); v.Add(p7); v.Add(p3); v.Add(p2);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);
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
	public void Draw(List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t){

		if ( bType == BlockType.AIR ) 
      return;

		if ( !HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z + 1) )
			CreateQuad( Cubeside.FRONT, v, n, u, t );

		if ( !HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z - 1) )
			CreateQuad( Cubeside.BACK, v, n, u, t );

    if ( !HasSolidNeighbour((int)position.x,(int)position.y + 1,(int)position.z) )
			CreateQuad( Cubeside.TOP, v, n, u, t );
		
    if ( !HasSolidNeighbour((int)position.x,(int)position.y - 1,(int)position.z) )
			CreateQuad( Cubeside.BOTTOM, v, n, u, t );
		
    if ( !HasSolidNeighbour((int)position.x - 1,(int)position.y,(int)position.z) )
			CreateQuad( Cubeside.LEFT, v, n, u, t );
		
    if ( !HasSolidNeighbour((int)position.x + 1,(int)position.y,(int)position.z) )
			CreateQuad( Cubeside.RIGHT, v, n, u, t );
	
  }//END Draw

}//END class Block

