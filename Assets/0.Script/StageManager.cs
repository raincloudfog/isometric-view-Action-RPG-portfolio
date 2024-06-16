using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Enemy = Monster.Monster;
using SceneName = SceneLoaderManager.SceneName;


public class StageManager : MonoBehaviour
{
    public StageUI _stageUI;

    public Portal stagePortal;

    public Enemy[] monster;
    private int monsterNumber;

    public int stageNumber;

    public CheckMonster checkMonster;

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
    

    public void CheckKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            string key = context.control.name;
            Debug.Log("입력된 키 " + key);
            if (key == "f")
            {
                if (stagePortal.IsPortal && _stageUI == null)
                {
                    SceneLoaderManager.Instance.LoadScene(SceneName.Town);
                }
                else if (stagePortal.IsPortal && _stageUI != null)
                {
                    _stageUI.gameObject.SetActive(true);
                }
            }
            else if (key == "escape")
            {
                _stageUI.gameObject.SetActive(false);
            }
        }

        
    }    

    public void MonsterDead()
    {
        monsterNumber--;

        if(monsterNumber <= 0)
        {
            Debug.Log("모든 몬스터가 죽었습니다.");
            GameStateManager.ChageState(GameStateManager.State.StageClear);
        }
    }


}
