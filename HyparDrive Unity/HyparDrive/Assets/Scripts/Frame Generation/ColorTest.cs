using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTest : MonoBehaviour {

    private RawImage image;

    private Color[] colours;

    public int WIDTH = 175;
    public int HEIGHT = 175;

    private void Start () {
        image = GetComponent<RawImage>();

        colours = new Color[WIDTH * HEIGHT];

        float r = 0;
        float g = 0;
        float b = 0;

        for (int y = 0; y < HEIGHT; y++, b += 1f / HEIGHT) {
            for (int x = 0; x < WIDTH; x++, r += 1f / WIDTH) {
                colours[x + y * WIDTH] = new Color(r, g, b);
            }
        }

        image.texture = ImageGenerator.GenerateImage(colours, WIDTH, HEIGHT);
    }
}
