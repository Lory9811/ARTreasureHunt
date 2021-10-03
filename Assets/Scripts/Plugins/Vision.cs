using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

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
    private static extern void Threshold(IntPtr texture, int width, int height);

    public static (int, int) GetVersion() {
        return (GetMajorVersion(), GetMinorVersion());
    }

    public unsafe static void Test(Texture2D texture) {
        Color32[] colorData = texture.GetPixels32();
        
        fixed (Color32* colorDataPointer = colorData) {
            // TODO: check bounds, scale image to texture
            Threshold((IntPtr)colorDataPointer, texture.width, texture.height);
        }

        texture.SetPixels32(colorData);
        texture.Apply();
    }
}
