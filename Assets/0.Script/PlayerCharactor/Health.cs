using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Health
{
    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int mana;
    [SerializeField]
    private int maxMana;

    public bool isDeath;

    public int HP
    {
        get
        {
            return hp;
        }
    }

    public int Mana
    {
        get
        {
            return mana;
        }
    }
    public int MaxHP
    {
        get
        {
            return maxHp;
        }
        set
        {
            //최소 체력보다 낮으면 최소체력으로 
            if(value < 50)
            {
                return;
            }
            
            
            maxHp = value;
            //만약 현재 체력이 최대 체력보다 높을시 현재체력은 최대체력으로
            if(hp >= maxHp)
            {
                hp = maxHp;
            }
        }
    }
    public int MaxMana
    {
        get
        {
            return maxMana;
        }
        set
        {
            //최소 체력보다 낮으면 최소체력으로 
            if (value < 50)
            {
                return;
            }


            maxMana = value;
            //만약 현재 체력이 최대 체력보다 높을시 현재체력은 최대체력으로
            if (mana >= maxMana)
            {
                mana = maxMana;
            }
        }
    }

    public void Init(int hp , int mana)
    {
        maxHp = hp;
        this.hp = hp;
        maxMana = mana;
        this.mana = mana;
    }

    public void ChangeMax(bool isHP, int MaxNum)
    {
        if(isHP)
        {
            maxHp = MaxNum;
        }
        else
        {
            maxMana = MaxNum;
        }
    }

    public bool UseSkill(int cost)
    {
        if(mana >= cost)
        {
            mana -= cost;
            return true;
        }

        return false;
    }

    public bool Hit(int damage)
    {
        bool Death = false;


        //Debug.Log("입은 데미지 :" + damage);
        hp -= damage;
        if (hp < 0) {
            hp = 0;
            isDeath = true;
            Death = true;
        }

        return Death;
    }

    public void Heal(int hp)
    {
        this.hp += hp;
        this.hp = this.hp >= maxHp ? maxHp : this.hp;
      
    }
    public int Regen(int hp)
    {
        this.hp += hp;
        this.hp = this.hp >= maxHp ? maxHp: this.hp;
        return hp;
    }

    public int manaRegen(int mana)
    {
        this.mana += mana;
        this.mana = this.mana >= maxMana ? maxMana : this.Mana;
        return mana;
    }
}
