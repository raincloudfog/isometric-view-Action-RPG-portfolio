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
        public int CoolTime = 0;
        public float CoolTimer = 0;
        [SerializeField]
        protected int Damage = 10;
        public int Manacost = 0;

        //��ų ��� ��������
        public bool isActive = true;

        protected Player.Player player;

        public SkillName skillName;


        private void Start()
        {
            
        }

        public virtual void Init(P player)
        {
            this.player = player;
            Damage = player.stat.Damage;
            SetSkill();
            //Debug.Log(gameObject.name + " ��ų ���� ������ : " + Damage);
        }
     
        public virtual void SetSkill()
        {

        }


        public virtual void Use()
        {
         //   Debug.Log(name + "�ߵ�");
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