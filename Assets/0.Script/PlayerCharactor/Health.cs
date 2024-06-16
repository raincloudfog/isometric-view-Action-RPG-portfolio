using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private int hp;

    private int maxHp;

    public bool isDeath;

    public int HP
    {
        get
        {
            return hp;
        }
    }

    public void Init(int hp)
    {
        maxHp = hp;
        this.hp = hp;
    }

    public void Hit(int damage)
    {
        Debug.Log("입은 데미지 :" + damage);
        hp -= damage;
        if(hp < 0) isDeath = true;
    }

    public void Heal(int hp)
    {
        this.hp += hp;
    }
}
