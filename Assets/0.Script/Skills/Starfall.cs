using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    using Monster;
    public class Starfall : Skill
    {
        Vector3 offset = new Vector3(0,6,0);

        public override void Use()
        {
            base.Use();
            player.anim.Attack();
            player.UsingSkill(true);
            //float damage = Damage * 0.33f;
            //Damage = (int)damage;
            transform.position = PlayManager.Instance.MousePosition(transform.position) + offset;
            skillEffect.Play();
        }

        public override void OnParticleCollision(GameObject other)
        {
            if (other != null)
            {
                if(other.CompareTag("Enemy"))
                {
                    Monster enemy = other.GetComponent<Monster>();
                    enemy.Hit(Damage);
                    Debug.Log("현재 스타폴 데미지 " + Damage);
                }
            }
        }

    }
}

