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
        public int SaveVersion;
        public SkillData[] skillDatas = new SkillData[4];        
        public ItemData[] itemData;
        public ItemData[] EquipData;
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
        public ItemData()
        {
            isNull = true;
        }

        public bool isNull;
        public int id;
        public eEquipmentType type;
        public int Lank;
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
    [SerializeField]SaveData saveData = new SaveData();
    public string filePath;

    public bool isNewGame = false; 

    public override void Init()
    {
        base.Init();
        saveData = Load();
        bool isNull = saveData.itemData[0] == null;

        

        if (saveData.GetType() == typeof(SaveData))
        {

        }
    }

    public void DeleteSave()
    {
        string saveNamePath = Application.persistentDataPath + "/SaveName.json";
        string loadstr = File.ReadAllText(saveNamePath);
        SaveName saveName = JsonUtility.FromJson<SaveName>(loadstr);
        saveName.Name[GameData.playerNumber] = null;
        string savestr = JsonUtility.ToJson(saveName);
        File.WriteAllText(saveNamePath, savestr);
        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];        
        File.Delete(path);        
}

    public void NewSave(string Name)
    {
        string saveNamePath = Application.persistentDataPath + "/SaveName.json";
        string loadstr;

        if (File.Exists(saveNamePath))
        {
            loadstr = File.ReadAllText(saveNamePath);
            SaveName saveName = JsonUtility.FromJson<SaveName>(loadstr);
            saveName.Name[GameData.playerNumber] = Name;
            string Namejson = JsonUtility.ToJson(saveName);
            File.WriteAllText(saveNamePath, Namejson);
        }
        else
        {
            SaveName saveName = new SaveName();
            saveName.Name[GameData.playerNumber] = Name;
            string Namejson = JsonUtility.ToJson(saveName);
            File.WriteAllText(saveNamePath, Namejson);
        }
        

        SaveData savedata = new SaveData();

        savedata.itemData = new ItemData[16];
        savedata.EquipData = new ItemData[5];

        for (int i = 0; i < savedata.itemData.Length; i++)
        {
            savedata.itemData[i] = new ItemData();
        }
        for (int i = 0; i < savedata.EquipData.Length; i++)
        {
            savedata.EquipData[i] = new ItemData();
            savedata.EquipData[i].type = (Items.eEquipmentType)i;
        }

        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        string json = JsonUtility.ToJson(savedata);
        File.WriteAllText(path, json);
    }

    public void Save(SaveData data = null)
    {
        #region 만약 아이템 데이터가 없을경우
        SaveData savedata = new SaveData();

        savedata.itemData = new ItemData[16];
        savedata.EquipData = new ItemData[5];

        
        for (int i = 0; i < savedata.itemData.Length; i++)
        {
            savedata.itemData[i] = new ItemData();
        }
        for (int i = 0; i < savedata.EquipData.Length; i++)
        {

            savedata.EquipData[i] = new ItemData();
            savedata.EquipData[i].type = (Items.eEquipmentType)i;
        }
       
        savedata.itemData = ItemManager.Instance._inventory.SettingItem();
        savedata.EquipData = ItemManager.Instance._Equipment.SettingItem();


        #endregion

        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        string json = JsonUtility.ToJson(savedata);
        File.WriteAllText(path, json);
    }

    /*public void Save0(SaveData data = null)
    {
        SaveData savedata = new SaveData();        

        savedata.itemData = new ItemData[ ItemManager.Instance._inventory.itemRange];
        for (int i = 0; i < savedata.itemData.Length; i++)
        {
            savedata.itemData[i].id = ItemManager.Instance._inventory.SettingItem(i).id;
            savedata.itemData[i].type = ItemManager.Instance._inventory.SettingItem(i).type;
        }


        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        string json = JsonUtility.ToJson(savedata);
        File.WriteAllText(path, json);
    }*/

    


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


        if (File.Exists(path))
        {
            Debug.Log("로드 성공");
            string loadStr = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(loadStr);            

            ItemManager.Instance.LoadInven(data);

            return data;
        }
        else
        {
            SaveData data = new SaveData();
           
            data.itemData = new ItemData[16];
            data.EquipData = new ItemData[5];

            for (int i = 0; i < data.itemData.Length; i++)
            {
                data.itemData[i] = new ItemData();
            }
            for (int i = 0; i < data.EquipData.Length; i++)
            {

                data.EquipData[i] = new ItemData();
                data.EquipData[i].type = (Items.eEquipmentType)i;
            }

            Debug.Log("세이브 파일이 없음");

            ItemManager.Instance.LoadInven(data);

            return data;
        }
    }    

    public SaveData Load0()
    {
        string path = Application.persistentDataPath + saveDataName[GameData.playerNumber];
        

        if (File.Exists(path)){
           // Debug.Log("로드 성공");
            string loadStr = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(loadStr);

            ItemManager.Instance.LoadInven(data);

            return data;
        }
        else
        {
            //Debug.Log("로드 실패");
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
       

        filePath = Application.persistentDataPath;

        SaveData testData = new SaveData();
        testData.itemData = new ItemData[16];

        for (int i = 0; i < testData.itemData.Length; i++)
        {
            if (testData.itemData[i] == null)

            {
                testData.itemData[i] = new ItemData();
            }
        }
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


