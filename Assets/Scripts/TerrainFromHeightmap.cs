using UnityEngine;
using System.Collections;

public class TerrainFromHeightmap : MonoBehaviour
{
  void Awake()
  {

    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    for( int i = 0; i < meshFilters.Length; ++i )
    {
      if( meshFilters[i].GetInstanceID() != GetInstanceID() )
      {
        combine[i].mesh = meshFilters[i].sharedMesh;
        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        meshFilters[i].gameObject.SetActive( false );
        Debug.Log( "set " + meshFilters[i].gameObject.name + " UNACTIVE, [" + i + "]" );
      }
    }

    MeshFilter meshFilter = GetComponent<MeshFilter>();
    meshFilter.mesh = new Mesh();
    meshFilter.mesh.CombineMeshes( combine );

    MeshCollider meshCollider = GetComponent<MeshCollider>();
    meshCollider.sharedMesh = new Mesh();
    meshCollider.sharedMesh.CombineMeshes( combine );

    transform.gameObject.SetActive( true );
    Debug.Log( "now, set " + transform.gameObject.name + "  ACTIVE" );

  }
}
