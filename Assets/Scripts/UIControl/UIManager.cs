using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI 매니저 스크립트 : 씬의 캔버스에서, StaticUI와 PopupUI 내의 ui 오브젝트 접근하기 위한 바인딩.
/// </summary>


public class UIManager : Singleton<UIManager>
{
    //[SerializeField] public Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    private Dictionary<Type, UnityEngine.Object[]> SceneObjDic = new Dictionary<Type, UnityEngine.Object[]>();
    //Dictionary<Type, UnityEngine.Object[]>[] SceneObjDic = new Dictionary<Type, UnityEngine.Object[]>[4];
    private GameObject _Canvas;
    private GameObject StaticUI;
    private GameObject PopUpUI;

    private string[] TEMP_TEXT_CHAR_NAME = { "ROGUE", "GUNSLINGER" };
    private string[] TEMP_TEXT_CHAR_EXPLANATION = { "ROGUE EXPLANATION", "GUNSLINGER EXPLANATION" };

    private readonly WaitForSeconds wfs10 = new WaitForSeconds(0.1f);


    enum Scene1_Text 
    {
        Text_instMsg_shadow,
        Text_instMsg,
    }
    enum Scene1_Button
    {
        Button_Rogue,
        Button_Gunslinger,
        ButtonUI_Start,
        ButtonUI_Return,
    }

    enum Scene4_Text
    {
        text_DeckRemainNumber,
        text_Energy,
    }



    protected new void Awake()          // 씬에 따라 바인딩 달라야하므로 이벤트 붙이기
    {
        base.Awake();
        SceneManager.sceneLoaded += EverySceneEvent;
    }

    private void EverySceneEvent(Scene scene, LoadSceneMode mode)
    {
        SceneObjDic.Clear();
        InitBasic();
        var currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "0.MainScreen":
                break;
            case "1.CharacterSelect":
                break;
            case "2.MapView":
                break;
            case "4.BattleScene":
                Bind_Scene4();
                StartCoroutine(TEMP_Update_Scene4());
                break;
        }
    }
    #region Scene4에 대한 UI 컨트롤 레거시 코드
    private void Bind_Scene4()
    {
        BindStatic<TMP_Text>(typeof(Scene4_Text));
    }
    public void Update_Scene4_DeckRemainNumber()
    {
        Get<TMP_Text>((int)Scene4_Text.text_DeckRemainNumber).text = CardManager.Instance.GetReadyQueueSize().ToString();
    }
    public void Update_Scene4_Energy()
    {
        Get<TMP_Text>((int)Scene4_Text.text_Energy).text = GameManager.Instance.GetEnergy().ToString();
    }
    private IEnumerator TEMP_Update_Scene4()            // 4번째 씬에 대한 UI 업데이트 코루틴.
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name != "4.BattleScene")
                break;
            yield return null;
            Update_Scene4_DeckRemainNumber();
            Update_Scene4_Energy();
        }
    }

    public void Popup_NotifyTurn()              // 턴 시작 알림창 띄우기
    {
        StartCoroutine(Popup_NotifyTurn_COR());
    }
    private IEnumerator Popup_NotifyTurn_COR()
    {
        AudioManager.Instance.PlaySFX(SFX_TYPE.TurnChange);      // sfx는 임시로
        GameObject Popup_NotifyTurn = MyUtils.FindChildObj(PopUpUI, "Popup_NotifyTurn");
        Popup_NotifyTurn.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Popup_NotifyTurn.SetActive(false);
    }

    public void Popup_NotifyMsg(string msg, bool isActive)           // 알림메시지 onoff로 보여주기
    {
        GameObject Popup_NotifyMsg = MyUtils.FindChildObj(PopUpUI, "Popup_NotifyMsg");
        GameObject text_MsgString = MyUtils.FindChildObj(Popup_NotifyMsg, "text_MsgString");
        text_MsgString.GetComponent<TMP_Text>().text = msg;
        Popup_NotifyMsg.SetActive(isActive);
    }

    public void Popup_WarnMsg(string msg)           // 경고 알림창 깜빡이기
    {
        StartCoroutine(Popup_WarnMsg_COR(msg));
    }
    private IEnumerator Popup_WarnMsg_COR(string msg)            // 
    {
        int loop = 3;
        GameObject Popup_WarnMsg = MyUtils.FindChildObj(PopUpUI, "Popup_WarnMsg");
        GameObject text_MsgString = MyUtils.FindChildObj(Popup_WarnMsg, "text_MsgString");
        text_MsgString.GetComponent<TMP_Text>().text = msg;
        while (loop-- > 0)
        {
            yield return wfs10;
            Popup_WarnMsg.SetActive(true);
            yield return wfs10;
            Popup_WarnMsg.SetActive(false);
        }
    }

    #endregion

    #region 초기화와 바인딩, GET 코드 << 불변
    private void InitBasic()              // 캔버스로부터 StaticUI, PopUpUI 찾기. 주의: gameobj의 tag 설정 필요
    {
        _Canvas = GameObject.FindWithTag("Canvas");
        StaticUI = GameObject.FindWithTag("StaticUI");
        PopUpUI = GameObject.FindWithTag("PopUpUI");
    }
    // StaticUI 요소 바인딩
    #region StaticUI에서 컴포넌트를 통한 바인딩과 GET
    private void Bind<T>(Type type) where T : UnityEngine.Object 
    {
        String[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        SceneObjDic.Add(typeof(T), objects);
        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = MyUtils.FindChild<T>(_Canvas, names[i], true);
        }
    }

    private void BindStatic<T>(Type type) where T : UnityEngine.Object
    {
        String[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        SceneObjDic.Add(typeof(T), objects);
        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = MyUtils.FindChild<T>(StaticUI, names[i], true);
        }
    }

    private T Get<T>(int index) where T : UnityEngine.Object 
    {
        UnityEngine.Object[] objects = null;
        if (SceneObjDic.TryGetValue(typeof(T), out objects) == false) return null;
        return objects[index].GetComponent<T>() as T;
    }
    #endregion
    #endregion
}
