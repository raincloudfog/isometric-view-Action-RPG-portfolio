using Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ItemLog : MonoBehaviour
{

    public string checkstr;
    public ItemSprite sprite;

    public void Start()
    {
        //Init();
    }

    public void Init()
    {
        /*string lank = Regex.Replace(name, "[^0-9]", "");
        int num;
        
        if (int.TryParse(lank, out num))
        {
            Debug.Log("정수로 변환된 값: " + num);
            // 이곳에서 정수로 변환된 lank 변수를 사용할 수 있습니다.
            sprite.Lank = num;
            sprite.imgid = num -1 ;
            sprite.type =  SetItemType(num - 1);
            sprite.Lank = SetLank(sprite.type, sprite.imgid) + 1;

        }*/
    }

    int SetLank(eEquipmentType type, int num)
    {
        int getnum = 0;

        switch (type)
        {
            case eEquipmentType.Helmet:
                getnum = Array.FindIndex(Itemid.Helmetid, id => id == num);
                break;
            case eEquipmentType.ChestArmor:
                getnum = Array.FindIndex(Itemid.ChestArmorid, id => id == num);
                break;
            case eEquipmentType.LegArmor:
                getnum = Array.FindIndex(Itemid.LegArmorid, id => id == num);
                break;
            case eEquipmentType.Boots:
                getnum = Array.FindIndex(Itemid.Bootsid, id => id == num);
                break;
            case eEquipmentType.Weapon:
                getnum = Array.FindIndex(Itemid.Weaponid, id => id == num);
                break;
            default:
                break;
        }

        return getnum;
    }




    eEquipmentType SetItemType(int id)
    {
        if (id <= Itemid.Weaponid[Itemid.Weaponid.Length - 1] && id >= Itemid.Weaponid[0])
        {
            return eEquipmentType.Weapon;
        }
        else if (id <= Itemid.LegArmorid[Itemid.LegArmorid.Length - 1] && id >= Itemid.LegArmorid[0])
        {
            return eEquipmentType.LegArmor;
        }
        else if (id <= Itemid.Helmetid[Itemid.Helmetid.Length - 1] && id >= Itemid.Helmetid[0])
        {
            return eEquipmentType.Helmet;
        }
        else if (id <= Itemid.Bootsid[Itemid.Bootsid.Length - 1] && id >= Itemid.Bootsid[0])
        {
            return eEquipmentType.Boots;
        }
        else if (id <= Itemid.ChestArmorid[Itemid.ChestArmorid.Length - 1] && id >= Itemid.ChestArmorid[0])
        {
            return eEquipmentType.ChestArmor;
        }

        //Debug.Log("해당 번호에 포함되어있지 않는 번호의 아이템!!!! ");
        return eEquipmentType.Helmet;
    }

}
