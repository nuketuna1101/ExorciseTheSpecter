using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 유틸리티 클래스
/// </summary>

public class MyUtils
{
    public static Quaternion QI => Quaternion.identity;
}


[Serializable]
public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }   
}
