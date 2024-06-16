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
    //��� ���� ���� Ȯ��
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


    //�̰Ŵ� UI���� ���� ������Ʈ���� ���� �ȴٰ� ��.
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

            Debug.Log("�ڷ�ƾ �ݺ��� ����");
            timer += Time.deltaTime;
            if(ClickCount >= 2)
            {
                if (ItemManager.Instance.inventoryUI.IsDrop)
                {
                    Debug.Log("������ ������ �ڷ�ƾ");
                    DropItem();
                }
                else
                {
                    Debug.Log("������ ���� �ڷ�ƾ");
                    ChangeItem();
                }
                break;
            } 
        }
        Debug.Log("�ڷ�ƾ ��");
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
