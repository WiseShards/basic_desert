using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Desert : MonoBehaviour {
    public float NoiseScale = 1.0f;
    public Material Material;
    public int Width = 100;
    public int Depth = 100;
    public float Height = 4.0f;

    public void Start() {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        for(int i = 0; i < Width; i++) {
            for(int j = 0; j < Depth; j++) {
                float xCoord = (i / (float) Width) * NoiseScale;
                float yCoord = (j / (float) Depth) * NoiseScale;
                float y = Height * Mathf.Clamp01(Mathf.PerlinNoise(xCoord, yCoord));

                vertices.Add(new Vector3(i - Width * 0.5f, y, j - Depth * 0.5f));
                uv.Add(new Vector2((float) i / Width, (float) j / Depth));
                if(i == 0 || j == 0) continue; // First bottom and left skipped
                triangles.Add(Width * i + j); // Top right
                triangles.Add(Width * i + (j - 1)); // Bottom right
                triangles.Add(Width * (i - 1) + (j - 1)); // Bottom left - First triangle
                triangles.Add(Width * (i - 1) + (j - 1)); // Bottom left 
                triangles.Add(Width * (i - 1) + j); // Top left
                triangles.Add(Width * i + j); // Top right - Second triangle
            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        Renderer ren = gameObject.AddComponent<MeshRenderer>();
        ren.shadowCastingMode = ShadowCastingMode.Off;
        ren.receiveShadows = false;
        ren.lightProbeUsage = LightProbeUsage.Off;
        ren.reflectionProbeUsage = ReflectionProbeUsage.Off;
        ren.material = Material;
        meshFilter.mesh = mesh;
    }
}
