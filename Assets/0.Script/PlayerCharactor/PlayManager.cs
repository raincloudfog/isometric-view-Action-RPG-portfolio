using Cinemachine.Utility;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static SettingManager;

public class PlayManager : Singleton<PlayManager> 
{
    public Player.Player player;

    public PlayerUI playerUI;

    //오브젝트 풀
    public ParticleSystem _hitparticle;
    public ObjectPool _hitParticlePool;

    public Vector3 pointPosition = Vector3.zero; 

    public void Start()
    {
        Setting();
    }

    public void Update()
    {
        
    }


    public void UIOnOff(bool istrue)
    {
        playerUI.gameObject.SetActive(istrue);
    }

    public void FindPlayer()
    {
        if(player == null)
        {
            Player.Player p = FindObjectOfType<Player.Player>();

            player = p;
            if(player == null)
            {
                player  = new GameObject(typeof(Player.Player).Name).AddComponent<Player.Player>();
            }
            player.Init();
        }
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
        

        
        playerUI =
        Instantiate(SettingManager.Instance.APlayerUI).GetComponent<PlayerUI>();
        Debug.Log("에셋 제대로 로드되어서 이걸로 사용");
        playerUI.gameObject.SetActive(false);


        playerUI.gameObject.SetActive(true);
        Canvas canvas = FindObjectOfType<Canvas>();
        playerUI.transform.SetParent(canvas.transform, false);
        UIOnOff(true);
        FindPlayer();
        _hitParticlePool = new ObjectPool(_hitparticle.gameObject);

        //playerUI.transform.SetParent(SettingManager.Instance.canvas.transform, false);
    }

    public Vector3 MousePositionNoClick(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 point = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("DropItem"))
            {
                /*GameData.isPickingItem = true;
                Debug.Log(GameData.isPickingItem);*/

                return transform.position;
            }

            point = hit.point;
            return point;
        }

        return position;
    }

    public void SetPoint(Vector3 point)
    {
        pointPosition = point;
    }

    public Vector3 MousePosition(Vector3 position)
    {

        if((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) )&& !GameData.isOpenUI)
        {
            Vector3 mouseposition = Vector3.zero;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("DropItem"))
                {
                    /*GameData.isPickingItem = true;
                    Debug.Log(GameData.isPickingItem);*/

                    return transform.position;
                }

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
