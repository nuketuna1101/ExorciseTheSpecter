using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ChamberState { Visited, Accessable, Selected, RestOf }

public class ChamberManager : MonoBehaviour
{
    private bool flag = false;
    [SerializeField]
    private GameObject[] _ChamberObjs;

    private int stageChamberNumber = 13;


    // 챔버 상태 : 현재 시점 기준, 방문했거나, 방문가능하거나, 그 외
    private readonly Color normalColor = new Color(1f, 1f, 1f);
    private readonly Color darkColor = new Color(50f / 255f, 50f / 255f, 50f / 255f);
    private readonly Color greenColor = new Color(0f, 1f, 0f);
    private readonly Color yellowColor = new Color(1f, 1f, 0f);

    //private ChamberState[] _ChamberStates;
    private void Awake()
    {

        SetAllChambers();

    }


    private void SetChamberAsState(GameObject _ChamberObj, ChamberState _ChamberState)
    {
        // visited: 버튼 비활성화, 어두운 이미지
        // accessable: 버튼 활성화, 강조 표시
        // selected:
        // restof: 버튼 비활성화, 기본 이미지
        var img_chamber = _ChamberObj.transform.GetChild(0).gameObject;
        var img_frame = _ChamberObj.transform.GetChild(1).gameObject;
        var btnObj = _ChamberObj.transform.GetChild(2).gameObject;
        switch (_ChamberState)
        {
            case ChamberState.Visited:
                img_chamber.GetComponent<Image>().color = darkColor;
                img_frame.SetActive(false);
                btnObj.SetActive(false);
                break;
            case ChamberState.Accessable:
                //img_chamber.GetComponent<Image>().color = yellowColor;
                StartCoroutine(BlinkingChamber(img_chamber, yellowColor));
                img_frame.SetActive(false);
                btnObj.SetActive(true);
                break;
            case ChamberState.Selected:
                //img_chamber.GetComponent<Image>().color = greenColor;
                StartCoroutine(BlinkingChamber(img_chamber, greenColor));
                StartCoroutine(BlinkingFrame(img_frame));
                btnObj.SetActive(true);
                break;
            case ChamberState.RestOf:
                img_chamber.GetComponent<Image>().color = normalColor;
                img_frame.SetActive(false);
                btnObj.SetActive(false);
                break;
        }
    }
    private IEnumerator BlinkingChamber(GameObject _ChamberObject, Color _Color)
    {
        bool flag = false;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _ChamberObject.GetComponent<Image>().color = (flag ? normalColor : _Color);
            flag = !flag;
        }
    }
    private IEnumerator BlinkingFrame(GameObject _FrameObject)
    {
        bool flag = false;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _FrameObject.SetActive(flag);
            flag = !flag;
        }
    }

    private void SetAllChambers()
    {
        var _ChamberStates = DataManager.Instance.publicChamberStates;

        for (int i = 1; i <= stageChamberNumber; i++)
        {
            SetChamberAsState(_ChamberObjs[i], _ChamberStates[i]);
        }
    }


    // characterselectmanager 와 비슷한 매커니즘으로 일단 구현

    private void PressChamberBTN()
    {
        //

    }



}
