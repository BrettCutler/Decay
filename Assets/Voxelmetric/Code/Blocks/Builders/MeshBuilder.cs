using System.Collections;
using UnityEngine;

public class MeshBuilder {

    public static void CrossMeshRenderer(Chunk chunk, BlockPos pos, MeshData meshData, TextureCollection texture, Block block)
    {
        Vector3 halfBlock = new Vector3( (Config.Env.BlockSize.x / 2 ) + Config.Env.BlockFacePadding,
                                         (Config.Env.BlockSize.y / 2 ) + Config.Env.BlockFacePadding,
                                         (Config.Env.BlockSize.z / 2 ) + Config.Env.BlockFacePadding );
        float colliderYOffset = 0.05f * Config.Env.BlockSize.y;
        float blockHeight = halfBlock.y * 2 * (block.data2 / 255f);

        float offsetX = (halfBlock.x * 2 * ((byte)(block.data3 & 0x0F) / 32f)) - (halfBlock.x / 2);
        float offsetZ = (halfBlock.z * 2 * ((byte)((block.data3 & 0xF0) >> 4) / 32f)) - (halfBlock.z / 2);

        //Converting the position to a vector adjusts it based on block size and gives us real world coordinates for x, y and z
        Vector3 vPos = pos;
        Vector3 vPosCollider = pos;
        vPos += new Vector3(offsetX, 0, offsetZ);

        float blockLight = ( (block.data1/255f) * Config.Env.BlockLightStrength) + (0.8f*Config.Env.AOStrength);

        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y, vPos.z + halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z + halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z - halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y, vPos.z - halfBlock.z));
        meshData.AddQuadTriangles();
        BlockBuilder.BuildTexture(chunk, vPos, meshData, Direction.north, texture);
        meshData.AddColors(blockLight, blockLight, blockLight, blockLight, blockLight);

        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y, vPos.z - halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z - halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z + halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y, vPos.z + halfBlock.z));
        meshData.AddQuadTriangles();
        BlockBuilder.BuildTexture(chunk, vPos, meshData, Direction.north, texture);
        meshData.AddColors(blockLight, blockLight, blockLight, blockLight, blockLight);

        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y, vPos.z + halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z + halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z - halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y, vPos.z - halfBlock.z));
        meshData.AddQuadTriangles();
        BlockBuilder.BuildTexture(chunk, vPos, meshData, Direction.north, texture);
        meshData.AddColors(blockLight, blockLight, blockLight, blockLight, blockLight);

        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y, vPos.z - halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x - halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z - halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y + blockHeight, vPos.z + halfBlock.z));
        meshData.AddVertex(new Vector3(vPos.x + halfBlock.x, vPos.y - halfBlock.y, vPos.z + halfBlock.z));
        meshData.AddQuadTriangles();
        BlockBuilder.BuildTexture(chunk, vPos, meshData, Direction.north, texture);
        meshData.AddColors(blockLight, blockLight, blockLight, blockLight, blockLight);

        meshData.AddVertex(new Vector3(vPosCollider.x - halfBlock.x, vPosCollider.y - halfBlock.y + colliderYOffset, vPosCollider.z + halfBlock.z), collisionMesh: true);
        meshData.AddVertex(new Vector3(vPosCollider.x + halfBlock.x, vPosCollider.y - halfBlock.y + colliderYOffset, vPosCollider.z + halfBlock.z), collisionMesh: true);
        meshData.AddVertex(new Vector3(vPosCollider.x + halfBlock.x, vPosCollider.y - halfBlock.y + colliderYOffset, vPosCollider.z - halfBlock.z), collisionMesh: true);
        meshData.AddVertex(new Vector3(vPosCollider.x - halfBlock.x, vPosCollider.y - halfBlock.y + colliderYOffset, vPosCollider.z - halfBlock.z), collisionMesh: true);
        meshData.AddQuadTriangles(collisionMesh:true);
    }
}
