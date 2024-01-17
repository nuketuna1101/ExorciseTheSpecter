using System;
using System.Collections;
using System.Collections.Generic;
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
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    private GameObject StaticUI;
    private GameObject PopUpUI;


    enum Popup_GO_0
    {
        PopUpScreen_Settings,
        PopUpScreen_ExitConfirm,
    }

    enum Static_Text_1 
    {
        Text_instMsg_shadow,
        Text_instMsg,
    }
    enum Static_Button_1
    {
        Button_Rogue,
        Button_Gunslinger,
        ButtonUI_Start,
        ButtonUI_Return,
    }
    enum Popup_GO_1 
    {
        PopupScreen_InfoScreen,
        PopUpScreen_CharacterConfirm,
    }
    enum Popup_Text_1
    {
        text_CHAR_NAME,
        text_CHAR_EXPLANATION,
    }


    protected new void Awake()
    {
        base.Awake();
        InitBasic();

        var currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("curscene :: " + currentSceneName);
        switch (currentSceneName)
        {
            case "0.MainScreen":
                Bind0();
                break;
            case "1.CharacterSelect":
                Bind1();
                break;
            case "2.MapView":
                Bind2();
                break;
            case "4.BattleScene":
                Bind4();
                break;

        }
        /*
        BindStatic<TMP_Text>(typeof(Texts));
        Get<TMP_Text>((int)Texts.PointText).text = "THIS IS POINTTEXT"; //실험 부분
        Get<TMP_Text>((int)Texts.ScoreText).text = "THISISSCORETEXT"; //실험 부분
        */
        //BindStatic<TMP_Text>(typeof(Static_Text_1));
        //BindStatic<Button>(typeof(Static_Button_1));
        //BindPopup<Image>(typeof(Popup_GO_1));
        //Get<Image>((int)Popup_GO_1.PopupScreen_InfoScreen).transform.gameObject.SetActive(true);

    }

    private void Bind0()
    {
        //BindPopup<GameObject>(typeof(Popup_GO_0));
    }
    private void Bind1()
    {
        BindStatic<TMP_Text>(typeof(Static_Text_1));
        BindStatic<Button>(typeof(Static_Button_1));
        BindPopup<UnityEngine.Object>(typeof(Popup_GO_1));
    }
    private void Bind2()
    {

    }
    private void Bind4()
    {

    }


    private string[] TEMP_TEXT_CHAR_NAME = { "ROGUE", "GUNSLINGER" };
    private string[] TEMP_TEXT_CHAR_EXPLANATION = { "ROGUE EXPLANATION", "GUNSLINGER EXPLANATION" };




    private void InitBasic()              // 캔버스로부터 StaticUI, PopupUI 찾기.
    {
        var _Canvas = GameObject.Find("Canvas");
        StaticUI = _Canvas.transform.GetChild(0).gameObject;
        PopUpUI = _Canvas.transform.GetChild(1).gameObject;
    }
    // StaticUI 요소 바인딩
    private void BindStatic<T>(Type type) where T : UnityEngine.Object 
    {
        String[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);
        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = MyUtils.FindChild<T>(StaticUI, names[i], true);
        }
    }   
    private void BindPopup<T>(Type type) where T : UnityEngine.Object
    {
        String[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = MyUtils.FindChild<T>(PopUpUI, names[i], true);
        }
    }
    private T Get<T>(int index) where T : UnityEngine.Object 
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;
        return objects[index] as T;
    }
}
