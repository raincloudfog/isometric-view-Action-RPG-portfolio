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

        //로드시 아이템 전달
        public ItemData[] SettingItem()
        {

            if( itemList == null )
            {
                Debug.Log("아이템 리스트가 없으니 다시 생성후 전달");
                itemList= new ItemData[itemRange];
            }

            return itemList;
        }

        //10001
        //아이템 전달하기 (장비 교체용)
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
        //아이템이 꽉찼는지
        public bool IsFullItem()
        {
            int ItemCount = 0;

            foreach (ItemData item in itemList)
            {
                //아이템이 Null이 아니므로 
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

        // 10001 : 다시 만들어야 되는 코드
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
        #region 예전 코드
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
                Debug.Log("아이템 리스트가 없으니 다시 생성후 전달");
                itemList = new ItemData[5];
            }
            return itemList;
        }

        public void UnEquipItem(eEquipmentType Equip)
        {
            int index = Array.FindIndex(itemList, item => item != null && item.type == Equip);

            //해당 인덱스가 있는경우
            if(index >= 0)
            {
                itemList[index] = new ItemData();
                itemList[index].type = Equip;
            }
            else
            {
                Debug.LogError("장비 아이템 리스트에 해당 인덱스 번호가 없습니다.");
            }
        }

        //아이템 교체 일수도 있으니 이렇게 하기
        /// <summary>
        /// 매개변수 아이템을 현재 장비에 넣어놓고 장착하고있던 아이템을 넘겨줌.
        /// </summary>
        /// <param name="Equip"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public ItemData EquipItem(eEquipmentType Equip, ItemData item)
        {
            ItemData getitem = new ItemData();

            // itemList 배열 내에서 item.type과 같은 타입의 첫 번째 아이템을 찾음
            int index = Array.FindIndex(itemList, itemid => itemid != null && itemid.type == item.type);

            if (index == -1)
            {
                Debug.Log(" itemList에서 해당 타입의 아이템을 찾지 못했습니다.");
                return getitem;
            }

            // itemList에서 기존 아이템을 가져옴
            getitem = itemList[index];

            // itemList에서 기존 아이템을 새로운 아이템으로 교체
            itemList[index] = item;

            /*Debug.Log("현재 해당 장비칸에 아이템이 있을 경우 타입 " + getitem.type);
            Debug.Log("인벤토리에 전달할 아이템 정보 " + getitem.type + " /" + getitem.id);
            Debug.Log(itemList.Length + " 인덱스 갯수, 해당 인덱스 번호: " + index);
            Debug.Log("현재아이템 정보 " + itemList[index].type + " /" + itemList[index].id);*/

            return getitem;
        }

    }

    #region 정적 클래스 인벤토리
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
            //아이템이 비었는지 확인후 비었으면 전에 있던 아이템 앞으로 옮기기

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
                    Debug.Log("아이템의 공간이 생성 되지 않았습니다.");
                }
            }
        }

        //아이템이 꽉찼는지
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
                Debug.Log("아이템 꽉차있는 상태");
                return true;
            }


            return false;
        }

        public static void AddItem(Item item)
        {
            if(IsFullItem())
            {
                //아이템 곽차있는 상태 더이상 못받음
                Debug.Log("아이템 꽉차있음");
                return;
            }
            else
            {
                //아이템 공간이 비어있으니 그곳에다 넣어준다.
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
            //아이템 공간이 비어있으니 그곳에다 넣어준다.
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

                //아이템이 꽉차있지 않으므로 장비 해제 + 아이템 주기
                getItem = Equipment[Equip];
                Equipment[Equip] = null;
            }

            return getItem;
        }

        //아이템 교체 일수도 있으니 이렇게 하기
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


