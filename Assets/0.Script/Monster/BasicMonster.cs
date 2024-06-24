using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster
{

    public class BasicMonster : Monster
    {

        public FSMIdle idle;
        public FSMFindPlayer findPlayer;
        public FSMDeath death ;

        public override void ChageFSM(State state)
        {
            fsmState.OnExit();
            this.state = state;

            switch (state)
            {
                case State.Idle:
                    fsmState = idle;
                    break;
                case State.FindPlayer:
                    fsmState = findPlayer;
                    break;
                case State.Death:
                    fsmState = death;
                    break;

            }
            fsmState.OnEnter();
        }


        // Start is called before the first frame update
        void Start()
        {
            idle = new FSMIdle(this);
            findPlayer = new FSMFindPlayer(this);
            death = new FSMDeath(this);
            fsmState = idle;
            fsmState.OnEnter();


        }



        // Update is called once per frame
        void Update()
        {
            fsmState.OnUpdate();
        }

        public void Attack()
        {
            float sqr = Vector3.SqrMagnitude(transform.position -  Target.position);
            Debug.Log(sqr);
            //사거리확인
            if (sqr <= Range)
            {
                
                Player.Player player = Target.GetComponent<Player.Player>();

                if (player != null)
                {
                    Debug.Log("한대때림");
                    player.Hit(Damage);
                }
            }            
        }

    }
}