using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneName = SceneLoaderManager.SceneName;

public class StageUI : MonoBehaviour
{
    public Button[] Stage;
    public Button[] updown;

    public TMP_Text Lank;

    // Start is called before the first frame update
    void Start()
    {
        SetButton();
    }    

    void SetButton()
    {
        for (int i = 0; i < Stage.Length; i++)
        {
            int num = i;
            Stage[i].onClick.AddListener(() => SceneChange((SceneName)num));
        }

        updown[0].onClick.AddListener(() => updownButtonSet(true));
        updown[1].onClick.AddListener(() => updownButtonSet(false));
    }

    void SceneChange(SceneName name)
    {
        SceneLoaderManager.Instance.LoadScene(name);

    }

    void updownButtonSet(bool isPlus)
    {
        if(isPlus == true)
        {
            GameStateManager.lank++;
        }
        else 
        {
            GameStateManager.lank--;
        }

        Lank.text = (GameStateManager.lank).ToString();
    }
}
