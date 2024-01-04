using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChamberManager : MonoBehaviour
{
    private bool flag = false;
    [SerializeField]
    private GameObject[] _ChamberObjs;

    //
    private readonly Color[] colorArr = new Color[3];
    private readonly Color cc1 = new Color(1f, 1f, 0f);
    private readonly Color cc2 = new Color(175f / 255f, 175f / 255f, 0f);
    private readonly Color cc3 = new Color(125f / 255f, 125f / 255f, 0f);

    //
    private ColorBlock tmpCB;


    // 챔버 상태 : 현재 시점 기준, 방문했거나, 방문가능하거나, 그 외
    public enum ChamberState { Visited, Accessable, Selected, RestOf }

    private readonly Color normalColor = new Color(1f, 1f, 1f);
    private readonly Color darkColor = new Color(50f / 255f, 50f / 255f, 50f / 255f);
    private readonly Color greenColor = new Color(0f, 1f, 0f);
    private readonly Color yellowColor = new Color(1f, 1f, 0f);

    private void SetChamber()
    {

    }

    private void Awake()
    {
        //StartCoroutine(FlashingChamberSprite(_ChamberObjs[0]));
        SetChamberAsState(_ChamberObjs[1], ChamberState.Visited);
        SetChamberAsState(_ChamberObjs[2], ChamberState.Accessable);
        SetChamberAsState(_ChamberObjs[3], ChamberState.RestOf);
        SetChamberAsState(_ChamberObjs[0], ChamberState.Selected);

    }


    private IEnumerator FlashingChamberSprite(GameObject _ChamberObj)
    {
        int i = 0;

        var gobj1 = _ChamberObj.transform.GetChild(0).gameObject;
        var gobj2 = _ChamberObj.transform.GetChild(1).gameObject;
        bool tmpflag = false;
        while (true)
        {
            // 오직 아무것도 선택 안되어 있을 때만.
            if (flag) break;
            //
            yield return new WaitForSeconds(0.1f);

            
            DebugOpt.Log(" ");
            gobj2.SetActive(tmpflag);
            tmpflag = !tmpflag;
        }
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
                img_chamber.GetComponent<Image>().color = greenColor;
                StartCoroutine(ObjFrameLoop(img_frame));
                btnObj.SetActive(true);
                break;
            case ChamberState.Selected:
                img_chamber.GetComponent<Image>().color = yellowColor;
                img_frame.SetActive(false);
                btnObj.SetActive(true);
                break;
            case ChamberState.RestOf:
                img_chamber.GetComponent<Image>().color = normalColor;
                img_frame.SetActive(false);
                btnObj.SetActive(false);
                break;

        }
    }

    private IEnumerator ObjFrameLoop(GameObject _FrameObject)
    {
        bool flag = false;
        while (true)
        {
            // 오직 아무것도 선택 안되어 있을 때만.
            //if (flag) break;
            //
            yield return new WaitForSeconds(0.1f);
            _FrameObject.SetActive(flag);
            flag = !flag;
        }
    }


}
