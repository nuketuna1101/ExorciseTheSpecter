using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�� �������� ������ �ڵ�ȭ ���� ��ũ��Ʈ
/// </summary>

public class CardOrder : MonoBehaviour
{
    [SerializeField]
    private Renderer[] RenderOrder0; 
    [SerializeField]
    private Renderer[] RenderOrder1; 
    [SerializeField]
    private Renderer[] RenderOrder2;
    [SerializeField]
    private Renderer[] RenderOrder3; 
    [SerializeField]
    private Renderer[] RenderOrder4;


    private const string _SortingLayerName = "Card";
    int originOrder;

    private void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }
    private void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }
    private void SetOrder(int order)
    {
        int mulOrder = order * 10;
        foreach (var renderer in RenderOrder0)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = mulOrder;
        }
        foreach (var renderer in RenderOrder1)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }
        foreach (var renderer in RenderOrder2)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = mulOrder + 2;
        }
        foreach (var renderer in RenderOrder3)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = mulOrder + 3;
        }
        foreach (var renderer in RenderOrder4)
        {
            renderer.sortingLayerName = _SortingLayerName;
            renderer.sortingOrder = mulOrder + 4;
        }
    }

}
