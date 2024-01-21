using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum ChamberState { Visited, Accessable, Selected, RestOf }

public class ChamberManager : MonoBehaviour
{
    /// <summary>
    /// ChamberManager �� ���� :: 2.MapView���� �����ϴ� Chamber UI�� ���� �ð�ȭ �۾�
    /// 
    /// </summary>
    private bool flag = false;
    [SerializeField]
    private GameObject[] _ChamberObjs;

    [SerializeField]
    private List<GameObject> _TEMP_CHAMBER_OBJ_AUTO; 
    [SerializeField]
    private Canvas myCanvas;

    private int stageChamberNumber = 13;

    // ���߿� �ۺ��� ���� Ŭ������ �÷� ��Ÿ�Ϸ� ������.
    // è�� ���� : ���� ���� ����, �湮�߰ų�, �湮�����ϰų�, �� ��
    [SerializeField]
    private GameObject _EnterBtn;

    // ���� �����丵���� UI �ڵ�ȭ �Ҵ� ����
    private void TEMP_METHOD_FOR_FUTURE_REFACTORING()
    {
        Transform[] _tra = myCanvas.gameObject.transform.GetChild(0).GetChild(2).GetChild(2).gameObject.GetComponentsInChildren<Transform>();
        _TEMP_CHAMBER_OBJ_AUTO.Clear();
        foreach (Transform t in _tra)
        {
            _TEMP_CHAMBER_OBJ_AUTO.Add(t.gameObject);
        }
    }

    private void Awake()
    {
        StartCoroutine(ActivateEnterBtn());
    }

    private IEnumerator ActivateEnterBtn()
    {
        while (true)
        {
            yield return null;
            if (GameManager.Instance.CurSelectedChamberNumber == -1)
            {
                _EnterBtn.SetActive(false);
            }
            else
            {
                _EnterBtn.SetActive(true);
                break;
            }
        }
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "2.MapView") return;
        RedrawAllChambers();
    }

    private void RedrawAllChambers()
    {
        // �� �Լ��� �������� ��� è���� ���� �ð�ȭ
        var _ChamberStates = DataManager.Instance.publicChamberStates;
        List<Image> img_chamber_accessable = new List<Image>();
        GameObject img_frame_selected = null;

        for (int i = 1; i <= stageChamberNumber; i++)
        {
            // �ش� è���� ����,
            var _ChamberObj = _ChamberObjs[i];
            var img_chamber = _ChamberObj.transform.GetChild(0).gameObject;
            var img_frame = _ChamberObj.transform.GetChild(1).gameObject;
            var btnObj = _ChamberObj.transform.GetChild(2).gameObject;
            switch (_ChamberStates[i])
            {
                case ChamberState.Visited:
                    img_chamber.GetComponent<Image>().color = ColorSettings.darkColor;
                    img_frame.SetActive(false);
                    btnObj.SetActive(false);
                    break;
                case ChamberState.Accessable:
                    //img_chamber_accessable.Add(img_chamber.GetComponent<Image>());
                    img_chamber.GetComponent<Image>().color = ColorSettings.yellowColor;
                    img_frame.SetActive(false);
                    btnObj.SetActive(true);
                    break;
                case ChamberState.Selected:
                    img_chamber.GetComponent<Image>().color = ColorSettings.greenColor;
                    img_frame_selected = img_frame;
                    btnObj.SetActive(true);
                    break;
                case ChamberState.RestOf:
                    img_chamber.GetComponent<Image>().color = ColorSettings.normalColor;
                    img_frame.SetActive(false);
                    btnObj.SetActive(false);
                    break;
            }
        }
        // Accessable �� Selected�� ���ؼ��� �ڷ�ƾ���� �ݺ�����
        // ���� ����Ǵ� �ڷ�ƾ ����ϰ�
        /*
        StopAllCoroutines();
        if (img_chamber_accessable.Count > 0)
            StartCoroutine(BlinkEfxAccessable(img_chamber_accessable, yellowColor));
        if (img_frame_selected != null)
            StartCoroutine(BlinkEfxSelected(img_frame_selected));
        */
    }
    private IEnumerator BlinkEfxAccessable(List<Image> _Chambers_Accessable, Color _Color)
    {
        yield return null;
        bool flag = false;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            foreach(Image _img_Chamber in _Chambers_Accessable)
            {
                _img_Chamber.color = (flag ? ColorSettings.normalColor : _Color);
            }
            DebugOpt.Log("CM :: BlinkEfxAccessable checker");
            flag = !flag;
        }
    }
    private IEnumerator BlinkEfxSelected(GameObject _Frame_Selected)
    {
        yield return null;
        bool flag = false;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _Frame_Selected.SetActive(flag);
            DebugOpt.Log("CM :: BlinkEfxSelected checker");
            flag = !flag;
        }
    }
}