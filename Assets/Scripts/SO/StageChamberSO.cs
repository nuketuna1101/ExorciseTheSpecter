using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
public class ChamberInfo
{
    public int StageNumber;
    public int ChamberNumber;
    public int ChamberType;
    public int NextChamber1;
    public int NextChamber2;
    public int NextChamber3;
}

[CreateAssetMenu(fileName = "StageChamberSO", menuName = "Scriptable Object/StageChamberSO")]
public class StageChamberSO : ScriptableObject
{
    // 스테이지
    public List<ChamberInfo> ChamberInfoList;
}
