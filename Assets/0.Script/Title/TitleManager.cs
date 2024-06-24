using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    

    public PlayableDirector gameStartTimeLine;

    public Button[] buttons;

    public Button[] charactorSelectButton = new Button[3];

    public GameObject title;   
    public GameObject CharactorSelect;
    public GameObject NewCharactorUI;

    public SelectCharactor[] selectCharactors;

    public TMP_InputField CharactorName;

    public Camera maincam;
    public Transform camStartTransform;

    // Start is called before the first frame update
    void Start()
    {
        maincam.transform.position = camStartTransform.position; 
        gameStartTimeLine.stopped += direct => OnO0ffCharactorSelect(direct, true);
        buttons[0].onClick.AddListener(titleGameStart);
        buttons[1].onClick.AddListener(Option);
        buttons[2].onClick.AddListener(GameExit);
        CharactorName.characterLimit = 8; // ±ÛÀÚ¼ö Á¦ÇÑ
        CharactorName.onValueChanged.AddListener(
            (word) => CharactorName.text = Regex.Replace(word, @"[^0-9a-zA-z°¡-ÆR]", ""));
        //Æ¯¼ö¹®ÀÚ ÇÊÅÍ
        CharactorName.onEndEdit.AddListener(InputName);

        for (int i = 0; i < selectCharactors.Length; i++)
        {
            selectCharactors[i].id = i;
            selectCharactors[i].Init(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnO0ffCharactorSelect(PlayableDirector director, bool onoff)
    {
        CharactorSelect.SetActive(onoff);
    }

    #region Ä³¸¯ÅÍ ¼±ÅÃ ¹öÆ°

    
    #endregion

    #region °ÔÀÓ Å¸ÀÌÆ² ¹öÆ°
    void titleGameStart()
    {
        title.SetActive(false);

        gameStartTimeLine.Play();
        
    }

    void Option()
    {


    }

    void GameExit()
    {
        Application.Quit();
    }
    #endregion

    #region NewCharactorUI

    public void OnOffNewCharactor(bool isactive)
    {
        NewCharactorUI.SetActive(isactive);
    }

    public void InputName(string Name)
    {
        if(Name == null | Name =="")
        {
            OnOffNewCharactor(false);
            return;

        }


        SaveManager.Instance.NewSave(Name);
        selectCharactors[GameData.playerNumber].CreateCharactor(Name);
        OnOffNewCharactor(false);
    }
    

    #endregion
}
