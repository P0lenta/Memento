using UnityEngine;

public class AddMeshColliders : MonoBehaviour
{
    [ContextMenu("Adicionar Mesh Colliders")]
    void AddColliders()
    {
        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
        foreach (var mesh in meshes)
        {
            if (mesh.GetComponent<MeshCollider>() == null)
            {
                MeshCollider col = mesh.gameObject.AddComponent<MeshCollider>();
                col.sharedMesh = mesh.sharedMesh;
            }
        }
        Debug.Log($"Adicionados colliders em {meshes.Length} objetos!");
    }
}