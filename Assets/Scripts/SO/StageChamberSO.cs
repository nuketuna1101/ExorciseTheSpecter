using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "StageChamberSO", menuName = "Scriptable Object/StageChamberSO")]
public class StageChamberSO : ScriptableObject
{
    // 스테이지
    [Serializable]
    public struct ChamberArray 
    {
        public int StageNumber;
        public int ChamberNumber;
        public int ChamberType;
        public int NextChamber1;
        public int NextChamber2;
        public int NextChamber3;
    }
    public List<ChamberArray> StageChamberArray;

}
