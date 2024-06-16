using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static SettingManager;

public class PlayManager : Singleton<PlayManager> 
{
    public PlayerUI playerUI;

    public Vector3 pointPosition = Vector3.zero; 

    public void Start()
    {
        
    }

    public void UIOnOff(bool istrue)
    {
        playerUI.gameObject.SetActive(istrue);
    }

    public override void Init()
    {
        UnityEngine.SceneManagement.Scene scene =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        Debug.Log(scene.name + "씬이름");
        if (scene.name == "Title")
        {
            playerUI = null;
        }

        base.Init();
        Debug.Log(Time.time + " / 플레이매니저로드");

        Debug.Log(SettingManager.Instance.UI[0] + " " +SettingManager.Instance.UI[1]);

        playerUI =
            Instantiate(
             SettingManager.Instance.UI.Find(ui => ui is  PlayerUI) as PlayerUI);

        Canvas canvas = FindObjectOfType<Canvas>();
        playerUI.transform.SetParent(canvas.transform, false);
        UIOnOff(true);
        //playerUI.transform.SetParent(SettingManager.Instance.canvas.transform, false);
    }

    public Vector3 MousePosition(Vector3 position)
    {
        
        if(Input.GetMouseButtonDown(0) && !GameData.isPickingItem)
        {
            Vector3 mouseposition = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("DropItem"))
                {
                    GameData.isPickingItem = true;
                    Debug.Log(GameData.isPickingItem);

                    return transform.position;
                }

                pointPosition = hit.point;
                return pointPosition;
            }
        }
        else if (Input.GetMouseButton(0) && !GameData.isPickingItem)
        {
            Vector3 mouseposition = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                pointPosition = hit.point;
                return pointPosition;
            }
        }
        
        if(pointPosition != position)
        {
            return pointPosition;
        }


        return position;

    }    
}
