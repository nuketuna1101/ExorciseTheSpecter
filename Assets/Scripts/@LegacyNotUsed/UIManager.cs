using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI �Ŵ��� ��ũ��Ʈ : ���� ĵ��������, StaticUI�� PopupUI ���� ui ������Ʈ �����ϱ� ���� ���ε�.
/// </summary>

public class UIManager : Singleton<UIManager>
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    private GameObject StaticUI;
    private GameObject PopUpUI;

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

    enum Static_GO_0
    {
        DevSignature,
    }

    protected new void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += EverySceneEvent;
        /*
        BindStatic<TMP_Text>(typeof(Texts));
        Get<TMP_Text>((int)Texts.PointText).text = "THIS IS POINTTEXT"; //���� �κ�
        Get<TMP_Text>((int)Texts.ScoreText).text = "THISISSCORETEXT"; //���� �κ�
        */
        //BindStatic<TMP_Text>(typeof(Static_Text_1));
        //BindStatic<Button>(typeof(Static_Button_1));
        //BindPopup<Image>(typeof(Popup_GO_1));
        //Get<Image>((int)Popup_GO_1.PopupScreen_InfoScreen).transform.gameObject.SetActive(true);
    }

    private void EverySceneEvent(Scene scene, LoadSceneMode mode)
    {
        InitBasic();
        var currentSceneName = SceneManager.GetActiveScene().name;
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
    }


    private void Bind0()
    {
        BindGO(typeof(Static_GO_0));
    }
    private void Bind1()
    {
        BindStatic<TMP_Text>(typeof(Static_Text_1));
        BindStatic<Button>(typeof(Static_Button_1));
    }
    private void Bind2()
    {

    }
    private void Bind4()
    {

    }


    private string[] TEMP_TEXT_CHAR_NAME = { "ROGUE", "GUNSLINGER" };
    private string[] TEMP_TEXT_CHAR_EXPLANATION = { "ROGUE EXPLANATION", "GUNSLINGER EXPLANATION" };




    private void InitBasic()              // ĵ�����κ��� StaticUI, PopupUI ã��.
    {
        var _Canvas = GameObject.Find("Canvas");
        StaticUI = _Canvas.transform.GetChild(0).gameObject;
        PopUpUI = _Canvas.transform.GetChild(1).gameObject;
    }
    // StaticUI ��� ���ε�
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
    private T Get<T>(int index) where T : UnityEngine.Object 
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;
        return objects[index] as T;
    }

    private void BindGO(Type type)
    {
        String[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(GameObject), objects);
        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = MyUtils.FindGO(StaticUI, names[i], true);
        }
    }
    private GameObject GetGO(int index)
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(GameObject), out objects) == false)
            return null;
        return objects[index] as GameObject;
    }
}