using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BitMapGenerator {

    public static Color[,] GenerateBitMap ( Color[] data, int width, int height ) {
        Color[,] bitmap = new Color[width,height];

        for (int i = 0; i < data.Length; i++) {
            bitmap[i % width, (int)(i / width)] = data[i];
        }

        return bitmap;
    }

}