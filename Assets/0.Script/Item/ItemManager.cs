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

    //�κ� �丮
    public Inventory _inventory;
    public EquipmentInventory _Equipment;


    public void UIOnOff(bool istrue)
    {
        inventoryUI.gameObject.SetActive(istrue);
    }


    //���� �ε� �ɶ� �Ҹ�
    public override void Init()
    {
        base.Init();

        UnityEngine.SceneManagement.Scene scene =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name == "Title")
        {
            inventoryUI = null;
        }

        //[Serializable]�� ����� ��ũ��Ʈ�� ��� �ʱ�ȭ�� ���� �����൵ �ڵ����� �ʱ�ȭ�� ��.        
        //LoadInven();

        canvas = FindObjectOfType<Canvas>();

        inventoryUI = 
        Instantiate(SettingManager.Instance.AInventoryUI).GetComponent<InventoryUI>();
        Debug.Log("���� ����� �ε�Ǿ �̰ɷ� ���");
        inventoryUI.gameObject.SetActive(false);
        
        inventoryUI.transform.SetParent(canvas.transform, false);   

        inventoryUI.Init();

        //�κ��丮�� �ִ� �����۵� �����ֱ�.
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


    //ó�� �ε� �ɶ� �Ҹ�. �ϴ� �κ��丮 ���� ���� �޴� �Լ�.
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

    //�ش� ���Կ� ������ �־��ٶ�
    public void AddItem(int id, ItemData item)
    {
        _inventory.AddItem(id, item);
        inventoryUI.AddItem(id, item);
        SaveManager.Instance.Save();
    }

    public void AddItem(ItemData item)
    {
        //�ش� ��ȣ�� �κ��丮 ���Կ� ������
       // Debug.Log("���� ������ Ȯ��" + item);
       // Debug.Log("�κ��丮 Ȯ��" + _inventory.IsFullItem());
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

       // Debug.Log(isOnOff + "�κ��丮 �ѱ� / ����");
        inventoryUI.gameObject.SetActive(isOnOff);
        GameData.isOpenUI = inventoryUI.gameObject.activeSelf;

    }

    public void DropItem(int id)
    {
        _inventory.DropItem(id);
        SaveManager.Instance.Save();

    }

    #region ���� �ڵ�
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
            Debug.Log("�ҷ����⸦ �����߽��ϴ�.");
        }
    }*/
    #endregion
}
