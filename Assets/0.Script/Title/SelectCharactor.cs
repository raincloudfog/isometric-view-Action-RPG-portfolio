using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectCharactor : MonoBehaviour, IPointerClickHandler
{
    public int id;

    public TitleManager _titleManager;
    //0.name 1. Lv , 2. CreatePlus

    public TMP_Text[] texts;

    private int clickCount = 0;


    //세이브가 있는지
    public bool isSave;

    public PlayerPlate plate;

    Coroutine DoubleClickCoroutine;

    public void Init(TitleManager title)
    {
        _titleManager = title;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("세이브 이름  + [" + SaveManager.Instance.saveName.Name[id] + "]");
        isSave = SaveManager.Instance.saveName.Name[id] != "";
        //이런식으로 하면 비활성화되어있는 오브젝트도 담기가능.
        texts = GetComponentsInChildren<TMP_Text>(true);
        if (isSave)
        {
            plate.gameObject.SetActive(true);
            CreateCharactor(SaveManager.Instance.saveName.Name[id]);
        }
    }
        

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭된 버튼 확인
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(DoubleClickCoroutine == null)
            {
                StartCoroutine(DoubleClick());
            }

            if(isSave == false)
            {
                _titleManager.OnOffNewCharactor(true);
                //CreateCharactor();
            }
            else
            {
                clickCount++;
            }
            Debug.Log("Left button clicked");
        }
    }

    void ChangeScene()
    {
        SceneLoaderManager.Instance.LoadScene(SceneLoaderManager.SceneName.Town);
        //SceneManager.LoadSceneAsync("Town");
    }

    public void CreateCharactor(string Name = " ")
    {
        plate.gameObject.SetActive(true);
        texts[0].gameObject.SetActive(true);
        texts[1].gameObject.SetActive(true);
        texts[0].text =  "Name : "+ Name;
        texts[1].text = "Lv : " + 1;
        texts[2].gameObject.SetActive(false);


        isSave = true;
    }

    IEnumerator DoubleClick()
    {
        float timer = 0;
        float ClickTimer = 1f;

        while(timer < ClickTimer)
        {
            yield return null;
            if (clickCount >= 2)
            {
                GameData.playerNumber = id;
                SaveManager.Instance.Load();
                ChangeScene();
            }
        }
    }

    
}
