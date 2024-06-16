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

    public string CanvasAddress;
    Canvas lodingCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    //�ε� ������ ������ �ε� ĵ����
    async Task LoadingCanvasLoad()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(CanvasAddress);
        //Debug.Log(handle);

        await handle.Task;
        Debug.Log(handle.Status);
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log(handle.Result);
            lodingCanvas = Instantiate( handle.Result.GetComponent<Canvas>());
        }
        else
        {
            Debug.Log("����");
        }
        Addressables.Release(handle);
    }

    public async void LoadScene(SceneName sceneName)
    {
        await LoadSceneAsync(sceneName);
        SaveManager.Instance.Load();
    }

    async Task LoadSceneAsync(SceneName sceneName)
    {
        // ���� �� ����
        Scene currentScene = SceneManager.GetActiveScene();
        // �ε� �� �ε�
        SceneManager.LoadScene("LoadingScene");

        await LoadingCanvasLoad();
        await SettingManager.Instance.LoadAsset();
        

        //Image loadingbar = lodingCanvas.transform.Find("Bar").GetComponent<Image>();

        await Task.Delay(1000); // ��: 1�� ���� �ε� ���� ������


        // ���ο� �� �񵿱� �ε�
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
         asyncOperation.allowSceneActivation = false;

         // �ε� ���� üũ �� ������Ʈ
         while (!asyncOperation.isDone)
         {
            // 0.0f���� 0.9f���� �����մϴ�.
            //loadingbar.fillAmount = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (asyncOperation.progress >= 0.9f)
             {
                 Debug.Log("���� ������ �̵�");
                //loadingbar.fillAmount = 1;
                // �������� �ε��� ��ε�
                asyncOperation.allowSceneActivation = true;
                

                /*SceneManager.UnloadSceneAsync("LoadingScene");
                SceneManager.UnloadSceneAsync(currentScene);*/
            }
             await Task.Yield(); // �񵿱� �������� �ٸ� �۾��� ����� �� �ֵ���
            //�̰ž����� ���Ϲ����ѹݺ�? ���� ���ߴ°��� ���� �������.
         }

        PlayManager.Instance.Init();
        ItemManager.Instance.Init();
        SkillManager.Instance.SkillSet();       
        
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
