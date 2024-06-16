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

    //인벤 토리
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

        //[Serializable]을 사용한 스크립트의 경우 초기화를 따로 안해줘도 자동으로 초기화가 됨.        
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
            Debug.Log("아이템 꽉차있음");
            return;
        }

        //해당 번호의 인벤토리 슬롯에 아이템
        int id =  _inventory.AddItem(item);
        inventoryUI.AddItem(id,item);
        
    }

    public void OnOffInventory()
    {
        bool isOnOff = !inventoryUI.gameObject.activeSelf;
        Debug.Log(isOnOff + "인벤토리 켜기 / 끄기");
        inventoryUI.gameObject.SetActive(isOnOff);
    }

    public void DropItem(int id)
    {
        _inventory.DropItem(id);
        
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
