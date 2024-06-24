using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    using Monster;
    using Player;

    public class Starfall : Skill
    {
        Vector3 offset = new Vector3(0,6,0);

        public override void Init(Player player)
        {
            base.Init(player);
            CoolTime = 5;
            Manacost = 7;
        }

        private void Update()
        {
            if (CoolTimer <= 0)
            {
                isActive = true;
                CoolTimer = 0;
            }
            else
            {
                CoolTimer -= Time.deltaTime;
            }
        }

        public override void Use()
        {
            if(CoolTimer > 0)
            {
                return;
            }

            base.Use();
            player.anim.Attack();
            player.UsingSkill(true);
            //float damage = Damage * 0.33f;
            //Damage = (int)damage;
            transform.position = PlayManager.Instance.MousePositionNoClick(transform.position) + offset;
            skillEffect.Play();
            CoolTimer = CoolTime;
            StartCoroutine(PlayManager.Instance.playerUI.SkillCool(SkillManager.SkillName.Starfall, CoolTime));
            StartCoroutine(Skillduration());
            isActive = false;
        }

        public override void OnParticleCollision(GameObject other)
        {
            if (other != null)
            {
                if(other.CompareTag("Enemy"))
                {
                    Monster enemy = other.GetComponent<Monster>();
                    enemy.Hit(Damage);
                    //Debug.Log("현재 스타폴 데미지 " + Damage);
                }
            }
        }

        IEnumerator Skillduration()
        {
            float timer = 0;
            float duration = 1;

            while(timer < duration)
            {
                yield return null;
                timer += Time.deltaTime;                
            }
            skillEffect.Stop();
        }

    }
}

