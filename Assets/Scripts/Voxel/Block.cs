using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for a block. Creates an even cube.
/// Child classes can be derived to place faces at different locations,
/// for example, to create slopes.
/// </summary>
[System.Serializable]
public class Block
{
  /// <summary>
  /// 1 / number of tiles per side of texture
  /// </summary>
  const float k_TileSize = 0.25f;

  public Block()
  {

  }

  public virtual MeshData Blockdata( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.m_UseRenderDataForCollider = true;

    if( !chunk.GetBlock( x, y, z + 1 ).IsSolid( Direction.Down ) )
    {
      meshData = FaceDataNorth( chunk, x, y, z, meshData );
    }
    if( !chunk.GetBlock( x, y, z - 1 ).IsSolid( Direction.Down ) )
    {
      meshData = FaceDataSouth( chunk, x, y, z, meshData );
    }
    if( !chunk.GetBlock( x + 1, y, z ).IsSolid( Direction.Down ) )
    {
      meshData = FaceDataEast( chunk, x, y, z, meshData );
    }
    if( !chunk.GetBlock( x - 1, y, z ).IsSolid( Direction.Down ) )
    {
      meshData = FaceDataWest( chunk, x, y, z, meshData );
    }
    if( !chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down ) )
    {
      meshData = FaceDataUp( chunk, x, y, z, meshData );
    }
    if( !chunk.GetBlock( x, y - 1, z ).IsSolid( Direction.Down ) )
    {
      meshData = FaceDataDown( chunk, x, y, z, meshData );
    }

    return meshData;
  }

  protected virtual MeshData FaceDataNorth( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.AddVertex( new Vector3( x + 0.5f, y - 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y + 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y + 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y - 0.5f, z + 0.5f ) );

    meshData.AddQuadTriangles();

    meshData.m_uv.AddRange( FaceUVs( Direction.North ) );

    return meshData;
  }
  protected virtual MeshData FaceDataSouth( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.AddVertex( new Vector3( x - 0.5f, y - 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y + 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y + 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y - 0.5f, z - 0.5f ) );

    meshData.AddQuadTriangles();

    meshData.m_uv.AddRange( FaceUVs( Direction.South ) );

    return meshData;
  }
  protected virtual MeshData FaceDataEast( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.AddVertex( new Vector3( x + 0.5f, y - 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y + 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y + 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y - 0.5f, z + 0.5f ) );

    meshData.AddQuadTriangles();

    meshData.m_uv.AddRange( FaceUVs( Direction.East ) );

    return meshData;
  }
  protected virtual MeshData FaceDataWest( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.AddVertex( new Vector3( x - 0.5f, y - 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y + 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y + 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y - 0.5f, z - 0.5f ) );

    meshData.AddQuadTriangles();

    meshData.m_uv.AddRange( FaceUVs( Direction.West ) );

    return meshData;
  }
  protected virtual MeshData FaceDataUp( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.AddVertex( new Vector3( x - 0.5f, y + 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y + 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y + 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y + 0.5f, z - 0.5f ) );

    meshData.AddQuadTriangles();

    meshData.m_uv.AddRange( FaceUVs( Direction.Up ) );

    return meshData;
  }
  protected virtual MeshData FaceDataDown( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    meshData.AddVertex( new Vector3( x - 0.5f, y - 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y - 0.5f, z - 0.5f ) );
    meshData.AddVertex( new Vector3( x + 0.5f, y - 0.5f, z + 0.5f ) );
    meshData.AddVertex( new Vector3( x - 0.5f, y - 0.5f, z + 0.5f ) );

    meshData.AddQuadTriangles();

    meshData.m_uv.AddRange( FaceUVs( Direction.Down ) );

    return meshData;
  }

  public enum Direction { North, East, South, West, Up, Down };

  public virtual bool IsSolid( Direction direction )
  {
    switch(direction)
    {
      case Direction.North:
        return true;
      case Direction.South:
        return true;
      case Direction.East:
        return true;
      case Direction.West:
        return true;
      case Direction.Up:
        return true;
      case Direction.Down:
        return true;
    }

    return false;
  }

  public struct Tile
  {
    public int x;
    public int y;
  }

  /// <summary>
  /// Return this block's position in the tile/texture sheet
  /// </summary>
  public virtual Tile TexturePosition(Direction direction)
  {
    Tile tile = new Tile();
    tile.x = 0;
    tile.y = 0;

    return tile;
  }

  public virtual Vector2[] FaceUVs(Direction direction)
  {
    Vector2[] UVs = new Vector2[4];
    Tile tilePos = TexturePosition( direction );

    UVs[0] = new Vector2( k_TileSize * tilePos.x + k_TileSize,
                          k_TileSize * tilePos.y );

    UVs[1] = new Vector2( k_TileSize * tilePos.x + k_TileSize,
                          k_TileSize * tilePos.y + k_TileSize );

    UVs[2] = new Vector2( k_TileSize * tilePos.x,
                          k_TileSize * tilePos.y + k_TileSize );

    UVs[3] = new Vector2( k_TileSize * tilePos.x,
                          k_TileSize * tilePos.y );

    return UVs;
  }
}
