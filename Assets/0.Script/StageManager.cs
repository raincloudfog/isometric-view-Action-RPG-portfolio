using Monster;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Enemy = Monster.Monster;
using SceneName = SceneLoaderManager.SceneName;


public class StageManager : MonoBehaviour
{
    public OptionUI _optionUi;

    public StageUI _stageUI;

    public Portal stagePortal;
    public Portal _bossRoomPortal;

    public Enemy[] monster;
    [SerializeField]
    private int monsterNumber;

    public int stageNumber;

    public CheckMonster checkMonster;

    //public UnityEvent OnEnter;
    public Boss _boss;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        if(checkMonster != null)
        {
            checkMonster.CheckMosnter();
            monster = checkMonster.monster;
            monsterNumber = checkMonster.monsterNumber;
        }

        GameStateManager.stageManage = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameData.isOpenUI)
            {

                if(ItemManager.Instance.inventoryUI.gameObject.activeSelf == true)
                {
                    ItemManager.Instance.OnOffInventory();
                }

                if (_stageUI != null && _stageUI.gameObject.activeSelf)
                {
                    _stageUI.gameObject.SetActive(false);
                }

                if (_optionUi != null && _optionUi.gameObject.activeSelf)
                {
                    _optionUi.gameObject.SetActive(false);
                }
                GameData.isOpenUI = false;
                return;
            }

            if (_optionUi != null && _optionUi.gameObject.activeSelf == false)
            {
                GameData.isOpenUI = true;
                _optionUi.gameObject.SetActive(true);
            }
        }
    }

    public void OpenExitPortal()
    {
        stagePortal.gameObject.SetActive(true);
    }

    public void CheckKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            string key = context.control.name;
            Debug.Log("입력된 키 " + key);
            if (key == "f")
            {
                if(_bossRoomPortal != null &&_bossRoomPortal.isPortal)
                {
                    GameData.isEnterthebossroom = true;
                    _boss.Init();
                    _boss.OnDeath.AddListener(OpenExitPortal);
                    PlayManager.Instance.player.MovePosition( _bossRoomPortal.EndPos.position);

                    return;
                }

                if (stagePortal.isPortal && _stageUI == null)
                {
                    SceneLoaderManager.Instance.LoadScene(SceneName.Town);
                }
                else if (stagePortal.isPortal && _stageUI != null)
                {
                    _stageUI.gameObject.SetActive(true);
                }
            }
            /*else if (key == "escape")
            {
                _stageUI.gameObject.SetActive(false);
            }*/
        }

        
    }    

    public void MonsterDead()
    {
        /*monsterNumber--;

        if(monsterNumber <= 0)
        {
            Debug.Log("모든 몬스터가 죽었습니다.");
            GameStateManager.ChageState(GameStateManager.State.StageClear);
        }*/
    }


}
