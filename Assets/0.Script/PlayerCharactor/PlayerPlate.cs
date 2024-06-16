using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerPlate : MonoBehaviour
{
    public enum ePlateName
    {
        Helmet = 0,
        ChestArmor,
        LegArmor,
        Boots
    }

    public GameObject[] plate = new GameObject[4];

    public void EquipPlate(ePlateName plateNum, bool isEquip)
    {
        plate[(int)plateNum].SetActive(true);
    }
}
