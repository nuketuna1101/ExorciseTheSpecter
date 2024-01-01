using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    private int stateNo = 0;
    [SerializeField]
    Image loadingBar;
    [SerializeField]
    TMP_Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    public static void LoadScene(string sceneName)
    {
        // ���� �޼ҵ�� ��𼭳� ��������
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    IEnumerator LoadScene()
    {
        // �񵿱�� ���� �̵���Ű�鼭, �ε� ȭ�� ȿ�� �ð�ȭ
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;// ���� �����ӱ��� ���
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, op.progress * 0.5f, timer);
                changeText();
                if (loadingBar.fillAmount >= op.progress)
                    timer = 0f;
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 0.75f, timer);
                changeText();
                if (loadingBar.fillAmount == 0.75f)
                    break;
            }
        }
        while (loadingBar.fillAmount < 0.95f)
        {
            yield return null;// ���� �����ӱ��� ���
            timer += Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 0.95f, timer);
            changeText();
        }
        op.allowSceneActivation = true;
        yield break;
    }
    private void changeText()
    {
        // ���� ȿ���� ���ؼ� �ؽ�Ʈ �ٲٱ�
        switch (stateNo)
        {
            case 0:
                loadingText.text = "Loading..";
                stateNo = 1;
                break;
            case 1:
                loadingText.text = "Loading...";
                stateNo = 2;
                break;
            case 2:
                loadingText.text = "Loading....";
                stateNo = 0;
                break;
        }
    }
}
