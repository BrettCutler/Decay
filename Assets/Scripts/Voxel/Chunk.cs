using UnityEngine;
using System.Collections;

/// <summary>
/// Collection of blocks
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
  /// <summary>
  /// Flag: do we need to update the Chunk?
  /// </summary>
  public bool m_Update = true;

  public World m_World;
  public WorldPos m_Pos;
  
  public Block[ , , ] m_Blocks = new Block[k_ChunkSize, k_ChunkSize, k_ChunkSize];

  private MeshFilter m_Filter;
  private MeshCollider m_Collider;

  /// <summary>
  /// Number of blocks in a chunk dimension.
  /// Total blocks = m_ChunkSize ^ 3
  /// </summary>
  public static int k_ChunkSize = 16;

  private void Awake()
  {
    m_Filter = gameObject.GetComponent<MeshFilter>();
    m_Collider = gameObject.GetComponent<MeshCollider>();

    //SetupExampleChunk();
  }

  private void Update()
  {
    if( m_Update )
    {
      UpdateChunk();
    }
  }

  public Block GetBlock(int x, int y, int z)
  {
    if( InRange( x, y, z ) )
    {
      return m_Blocks[x, y, z];
    }

    // It's not in this chunk, check again up top
    return m_World.GetBlock( m_Pos.x + x, m_Pos.y + y, m_Pos.z + z );
  }

  /// <summary>
  /// Is a coordinate within possible chunk index values?
  /// </summary>
  public static bool InRange( int index )
  {
    if( index < 0 || index >= k_ChunkSize )
    {
      return false;
    }

    return true;
  }

  /// <summary>
  /// Is a coordinate set within possible chunk index values?
  /// </summary>
  public static bool InRange( int x, int y, int z )
  {
    return InRange( x ) && InRange( y ) && InRange( z );
  }

  /// <summary>
  /// Assign block index in chunk.
  /// </summary>
  public void SetBlock( int x, int y, int z, Block block )
  {
    if( InRange( x, y, z ) )
    {
      m_Blocks[x, y, z] = block;

      m_Update = true;
    }
    else
    {
      m_World.SetBlock( m_Pos.x + x, m_Pos.y + y, m_Pos.z + z, block );
    }
  }

  /// <summary>
  /// Updates chunk based on its contents
  /// </summary>
  void UpdateChunk()
  {
    m_Update = false;

    MeshData meshData = new MeshData();

    for( int x = 0; x < k_ChunkSize; x++ )
      for( int y = 0; y < k_ChunkSize; y++ )
        for( int z = 0; z < k_ChunkSize; z++ )
        {
          meshData = m_Blocks[x, y, z].Blockdata( this, x, y, z, meshData );
        }

    RenderMesh( meshData );
  }

  /// <summary>
  /// Sends the calculated mesh information to the mesh and collision components
  /// </summary>
  void RenderMesh( MeshData meshData )
  {
    // rendering
    m_Filter.mesh.Clear();

    m_Filter.mesh.vertices = meshData.m_Vertices.ToArray();
    m_Filter.mesh.triangles = meshData.m_Triangles.ToArray();

    m_Filter.mesh.uv = meshData.m_uv.ToArray();
    m_Filter.mesh.RecalculateNormals();

    // collision
    m_Collider.sharedMesh = null;
    Mesh mesh = new Mesh();
    mesh.vertices = meshData.m_ColVertices.ToArray();
    mesh.triangles = meshData.m_ColTriangles.ToArray();
    mesh.RecalculateNormals();

    m_Collider.sharedMesh = mesh;
  }

  private void SetupExampleChunk()
  {
    m_Blocks = new Block[k_ChunkSize, k_ChunkSize, k_ChunkSize];

    for( int x = 0; x < k_ChunkSize; x++ )
      for( int y = 0; y < k_ChunkSize; y++ )
        for( int z = 0; z < k_ChunkSize; z++ )
        {
          m_Blocks[x, y, z] = new BlockAir();
        }

    m_Blocks[3, 5, 2] = new Block();
    m_Blocks[4, 5, 2] = new BlockGrass();

    UpdateChunk();
  }
}
