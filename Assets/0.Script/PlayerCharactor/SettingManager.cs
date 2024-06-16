using Items;
using Player;
using Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public struct Itemid
{
    public static int[] ChestArmorid = new int[] { 0,1,2,3};
    public static int[] LegArmorid = new int[] { 4, 5, 6, 7 };
    public static int[] Helmetid = new int[] { 8, 9, 10, 11 };
    public static int[] Bootsid = new int[] { 12, 13, 14, 15 };
    public static int[] Weaponid = new int[] { 16, 17, 18, 19 };
}

[Serializable]
public class ItemSprite
{
    public ItemSprite() { }
    public ItemSprite(int id, SettingManager.ItemId type,Sprite spr) { imgid = id; this.type = type; itemSpr = spr; }
    public int imgid;
    public SettingManager.ItemId type;
    public Sprite itemSpr;
}

public class SettingManager : Singleton<SettingManager>
{    

    public enum ItemId
    {
        //���⼭ �׳� �����۹�ȣ�� 1000����� �Űܵ� �ɰŰ�����.
        //for ���� i = 1000���� �����ؼ� �ص� �� �� ����.
        ChestArmor0 = 0, 
        ChestArmor1 ,
        ChestArmor2,
        ChestArmor3,
        LegArmor0,
        LegArmor1,
        LegArmor2,
        LegArmor3,
        Helmet0,
        Helmet1,
        Helmet2,
        Helmet3,
        Boots0,
        Boots1,
        Boots2,
        Boots3,
        Weapon0,
        Weapon1,
        Weapon2, 
        Weapon3

    }

    public enum AddressableLabel
    {
        UI,
        Item,
        DropItem,
        Skill,
        ItemSpr,
    }

    public enum eUIResouceName
    {
        PlayerUI = 0,
        Inventory,
    }

    

    [Header("addressableAsset")]
    public string UIlabel;
    public string itemlabel;
    public string DropItemlabel;
    public GameObject[] UItest = new GameObject[2];
    public List<IUserInterface> UI = new List<IUserInterface>();
    public Dictionary<IUserInterface, GameObject> ResourcesObj = new Dictionary<IUserInterface, GameObject>();
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public GameObject[] Dropitems = new GameObject[2];
    public Skill[] Skills = new Skill[4];
    public ItemSprite[] itemSprites = new ItemSprite[16];


    public bool loadingUI;


    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        Setting();

