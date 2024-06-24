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
    //��� ���� ���� Ȯ��
    public bool isEquip;

    //�ش� ���� ��ȣ
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


    //�̰Ŵ� UI���� ���� ������Ʈ���� ���� �ȴٰ� ��.
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

    //������ ��ȯ
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


            // Debug.Log("�ڷ�ƾ �ݺ��� ����");
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
                    //Debug.Log("������ ������ �ڷ�ƾ");
                    DropItem();
                }                
                else
                {
                   // Debug.Log("������ ���� �ڷ�ƾ");
                    ChangeItem();
                }
                break;
            } 
        }
        //Debug.Log("�ڷ�ƾ ��");
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
