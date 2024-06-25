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
    public ItemSprite(int id, eEquipmentType type, Sprite spr, int Lank) { imgid = id; this.type = type; itemSpr = spr; this.Lank = Lank;  Debug.Log("아이템 스프라이트 생성됨!!"); }

    public int imgid;
    public int Lank;
    public eEquipmentType type;
    public UnityEngine.Sprite itemSpr;
}

public class SettingManager : Singleton<SettingManager>
{

    public enum ItemId
    {
        //여기서 그냥 아이템번호를 1000번대로 옮겨도 될거같긴함.
        //for 문의 i = 1000으로 시작해서 해도 될 것 같음.
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
        Debug.Log("셋팅 매니저 불림 ");
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
        //Debug.Log("체크 1 : LoadUI");
        yield return StartCoroutine(LoadUI());
        //Debug.Log("체크 2 : LoadSkill");
        yield return StartCoroutine(LoadSkill());
        //Debug.Log("체크 3 : SpriteI");
        yield return StartCoroutine(LoadSprite());
        //Debug.Log("체크 4 : LoadDropItem");
        yield return StartCoroutine(LoadDropitem());
        
        isLoadedAsset = true;
    } 

    

    public void ReleaseAddressable()
    {
        Debug.Log("ASkillsHandle / AItemSpriteHandle / ADropItemsHandle / AUIHandle 이 유효한지 확인 " + ASkillsHandle.IsValid() + " / " + AItemSpriteHandle.IsValid() + " / " + ADropItemsHandle.IsValid() + " / " + AUIHandle.IsValid());

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

        Debug.Log("ASkillsHandle / AItemSpriteHandle / ADropItemsHandle / AUIHandle 이 유효한지 확인 " + ASkillsHandle.IsValid() + " / " + AItemSpriteHandle.IsValid() + " / " + ADropItemsHandle.IsValid() + " / " + AUIHandle.IsValid()) ;
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
            Debug.Log(testhandle.IsValid() + "테스트 핸들이 유효한지");
            Instantiate(testUI);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ReleaseAddressable();
        }
    }

    public void CheckAsync()
    {
        // AItemSpriteHandle이 유효하고, 결과가 null이 아닌지 확인
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

    

    #region 로드스프라이트
    private IEnumerator LoadSprite()
    {
        AItemSpriteHandle = Addressables.LoadResourceLocationsAsync(AddressableLabel.ItemSprite.ToString());
        yield return new WaitUntil(() => AItemSpriteHandle.Task.IsCompleted); // 비동기 작업이 완료될 때까지 기다립니다.


        if (AItemSpriteHandle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (IResourceLocation location in AItemSpriteHandle.Result)
            {

                AsyncOperationHandle<GameObject> itemlog = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey);
                yield return new WaitUntil(() => itemlog.Task.IsCompleted); // 각각의 에셋 로드를 기다립니다.

                if (itemlog.Status == AsyncOperationStatus.Succeeded)
                {
                        //Instantiate(itemlog.Result);
                        ItemLog log = itemlog.Result.GetComponent<ItemLog>();
                    //log.Init();
                        Debug.Log("아이템 로그 복사");
                        ItemSprite sprite = log.sprite;
                        AitemSprites.Add(sprite);
                }
                
            }

            
        }
        else
        {
            Debug.LogError("로드된 위치 가져오기 실패: " + AItemSpriteHandle.Status);
        }

        
    }   
    #endregion

    #region 로드 스킬
    private IEnumerator LoadSkill()
    {
        ASkillsHandle = Addressables.LoadResourceLocationsAsync(Skilllabel);
        yield return new WaitUntil(() => ASkillsHandle.Task.IsCompleted); // 비동기 작업이 완료될 때까지 기다립니다.


        if (ASkillsHandle.Status == AsyncOperationStatus.Succeeded)
        {

            foreach (IResourceLocation location in ASkillsHandle.Result)
            {
                Debug.Log("로드된 위치: " + location.PrimaryKey);

                AsyncOperationHandle<GameObject> skill = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey);
                yield return new WaitUntil(() => skill.Task.IsCompleted); // 각각의 에셋 로드를 기다립니다.

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
            Debug.LogError("로드된 위치 가져오기 실패: " + ASkillsHandle.Status);
        }

        /*// 단일 객체 로드 예시
        Addressables.LoadAssetAsync<GameObject>("Assets/Packege/NewSkill/Ground Hit.prefab").Completed +=
            (obj =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("단일 로드 오브젝트 성공: " + obj.Status);
                    Instantiate(obj.Result);
                }
                else
                {
                    Debug.LogError("단일 로드 오브젝트 실패: " + obj.Status);
                }
            });*/
        /* 오래 걸렸지만 해결했음 
            어드레서블로 불러올때 await, async등 비동기로 부르다가 오브젝트가 안나오는걸 확인후
            여러 과정을 걸쳐서 코루틴으로 다시 시도해봤지만 이번에는 단일 객체로드만 나와서 확인해보고
            이상해서 yield return new aa.Task; // 각각의 에셋 로드를 기다립니다.
            이 부분에서 기다리는거 같지 않다는걸 찾았고 3초동안 기다리고 해보니 잘 로드되는거 확인후 
            유니티 포럼에서 자료를 찾아서 
        yield return new WaitUntil(() => aa.Task.IsCompleted); // 각각의 에셋 로드를 기다립니다.
        이렇게 수정후 제대로 기다리고 생성 되는걸 확인함.
        즉 코루틴에서 태스크를 사용하려면 저런식으로 사용해야함.
        어드레서블에 너무 많은 시간을 소모했음.
        
         */
    }



    #endregion

    #region UI 로드

    private IEnumerator LoadUI()
    {
        AUIHandle= Addressables.LoadResourceLocationsAsync(UIlabel);
        yield return new WaitUntil(() => AUIHandle.Task.IsCompleted); // 비동기 작업이 완료될 때까지 기다립니다.

        if (AUIHandle.Status == AsyncOperationStatus.Succeeded)
        {

            foreach (IResourceLocation location in AUIHandle.Result)
            {

                AsyncOperationHandle<GameObject> ui = Addressables.LoadAssetAsync<GameObject>(location.PrimaryKey);
                yield return new WaitUntil(() => ui.Task.IsCompleted); // 각각의 에셋 로드를 기다립니다.

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
            Debug.LogError("로드된 위치 가져오기 실패: " + AUIHandle.Status);
        }       
       
    }


    #endregion

    #region 드랍 아이템
    private IEnumerator LoadDropitem()
    {
        ADropItemsHandle = Addressables.LoadAssetAsync<GameObject>(AddressableLabel.DropItem.ToString());
        yield return new WaitUntil(() => ADropItemsHandle.Task.IsCompleted); // 비동기 작업이 완료될 때까지 기다립니다.

        if (ADropItemsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            ADropitem = ADropItemsHandle.Result;
        }
        else
        {
            Debug.LogError("로드된 위치 가져오기 실패: " + ADropItemsHandle.Status);
        }

    }
    #endregion
    

  /*  async Task LoadItemSprite()
    {
        //.IsValid() 유효한 핸들이 있는지 참조
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
            Debug.Log("로드 실패 " + AItemSpriteHandle.Status);
        }
    }*/

    /*void ReleaseItemSprites()
    {
        foreach (IResourceLocation location in AItemSpriteHandle.Result)
        {
            Addressables.Release(location);
        }

        //AItemSpriteHandle이 유효한지 확인하고 해제
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
            Debug.Log("드랍 아이템 불러오기실패" + ADropItemsHandle.Status);
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
            Debug.Log("로드 실패 " + ASkillsHandle.Status);
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
            Debug.Log("로드 실패 " + AUIHandle.Status);
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

        //Debug.Log("해당 번호에 포함되어있지 않는 번호의 아이템!!!! ");
        return eEquipmentType.Helmet;
    }
}
