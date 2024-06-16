using Save;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Items
{
    public enum eEquipmentType
    {
        Helmet = 0,
        ChestArmor ,
        LegArmor,
        Boots,
        Weapon
    }


    

    [Serializable]
    public class Inventory
    {
        public int itemRange = 16;

        public Dictionary<int, Item> ItemCheckList = new Dictionary<int, Item>();
        public ItemData[] itemList;
        public Inventory()
        {
            itemList = new ItemData[itemRange];
            for (int i = 0; i < itemRange; i++)
            {
                itemList[i] = new ItemData();
                ItemCheckList.Add(i, null);
            }

        }

        public ItemData SettingItem(int id)
        {
            ItemData item = null;
            if (itemList[id] != null)
            {
                /*for (int i = 0; i < itemList.Length; i++)
                {
                    ItemCheckList[i].id = itemList[i].id;
                    ItemCheckList[i].type = itemList[i].type;
                    ItemSprite[] result =
                        SettingManager.Instance.itemSprites.Where
                        (item => item.imgid == ItemCheckList[i].id).ToArray();
                    ItemCheckList[i].img = result[result.Length - 1].itemSpr;
                    //ItemCheckList[i].img = SettingManager.Instance.itemSprites[itemList[i].id].itemSpr;                    

                }*/

                if (itemList[id] != null)
                {
                    item = itemList[id];
                    return item;
                }
            }
            

            return null;
        }

        public ItemData GetItem(int id)
        {

            Item item = null;
            if (itemList[id] != null)
            {

            }
            if (ItemCheckList.TryGetValue(id, out item))
            {
                ItemCheckList[id] = null;
                item = ItemCheckList[id];
                return item;
            }

            return null;
        }

        public void SortItmes()
        {
            //�������� ������� Ȯ���� ������� ���� �ִ� ������ ������ �ű��

            for (int i = 0; i < ItemCheckList.Count - 1; i++)
            {
                if (ItemCheckList.ContainsKey(i))
                {
                    if (ItemCheckList[i] == null)
                    {
                        ItemCheckList[i] = ItemCheckList[i + 1];
                        ItemCheckList[i + 1] = null;
                    }
                }
                else
                {
                    Debug.Log("�������� ������ ���� ���� �ʾҽ��ϴ�.");
                }
            }
        }

        //�������� ��á����
        public bool IsFullItem()
        {
            int ItemCount = 0;

            foreach (Item item in ItemCheckList.Values)
            {
                if (item != null)
                {
                    ItemCount++;
                }
            }

            Debug.Log(" itemRange :" + itemRange + ",ItemCount : " + ItemCount);
            if (ItemCount == itemRange)
            {
                Debug.Log("������ �����ִ� ����");
                return true;
            }


            return false;
        }

        public int AddItem(ItemData item)
        {
            int id = 0;

            if (IsFullItem())
            {
                //������ �����ִ� ���� ���̻� ������
                Debug.Log("������ ��������");
            }
            else
            {
                //������ ������ ��������� �װ����� �־��ش�.
                for (int i = 0; i < ItemCheckList.Count; i++)
                {
                    if (ItemCheckList[i] == null)
                    {
                        itemList[i] = item;
                        id = i;
                        return id;
                    }
                }
            }

            return -1;

        }

        public void ChangeItem(ItemData item)
        {
            //������ ������ ��������� �װ����� �־��ش�.
            for (int i = 0; i < ItemCheckList.Count; i++)
            {
                if (itemList[i] == null)
                {
                    itemList[i] = item;
                    return;
                }
            }
        }

        public ItemData DropItem(int itemNumber)
        {
            if (ItemData.ContainsKey(itemNumber))
            {
                ItemData getItem = ItemCheckList[itemNumber];
                ItemCheckList[itemNumber] = null;
                return getItem;
            }

            return null;
        }

        public Item BeforeSettingItem(int id )
        {
            Item item = null;
            if (itemList[id] != null)
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    ItemCheckList[i].id = itemList[i].id;
                    ItemCheckList[i].type = itemList[i].type;
                    ItemSprite[] result = 
                        SettingManager.Instance.itemSprites.Where
                        (item => item.imgid == ItemCheckList[i].id).ToArray();
                    ItemCheckList[i].img = result[result.Length - 1].itemSpr;
                    //ItemCheckList[i].img = SettingManager.Instance.itemSprites[itemList[i].id].itemSpr;                    
                }
            }
            if (ItemCheckList.TryGetValue(id, out item))
            {
                item = ItemCheckList[id];
                return item;
            }

            return null;
        }

      
    }
    [Serializable]
    public class EquipmentInventory
    {
        public  Dictionary<eEquipmentType, Item> Equipment = new Dictionary<eEquipmentType, Item>();

        public EquipmentInventory()
        {
            int enumCount = Enum.GetValues(typeof(eEquipmentType)).Length;
            for (int i = 0; i < enumCount; i++)
            {
                
                  Equipment.Add((eEquipmentType)i, null);
                
            }
        }

        #region ���� �ڵ�
        /* public void SettingEquip()
         {
             int enumCount = Enum.GetValues(typeof(eEquipmentType)).Length;
             for (int i = 0; i < enumCount; i++)
             {
                 if (Equipment[(eEquipmentType)i] == null)
                 {
                     Equipment.Add((eEquipmentType)i, null);
                 }
             }
         }*/
        #endregion

        public Item UnequipItem(eEquipmentType Equip)
        {
            Item getItem = null;

            
            //�������� �������� �����Ƿ� ��� ���� + ������ �ֱ�
            getItem = Equipment[Equip];
            Equipment[Equip] = null;
            

            return getItem;
        }

        //������ ��ü �ϼ��� ������ �̷��� �ϱ�
        public Item EquipItem(eEquipmentType Equip, Item item)
        {
            Item getItem = null;

            if (Equipment.ContainsKey(Equip))
            {
                getItem = Equipment[Equip];

                Equipment[Equip] = item;
            }
            else
            {
                Equipment.Add(Equip, item);
            }

            return getItem;

        }

    }

    #region ���� Ŭ���� �κ��丮
    /*public static class Inventory
    {
        public static bool IsDrop;

        public static int itemRange;

        public static Dictionary<int, Item> ItemCheckList = new Dictionary<int, Item>();

        public static Item GetItem(int id)
        {
            if (ItemCheckList.ContainsKey(id))
            {
                return ItemCheckList[id];
            }

            return null;
        }

        public static void SortItmes()
        {
            //�������� ������� Ȯ���� ������� ���� �ִ� ������ ������ �ű��

            for (int i = 0; i < ItemCheckList.Count - 1; i++)
            {
                if(ItemCheckList.ContainsKey(i))
                {
                    if (ItemCheckList[i] == null)
                    {
                        ItemCheckList[i] = ItemCheckList[i + 1];                        
                        ItemCheckList[i + 1] = null;
                    }
                }
                else
                {
                    Debug.Log("�������� ������ ���� ���� �ʾҽ��ϴ�.");
                }
            }
        }

        //�������� ��á����
        public static bool IsFullItem()
        {

            int ItemCount = 0;

            foreach (Item item in ItemCheckList.Values)
            {
                if (item == null)
                {
                    ItemCount++;
                }
            }

            if (ItemCount == itemRange)
            {
                Debug.Log("������ �����ִ� ����");
                return true;
            }


            return false;
        }

        public static void AddItem(Item item)
        {
            if(IsFullItem())
            {
                //������ �����ִ� ���� ���̻� ������
                Debug.Log("������ ��������");
                return;
            }
            else
            {
                //������ ������ ��������� �װ����� �־��ش�.
                for (int i = 0; i < ItemCheckList.Count; i++)
                {
                    if( ItemCheckList[i] == null)
                    {
                        ItemCheckList[i] = item;
                        return;
                    }
                }                
            }

        }

        public static void ChangeItem(Item item)
        {
            //������ ������ ��������� �װ����� �־��ش�.
            for (int i = 0; i < ItemCheckList.Count; i++)
            {
                if (ItemCheckList[i] == null)
                {
                    ItemCheckList[i] = item;
                    return;
                }
            }
            

        }

        public static Item DropItem(int itemNumber)
        {
            if(ItemCheckList.ContainsKey(itemNumber))
            {
                Item getItem = ItemCheckList[itemNumber];
                ItemCheckList[itemNumber] = null;
                return getItem;
            }

            return null;
        }
    }*/

    /*public static class EquipmentInventory
    {
        public static Dictionary<eEquipmentType, Item> Equipment = new Dictionary<eEquipmentType, Item>();

        public static void SettingEquip()
        {
            int enumCount = Enum.GetValues(typeof(eEquipmentType)).Length;
            for (int i = 0; i < enumCount; i++)
            {
                if (Equipment[(eEquipmentType)i] == null)
                {
                    Equipment.Add((eEquipmentType)i, null);
                }
            }
        }

        public static Item UnequipItem(eEquipmentType Equip)
        {
            Item getItem = null;

            if (Inventory.IsFullItem())
            {
                getItem = null;
            }
            else
            {

                //�������� �������� �����Ƿ� ��� ���� + ������ �ֱ�
                getItem = Equipment[Equip];
                Equipment[Equip] = null;
            }

            return getItem;
        }

        //������ ��ü �ϼ��� ������ �̷��� �ϱ�
        public static Item EquipItem(eEquipmentType Equip, Item item)
        {
            Item getItem = null;

            if (Equipment.ContainsKey(Equip))
            {
                getItem = Equipment[Equip];

                Equipment[Equip] = item;
            }
            else
            {
                Equipment.Add(Equip, item);
            }




            return getItem;

        }

    }
*/
    #endregion

    

   

}


