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

    // ���� �÷��̾ ��ġ�� è�� ��ȣ (0: ó�� �������� ���� ����.)
    private int curChamberNumber = 0;

    private List<int> possibleChamberList;

    private void SetPossibleChamber()
    {
        // ���� ���¿��� ���� ������ è�� ����ȭ
        // �ʱ�ȭ
        possibleChamberList.Clear();

    }


}
