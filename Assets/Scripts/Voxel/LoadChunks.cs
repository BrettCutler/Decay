using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
  /// <summary>
  /// This script loads/unloads chunks based on proximity to its transform.
  /// Attach to player or camera.
  /// </summary>
  public class LoadChunks : MonoBehaviour
  {
    public World m_World;

    private int m_DeleteFrameTimer = 0;

    private List<WorldPos> m_UpdateList = new List<WorldPos>();
    private List<WorldPos> m_BuildList = new List<WorldPos>();

    private const int k_ChunksToLoadPerFrame = 4;
    private const int k_DeleteChunksFrameFrequency = 10;
    private const int k_DeleteChunkDistance = 256;

    /// <summary>
    /// Collection of chunk offsets to check from our target position for changes
    /// </summary>
    static  WorldPos[] k_ChunkPositions = {
    new WorldPos( 0, 0,  0), new WorldPos(-1, 0,  0), new WorldPos( 0, 0, -1), new WorldPos( 0, 0,  1), new WorldPos( 1, 0,  0),
    new WorldPos(-1, 0, -1), new WorldPos(-1, 0,  1), new WorldPos( 1, 0, -1), new WorldPos( 1, 0,  1), new WorldPos(-2, 0,  0),
    new WorldPos( 0, 0, -2), new WorldPos( 0, 0,  2), new WorldPos( 2, 0,  0), new WorldPos(-2, 0, -1), new WorldPos(-2, 0,  1),
    new WorldPos(-1, 0, -2), new WorldPos(-1, 0,  2), new WorldPos( 1, 0, -2), new WorldPos( 1, 0,  2), new WorldPos( 2, 0, -1),
    new WorldPos( 2, 0,  1), new WorldPos(-2, 0, -2), new WorldPos(-2, 0,  2), new WorldPos( 2, 0, -2), new WorldPos( 2, 0,  2),
    new WorldPos(-3, 0,  0), new WorldPos( 0, 0, -3), new WorldPos( 0, 0,  3), new WorldPos( 3, 0,  0), new WorldPos(-3, 0, -1),
    new WorldPos(-3, 0,  1), new WorldPos(-1, 0, -3), new WorldPos(-1, 0,  3), new WorldPos( 1, 0, -3), new WorldPos( 1, 0,  3),
    new WorldPos( 3, 0, -1), new WorldPos( 3, 0,  1), new WorldPos(-3, 0, -2), new WorldPos(-3, 0,  2), new WorldPos(-2, 0, -3),
    new WorldPos(-2, 0,  3), new WorldPos( 2, 0, -3), new WorldPos( 2, 0,  3), new WorldPos( 3, 0, -2), new WorldPos( 3, 0,  2),
    new WorldPos(-4, 0,  0), new WorldPos( 0, 0, -4), new WorldPos( 0, 0,  4), new WorldPos( 4, 0,  0), new WorldPos(-4, 0, -1),
    new WorldPos(-4, 0,  1), new WorldPos(-1, 0, -4), new WorldPos(-1, 0,  4), new WorldPos( 1, 0, -4), new WorldPos( 1, 0,  4),
    new WorldPos( 4, 0, -1), new WorldPos( 4, 0,  1), new WorldPos(-3, 0, -3), new WorldPos(-3, 0,  3), new WorldPos( 3, 0, -3),
    new WorldPos( 3, 0,  3), new WorldPos(-4, 0, -2), new WorldPos(-4, 0,  2), new WorldPos(-2, 0, -4), new WorldPos(-2, 0,  4),
    new WorldPos( 2, 0, -4), new WorldPos( 2, 0,  4), new WorldPos( 4, 0, -2), new WorldPos( 4, 0,  2), new WorldPos(-5, 0,  0),
    new WorldPos(-4, 0, -3), new WorldPos(-4, 0,  3), new WorldPos(-3, 0, -4), new WorldPos(-3, 0,  4), new WorldPos( 0, 0, -5),
    new WorldPos( 0, 0,  5), new WorldPos( 3, 0, -4), new WorldPos( 3, 0,  4), new WorldPos( 4, 0, -3), new WorldPos( 4, 0,  3),
    new WorldPos( 5, 0,  0), new WorldPos(-5, 0, -1), new WorldPos(-5, 0,  1), new WorldPos(-1, 0, -5), new WorldPos(-1, 0,  5),
    new WorldPos( 1, 0, -5), new WorldPos( 1, 0,  5), new WorldPos( 5, 0, -1), new WorldPos( 5, 0,  1), new WorldPos(-5, 0, -2),
    new WorldPos(-5, 0,  2), new WorldPos(-2, 0, -5), new WorldPos(-2, 0,  5), new WorldPos( 2, 0, -5), new WorldPos( 2, 0,  5),
    new WorldPos( 5, 0, -2), new WorldPos( 5, 0,  2), new WorldPos(-4, 0, -4), new WorldPos(-4, 0,  4), new WorldPos( 4, 0, -4),
    new WorldPos( 4, 0,  4), new WorldPos(-5, 0, -3), new WorldPos(-5, 0,  3), new WorldPos(-3, 0, -5), new WorldPos(-3, 0,  5),
    new WorldPos( 3, 0, -5), new WorldPos( 3, 0,  5), new WorldPos( 5, 0, -3), new WorldPos( 5, 0,  3), new WorldPos(-6, 0,  0),
    new WorldPos( 0, 0, -6), new WorldPos( 0, 0,  6), new WorldPos( 6, 0,  0), new WorldPos(-6, 0, -1), new WorldPos(-6, 0,  1),
    new WorldPos(-1, 0, -6), new WorldPos(-1, 0,  6), new WorldPos( 1, 0, -6), new WorldPos( 1, 0,  6), new WorldPos( 6, 0, -1),
    new WorldPos( 6, 0,  1), new WorldPos(-6, 0, -2), new WorldPos(-6, 0,  2), new WorldPos(-2, 0, -6), new WorldPos(-2, 0,  6),
    new WorldPos( 2, 0, -6), new WorldPos( 2, 0,  6), new WorldPos( 6, 0, -2), new WorldPos( 6, 0,  2), new WorldPos(-5, 0, -4),
    new WorldPos(-5, 0,  4), new WorldPos(-4, 0, -5), new WorldPos(-4, 0,  5), new WorldPos( 4, 0, -5), new WorldPos( 4, 0,  5),
    new WorldPos( 5, 0, -4), new WorldPos( 5, 0,  4), new WorldPos(-6, 0, -3), new WorldPos(-6, 0,  3), new WorldPos(-3, 0, -6),
    new WorldPos(-3, 0,  6), new WorldPos( 3, 0, -6), new WorldPos( 3, 0,  6), new WorldPos( 6, 0, -3), new WorldPos( 6, 0,  3),
    new WorldPos(-7, 0,  0), new WorldPos( 0, 0, -7), new WorldPos( 0, 0,  7), new WorldPos( 7, 0,  0), new WorldPos(-7, 0, -1),
    new WorldPos(-7, 0,  1), new WorldPos(-5, 0, -5), new WorldPos(-5, 0,  5), new WorldPos(-1, 0, -7), new WorldPos(-1, 0,  7),
    new WorldPos( 1, 0, -7), new WorldPos( 1, 0,  7), new WorldPos( 5, 0, -5), new WorldPos( 5, 0,  5), new WorldPos( 7, 0, -1),
    new WorldPos( 7, 0,  1), new WorldPos(-6, 0, -4), new WorldPos(-6, 0,  4), new WorldPos(-4, 0, -6), new WorldPos(-4, 0,  6),
    new WorldPos( 4, 0, -6), new WorldPos( 4, 0,  6), new WorldPos( 6, 0, -4), new WorldPos( 6, 0,  4), new WorldPos(-7, 0, -2),
    new WorldPos(-7, 0,  2), new WorldPos(-2, 0, -7), new WorldPos(-2, 0,  7), new WorldPos( 2, 0, -7), new WorldPos( 2, 0,  7),
    new WorldPos( 7, 0, -2), new WorldPos( 7, 0,  2), new WorldPos(-7, 0, -3), new WorldPos(-7, 0,  3), new WorldPos(-3, 0, -7),
    new WorldPos(-3, 0,  7), new WorldPos( 3, 0, -7), new WorldPos( 3, 0,  7), new WorldPos( 7, 0, -3), new WorldPos( 7, 0,  3),
    new WorldPos(-6, 0, -5), new WorldPos(-6, 0,  5), new WorldPos(-5, 0, -6), new WorldPos(-5, 0,  6), new WorldPos( 5, 0, -6),
    new WorldPos( 5, 0,  6), new WorldPos( 6, 0, -5), new WorldPos( 6, 0,  5) };


    private void Update( )
    {
      if( DeleteChunks( ) )
      {
        // exit early if we deleted chunks as we've already done much work this frame
        return;
      }

      FindChunksToLoad( );
      LoadAndRenderChunks( );
    }


    private void FindChunksToLoad( )
    {
      // Get the position of this game object to generate around
      WorldPos playerPos = new WorldPos(
      Mathf.FloorToInt( transform.position.x / Chunk.k_ChunkSize ) * Chunk.k_ChunkSize,
      Mathf.FloorToInt( transform.position.y / Chunk.k_ChunkSize ) * Chunk.k_ChunkSize,
      Mathf.FloorToInt( transform.position.z / Chunk.k_ChunkSize ) * Chunk.k_ChunkSize );

      // If there aren't any chunks to generate
      if( m_UpdateList.Count == 0 )
      {
        // Cycle through the array of positions
        for( int i = 0; i < k_ChunkPositions.Length; i++ )
        {
          // translate game obj position and array position into chunk position
          WorldPos newChunkPos = new WorldPos(
          k_ChunkPositions[i].x * Chunk.k_ChunkSize + playerPos.x,
          0,
          k_ChunkPositions[i].z * Chunk.k_ChunkSize + playerPos.z );

          // Get the chunk in the defined position
          Chunk newChunk = m_World.GetChunk(
          newChunkPos.x, newChunkPos.y, newChunkPos.z );

          // If the chunk already exists and it's already being rendered or in queue to be rendered, continue
          if( newChunk != null && ( newChunk.m_Rendered || m_UpdateList.Contains( newChunkPos ) ) )
          {
            continue;
          }

          // Load a column of chunks in this position
          for( int y = -4; y < 4; y++ )
          {
            m_BuildList.Add( new WorldPos( newChunkPos.x, y * Chunk.k_ChunkSize, newChunkPos.z ) );
          }

          for( int y = -4; y < 4; y++ )
          {
            for( int x = newChunkPos.x - Chunk.k_ChunkSize; x <= newChunkPos.x + Chunk.k_ChunkSize; x += Chunk.k_ChunkSize )
            {
              for( int z = newChunkPos.z - Chunk.k_ChunkSize; z <= newChunkPos.z + Chunk.k_ChunkSize; z += Chunk.k_ChunkSize )
              {
                m_BuildList.Add( new WorldPos( x, y * Chunk.k_ChunkSize, z ) );
              }
            }

            m_UpdateList.Add( new WorldPos( newChunkPos.x, y * Chunk.k_ChunkSize, newChunkPos.z ) );
          }

        }
      }
    }

    private void BuildChunk( WorldPos pos )
    {
      if( m_World.GetChunk( pos.x, pos.y, pos.z ) == null )
      {
        m_World.CreateChunk( pos.x, pos.y, pos.z );
      }
    }

    private void LoadAndRenderChunks( )
    {
      if( m_BuildList.Count != 0 )
      {
        for( int i = 0; i < m_BuildList.Count && i < k_ChunksToLoadPerFrame; i++ )
        {
          BuildChunk( m_BuildList[0] );
          m_BuildList.RemoveAt( 0 );
        }

        // If chunks were built, return early
        return;
      }

      for( int i = 0; i < m_UpdateList.Count; i++ )
      {
        Chunk chunk = m_World.GetChunk( m_UpdateList[0].x, m_UpdateList[0].y, m_UpdateList[0].z );
        if( chunk != null )
        {
          chunk.m_Update = true;
        }
        m_UpdateList.RemoveAt( 0 );
      }
    }

    /// <summary>
    /// Every [k_DeleteChunksFrameFrequency] calls, this deletes chunks past the distance threshold.
    /// </summary>
    /// <returns>True if this call attempted to delete chunks, else false.</returns>
    private bool DeleteChunks( )
    {
      if( m_DeleteFrameTimer == k_DeleteChunksFrameFrequency )
      {
        List<WorldPos> chunksToDelete = new List<WorldPos>();

        foreach( var chunk in m_World.m_Chunks )
        {
          float distance = Vector3.Distance(
          new Vector3( chunk.Value.m_Pos.x, 0, chunk.Value.m_Pos.z ),
          new Vector3( transform.position.x, 0, transform.position.z ) );

          if( distance > k_DeleteChunkDistance )
          {
            chunksToDelete.Add( chunk.Key );
          }
        }

        foreach( var chunk in chunksToDelete )
        {
          m_World.DestroyChunk( chunk.x, chunk.y, chunk.z );
        }

        m_DeleteFrameTimer = 0;
        return true;
      }

      m_DeleteFrameTimer++;
      return false;
    }
  }
}