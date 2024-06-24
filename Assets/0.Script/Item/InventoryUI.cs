using Items;
using Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour , IUserInterface
{
    public Button DropButton;

    public GameObject slotSpace;
    public Slot[] slots;

    public bool IsDrop;

    [Header("Equip")]
    public GameObject equipSpace;
    public Dictionary<Items.eEquipmentType, Slot> equipSlot = new Dictionary<eEquipmentType, Slot>();

    public void Init()
    {
        if(slotSpace != null)
        {
            slots = slotSpace.GetComponentsInChildren<Slot>();
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].id = i;
            }
        }
        if(equipSpace != null)
        {
            Slot[] equipslots = equipSpace.GetComponentsInChildren<Slot>();
           // Debug.Log(equipslots.Length + " equipslots.Length");

            if(equipslots.Length > 0)
            {
                for (int i = 0; i < equipslots.Length; i++)
                {
                    eEquipmentType type = (eEquipmentType)i;
                    equipSlot.Add(type, equipslots[i]);
                    equipSlot[type].id = i;
                }
            }
        }

       // Debug.Log(equipSlot.Count + " equipSlot.Count");

    }

    //������ ���� , �ε������� ������
    public void EquipItem(ItemData item)
    {
         equipSlot[item.type].SetItem(item);
        PlayManager.Instance.player.EquipChangeStat(item.type,item.Lank);
        PlayManager.Instance.player.HealthInit();

    }


    //������ ���� �κ��丮�� �ִ� ������ ����
    public void EquipItem(int slotid, ref ItemData item)
    {
        ItemData getitem = null;

        if(ItemManager.Instance._inventory.IsFullItem())
        {
            //Debug.Log("������ ���� �Ұ� (�κ��丮 ��������)");
        }
        else
        {
            ItemData setitem = ItemManager.Instance._inventory.GetItem(slotid);
            
            if(ItemManager.Instance._Equipment == null)
            {
                Debug.Log("��� ������ϴ�.");
            }
            getitem = ItemManager.Instance._Equipment.EquipItem(item.type, setitem);
            equipSlot[item.type].SetItem(item);

            if (PlayManager.Instance.player)
            {
                PlayManager.Instance.player.UnEquipItem(getitem.type, getitem.Lank);
                PlayManager.Instance.player.EquipChangeStat(item.type, item.Lank);
            }

            item = getitem;            
             ItemManager.Instance.AddItem(slotid, item);
            SaveManager.Instance.Save();
        }
    }

    //������ ����
    public void UnEquipItem(ItemData item)
    {
        if (ItemManager.Instance._inventory.IsFullItem())
        {
            //Debug.Log("������ ���� �Ұ� (�κ��丮 ��������)");
        }
        else
        {
            if (PlayManager.Instance.player)
            {
                PlayManager.Instance.player.UnEquipItem(item.type, item.Lank);

            }
            ItemManager.Instance._Equipment.UnEquipItem(item.type);
             SaveManager.Instance.Save();

        }
    }

    /// <summary>
    /// ������ ������ ���
    /// </summary>
    public void DropItem()
    {
        if (IsDrop)
        {
            IsDrop = false;
            var dropButtonColor = DropButton.image;
            dropButtonColor.color = Color.white;
        }
        else
        {
            IsDrop = true;
            var dropButtonColor = DropButton.image;
            dropButtonColor.color = Color.gray;
        }
    }    

    public void DropItem(int id)
    {
        
    }

    public void AddItem(int id, ItemData item)
    {
        //Debug.Log("id : " + id + "item : " + item.id);
       
        //Inventory.AddItem(item);
        slots[id].AddItem(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        DropButton.onClick.AddListener(DropItem);
    }

    
}
