using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization
{
  public static string k_SaveFolderName = "VoxelWorldSaves";

  public static string SaveLocation( string worldName )
  {
    string savePath = k_SaveFolderName + "/" + worldName + "/";

    if( !Directory.Exists( savePath ) )
    {
      Directory.CreateDirectory( savePath );
    }

    return savePath;
  }

  public static string FileName( WorldPos chunkLocation )
  {
    string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
    return fileName;
  }

  public static void SaveChunk( Chunk chunk )
  {
    string saveFile = SaveLocation( chunk.m_World.m_WorldName );
    saveFile += FileName( chunk.m_Pos );

    IFormatter formatter = new BinaryFormatter();
    Stream stream = new FileStream( saveFile, FileMode.Create, FileAccess.Write, FileShare.None );
    formatter.Serialize( stream, chunk.m_Blocks );
    stream.Close( );
  }

  public static bool LoadChunk( Chunk chunk )
  {
    string saveFile = SaveLocation( chunk.m_World.m_WorldName );
    saveFile += FileName( chunk.m_Pos );

    if( !File.Exists( saveFile ) )
    {
      return false;
    }

    IFormatter formatter = new BinaryFormatter( );
    FileStream stream = new FileStream( saveFile, FileMode.Open );

    chunk.m_Blocks = (Block[,,])formatter.Deserialize( stream );
    stream.Close( );
    return true;
  }
}
