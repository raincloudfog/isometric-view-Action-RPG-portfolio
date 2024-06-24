using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Item = Items.Item;
using UnityEngine.UI;
using Save;
using System.Linq;



public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    //장비 슬롯 인지 확인
    public bool isEquip;

    //해당 슬롯 번호
    public int id;
    [SerializeField]
    private ItemData item = null;
    public Image itemimage;

    private int ClickCount = 0;
    Coroutine ClickPointCoroutine = null;    

    public void OnEnable()
    {
        if(item == null)
        {
            itemimage.enabled = false;
        }
    }

    public void AddItem(ItemData getitem)
    {
        this.item = getitem;
        var result = SettingManager.Instance.AitemSprites.Where
                        (item => item.imgid == getitem.id).ToArray();

        itemimage.sprite = result[0].itemSpr;
        itemimage.enabled = item.isNull == true ? false : true;
    }


    //이거는 UI말고 게임 오브젝트에만 적용 된다고 함.
    public void OnMouseEnter()
    {


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ClickPointCoroutine == null)
        {
            ClickPointCoroutine = StartCoroutine(ClickPoint());
        }

         ClickCount++;
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void SetItem(ItemData getitem)
    {
        this.item = getitem;
        var result = SettingManager.Instance.AitemSprites.Where
                        (item => item.imgid == getitem.id).ToArray();

        itemimage.sprite = result[0].itemSpr;
        itemimage.enabled = item.isNull == true ? false : true;
    }

    public ItemData GetItem()
    {
        
        return item; 
    }

    //아이템 교환
    void ChangeItem()
    {
        if(item == null)
        {
            return;
        }        

        ItemManager.Instance.inventoryUI.EquipItem(id, ref item);

        if(item.isNull)
        {
            itemimage.sprite = null;    
            itemimage.enabled = false;
            return;
        }

        var result = SettingManager.Instance.AitemSprites.Where
                        (item => item.imgid == this.item.id).ToArray();

        itemimage.sprite = result[0].itemSpr;
        itemimage.enabled = item == null ? false : true;
    }

    void UnEquip()
    {
        if(!isEquip ||item == null)
        {
            return;
        }

        ItemData NewData = new ItemData();
        NewData.type = item.type;

        ItemManager.Instance.AddItem(item);
        ItemManager.Instance.inventoryUI.UnEquipItem(item);
        item = NewData;
        if (item.isNull)
        {
            itemimage.sprite = null;
            itemimage.enabled = false;
        }
       
    }

    void DropItem()
    {
        ItemManager.Instance.DropItem(this.id);
        item = new ItemData();
        itemimage.sprite = null;
        itemimage.enabled = false ;
        SaveManager.Instance.Save();
    }

    IEnumerator ClickPoint()
    {
        float timer = 0;
        float clickTiemr = 1f;



        while(timer < clickTiemr)
        {
            yield return null;

            if (item.isNull)
            {
                break;
            }


            // Debug.Log("코루틴 반복문 실행");
            timer += Time.deltaTime;
            if(ClickCount >= 2)
            {
                if (isEquip)
                {
                    UnEquip();
                    break;
                }

                if (ItemManager.Instance.inventoryUI.IsDrop)
                {
                    //Debug.Log("아이템 버리기 코루틴");
                    DropItem();
                }                
                else
                {
                   // Debug.Log("아이템 장착 코루틴");
                    ChangeItem();
                }
                break;
            } 
        }
        //Debug.Log("코루틴 끝");
        ClickCount = 0;
        ClickPointCoroutine = null;

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
