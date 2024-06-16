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
            //아이템이 비었는지 확인후 비었으면 전에 있던 아이템 앞으로 옮기기

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
                    Debug.Log("아이템의 공간이 생성 되지 않았습니다.");
                }
            }
        }

        //아이템이 꽉찼는지
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
                Debug.Log("아이템 꽉차있는 상태");
                return true;
            }


            return false;
        }

        public int AddItem(ItemData item)
        {
            int id = 0;

            if (IsFullItem())
            {
                //아이템 곽차있는 상태 더이상 못받음
                Debug.Log("아이템 꽉차있음");
            }
            else
            {
                //아이템 공간이 비어있으니 그곳에다 넣어준다.
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
            //아이템 공간이 비어있으니 그곳에다 넣어준다.
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

        public Item UnequipItem(eEquipmentType Equip)
        {
            Item getItem = null;

            
            //아이템이 꽉차있지 않으므로 장비 해제 + 아이템 주기
            getItem = Equipment[Equip];
            Equipment[Equip] = null;
            

            return getItem;
        }

        //아이템 교체 일수도 있으니 이렇게 하기
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


