using Items;
using Player;
using Save;
using Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.XR;
using static UnityEditor.FilePathAttribute;

public struct Itemid
{
    public static int[] ChestArmorid = new int[] { 0,1,2,3};
    public static int[] Bootsid = new int[] { 4, 5, 6, 7 };
    public static int[] Helmetid = new int[] { 8, 9, 10, 11 };
    public static int[] LegArmorid = new int[] { 12, 13, 14, 15 };
    public static int[] Weaponid = new int[] { 16, 17, 18, 19 };
}

[Serializable]
public class ItemSprite
{
    public ItemSprite() { }
    public ItemSprite(int id, eEquipmentType type, Sprite spr, int Lank) { imgid = id; this.type = type; itemSpr = spr; this.Lank = Lank;  Debug.Log("������ ��������Ʈ ������!!"); }

    public int imgid;
    public int Lank;
    public eEquipmentType type;
    public UnityEngine.Sprite itemSpr;
}

public class SettingManager : Singleton<SettingManager>
{

    public enum ItemId
    {
        //���⼭ �׳� �����۹�ȣ�� 1000����� �Űܵ� �ɰŰ�����.
        //for ���� i = 1000���� �����ؼ� �ص� �� �� ����.
        ChestArmor0 = 0,
        ChestArmor1,
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
        ItemSprite,
    }

    public enum eUIResouceName
    {
        PlayerUI = 0,
        Inventory,
    }

    [Header("addressableAssetReference")]

    public AssetLabelReference UIlabel;
    public AssetLabelReference Skilllabel;

    /*public string UIlabel;
    public string itemlabel;
    public string DropItemlabel;
    public GameObject[] UItest = new GameObject[2];
    public List<IUserInterface> UI = new List<IUserInterface>();
    public Dictionary<IUserInterface, GameObject> ResourcesObj = new Dictionary<IUserInterface, GameObject>();
    public Dictionary<int, Item> ItemDic = new Dictionary<int, Item>();
    public GameObject[] Dropitems = new GameObject[2];
    public Skill[] Skills = new Skill[4];
    public ItemSprite[] itemSprites = new ItemSprite[16];
*/
    AsyncOperationHandle<IList<IResourceLocation>> AUIHandle;
    AsyncOperationHandle<GameObject> ADropItemsHandle;
    AsyncOperationHandle<IList<IResourceLocation>> ASkillsHandle;
    AsyncOperationHandle<IList<IResourceLocation>> AItemSpriteHandle;

    [Header("addressableAsset")]
    public List<ItemSprite> AitemSprites = new List<ItemSprite>();
    public GameObject APlayerUI;
    public GameObject AInventoryUI;
    public GameObject ADropitem;
    public List<GameObject> ASkills = new List<GameObject>();
    public bool isLoadedAsset =false;

    [Header("TestaddressableAsset")]
    public GameObject testUI;
    public AsyncOperationHandle<GameObject> testhandle;
    public bool loadingUI;


    public Canvas canvas;   
   

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("���� �Ŵ��� �Ҹ� ");
        Setting();
        //StartCoroutine( LoadAssetCoroutine());
        /*StartCoroutine(LoadUI());
        StartCoroutine( LoadSkill());
        StartCoroutine(LoadSprite());
        StartCoroutine(LoadDropitem());*/

