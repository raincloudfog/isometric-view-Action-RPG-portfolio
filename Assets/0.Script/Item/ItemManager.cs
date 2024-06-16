using Items;
using Player;
using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static SettingManager;

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

        inventoryUI = Instantiate(
            SettingManager.Instance.UI.Find(ui => ui is InventoryUI) as InventoryUI);
        
        canvas = FindObjectOfType<Canvas>();

        inventoryUI.transform.SetParent(canvas.transform, false);   

        inventoryUI.Init();
        //inventoryUI.transform.SetParent(SettingManager.Instance.canvas.transform, false);        
    }


    public void LoadInven(SaveData data = null)
    {
        if (data == null)
        {
            _inventory = _inventory = new Inventory(); 
            _Equipment = new EquipmentInventory();
        }
        else
        {
            _inventory = data.inventory;
            _Equipment = data.equipment;
        }
        
        
    }

    public void LoadInvenUI()
    {
        for (int i = 0; i < _inventory.ItemCheckList.Count; i++)
        {
            AddItem( _inventory.SettingItem(i));
            inventoryUI.EquipItem(_Equipment.Equipment[(eEquipmentType)i]);
        }
    }

    public void AddItem(Item item)
    {
        if(_inventory.IsFullItem())
        {
            Debug.Log("������ ��������");
            return;
        }

        //�ش� ��ȣ�� �κ��丮 ���Կ� ������
        int id =  _inventory.AddItem(item);
        inventoryUI.AddItem(id,item);
        
    }

    public void OnOffInventory()
    {
        bool isOnOff = !inventoryUI.gameObject.activeSelf;
        Debug.Log(isOnOff + "�κ��丮 �ѱ� / ����");
        inventoryUI.gameObject.SetActive(isOnOff);
    }

    public void DropItem(int id)
    {
        _inventory.DropItem(id);
        
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
