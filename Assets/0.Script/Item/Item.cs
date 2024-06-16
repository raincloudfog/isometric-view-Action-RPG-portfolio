using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Items
{
    [Serializable]
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptObject/ItemData", order = int.MaxValue)]
    public class Item : ScriptableObject
    {
        public int id;
        public eEquipmentType type;
        public int imgid;
        public Sprite img;
    }
}