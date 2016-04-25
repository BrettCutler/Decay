using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Voxel
{
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
      Save save = new Save( chunk );
      if( save.m_Blocks.Count == 0 )
      {
        return;
      }

      string saveFile = SaveLocation( chunk.m_World.m_WorldName );
      saveFile += FileName( chunk.m_Pos );

      IFormatter formatter = new BinaryFormatter();
      Stream stream = new FileStream( saveFile, FileMode.Create, FileAccess.Write, FileShare.None );
      formatter.Serialize( stream, save );
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

      Save save = (Save)formatter.Deserialize(stream);

      foreach( var block in save.m_Blocks )
      {
        chunk.m_Blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
      }

      stream.Close( );
      return true;
    }
  }
}