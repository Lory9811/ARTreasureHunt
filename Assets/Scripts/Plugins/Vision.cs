using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Vision {
    [DllImport("vision")]
    private static extern int GetMajorVersion();

    [DllImport("vision")]
    private static extern int GetMinorVersion();

    public static (int, int) GetVersion() {
        Debug.Log("test");
        Debug.Log(GetMajorVersion());
        Debug.Log(GetMinorVersion());
        return (GetMajorVersion(), GetMinorVersion());
    }
}