        Cursor.visible = true;
        //await LoadAsset();      
    }

    // Update is called once per frame
    public IEnumerator LoadAssetCoroutine()
    {
        if(isLoadedAsset)
        {
            yield break;
        }
        //Debug.Log("üũ 1 : LoadUI");
        yield return StartCoroutine(LoadUI());
        //Debug.Log("üũ 2 : LoadSkill");
        yield return StartCoroutine(LoadSkill());
        //Debug.Log("üũ 3 : SpriteI");
        yield return StartCoroutine(LoadSprite());
        //Debug.Log("üũ 4 : LoadDropItem");
        yield return StartCoroutine(LoadDropitem());
        
        isLoadedAsset = true;
    } 

    

    public void ReleaseAddressable()
    {
        Debug.Log("ASkillsHandle / AItemSpriteHandle / ADropItemsHandle / AUIHandle �� ��ȿ���� Ȯ�� " + ASkillsHandle.IsValid() + " / " + AItemSpriteHandle.IsValid() + " / " + ADropItemsHandle.IsValid() + " / " + AUIHandle.IsValid());

        if (ASkillsHandle.IsValid())
        {
            
            Addressables.Release(ASkillsHandle);
        }

        if (AItemSpriteHandle.IsValid())
        {
            
            Addressables.Release(AItemSpriteHandle);
        }

        if (ADropItemsHandle.IsValid())
        {            
            Addressables.Release(ADropItemsHandle);
        }

        if (AUIHandle.IsValid())
        {
            
            Addressables.Release(AUIHandle);
        }

        Debug.Log("ASkillsHandle / AItemSpriteHandle / ADropItemsHandle / AUIHandle �� ��ȿ���� Ȯ�� " + ASkillsHandle.IsValid() + " / " + AItemSpriteHandle.IsValid() + " / " + ADropItemsHandle.IsValid() + " / " + AUIHandle.IsValid()) ;
    }

    void Test()
    {
        testhandle = Addressables.LoadAssetAsync<GameObject>(AddressableLabel.UI.ToString());
        testhandle.Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            testUI = obj.Result; //Instantiate(obj.Result, Vector3.zero, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(testhandle.IsValid() + "�׽�Ʈ �ڵ��� ��ȿ����");
            Instantiate(testUI);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ReleaseAddressable();
        }
    }

    public void CheckAsync()
    {
        // AItemSpriteHandle�� ��ȿ�ϰ�, ����� null�� �ƴ��� Ȯ��
        if (AItemSpriteHandle.IsValid() && AItemSpriteHandle.Result != null)
        {
            foreach (IResourceLocation location in AItemSpriteHandle.Result)
            {
                Debug.Log("Location: " + location);
            }
        }
        else
        {
            Debug.LogError("AItemSpriteHandle is not valid or the result is null.");
        }
    }

    

    #region �ε彺������Ʈ
    private IEnumerator LoadSprite()
    {
        AItemSpriteHandle = Addressables.LoadResourceLocationsAsync(AddressableLabel.ItemSprite.ToString());
        yield return new WaitUntil(() => AItemSpriteHandle.Task.IsCompleted); // �񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.


        if (AItemSpriteHandle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (IResourceLocation location in AItemSpriteHandle.Result)
            {

                AsyncOperationHandle<GameObject> itemlog = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey);
                yield return new WaitUntil(() => itemlog.Task.IsCompleted); // ������ ���� �ε带 ��ٸ��ϴ�.

                if (itemlog.Status == AsyncOperationStatus.Succeeded)
                {
                        //Instantiate(itemlog.Result);
                        ItemLog log = itemlog.Result.GetComponent<ItemLog>();
                    //log.Init();
                        Debug.Log("������ �α� ����");
                        ItemSprite sprite = log.sprite;
                        AitemSprites.Add(sprite);
                }
                
            }

            
        }
        else
        {
            Debug.LogError("�ε�� ��ġ �������� ����: " + AItemSpriteHandle.Status);
        }

        
    }   
    #endregion

    #region �ε� ��ų
    private IEnumerator LoadSkill()
    {
        ASkillsHandle = Addressables.LoadResourceLocationsAsync(Skilllabel);
        yield return new WaitUntil(() => ASkillsHandle.Task.IsCompleted); // �񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.


        if (ASkillsHandle.Status == AsyncOperationStatus.Succeeded)
        {

            foreach (IResourceLocation location in ASkillsHandle.Result)
            {
                Debug.Log("�ε�� ��ġ: " + location.PrimaryKey);

                AsyncOperationHandle<GameObject> skill = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey);
                yield return new WaitUntil(() => skill.Task.IsCompleted); // ������ ���� �ε带 ��ٸ��ϴ�.

                if (skill.Status == AsyncOperationStatus.Succeeded)
                {
                    if (skill.Result != null)
                    {
                        ASkills.Add(skill.Result);                                                                                         
                    }
                   
                }              
            }

        }
        else
        {
            Debug.LogError("�ε�� ��ġ �������� ����: " + ASkillsHandle.Status);
        }

        /*// ���� ��ü �ε� ����
        Addressables.LoadAssetAsync<GameObject>("Assets/Packege/NewSkill/Ground Hit.prefab").Completed +=
            (obj =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("���� �ε� ������Ʈ ����: " + obj.Status);
                    Instantiate(obj.Result);
                }
                else
                {
                    Debug.LogError("���� �ε� ������Ʈ ����: " + obj.Status);
                }
            });*/
        /* ���� �ɷ����� �ذ����� 
            ��巹����� �ҷ��ö� await, async�� �񵿱�� �θ��ٰ� ������Ʈ�� �ȳ����°� Ȯ����
            ���� ������ ���ļ� �ڷ�ƾ���� �ٽ� �õ��غ����� �̹����� ���� ��ü�ε常 ���ͼ� Ȯ���غ���
            �̻��ؼ� yield return new aa.Task; // ������ ���� �ε带 ��ٸ��ϴ�.
            �� �κп��� ��ٸ��°� ���� �ʴٴ°� ã�Ұ� 3�ʵ��� ��ٸ��� �غ��� �� �ε�Ǵ°� Ȯ���� 
            ����Ƽ �������� �ڷḦ ã�Ƽ� 
        yield return new WaitUntil(() => aa.Task.IsCompleted); // ������ ���� �ε带 ��ٸ��ϴ�.
        �̷��� ������ ����� ��ٸ��� ���� �Ǵ°� Ȯ����.
        �� �ڷ�ƾ���� �½�ũ�� ����Ϸ��� ���������� ����ؾ���.
        ��巹���� �ʹ� ���� �ð��� �Ҹ�����.
        
         */
    }



    #endregion

    #region UI �ε�

    private IEnumerator LoadUI()
    {
        AUIHandle= Addressables.LoadResourceLocationsAsync(UIlabel);
        yield return new WaitUntil(() => AUIHandle.Task.IsCompleted); // �񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.

        if (AUIHandle.Status == AsyncOperationStatus.Succeeded)
        {

            foreach (IResourceLocation location in AUIHandle.Result)
            {

                AsyncOperationHandle<GameObject> ui = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey);
                yield return new WaitUntil(() => ui.Task.IsCompleted); // ������ ���� �ε带 ��ٸ��ϴ�.

                if (ui.Status == AsyncOperationStatus.Succeeded)
                {

                    if (ui.Result.GetComponent<PlayerUI>() != null)
                    {
                        APlayerUI = ui.Result;
                    }
                    else if(ui.Result.GetComponent<InventoryUI>() != null)

                    {
                        AInventoryUI = ui.Result;
                    }
                   
                }
                
            }
        }
        else
        {
            Debug.LogError("�ε�� ��ġ �������� ����: " + AUIHandle.Status);
        }       
       
    }


    #endregion

    #region ��� ������
    private IEnumerator LoadDropitem()
    {
        ADropItemsHandle = Addressables.LoadAssetAsync<GameObject>(AddressableLabel.DropItem.ToString());
        yield return new WaitUntil(() => ADropItemsHandle.Task.IsCompleted); // �񵿱� �۾��� �Ϸ�� ������ ��ٸ��ϴ�.

        if (ADropItemsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ADropitem = ADropItemsHandle.Result;
        }
        else
        {
            Debug.LogError("�ε�� ��ġ �������� ����: " + ADropItemsHandle.Status);
        }

    }
    #endregion
    

  /*  async Task LoadItemSprite()
    {
        //.IsValid() ��ȿ�� �ڵ��� �ִ��� ����
        if (AItemSpriteHandle.IsValid())
        {
            Addressables.Release(AItemSpriteHandle);
        }

        AItemSpriteHandle = Addressables.LoadResourceLocationsAsync(AddressableLabel.ItemSprite.ToString(), typeof(Sprite));

        await AItemSpriteHandle.Task;

        if(AItemSpriteHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = AItemSpriteHandle.Result;
            List<Task<Sprite>> loadTask = new List<Task<Sprite>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<Sprite> loadhandle = Addressables.LoadAssetAsync<Sprite>(location);
                loadTask.Add(loadhandle.Task);
            }

            Sprite[] sprites = await Task.WhenAll(loadTask);
            AitemSprites = new ItemSprite[sprites.Length];

            for (int i = 0; i < AitemSprites.Length; i++)
            {
                int num = i;
                eEquipmentType type = SetItemType(num);
                int lank = SetLank(type, num);

                AitemSprites[i] = new ItemSprite(num, type, sprites[num], lank);
            }

        }
        else
        {
            Debug.Log("�ε� ���� " + AItemSpriteHandle.Status);
        }
    }*/

    /*void ReleaseItemSprites()
    {
        foreach (IResourceLocation location in AItemSpriteHandle.Result)
        {
            Addressables.Release(location);
        }

        //AItemSpriteHandle�� ��ȿ���� Ȯ���ϰ� ����
        if(AItemSpriteHandle.IsValid())
        {
            Addressables.Release(AitemSprites);
        }
    }*/

    /*async Task LoadDropItem()
    {
        ADropItemsHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.DropItem.ToString(), typeof(GameObject));


        await ADropItemsHandle.Task;

        if (ADropItemsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = ADropItemsHandle.Result;
            List<Task<GameObject>> loadTask = new List<Task<GameObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<GameObject> loadHandle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                loadTask.Add(loadHandle.Task);
            }
            GameObject[] gameObjects = await Task.WhenAll(loadTask);


            ADropitems = gameObjects;                        
        }
        else
        {
            Debug.Log("��� ������ �ҷ��������" + ADropItemsHandle.Status);
        }
    }*/
   

   /* async Task SkillLoad()
    {
        ASkillsHandle =
            Addressables.LoadResourceLocationsAsync(AddressableLabel.Skill.ToString());

        await ASkillsHandle.Task;

        if (ASkillsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = ASkillsHandle.Result;
            List<Task<GameObject>> loadTask = new List<Task<GameObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<GameObject> loadHandle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                loadTask.Add(loadHandle.Task);
            }

            GameObject[] skills = await Task.WhenAll(loadTask);            
            ASkills = new Skill[skills.Length];

            for (int i = 0; i < skills.Length; i++)
            {
                ASkills[i] = skills[i].GetComponent<Skill>();
                skills[i].SetActive(false);
            }
        }
        else
        {
            Debug.Log("�ε� ���� " + ASkillsHandle.Status);
        }
    }*/

   

   /* async Task LoadUI()
    {
        AUIHandle
            = Addressables.LoadResourceLocationsAsync(AddressableLabel.UI.ToString(), typeof(GameObject));

        await AUIHandle.Task;

        if(AUIHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> locations = AUIHandle.Result;
            List<Task<GameObject>> loadTask = new List<Task<GameObject>>();

            foreach (IResourceLocation location in locations)
            {
                AsyncOperationHandle<GameObject> loadHandle = Addressables.LoadAssetAsync<GameObject>(location);
                loadTask.Add(loadHandle.Task);
            }

            GameObject[] UIobj = await Task.WhenAll(loadTask);
            for (int i = 0; i < UIobj.Length; i++)
            {
                if (UIobj[i].GetComponent<PlayerUI>() != null)
                {
                    APlayerUI = UIobj[i].GetComponent<PlayerUI>();
                }else if(UIobj[i].GetComponent<InventoryUI>() != null)
                {
                    AInventoryUI = UIobj[i].GetComponent<InventoryUI>();
                }
            }
            AUI = UIobj;
        }
        else
        {
            Debug.Log("�ε� ���� " + AUIHandle.Status);
        }
    }*/


    int SetLank(eEquipmentType type, int num)
    {
        int getnum = 0;

        switch (type)
        {
            case eEquipmentType.Helmet:
                getnum = Array.FindIndex(Itemid.Helmetid,id => id ==  num);
                break;
            case eEquipmentType.ChestArmor:
                getnum = Array.FindIndex(Itemid.ChestArmorid, id => id == num);
                break;
            case eEquipmentType.LegArmor:
                getnum = Array.FindIndex(Itemid.LegArmorid, id => id == num);
                break;
            case eEquipmentType.Boots:
                getnum = Array.FindIndex(Itemid.Bootsid, id => id == num);
                break;
            case eEquipmentType.Weapon:
                getnum = Array.FindIndex(Itemid.Weaponid, id => id == num);
                break;
            default:
                break;
        }

        return getnum;
    }


  

    eEquipmentType SetItemType(int id)
    {
        if(id <= Itemid.Weaponid[Itemid.Weaponid.Length - 1 ] && id >= Itemid.Weaponid[0])
        {
            return eEquipmentType.Weapon;
        }
        else if (id <= Itemid.LegArmorid[Itemid.LegArmorid.Length -1] && id >= Itemid.LegArmorid[0])
        {
            return eEquipmentType.LegArmor;
        }
        else if (id <= Itemid.Helmetid[Itemid.Helmetid.Length -1] && id >= Itemid.Helmetid[0])
        {
            return eEquipmentType.Helmet;
        }
        else if (id <= Itemid.Bootsid[Itemid.Bootsid.Length -1] && id >= Itemid.Bootsid[0])
        {
            return eEquipmentType.Boots;
        }
        else if (id <= Itemid.ChestArmorid[Itemid.ChestArmorid.Length - 1] && id >= Itemid.ChestArmorid[0])
        {
            return eEquipmentType.ChestArmor;
        }

        //Debug.Log("�ش� ��ȣ�� ���ԵǾ����� �ʴ� ��ȣ�� ������!!!! ");
        return eEquipmentType.Helmet;
    }
}
