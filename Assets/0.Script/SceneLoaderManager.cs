using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SceneLoaderManager;

public class SceneLoaderManager : Singleton<SceneLoaderManager>
{
    public enum SceneName
    {
        Stage1 = 0,
        Stage2,
        Title,
        Town,
        LoadingScene,
              
    }

    public string CanvasLabel = "LoadingCanvas";
    [SerializeField]
    Canvas lodingCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    //�ε� ������ ������ �ε� ĵ���� // ���� ����� �ʿ� �����Ű���.
    /*async Task LoadingCanvasLoad()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(CanvasLabel);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = handle.Result;

            if (loadedObject != null)
            {
                //Debug.Log("�ε� ����: " + loadedObject.name);

                // ���÷� Canvas ������Ʈ�� �����ͼ� Ȱ��ȭ�մϴ�.
                Canvas canvasComponent = loadedObject.GetComponent<Canvas>();
                if (canvasComponent != null)
                {
                    lodingCanvas = Instantiate(canvasComponent);
                    loadedObject.SetActive(true);

                    Image[] imgs = lodingCanvas.GetComponentsInChildren<Image>();

                    for (int i = 0; i < imgs.Length; i++)
                    {
                        Debug.Log(imgs[i].sprite + "�̹��� ��������ƮȮ��");
                        if (imgs[i].sprite == null)
                            Debug.Log("�̹��� ��������Ʈ ����");
                    }
                }
                else
                {
                    Debug.LogError("�ε��̹����¼����ߴµ� ���ӿ�����Ʈ�� ĵ������ ����");
                }
            }
            else
            {
                Debug.LogError("�ε��̹����� ���� ������Ʈ�� ����");
            }
        }
        else
        {
            Debug.LogError("�ε� �̹��� �ҷ����� ����: " + handle.OperationException);
        }

        Addressables.Release(handle);
    }*/

    public void LoadScene(SceneName sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public async void LoadSceneAsyncMethod(SceneName sceneName)
    {
        await LoadSceneAsync(sceneName);
        
    }

    IEnumerator LoadSceneCoroutine(SceneName sceneName)
    {
        // ���� �� ����
        Scene currentScene = SceneManager.GetActiveScene();
        // �ε� �� �ε�
        SceneManager.LoadScene("LoadingScene");

        // SettingManager�� LoadAsset ȣ���� ��ٸ�
        //yield return StartCoroutine( SettingManager.Instance.LoadAssetCoroutine());

         // SaveManager �ʱ�ȭ
         SaveManager.Instance.Init();

        // ��: 1�� ���� �ε� ���� ������
        yield return new WaitForSeconds(1f);

        // ���ο� �� �񵿱� �ε�
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOperation.allowSceneActivation = false;

        // �ε� ���� üũ �� ������Ʈ
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("���� ������ �̵�");
                // ���� ���� �ε� �� ��ε�
                asyncOperation.allowSceneActivation = true;
            }
            yield return null; // �񵿱� �������� �ٸ� �۾��� ����� �� �ֵ���
        }
        SettingManager.Instance.skillfind();

        SkillManager.Instance.SkillSet();
        PlayManager.Instance.Init();
        ItemManager.Instance.Init();
    }
     async Task LoadSceneAsync(SceneName sceneName)
    {
        // ���� �� ����
        Scene currentScene = SceneManager.GetActiveScene();
        // �ε� �� �ε�
        SceneManager.LoadScene("LoadingScene");

        //await LoadingCanvasLoad();
        //await SettingManager.Instance.LoadAsset();

        SaveManager.Instance.Init();

        await Task.Delay(1000); // ��: 1�� ���� �ε� ���� ������


        // ���ο� �� �񵿱� �ε�
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
            asyncOperation.allowSceneActivation = false;

            // �ε� ���� üũ �� ������Ʈ
            while (!asyncOperation.isDone)
            {
           
            if (asyncOperation.progress >= 0.9f)
                {
                    Debug.Log("���� ������ �̵�");
                // �������� �ε��� ��ε�
                asyncOperation.allowSceneActivation = true;                                
            }
                await Task.Yield(); // �񵿱� �������� �ٸ� �۾��� ����� �� �ֵ���
            //�̰ž����� ���Ϲ����ѹݺ�? ���� ���ߴ°��� ���� �������.
            }

        SkillManager.Instance.SkillSet();
        PlayManager.Instance.Init();
        ItemManager.Instance.Init();
    }

    /*async void LoadSceneAsync(SceneName sceneName)
    {
        // ���� �� ����
        Scene currentScene = SceneManager.GetActiveScene();
        // �ε� �� �ε�
        SceneManager.LoadScene("LoadingScene");

        // ���ο� �� �񵿱� �ε�
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOperation.allowSceneActivation = false;

        // �ε� ���� üũ �� ������Ʈ
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("���� ������ �̵�");
                asyncOperation.allowSceneActivation = true;
            }
            await Task.Yield(); // �񵿱� �������� �ٸ� �۾��� ����� �� �ֵ���
        }

        *//*//�ε� ����üũ �� ������Ʈ
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("�ϴܷε�´ٵǾ����� ���øŴ������� ���¹��������� ��ٸ���");
                await SettingManager.Instance.LoadAsset();
                asyncOperation.allowSceneActivation = true;
                Debug.Log("���·ε� �ٵǾ����� ���� ������ �̵�");
            }
        }

        AsyncOperationHandle asyncOperationHandle = 

        await asyncOperation.;

        await SettingManager.Instance.LoadAsset();*/


        /*if (sceneName != SceneName.Title)
        {
            PlayManager.Instance.Init();
            ItemManager.Instance.Init();
            SkillManager.Instance.SkillSet();
        }
        else
        {

        }*//*
        //�������� �ε��� ��ε�
        //SceneManager.UnloadSceneAsync("LoadingScene");
        //SceneManager.UnloadSceneAsync(currentScene);
    }*/



    /*IEnumerator LoadSceneAsync(SceneName sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene("LoadingScene");



        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOperation.allowSceneActivation = false;

        //�ε� ����üũ �� ������Ʈ
        while(!asyncOperation.isDone)
        {
            if(asyncOperation.progress >= 0.9f)
            {
                Debug.Log("�ϴܷε�´ٵǾ����� ���øŴ������� ���¹��������� ��ٸ���");
                await SettingManager.Instance.loadingUI;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        //�������� �ε��� ��ε�
        SceneManager.UnloadSceneAsync("LoadingScene");
        SceneManager.UnloadSceneAsync(currentScene);

    }*/

    #region  �� �ε�� �񵿱� �ε忡 ����

    /*
      �� �ε� 
    ���� ���Ե� ��� ���ҽ��� �޸𸮿� �ε�, ���ӿ�����Ʈ ���� ������������ ������Ʈ�� awakeȣ��
    ��Ȱ��ȭ�� Start�� ȣ��

      �񵿱� �ε�
    ���� �ε� �Ǵµ��� ������Ʈ�� �����Ǹ� awake ȣ�� ������ start�� ���� Ȱ��ȭ ���� ȣ��
     */

    #endregion


}
