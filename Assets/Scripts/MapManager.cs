using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    // �� �Ŵ��� : ����� ����

    //
    private bool isSelectedAny = false;
    private int selectedCode = -1;

    // ������� ���� : �̹� �湮��, ���� �湮������, ���� �湮 �Ұ���
    private int StageNumber;
    private int totalChamberNumber;
    // ��������Ʈ�� �湮���
    private List<int>[] adj;
    private bool[] visited;


    private void InitEdge()
    {
        // ���� �������� �ѹ� ��������.
        StageNumber = GameManager.Instance.StageNumber;
        int numberofstageChambers= 25;
        // �������� �ѹ��� �ش��ϴ� �����Ϳ� ���� �� ����

        // ������ ���̺�κ��� ���� ����
        adj = new List<int>[numberofstageChambers + 1];
        visited = new bool[numberofstageChambers + 1];
        Array.Fill(visited, false);

        //


    }


}
