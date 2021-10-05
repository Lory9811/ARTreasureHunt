using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using OpenCvSharp;

/// <summary>
/// C# to Native interface
/// Seee vision.h in libvision
/// </summary>
public class Vision {
    [DllImport("vision")]
    private static extern void Init();

    [DllImport("vision")]
    private static extern void Quit();

    [DllImport("vision")]
    private static extern int GetMajorVersion();

    [DllImport("vision")]
    private static extern int GetMinorVersion();

    [DllImport("vision")]
    private static extern bool IsObjectInImage(Color32[] image, Color32[] obj,
                             int imageWidth, int imageHeight,
                             int objectWidth, int objectHeight);

    public static (int, int) GetVersion() {
        return (GetMajorVersion(), GetMinorVersion());
    }

    public unsafe static bool DetectObject(Texture2D image, Texture2D obj) {
        /*Color32[] imageColorData = image.GetPixels32();
        Color32[] objectColorData = obj.GetPixels32();

        return IsObjectInImage(imageColorData, objectColorData,
                image.width, image.height, obj.width, obj.height);*/
        //Mat img = OpenCvSharp.Unity.TextureToMat(image);
        return false;
    }
}
