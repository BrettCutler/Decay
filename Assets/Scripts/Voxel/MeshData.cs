using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData
{
  public List<Vector3> m_Vertices = new List<Vector3>();
  public List<int> m_Triangles = new List<int>();
  public List<Vector2> m_uv = new List<Vector2>();

  public List<Vector3> m_ColVertices = new List<Vector3>();
  public List<int> m_ColTriangles = new List<int>();

  public bool m_UseRenderDataForCollider;

  public MeshData() { }

  public void AddTriangle( int tri )
  {
    m_Triangles.Add( tri );

    if( m_UseRenderDataForCollider )
    {
      // because triangles correspond to entries in the vertices list, make sure
      // we adjust for difference between vertices lists
      m_ColTriangles.Add( tri - ( m_Vertices.Count - m_ColTriangles.Count ) );
    }
  }

  /// <summary>
  /// Take the 6 most recent vertices and add them to the triangles collection.
  /// </summary>
  public void AddQuadTriangles()
  {
    m_Triangles.Add( m_Vertices.Count - 4 );
    m_Triangles.Add( m_Vertices.Count - 3 );
    m_Triangles.Add( m_Vertices.Count - 2 );
    
    m_Triangles.Add( m_Vertices.Count - 4 );
    m_Triangles.Add( m_Vertices.Count - 2 );
    m_Triangles.Add( m_Vertices.Count - 1 );

    if( m_UseRenderDataForCollider )
    {
      m_ColTriangles.Add( m_ColVertices.Count - 4 );
      m_ColTriangles.Add( m_ColVertices.Count - 3 );
      m_ColTriangles.Add( m_ColVertices.Count - 2 );

      m_ColTriangles.Add( m_ColVertices.Count - 4 );
      m_ColTriangles.Add( m_ColVertices.Count - 2 );
      m_ColTriangles.Add( m_ColVertices.Count - 1 );
    }
  }

  public void AddVertex( Vector3 vertex )
  {
    m_Vertices.Add( vertex );

    if( m_UseRenderDataForCollider )
    {
      m_ColVertices.Add( vertex );
    }
  }
}
