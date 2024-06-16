using Items;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            if (slots != null || slots.Length == 0)
            {
                Debug.Log("슬롯이 부족함.");
            }
            else
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].id = i;
                }
            }
        }
        if(equipSpace != null)
        {
            Slot[] equipslots = equipSpace.GetComponentsInChildren<Slot>();
            Debug.Log(equipslots.Length + " equipslots.Length");

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

        Debug.Log(equipSlot.Count + " equipSlot.Count");

    }

    public void EquipItem(Item item)
    {
        equipSlot[item.type].SetItem(item);
    }

    public void EquipItem(int slotid, ref Item item)
    {
        Item getitem = null;

        if(ItemManager.Instance._inventory.IsFullItem())
        {
            Debug.Log("아이템 해제 불가 (인벤토리 꽉차있음)");
        }
        else
        {
            getitem = equipSlot[item.type].GetItem();
            Item setitem = ItemManager.Instance._inventory.GetItem(slotid);
            ItemManager.Instance._Equipment.EquipItem(item.type, setitem);
            equipSlot[item.type].SetItem(item);
            item = getitem;

            //ItemManager.Instance._inventory.AddItem(getitem);
        }


        //return getitem;
    }

    /// <summary>
    /// 아이템 버리는 모드
    /// </summary>
    public void DropItem()
    {
         IsDrop = !IsDrop;

        if (IsDrop)
        {
            IsDrop = false;
            var dropButtonColor = DropButton.image;
            dropButtonColor.color = Color.gray;
        }
        else
        {
            IsDrop = true;
            var dropButtonColor = DropButton.image;
            dropButtonColor.color = Color.white;
        }
    }    

    public void DropItem(int id)
    {
        
    }

    public void AddItem(int id,Item item)
    {
        //Inventory.AddItem(item);
        slots[id].AddItem(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        DropButton.onClick.AddListener(DropItem);
    }

    
}
