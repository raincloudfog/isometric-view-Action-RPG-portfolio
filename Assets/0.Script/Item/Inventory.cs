using Save;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

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

        public ItemData[] itemList;
        public Inventory()
        {
            itemList = new ItemData[itemRange];
            for (int i = 0; i < itemRange; i++)
            {
                itemList[i] = new ItemData();
            }
        }

        //�ε�� ������ ����
        public ItemData[] SettingItem()
        {

            if( itemList == null )
            {
                Debug.Log("������ ����Ʈ�� ������ �ٽ� ������ ����");
                itemList= new ItemData[itemRange];
            }

            return itemList;
        }

        //10001
        //������ �����ϱ� (��� ��ü��)
        public ItemData GetItem(int id)
        {
            ItemData item = null;
            if (itemList[id] != null)
            {
                item = itemList[id];
                itemList[id] = null;
                return item;
            }

            return null;
        }
     

        //10001
        //�������� ��á����
        public bool IsFullItem()
        {
            int ItemCount = 0;

            foreach (ItemData item in itemList)
            {
                //�������� Null�� �ƴϹǷ� 
                if (item != null &&item.isNull == false)
                {
                    ItemCount++;
                }
            }

            if (ItemCount == itemRange)
            {
                return true;
            }


            return false;
        }

        public int AddItem(ItemData item)
        {
            int id = 0;

            if (IsFullItem())
            {
                return -1;
            }
            else
            {

                for (int i = 0; i < itemList.Length; i++)
                {

                    if (itemList[i] == null  || itemList[i].isNull == true)
                    {
                        itemList[i] = item;
                        id = i;
                        return id;
                    }
                }
            }

            return -1;

        }

        public void AddItem(int id , ItemData item)
        {
            itemList[id] = item;
        }

        // 10001 : �ٽ� ������ �Ǵ� �ڵ�
        public ItemData DropItem(int itemNumber)
        {

            if (itemList[itemNumber] != null)
            {
                ItemData getitem = itemList[itemNumber];
                itemList[itemNumber] = new ItemData();
                return getitem;
            }

                /* if (ItemData.ContainsKey(itemNumber))
            {
                
            {
                ItemData getItem = ItemCheckList[itemNumber];
                ItemCheckList[itemNumber] = null;
                return getItem;
            }*/

            return null;
        }

        

      
    }
    [Serializable]
    public class EquipmentInventory
    {
        public ItemData[] itemList;
        public EquipmentInventory()
        {
            int enumCount = Enum.GetValues(typeof(eEquipmentType)).Length;
            itemList = new ItemData[enumCount];
            for (int i = 0; i < enumCount; i++)
            {
                itemList[i] = new ItemData();
                itemList[i].type = (eEquipmentType)i;
                
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

        public ItemData[] SettingItem()
        {
            if (itemList == null)
            {
                Debug.Log("������ ����Ʈ�� ������ �ٽ� ������ ����");
                itemList = new ItemData[5];
            }
            return itemList;
        }

        public void UnEquipItem(eEquipmentType Equip)
        {
            int index = Array.FindIndex(itemList, item => item != null && item.type == Equip);

            //�ش� �ε����� �ִ°��
            if(index >= 0)
            {
                itemList[index] = new ItemData();
                itemList[index].type = Equip;
            }
            else
            {
                Debug.LogError("��� ������ ����Ʈ�� �ش� �ε��� ��ȣ�� �����ϴ�.");
            }
        }

        //������ ��ü �ϼ��� ������ �̷��� �ϱ�
        /// <summary>
        /// �Ű����� �������� ���� ��� �־���� �����ϰ��ִ� �������� �Ѱ���.
        /// </summary>
        /// <param name="Equip"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ItemData EquipItem(eEquipmentType Equip, ItemData item)
        {
            ItemData getitem = new ItemData();

            // itemList �迭 ������ item.type�� ���� Ÿ���� ù ��° �������� ã��
            int index = Array.FindIndex(itemList, itemid => itemid != null && itemid.type == item.type);

            if (index == -1)
            {
                Debug.Log(" itemList���� �ش� Ÿ���� �������� ã�� ���߽��ϴ�.");
                return getitem;
            }

            // itemList���� ���� �������� ������
            getitem = itemList[index];

            // itemList���� ���� �������� ���ο� ���������� ��ü
            itemList[index] = item;

            /*Debug.Log("���� �ش� ���ĭ�� �������� ���� ��� Ÿ�� " + getitem.type);
            Debug.Log("�κ��丮�� ������ ������ ���� " + getitem.type + " /" + getitem.id);
            Debug.Log(itemList.Length + " �ε��� ����, �ش� �ε��� ��ȣ: " + index);
            Debug.Log("��������� ���� " + itemList[index].type + " /" + itemList[index].id);*/

            return getitem;
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


