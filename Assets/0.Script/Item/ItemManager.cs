using Items;
using Player;
using Save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class ItemManager : Singleton<ItemManager>
{
    public int itemRange = 16;

    public string[] address;

    public Canvas canvas;
    public InventoryUI inventoryUI;

    //인벤 토리
    public Inventory _inventory;
    public EquipmentInventory _Equipment;


    public void UIOnOff(bool istrue)
    {
        inventoryUI.gameObject.SetActive(istrue);
    }


    //에셋 로드 될때 불림
    public override void Init()
    {
        base.Init();

        UnityEngine.SceneManagement.Scene scene =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name == "Title")
        {
            inventoryUI = null;
        }

        //[Serializable]을 사용한 스크립트의 경우 초기화를 따로 안해줘도 자동으로 초기화가 됨.        
        //LoadInven();

        canvas = FindObjectOfType<Canvas>();

        inventoryUI = 
        Instantiate(SettingManager.Instance.AInventoryUI).GetComponent<InventoryUI>();
        Debug.Log("에셋 제대로 로드되어서 이걸로 사용");
        inventoryUI.gameObject.SetActive(false);
        
        inventoryUI.transform.SetParent(canvas.transform, false);   

        inventoryUI.Init();

        //인벤토리에 있던 아이템들 전해주기.
        for (int i = 0; i < _inventory.itemList.Length; i++)
        {
            inventoryUI.AddItem(i, _inventory.itemList[i]);
        }

        for (int i = 0; i < _Equipment.itemList.Length; i++)
        {
            inventoryUI.EquipItem(_Equipment.itemList[i]);
        }

        //inventoryUI.transform.SetParent(SettingManager.Instance.canvas.transform, false);        
    }


    //처음 로드 될때 불림. 일단 인벤토리 값을 전달 받는 함수.
    public void LoadInven(SaveData data = null)
    {
        _inventory = new Inventory();
        _Equipment = new EquipmentInventory();

        for (int i = 0; i < data.itemData.Length; i++)
        {
            //Debug.Log(data.itemData[i].isNull);
            int id = _inventory.AddItem(data.itemData[i]);
        }

        for (int i = 0; i < data.EquipData.Length; i++)
        {
            _Equipment.EquipItem(data.EquipData[i].type,data.EquipData[i]);
        }


    }

    //해당 슬롯에 아이템 넣어줄때
    public void AddItem(int id, ItemData item)
    {
        _inventory.AddItem(id, item);
        inventoryUI.AddItem(id, item);
        SaveManager.Instance.Save();
    }

    public void AddItem(ItemData item)
    {
        //해당 번호의 인벤토리 슬롯에 아이템
       // Debug.Log("받은 아이템 확인" + item);
       // Debug.Log("인벤토리 확인" + _inventory.IsFullItem());
        int id =  _inventory.AddItem(item);
        if(id < 0)
        {
        }
        else
        {
            inventoryUI.AddItem(id, item);
            SaveManager.Instance.Save();
        }

    }

    public void OnOffInventory()
    {
        bool isOnOff = !inventoryUI.gameObject.activeSelf;

       // Debug.Log(isOnOff + "인벤토리 켜기 / 끄기");
        inventoryUI.gameObject.SetActive(isOnOff);
        GameData.isOpenUI = inventoryUI.gameObject.activeSelf;

    }

    public void DropItem(int id)
    {
        _inventory.DropItem(id);
        SaveManager.Instance.Save();

    }

    #region 이전 코드
    /*async void CreateObjetc()
    {
        var objHandle = Addressables.LoadAssetAsync<GameObject>(address[0]);

        await objHandle.Task;

        inventoryObj = Instantiate( objHandle.Result);
        inventoryObj.transform.SetParent(canvas.transform ,  false);
    }

    private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject prefab = obj.Result;
            inventoryObj = Instantiate(prefab);
        }
        else
        {
            Debug.Log("불러오기를 실패했습니다.");
        }
    }*/
    #endregion
}
