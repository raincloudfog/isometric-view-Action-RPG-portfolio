using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Save;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Save
{
    using Items;
    using Player;
    using Skills;
    [Serializable]
    public class SaveName
    {
        public string[] Name = new string[3];
        public int Level;
    }

    [Serializable]
    public class SaveData
    {
        public SkillData[] skillDatas = new SkillData[4];
        public Inventory inventory;
        public EquipmentInventory equipment;
        public ItemData[] itemData;
    }

    [Serializable]
    public class SkillData
    {
        public string Key;
        public SkillName Name;

    }

    [Serializable]
    public class ItemData
    {        
        public int id;
        public eEquipmentType type;
        public int imgid;
    }
}



public class SaveManager : Singleton<SaveManager>
{
    private string[] saveDataName = new string[3]
    {
        "/playerData1.json",
        "/playerData2.json",
        "/playerData3.json",
    }
    ;
    public SaveName saveName = new SaveName();
    SaveData saveData = new SaveData();
    public string filePath = Application.persistentDataPath + "/playerData.json";

    

    public override void Init()
    {
        base.Init();

        SaveData data = Load();

        if (data.GetType() == typeof(SaveData))
        {

        }
    }

    public void NewSave(string Name)
    {
        string saveNamePath = Application.persistentDataPath + "/SaveName.json";
        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        saveData = new SaveData();
        saveName.Name[GameData.playerNumber] = Name;
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
        string Namejson = JsonUtility.ToJson(saveName);
        File.WriteAllText(saveNamePath, Namejson);
    }

    public void Save(SaveData data = null)
    {
        SaveData savedata = new SaveData();
        savedata.inventory = ItemManager.Instance._inventory;
        savedata.equipment = ItemManager.Instance._Equipment;

        savedata.itemData = new ItemData[ ItemManager.Instance._inventory.itemRange];
        for (int i = 0; i < savedata.itemData.Length; i++)
        {
            savedata.itemData[i].id = ItemManager.Instance._inventory.SettingItem(i).id;
            savedata.itemData[i].type = ItemManager.Instance._inventory.SettingItem(i).type;
            savedata.itemData[i].imgid = ItemManager.Instance._inventory.SettingItem(i).id;
        }


        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        string json = JsonUtility.ToJson(savedata);
        File.WriteAllText(path, json);
    }

    


    public void LoadName()
    {
        string saveNamePath = Application.persistentDataPath + "/SaveName.json";
        if (File.Exists(saveNamePath))
        {
            string loadStr = File.ReadAllText(saveNamePath);
            saveName = JsonUtility.FromJson<SaveName>(loadStr);
        }
    }

    public SaveData Load()
    {
        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        

        if (File.Exists(path)){
            Debug.Log("로드 성공");
            string loadStr = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(loadStr);

            ItemManager.Instance.LoadInven(data);

            return data;
        }
        else
        {
            Debug.Log("로드 실패");
            return new SaveData();
        }
    }    

    public void UpdateData(SaveData data)
    {
        Save(data);
    }

    // Start is called before the first frame update
    void Start()
    {
        //filePath = Application.persistentDataPath + "/playerData.json";
        LoadName();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Save();
        }
    }
}


