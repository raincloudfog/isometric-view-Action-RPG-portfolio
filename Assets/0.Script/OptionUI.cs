using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Button[] buttons;

     void Start()
    {
        buttons[0].onClick.AddListener(Exit);
        buttons[1].onClick.AddListener(tothegame);
    }

    public void Exit()
    {
        if(SceneManager.GetActiveScene().name != "Town")
        {
            SceneLoaderManager.Instance.LoadScene(SceneLoaderManager.SceneName.Town);
        }
        else
        {
            Application.Quit();
        }
    }

    public void tothegame()
    {
        gameObject.SetActive(false);
    }
}
