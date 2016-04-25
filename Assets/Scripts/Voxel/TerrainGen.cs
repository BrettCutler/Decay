using UnityEngine;
using System.Collections;

namespace Voxel
{
  public class TerrainGen
  {
    private float m_StoneBaseHeight = -24f;
    private float m_StoneBaseNoise = 0.05f;
    private float m_StoneBaseNoiseHeight = 4f;

    private float m_StoneMountainHeight = 48f;
    private float m_StoneMountainFrequency = 0.008f;
    private float m_StoneMinHeight = -12f;

    private float m_DirtBaseHeight = 1;
    private float m_DirtNoise = 0.04f;
    private float m_DirtNoiseHeight = 3f;

    private float m_CaveFrequency = 0.025f;
    private int m_CaveSize = 7;

    /// <summary>
    /// Fill a chunk with randomly-generated terrain.
    /// </summary>
    public Chunk ChunkGen( Chunk chunk )
    {
      for( int x = chunk.m_Pos.x; x < chunk.m_Pos.x + Chunk.k_ChunkSize; x++ )
        for( int z = chunk.m_Pos.z; z < chunk.m_Pos.z + Chunk.k_ChunkSize; z++ )
        {
          chunk = ChunkColumnGen( chunk, x, z );
        }

      return chunk;
    }

    public Chunk ChunkColumnGen( Chunk chunk, int x, int z )
    {
      int stoneHeight = Mathf.FloorToInt( m_StoneBaseHeight );
      stoneHeight += GetNoise( x, 0, z, m_StoneMountainFrequency, Mathf.FloorToInt( m_StoneMountainHeight ) );

      if( stoneHeight < m_StoneMinHeight )
      {
        stoneHeight = Mathf.FloorToInt( m_StoneMinHeight );
      }

      stoneHeight += GetNoise( x, 0, z, m_StoneBaseNoise, Mathf.FloorToInt( m_StoneBaseHeight ) );

      int dirtHeight = stoneHeight + Mathf.FloorToInt( m_DirtBaseHeight );
      dirtHeight += GetNoise( x, 100, z, m_DirtNoise, Mathf.FloorToInt( m_DirtNoiseHeight ) );

      for( int y = chunk.m_Pos.y; y < chunk.m_Pos.y + Chunk.k_ChunkSize; y++ )
      {
        int caveChance = GetNoise( x, y, z, m_CaveFrequency, 100 );

        if( y <= stoneHeight && m_CaveSize < caveChance )
        {
          chunk.SetBlock( x - chunk.m_Pos.x, y - chunk.m_Pos.y, z - chunk.m_Pos.z, new Block( ) );
        }
        else if( y <= dirtHeight && m_CaveSize < caveChance )
        {
          chunk.SetBlock( x - chunk.m_Pos.x, y - chunk.m_Pos.y, z - chunk.m_Pos.z, new BlockGrass( ) );
        }
        else
        {
          chunk.SetBlock( x - chunk.m_Pos.x, y - chunk.m_Pos.y, z - chunk.m_Pos.z, new BlockAir( ) );
        }
      }

      return chunk;
    }

    /// <summary>
    /// Wrapper for our noise generation.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="scale">Scale noise results to generate different output. Smaller scale presents smoother randomness, ideal for large terrain like mountains.</param>
    /// <param name="max">Return is in range [0, max]</param>
    /// <returns></returns>
    public static int GetNoise( int x, int y, int z, float scale, int max )
    {
      return Mathf.FloorToInt( ( VoxelSimplexNoise.Noise.Generate( x * scale, y * scale, z * scale ) + 1f ) * ( max * 0.5f ) );
    }
  }
}