        //await LoadAsset();
        //CreatePlayerUI();
    }

    // Update is called once per frame
   

    

    public async Task LoadAsset()
    {
        await PlayerUILoad();
        await ItemLoad();
        await CreateDropItem();
        await LoadItemSprite();
        await SkillLoad();

        Debug.Log("���� �� �ε� ��");
    }


    async Task LoadItemSprite()
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.ItemSpr.ToString());

        await locationsHandle.Task;

        if(locationsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = locationsHandle.Result;
            List<Task<Sprite>> loadTask = new List<Task<Sprite>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<Sprite> loadHandle =
                    Addressables.LoadAssetAsync<Sprite>(location);
                loadTask.Add(loadHandle.Task);
            }

            Sprite[] sprites =  await Task.WhenAll(loadTask);
            itemSprites = new ItemSprite[sprites.Length];

            for (int i = 0; i < itemSprites.Length; i++)
            {
                itemSprites[i] = new ItemSprite(i,(ItemId)i ,sprites[i]);
            }

            Addressables.Release(locationsHandle);
        }
    }

    async Task CreateDropItem()
    {
        AsyncOperationHandle<IList<IResourceLocation>> dropItemHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.DropItem.ToString(), typeof(GameObject));


        await dropItemHandle.Task;

        if (dropItemHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = dropItemHandle.Result;
            List<Task<GameObject>> loadTask = new List<Task<GameObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<GameObject> loadHandle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                loadTask.Add(loadHandle.Task);
            }
            GameObject[] gameObjects = await Task.WhenAll(loadTask);


            Dropitems = gameObjects;
            Addressables.Release(dropItemHandle);
        }


    }

    async Task SkillLoad()
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.Skill.ToString());

        await locationsHandle.Task;

        if(locationsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = locationsHandle.Result;
            List<Task<GameObject>> loadTask = new List<Task<GameObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<GameObject> loadHandle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                loadTask.Add(loadHandle.Task);
            }

            GameObject[] skills = await Task.WhenAll(loadTask);
            Skills = new Skill[skills.Length];

            for (int i = 0; i < skills.Length; i++)
            {

                Skills[i] = skills[i].GetComponent<Skill>();
                skills[i].SetActive(false);
            }

            Addressables.Release(locationsHandle);
        }
    }

    /*async Task SkillLoad()
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle =
            Addressables.LoadResourceLocationsAsync("Skill");

        await locationsHandle.Task;

        if (locationsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = locationsHandle.Result;
            List<Task<Skill>> loadTask = new List<Task<Skill>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<Skill> loadHandle =
                    Addressables.LoadAssetAsync<Skill>(location);
                loadTask.Add(loadHandle.Task);
            }

            Skill[] skills = await Task.WhenAll(loadTask);
            Skills = new Skill[skills.Length];

            for (int i = 0; i < skills.Length; i++)
            {

                Skills[i] = skills[i];
                skills[i].gameObject.SetActive(false);
            }

            Addressables.Release(locationsHandle);
        }
    }*/

    async Task PlayerUILoad()
    {
        /*canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            canvas = new GameObject("Canvas").AddComponent<Canvas>();
        }*/
        #region ���� �ڵ�
        /*for (int i = 0; i < address.Length; i++)
        {
            var Resource = Addressables.LoadAssetAsync<GameObject>(address[i]);

            await Resource.Task;

            if(ResourcesObj.ContainsKey((eUIResouceName)i))
            {
                Debug.Log(Resource.Result.name + "/ UI�̸� / UI�� �־ �ٲٴ°���");
                ResourcesObj[(eUIResouceName)i] = Instantiate( Resource.Result);

            }
            else
            {
                Debug.Log(Resource.Result.name + "/ UI�̸� / UI�� ��� ����°���");

                ResourcesObj.Add((eUIResouceName)i, Instantiate(Resource.Result));
            }

            ResourcesObj[(eUIResouceName)i].transform.SetParent(canvas.transform, false);

            Debug.Log("���� �� / " + ResourcesObj[(eUIResouceName)i]);

            Addressables.Release(Resource);
            Debug.Log("���� �� / " + ResourcesObj[(eUIResouceName)i]);
        }*/
        #endregion

        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.UI.ToString(), typeof(GameObject));

        await locationsHandle.Task;

        if(locationsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = locationsHandle.Result;
            List<Task<GameObject>> loadTask = new List<Task<GameObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<GameObject> loadHandle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                loadTask.Add(loadHandle.Task);
            }            

            GameObject[] gameObjects = await Task.WhenAll(loadTask);

            for (int i = 0; i < gameObjects.Length; i++)
            {                 
                IUserInterface type = gameObjects[i].GetComponent<IUserInterface>();                    
                UI.Add(type);
                UItest[i] = gameObjects[i];
                gameObjects[i].SetActive(false);
            }

            Addressables.Release(locationsHandle);
        }

        Debug.Log("�÷��̾� UI" + UI.Find(ui => ui as PlayerUI) + "�κ��丮UI" + UI.Find(ui => ui as InventoryUI));
        Debug.Log(Time.time + " / UI�ε�");
    }

    //���� ������ �̹��� �ҷ����°� ���ܼ� ���� �� ���� ����.
    async Task ItemLoad()
    {

        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.Item.ToString(), typeof(ScriptableObject));

        await locationsHandle.Task;

        if (locationsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = locationsHandle.Result;
            List<Task<ScriptableObject>> loadTasks = new List<Task<ScriptableObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<ScriptableObject> loadHandle =
                    Addressables.LoadAssetAsync<ScriptableObject>(location);
                loadTasks.Add(loadHandle.Task);

            }

            ScriptableObject[] loadedObjects = await Task.WhenAll(loadTasks);

            for (int i = 0; i < loadedObjects.Length; i++)
            {
                if (loadedObjects[i] is Item == false)
                {
                    Debug.Log("�ش� ��ũ���ͺ� ������Ʈ�� �������� �����ϴ�.!!");
                }
                if (ItemDic.ContainsKey(i))
                {
                    ItemDic[i] = loadedObjects[i] as Item;
                    int itemid = ItemDic[i].id;
                    ItemDic[i].type =  SetItemType(itemid);                    
                }
                else
                {
                    ItemDic.Add(i, loadedObjects[i] as Item);
                    int itemid = ItemDic[i].id;
                    ItemDic[i].type = SetItemType(itemid);
                }
            }

            Addressables.Release(locationsHandle);

            Debug.Log(Time.time + " / �����۷ε�");

        }
    }

    eEquipmentType SetItemType(int id)
    {
        if(id <= Itemid.Weaponid[Itemid.Weaponid.Length - 1 ] && id >= Itemid.Weaponid[0])
        {
            return eEquipmentType.Weapon;
        }
        else if (id <= Itemid.Bootsid[Itemid.Bootsid.Length -1] && id >= Itemid.Bootsid[0])
        {
            return eEquipmentType.Boots;
        }
        else if (id <= Itemid.LegArmorid[Itemid.LegArmorid.Length -1] && id >= Itemid.LegArmorid[0])
        {
            return eEquipmentType.LegArmor;
        }
        else if (id <= Itemid.ChestArmorid[Itemid.ChestArmorid.Length -1] && id >= Itemid.ChestArmorid[0])
        {
            return eEquipmentType.ChestArmor;
        }
        else if (id <= Itemid.Helmetid[Itemid.Helmetid.Length - 1] && id >= Itemid.Helmetid[0])
        {
            return eEquipmentType.Helmet;
        }

        Debug.Log("�ش� ��ȣ�� ���ԵǾ����� �ʴ� ��ȣ�� ������!!!! ");
        return eEquipmentType.Helmet;
    }
}
