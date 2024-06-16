using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    using P = Player.Player;

    public enum SkillName
    {
        GroundHit,
        Teleport,
        Heal,
        Starfall
    }

    public class Skill : MonoBehaviour
    {

        public ParticleSystem skillEffect;

        public Vector3 skillPos;
        public float CoolTime = 0;
        protected int Damage = 10;

        protected Player.Player player;

        public SkillName skillName;


        private void Start()
        {
            
        }

        public virtual void Init(P player)
        {
            this.player = player;
            Damage = Player.Stat.Damage;
            SetSkill();
            //Debug.Log(gameObject.name + " 스킬 현재 데미지 : " + Damage);
        }
     
        public virtual void SetSkill()
        {

        }


        public virtual void Use()
        {
            
        }

        public virtual void OnParticleCollision(GameObject other)
        {
            if (other != null)
            {
                Debug.Log(other.name);
            }
        }

    }
}