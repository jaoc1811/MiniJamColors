using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowEffect : MonoBehaviour
{
    public float rainbowSpeed;

    float hue;
    float sat;
    float bri;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color.RGBToHSV(spriteRenderer.color, out hue, out sat, out bri);
        sat = 1;
        bri = 1;
        spriteRenderer.color = Color.HSVToRGB(hue, sat, bri);
    }

    void Update()
    {
        Color.RGBToHSV(spriteRenderer.color, out hue, out sat, out bri);
        hue += rainbowSpeed / 10000;
        if (hue >= 1){
            hue = 0;
        }
        spriteRenderer.color = Color.HSVToRGB(hue, sat, bri);
    }
}
