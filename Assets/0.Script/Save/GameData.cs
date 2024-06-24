using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class GameData
{
    //현재 UI가 열려있는지
    public static bool isOpenUI = false;
    //현재 플레이어의 데이터가 몇번째의 데이터인지 세이브1 , 세이브2 , 세이브3 등
    public static int playerNumber = 0;      

    public static bool isPickingItem  = false;

    public static bool isEnterthebossroom = false;         
}
