using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // �� �Ŵ��� : ����� ����

    //
    private bool isSelectedAny = false;
    private int selectedCode = -1;

    // ������� ���� : �̹� �湮��, ���� �湮������, ���� �湮 �Ұ���
    private List<int>[] adj;
    private bool[] visited;

}
