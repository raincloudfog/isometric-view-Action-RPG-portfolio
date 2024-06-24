using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class Heal : Skill
    {
        public override void Init(Player.Player player)
        {
            base.Init(player);
            Damage = Damage * 2;
            CoolTime = 5;
            Manacost = 5;
        }

        private void Update()
        {
            if (CoolTimer <= 0)
            {
                CoolTimer = 0;
                isActive = true;
            }
            else
            {
                CoolTimer -= Time.deltaTime;
            }
        }

        public override void Use()
        {
            if(CoolTimer> 0)
            {
                return;
            }

            base.Use();
            
            player.UsingSkill(true);
            //float heal = Damage * 0.7f;
            transform.position = player.transform.position;
            if (player != null)
            {
                player.Heal(Damage);
                Debug.Log("Èú!!");
            }
            skillEffect.Play();
            StartCoroutine(Skillduration());
            CoolTimer = CoolTime;
            StartCoroutine(PlayManager.Instance.playerUI.SkillCool(SkillManager.SkillName.Heal, CoolTime));
            isActive = false;
        }

        public override void OnParticleCollision(GameObject other)
        {
            
        }

        IEnumerator Skillduration()
        {
            float timer = 0;
            float duration = 1;

            while (timer < duration)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            skillEffect.Stop();
        }
    }

}

