using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Monster
{

    public enum State
    {
        Idle,
        FindPlayer,
        Death,
    }

    public abstract class Monster : MonoBehaviour
    {
        //���� ����
        public int Hp;
        public float moveSpeed;
        public float AttackSpeed;
        public int Damage;
        public float Range;
        public float FindDistance;


        public Transform Target;


        //������Ʈ
        public Animator anim;
        public NavMeshAgent agent;

        public Fsm fsmState;
        public State state;

        protected Health health = new Health();

        public LayerMask TargetLayer;

        private void OnEnable()
        {
            Init();

        }

        private void Start()
        {
        }

        public void Init()
        {
            Setting();
        }

        public virtual void Hit(int Damage)
        {
            int beforeHP = health.HP;


            health.Hit(Damage);

            //Debug.Log(gameObject.name + "�� ü�� �� �� / �� : " + beforeHP + "/" + health.HP);
            if(health.isDeath)
            {
                GameStateManager.stageManage.MonsterDead();
                Debug.Log(gameObject.name + "�׾���..");
                ChageFSM(State.Death);
                agent.isStopped = true;
            }
        }

        public virtual void Setting()
        {
            health.Init(Hp);
            agent.speed = moveSpeed;
        }

        public abstract void ChageFSM(State state);

        public virtual void Drop()
        {

        }
    }
}




