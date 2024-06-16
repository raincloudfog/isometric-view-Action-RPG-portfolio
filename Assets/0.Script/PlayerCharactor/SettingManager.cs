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
        //여기서 그냥 아이템번호를 1000번대로 옮겨도 될거같긴함.
        //for 문의 i = 1000으로 시작해서 해도 될 것 같음.
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

        Debug.Log("에셋 다 로드 됨");
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
        #region 예전 코드
        /*for (int i = 0; i < address.Length; i++)
        {
            var Resource = Addressables.LoadAssetAsync<GameObject>(address[i]);

            await Resource.Task;

            if(ResourcesObj.ContainsKey((eUIResouceName)i))
            {
                Debug.Log(Resource.Result.name + "/ UI이름 / UI가 있어서 바꾸는거임");
                ResourcesObj[(eUIResouceName)i] = Instantiate( Resource.Result);

            }
            else
            {
                Debug.Log(Resource.Result.name + "/ UI이름 / UI가 없어서 만드는거임");

                ResourcesObj.Add((eUIResouceName)i, Instantiate(Resource.Result));
            }

            ResourcesObj[(eUIResouceName)i].transform.SetParent(canvas.transform, false);

            Debug.Log("해제 전 / " + ResourcesObj[(eUIResouceName)i]);

            Addressables.Release(Resource);
            Debug.Log("해제 후 / " + ResourcesObj[(eUIResouceName)i]);
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

        Debug.Log("플레이어 UI" + UI.Find(ui => ui as PlayerUI) + "인벤토리UI" + UI.Find(ui => ui as InventoryUI));
        Debug.Log(Time.time + " / UI로드");
    }

    //위의 아이템 이미지 불러오는게 생겨서 수정 할 수도 있음.
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
                    Debug.Log("해당 스크립터블 오브젝트에 아이템이 없습니다.!!");
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

            Debug.Log(Time.time + " / 아이템로드");

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

        Debug.Log("해당 번호에 포함되어있지 않는 번호의 아이템!!!! ");
        return eEquipmentType.Helmet;
    }
}
