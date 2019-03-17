﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageGenerator { 

    public static int width, height;

    public static Texture2D GenerateImage ( Color[] data, int width, int height ) {
        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);

        /*
        if (data.Length != width * height) {
            Debug.LogError("Data length doesn't equal given image size.");
            return null;
        }
        */

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                tex.SetPixel(x, y, data[x + y * width]);
            }
        }

        tex.Apply();
        return tex;
    }

}
