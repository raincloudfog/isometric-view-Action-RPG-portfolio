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

    //로딩 씬에서 가져올 로딩 캔버스 // 굳이 사용할 필요 없을거같음.
    /*async Task LoadingCanvasLoad()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(CanvasLabel);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = handle.Result;

            if (loadedObject != null)
            {
                //Debug.Log("로드 성공: " + loadedObject.name);

                // 예시로 Canvas 컴포넌트를 가져와서 활성화합니다.
                Canvas canvasComponent = loadedObject.GetComponent<Canvas>();
                if (canvasComponent != null)
                {
                    lodingCanvas = Instantiate(canvasComponent);
                    loadedObject.SetActive(true);

                    Image[] imgs = lodingCanvas.GetComponentsInChildren<Image>();

                    for (int i = 0; i < imgs.Length; i++)
                    {
                        Debug.Log(imgs[i].sprite + "이미지 스프라이트확인");
                        if (imgs[i].sprite == null)
                            Debug.Log("이미지 스프라이트 널임");
                    }
                }
                else
                {
                    Debug.LogError("로드이미지는성공했는데 게임오브젝트에 캔버스가 없음");
                }
            }
            else
            {
                Debug.LogError("로드이미지는 성공 오브젝트가 널임");
            }
        }
        else
        {
            Debug.LogError("로드 이미지 불러오기 실패: " + handle.OperationException);
        }

        Addressables.Release(handle);
    }*/

    public void LoadScene(SceneName sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /*public async void LoadSceneAsyncMethod(SceneName sceneName)
    {
        await LoadSceneAsync(sceneName);
        
    }*/

    IEnumerator LoadSceneCoroutine(SceneName sceneName)
    {
        // 현재 씬 저장
        Scene currentScene = SceneManager.GetActiveScene();
        // 로딩 씬 로드
        SceneManager.LoadScene("LoadingScene");

        // SettingManager의 LoadAsset 호출을 기다림
        //yield return StartCoroutine( SettingManager.Instance.LoadAssetCoroutine());
        yield return StartCoroutine(SettingManager.Instance.LoadAssetCoroutine());

         // SaveManager 초기화
         SaveManager.Instance.Init();

        // 예: 1초 동안 로딩 씬을 보여줌
        yield return new WaitForSeconds(1f);

        // 새로운 씬 비동기 로드
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName.ToString());
        asyncOperation.allowSceneActivation = false;

        // 로딩 상태 체크 및 업데이트
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("다음 씬으로 이동");
                // 이전 씬과 로딩 씬 언로드
                asyncOperation.allowSceneActivation = true;
            }
            yield return null; // 비동기 루프에서 다른 작업이 실행될 수 있도록
        }

        SkillManager.Instance.SkillSet();
        PlayManager.Instance.Init();
        ItemManager.Instance.Init();
    }
 

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
