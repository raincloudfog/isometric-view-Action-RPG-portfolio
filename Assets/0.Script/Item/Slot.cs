using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Items;
using Item = Items.Item;
using UnityEngine.UI;
using UnityEditor.UIElements;


public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    //장비 슬롯 인지 확인
    public bool isEquip;

    public int id;
    private Item item = null;
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

    public void AddItem(Item getitem)
    {
        item = getitem;
        itemimage.sprite = item.img;

        itemimage.enabled = item == null ? false : true;
    }


    //이거는 UI말고 게임 오브젝트에만 적용 된다고 함.
    public void OnMouseEnter()
    {
        Debug.Log("Mouse entered the object.");

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ClickPointCoroutine == null)
        {
            ClickPointCoroutine = StartCoroutine(ClickPoint());
        }

        Debug.Log( ClickCount++);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void SetItem(Item item)
    {
        this.item = item;
        itemimage.sprite = item != null ? item.img : null;
        itemimage.enabled = item == null ? false : true;
    }

    public Item GetItem()
    {
        
        return item; 
    }

    void ChangeItem()
    {
        if(item == null)
        {
            return;
        }

        /*Item EquipItem = ItemManager.Instance._Equipment.EquipItem(item.type, item);
        ItemManager.Instance._inventory.ChangeItem(EquipItem);
        item = EquipItem;*/


        ItemManager.Instance.inventoryUI.EquipItem(id, ref item);


        itemimage.sprite = item != null ? item.img : null;
        itemimage.enabled = item == null ? false : true;
    }

    void DropItem()
    {
        ItemManager.Instance.DropItem(this.id);
        item = null;
        itemimage.sprite = null;
        itemimage.enabled = item == null ? false : true;
    }

    IEnumerator ClickPoint()
    {
        float timer = 0;
        float clickTiemr = 1f;

        while(timer < clickTiemr)
        {
            yield return null;

            Debug.Log("코루틴 반복문 실행");
            timer += Time.deltaTime;
            if(ClickCount >= 2)
            {
                if (ItemManager.Instance.inventoryUI.IsDrop)
                {
                    Debug.Log("아이템 버리기 코루틴");
                    DropItem();
                }
                else
                {
                    Debug.Log("아이템 장착 코루틴");
                    ChangeItem();
                }
                break;
            } 
        }
        Debug.Log("코루틴 끝");
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
