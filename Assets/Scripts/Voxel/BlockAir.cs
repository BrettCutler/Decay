using UnityEngine;
using System.Collections;

/// <summary>
/// Derived block representing empty space
/// </summary>
[System.Serializable]
public class BlockAir : Block
{
  public BlockAir() : base()
  {

  }

  public override MeshData Blockdata( Chunk chunk, int x, int y, int z, MeshData meshData )
  {
    return meshData;
  }

  public override bool IsSolid( Block.Direction direction )
  {
    return false;
  }
}
