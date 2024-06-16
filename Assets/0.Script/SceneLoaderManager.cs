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

    //로딩 씬에서 가져올 로딩 캔버스
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
            Debug.Log("실패");
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
        // 현재 씬 저장
        Scene currentScene = SceneManager.GetActiveScene();
        // 로딩 씬 로드
        SceneManager.LoadScene("LoadingScene");

        await LoadingCanvasLoad();
        await SettingManager.Instance.LoadAsset();
        

        //Image loadingbar = lodingCanvas.transform.Find("Bar").GetComponent<Image>();

        await Task.Delay(1000); // 예: 1초 동안 로딩 씬을 보여줌


        // 새로운 씬 비동기 로드
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
         asyncOperation.allowSceneActivation = false;

         // 로딩 상태 체크 및 업데이트
         while (!asyncOperation.isDone)
         {
            // 0.0f에서 0.9f까지 증가합니다.
            //loadingbar.fillAmount = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (asyncOperation.progress >= 0.9f)
             {
                 Debug.Log("다음 씬으로 이동");
                //loadingbar.fillAmount = 1;
                // 이전씬과 로딩씬 언로드
                asyncOperation.allowSceneActivation = true;
                

                /*SceneManager.UnloadSceneAsync("LoadingScene");
                SceneManager.UnloadSceneAsync(currentScene);*/
            }
             await Task.Yield(); // 비동기 루프에서 다른 작업이 실행될 수 있도록
            //이거없으면 와일문무한반복? 으로 멈추는건지 몰라도 멈춰버림.
         }

        PlayManager.Instance.Init();
        ItemManager.Instance.Init();
        SkillManager.Instance.SkillSet();       
        
    }

    /*async void LoadSceneAsync(SceneName sceneName)
    {
        // 현재 씬 저장
        Scene currentScene = SceneManager.GetActiveScene();
        // 로딩 씬 로드
        SceneManager.LoadScene("LoadingScene");

        // 새로운 씬 비동기 로드
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOperation.allowSceneActivation = false;

        // 로딩 상태 체크 및 업데이트
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("다음 씬으로 이동");
                asyncOperation.allowSceneActivation = true;
            }
            await Task.Yield(); // 비동기 루프에서 다른 작업이 실행될 수 있도록
        }

        *//*//로딩 상태체크 및 업데이트
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("일단로드는다되었으니 셋팅매니저에서 에셋받을때까지 기다리기");
                await SettingManager.Instance.LoadAsset();
                asyncOperation.allowSceneActivation = true;
                Debug.Log("에셋로드 다되었으니 다음 씬으로 이동");
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
        //이전씬과 로딩씬 언로드
        //SceneManager.UnloadSceneAsync("LoadingScene");
        //SceneManager.UnloadSceneAsync(currentScene);
    }*/



    /*IEnumerator LoadSceneAsync(SceneName sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene("LoadingScene");



        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOperation.allowSceneActivation = false;

        //로딩 상태체크 및 업데이트
        while(!asyncOperation.isDone)
        {
            if(asyncOperation.progress >= 0.9f)
            {
                Debug.Log("일단로드는다되었으니 셋팅매니저에서 에셋받을때까지 기다리기");
                await SettingManager.Instance.loadingUI;
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        //이전씬과 로딩씬 언로드
        SceneManager.UnloadSceneAsync("LoadingScene");
        SceneManager.UnloadSceneAsync(currentScene);

    }*/

    #region  씬 로드와 비동기 로드에 대해

    /*
      씬 로드 
    씬에 포함된 모든 리소스를 메모리에 로드, 게임오브젝트 생성 생성과정에서 오브젝트의 awake호출
    씬활성화시 Start가 호출

      비동기 로드
    씬이 로드 되는동안 오브젝트가 생성되면 awake 호출 하지만 start는 씬이 활성화 된후 호출
     */

    #endregion


}
