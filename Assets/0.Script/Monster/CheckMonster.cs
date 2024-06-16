using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy = Monster.Monster;


public class CheckMonster : MonoBehaviour
{
    public Enemy[] monster;
    public int monsterNumber;    

    public void CheckMosnter()
    {
        monster = GetComponentsInChildren<Enemy>();
        monsterNumber = monster.Length;
    }
}
