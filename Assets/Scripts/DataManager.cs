using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //
    private string[] fileNames = { "MapChamberInfo" };

    private void Awake()
    {
        TestDataLoad();
    }

    private void TestDataLoad()
    {
        var dataList = new List<Dictionary<string, object>>();
        string file = "MapChamberInfo";
        dataList = CSVReader.Read(file);

        DebugOpt.Log("test code ::" + int.Parse(dataList[0]["TempCol"].ToString()));
        DebugOpt.Log("test code ::" + int.Parse(dataList[1]["TempCol"].ToString()));
        DebugOpt.Log("test code ::" + int.Parse(dataList[2]["TempCol"].ToString()));
        DebugOpt.Log("test code ::" + int.Parse(dataList[3]["TempCol"].ToString()));

        /*
        for (int index = 0; index < _AreaFixedDatas.Length; index++)
        {

            AreaFixedData curArea = _AreaFixedDatas[index];
            curArea.areaID = int.Parse(dataList[index]["areaType"].ToString());
            curArea.prevAreaID = int.Parse(dataList[index]["openCondition"].ToString());
        }
        */
    }


    private void ManageMapChamberInfo()
    {
        //
        var dataList = new List<Dictionary<string, object>>();
        string file = "MapChamberInfo";
        dataList = CSVReader.Read(file);

    }

    private void SetByStageNumber(int _StageNumber)
    {
        //
        var dataList = new List<Dictionary<string, object>>();
        string file = "MapChamberInfo";
        dataList = CSVReader.Read(file);

        //
        // 현재 so에 대해
        

        /*
        for (int index = 0; index < _AreaFixedDatas.Length; index++)
        {

            AreaFixedData curArea = _AreaFixedDatas[index];
            curArea.areaID = int.Parse(dataList[index]["areaType"].ToString());
            curArea.prevAreaID = int.Parse(dataList[index]["openCondition"].ToString());
        }
        */
    }
}
