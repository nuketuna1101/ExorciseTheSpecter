using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  button controller Ȥ�� ui manager�� �����丵 �Ǿ���� �κ�!!!!
/// </summary>


public class CharacterSelectManager : MonoBehaviour
{
    /// <summary>
    /// CharacterSelectManager ::
    /// character select ���� ��ư ������ �� �Ͼ�� ���� ����ȭ
    /// ���Ŀ� button controller�� �̵��ؾ� �ҵ�
    /// </summary>
    // ĳ���� : Rogue, Gunslinger
    private bool isSelectedAny = false;
    private int selectedCode = -1;
    private Button[] CharacterButtons;

    // �ؽ�Ʈ
    [SerializeField]
    private TMP_Text _Text;

    // ������ �˾� ui
    [SerializeField]
    private GameObject _PopUpInfoObj;
    private Image _CHAR_PORTRAIT_IMG;
    private TMP_Text _TEXT_CHAR_NAME;
    private TMP_Text _TEXT_CHAR_EXPLANATION;
    // ������ static ui ��ư
    [SerializeField]
    private GameObject _Start_Button;

    // �ӽ� :: ������ �������� ���� ���Ž� ����
    // 
    [SerializeField]
    private Sprite[] TEMP_PopUpInfoObj;
    private string[] TEMP_TEXT_CHAR_NAME = { "ROGUE", "GUNSLINGER" };
    private string[] TEMP_TEXT_CHAR_EXPLANATION = { "ROGUE EXPLANATION", "GUNSLINGER EXPLANATION" };


    /*
     - ���߿� �߰��� �κ�

    :: �˾�â �ٲ� ������ ���� ������ �ָ��� �� ���ƿͼ� ������ ����Ʈ
    :: ���� ���õǾ��� ����â ��ư ��Ʈ ��¦�̱� + ��Ʈ �׸���
     
     */
    private void Awake()
    {
        // ĳ���� ���� ��ư�� �迭�� ����
        CharacterButtons = this.transform.gameObject.GetComponentsInChildren<Button>();

        StartCoroutine(FlashingText());
    }
    private IEnumerator FlashingText()
    {
        int i = 0;
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        while (true)
        {
            // ���� �ƹ��͵� ���� �ȵǾ� ���� ����.
            if (isSelectedAny) break;
            //
            yield return wfs;
            _Text.color = ColorSettings.colorArr[i];
            i = (i + 1) % ColorSettings.colorArr.Length;
        }
    }
    public void ShowSelectedCharacter(int _characterCode)
    {
        // ���� ���� ��, ���õ� ĳ���� ���ٰ� ���õ� ĳ���Ͱ� ����
        if (!isSelectedAny)
        {
            isSelectedAny = true;
            // ĳ���� ���� �˾�â ����
            _PopUpInfoObj.SetActive(true);
            //
            _CHAR_PORTRAIT_IMG = _PopUpInfoObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            _TEXT_CHAR_NAME = _PopUpInfoObj.transform.GetChild(1).GetComponent<TMP_Text>();
            _TEXT_CHAR_EXPLANATION = _PopUpInfoObj.transform.GetChild(2).GetComponent<TMP_Text>();
            // ���� ��ư Ȱ��ȭ
            _Start_Button.SetActive(true);
        }

        // �̹� ���õ� ���̸� �ƹ��� �ൿx
        if (selectedCode == _characterCode)
            return;

        // ���� �� ���õ� ĳ���� �ڵ� ����
        selectedCode = _characterCode;
        // �״�� ���ӸŴ����� ����        <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< �̺κ� ���Ŀ� �����丵�Ҷ�  �ٲ����
        //GameManager.Instance.CharacterCode = selectedCode;
        GameManager.Instance.SetCharacterCode(_characterCode);
        // ���� ����
        _CHAR_PORTRAIT_IMG.sprite = TEMP_PopUpInfoObj[selectedCode];
        _TEXT_CHAR_NAME.text = TEMP_TEXT_CHAR_NAME[selectedCode];
        _TEXT_CHAR_EXPLANATION.text = TEMP_TEXT_CHAR_EXPLANATION[selectedCode];
    }
}
