using UnityEngine;
using System.Collections;

[System.Serializable]
public class BlockGrass : Block
{
  public BlockGrass() : base()
  {

  }

  public override Tile TexturePosition( Direction direction )
  {
    Tile tile = new Tile();

    switch( direction )
    {
      case Direction.Up:
        tile.x = 2;
        tile.y = 0;
        return tile;
      case Direction.Down:
        tile.x = 1;
        tile.y = 0;
        return tile;
    }

    // default
    tile.x = 3;
    tile.y = 0;

    return tile;
  }
}
