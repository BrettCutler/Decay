using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Voxel
{
  [System.Serializable]
  public class Save
  {
    public Dictionary<WorldPos, Block> m_Blocks = new Dictionary<WorldPos, Block>();

    public Save( Chunk chunk )
    {
      for( int x = 0; x < Chunk.k_ChunkSize; ++x )
        for( int y = 0; y < Chunk.k_ChunkSize; ++y )
          for( int z = 0; z < Chunk.k_ChunkSize; ++z )
          {
            if( !chunk.m_Blocks[x, y, z].m_Changed )
            {
              continue;
            }

            WorldPos pos = new WorldPos( x, y, z );
            m_Blocks.Add( pos, chunk.m_Blocks[x, y, z] );
          }
    }
  }
}