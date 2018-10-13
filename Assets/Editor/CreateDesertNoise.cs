using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateDesertNoise {

    private const int Size = 1024;

    [MenuItem("Desert/Create Noise Texture")]
    static void CreateTexture() {
        Texture2D tex = new Texture2D(Size, Size);
        for(int i = 0; i < Size; ++i) {
            for(int j = 0; j < Size; ++j) {
                float random = Random.value;
                Color c = Color.white * random;
                c.a = 1.0f;
                tex.SetPixel(i, j, c);
            }
        }
        byte[] texture = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Textures/DesertNoise.png", texture);
    }
}
