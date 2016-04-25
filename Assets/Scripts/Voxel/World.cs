using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
  public class World : MonoBehaviour
  {
    public Dictionary<WorldPos, Chunk> m_Chunks = new Dictionary<WorldPos, Chunk>();

    public GameObject m_ChunkPrefab;

    public bool m_GenerateRandomTerrain;

    /// <summary>
    /// Identifies this world's save file
    /// </summary>
    public string m_WorldName = "World";


    //public int d_NewChunkX;
    //public int d_NewChunkY;
    //public int d_NewChunkZ;

    //public bool d_GenChunk;

    private void Awake( )
    {
      DebugCreateSampleChunk( );
    }

    //void Update( )
    //{
    //  DebugTryGenChunk( );
    //}

    /// <summary>
    /// Create a chunk at supplied WorldPos.
    /// Will first try to load a saved chunk at this pos.
    /// </summary>
    public void CreateChunk( int x, int y, int z )
    {
      WorldPos worldPos = new WorldPos(x, y, z);

      // Instantiate the chunk at the coordinates using the chunk prefab
      GameObject newChunkObject = Instantiate( m_ChunkPrefab, new Vector3(x, y, z ), Quaternion.identity )
                                              as GameObject;

      Chunk newChunk = newChunkObject.GetComponent<Chunk>();

      newChunk.m_Pos = worldPos;
      newChunk.m_World = this;

      m_Chunks.Add( worldPos, newChunk );

      if( m_GenerateRandomTerrain )
      {
        TerrainGen terrainGen = new TerrainGen();
        newChunk = terrainGen.ChunkGen( newChunk );
      }

      newChunk.SetBlocksUnmodified( );

      Serialization.LoadChunk( newChunk );

      //DebugFillSampleChunk( x, y, z );
    }

    public void DestroyChunk( int x, int y, int z )
    {
      Chunk chunk = null;
      if( m_Chunks.TryGetValue( new WorldPos( x, y, z ), out chunk ) )
      {
        Serialization.SaveChunk( chunk );
        UnityEngine.Object.Destroy( chunk.gameObject );
        m_Chunks.Remove( new WorldPos( x, y, z ) );
      }

    }

    public Chunk GetChunk( int x, int y, int z )
    {
      WorldPos pos = new WorldPos();

      // Truncate the passed-in postion to get the worldPos
      float multiple = Chunk.k_ChunkSize;
      pos.x = Mathf.FloorToInt( x / multiple ) * Chunk.k_ChunkSize;
      pos.y = Mathf.FloorToInt( y / multiple ) * Chunk.k_ChunkSize;
      pos.z = Mathf.FloorToInt( z / multiple ) * Chunk.k_ChunkSize;

      Chunk containerChunk = null;

      m_Chunks.TryGetValue( pos, out containerChunk );

      return containerChunk;
    }

    public Block GetBlock( int x, int y, int z )
    {
      Chunk containerChunk = GetChunk( x, y, z );

      if( containerChunk != null )
      {
        Block block = containerChunk.GetBlock( x - containerChunk.m_Pos.x,
                                             y - containerChunk.m_Pos.y,
                                             z - containerChunk.m_Pos.z );
        return block;
      }
      else
      {
        return new BlockAir( );
      }
    }

    public void SetBlock( int x, int y, int z, Block block )
    {
      Chunk chunk = GetChunk( x, y, z );

      if( chunk != null )
      {
        chunk.SetBlock( x - chunk.m_Pos.x, y - chunk.m_Pos.y, z - chunk.m_Pos.z, block );

        // If we're on the border of the chunk, update neighbor
        UpdateIfEqual( x - chunk.m_Pos.x, 0, new WorldPos( x - 1, y, z ) );
        UpdateIfEqual( x - chunk.m_Pos.x, Chunk.k_ChunkSize - 1, new WorldPos( x + 1, y, z ) );
        UpdateIfEqual( y - chunk.m_Pos.y, 0, new WorldPos( x, y - 1, z ) );
        UpdateIfEqual( y - chunk.m_Pos.y, Chunk.k_ChunkSize - 1, new WorldPos( x, y + 1, z ) );
        UpdateIfEqual( z - chunk.m_Pos.z, 0, new WorldPos( x, y, z - 1 ) );
        UpdateIfEqual( z - chunk.m_Pos.z, Chunk.k_ChunkSize - 1, new WorldPos( x, y, z + 1 ) );
      }
    }

    private void UpdateIfEqual( int value1, int value2, WorldPos pos )
    {
      if( value1 == value2 )
      {
        Chunk chunk = GetChunk( pos.x, pos.y, pos.z );
        if( chunk != null )
        {
          chunk.m_Update = true;
        }
      }
    }

    private void DebugCreateSampleChunk( )
    {
      for( int x = -4; x < 4; x++ )
        for( int y = -4; y < 7; y++ )
          for( int z = -4; z < 4; z++ )
          {
            CreateChunk( x * 16, y * 16, z * 16 );
          }
    }

    private void DebugFillSampleChunk( int x, int y, int z )
    {
      for( int xi = 0; xi < 16; xi++ )
        for( int yi = 0; yi < 16; yi++ )
          for( int zi = 0; zi < 16; zi++ )
          {
            if( yi <= 7 )
            {
              SetBlock( x + xi, y + yi, z + zi, new BlockGrass( ) );
            }
            else
            {
              SetBlock( x + xi, y + yi, z + zi, new BlockAir( ) );
            }
          }
    }

    private void DebugTryGenChunk( )
    {
      //if( d_GenChunk )
      //{
      //  d_GenChunk = false;
      //  WorldPos chunkPos = new WorldPos( d_NewChunkX, d_NewChunkY, d_NewChunkZ );
      //  Chunk chunk = null;

      //  if( m_Chunks.TryGetValue( chunkPos, out chunk ) )
      //  {
      //    DestroyChunk( chunkPos.x, chunkPos.y, chunkPos.z );
      //  }
      //  else
      //  {
      //    CreateChunk( chunkPos.x, chunkPos.y, chunkPos.z );
      //  }
      //}
    }
  }
}