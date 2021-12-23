using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Scaler : MonoBehaviour
{
    public Canvas canvas;

    public float HeightRatio()
    {
        //Get the ratio from current height compared to 1920
        float scaleRatioHeight = Screen.height/1920f;
        return scaleRatioHeight;
    }

    public float WidthRatio()
    {
        //get the ratio from current width compared to 1080
        float scaleRatioWidth = Screen.width/1080f;
        return scaleRatioWidth;
    }

    void Update()
    {
        //get the ratios
        float scaleRatioHeight = HeightRatio();
        float scaleRatioWidth = WidthRatio();

        //If one ratio is smaller than the other use that one
        if (scaleRatioHeight <= scaleRatioWidth)
        {
            canvas.scaleFactor = scaleRatioHeight;
        }
        else
        {
            canvas.scaleFactor = scaleRatioWidth;
        }

    }
}