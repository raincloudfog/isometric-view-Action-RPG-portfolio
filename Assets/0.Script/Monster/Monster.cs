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
        //스텟 관련
        public int Hp;
        public float moveSpeed;
        public float AttackSpeed;
        public int Damage;
        public float Range;
        public float FindDistance;


        public Transform Target;


        //컴포넌트
        public Animator anim;
        public NavMeshAgent agent;

        public Fsm fsmState;
        public State state;

        [SerializeField]
        protected Health health = new Health();

        public LayerMask TargetLayer;

        private void OnEnable()
        {
            Init();

        }

        private void Start()
        {
        }

        public virtual void Init()
        {
            Setting();
        }

        public virtual void Hit(int Damage)
        {
            if (health.isDeath) return;

            health.Hit(Damage);

            int beforeHP = health.HP;


            //Debug.Log(gameObject.name + "의 체력 비교 전 / 후 : " + beforeHP + "/" + health.HP);
            if (health.isDeath)
            {
                GameStateManager.stageManage.MonsterDead();
                Debug.Log(gameObject.name + "죽었다..");
                ChageFSM(State.Death);
                agent.isStopped = true;
                Invoke("activeOff", 2);
                return;
            }
            
        }

        void activeOff()
        {
            gameObject.SetActive(false);
        }

        public virtual void Setting()
        {
            health.Init(Hp, 0);
            agent.speed = moveSpeed;
        }

        public abstract void ChageFSM(State state);

        public virtual void Drop()
        {
            DropItem dropItem = Instantiate( SettingManager.Instance.ADropitem.GetComponent<DropItem>());
            dropItem.gameObject.SetActive(false);
             dropItem.Init();
            dropItem.gameObject.SetActive(true);
            dropItem.transform.position = transform.position;
        }
    }
}




