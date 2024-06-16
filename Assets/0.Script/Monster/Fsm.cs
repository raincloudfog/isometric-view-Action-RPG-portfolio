using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{
    #region �⺻ fsm
    public abstract class Fsm
    {
        public Fsm(Monster monster)
        {
            this.monster = monster;
        }

        public Monster monster;


        public abstract void OnEnter();
        public abstract void OnUpdate();

        public abstract void OnExit();


    }

    #endregion


    #region �⺻ ���� fsm ��Ʈ

    public class FSMIdle : Fsm
    {
        public FSMIdle(Monster monster) : base(monster)
        {
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
           
        }

        public override void OnUpdate()
        {
            Search();
        }
        


        void Search()
        {
            Collider[] findplayer = Physics.OverlapSphere(monster.transform.position, monster.FindDistance);

            foreach (Collider obj in findplayer)
            {
                //Debug.Log(obj);
                if (obj.gameObject.CompareTag("Player"))
                {
                    monster.Target = obj.transform;
                    monster.ChageFSM(State.FindPlayer);
                }
            }
        }

    }

    public class FSMFindPlayer : Fsm
    {
        Transform Target;

        float attackCheckTimer;
        float attackDelay;


        public FSMFindPlayer(Monster monster) : base(monster)
        {
            attackDelay = monster.AttackSpeed;
        }

        public override void OnEnter()
        {            
            Target = monster.Target;

        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            Attack();
            Move();
        }

        void Move()
        {
            monster.agent.destination = Target.position;
            monster.anim.SetBool("move",true);
        }

        void Attack()
        {

            attackCheckTimer += Time.deltaTime;
            
            if(attackCheckTimer < attackDelay)
            {
                return;
            }

            //��Ÿ�Ȯ��
            if (Vector3.Distance( monster.transform.position, Target.position) <= monster.Range)
            {
                monster.agent.isStopped = true;
                Player.Player player = Target.GetComponent<Player.Player>();
                monster.anim.SetTrigger("attack");

                if(AttackCheck())
                {
                    Debug.Log("�Ѵ붧��");
                    player.Hit(monster.Damage);
                }
            }
            else
            {
                monster.agent.isStopped = false;
            }
            attackCheckTimer = 0;
        }

        bool AttackCheck()
        {
            if(monster.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f
                && Vector3.Distance(monster.transform.position, Target.position) <= monster.Range)
            {
                return true;
            }

            return false;
        }
    }

    public class FSMDeath: Fsm
    {
        public FSMDeath(Monster monster) : base(monster)
        {
        }

        public override void OnEnter()
        {
            monster.anim.SetTrigger("death");
            monster.agent.isStopped = true;
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }

    #endregion


}