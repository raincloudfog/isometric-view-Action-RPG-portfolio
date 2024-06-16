using Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Monster
{
    public class Boss : Monster
    {
        public float[] pattonPercentage = new float[3]
        {
            30,30,30
        };

        protected Player.Player player;

        public ParticleSystem Patton1Effet;

        public Fsm[] pattons;

        Vector3 OriginPos;

        public float timer;

        public float attackDelay = 1f;



        private bool isPattonFinish;

        public int pattonDamage;
        

        private void Start()
        {
            OriginPos = transform.position;
            //StartPatton(3);
            isPattonFinish = true;
            StartCoroutine(PattonCoroutine());
        }

        public void Update()
        {
            timer += Time.deltaTime;
           
        }

        void StartPatton(int Number)
        {
            Debug.Log(Number + "받은 숫자");

            pattonDamage = (int)(Damage * 1.5f);
            agent.isStopped = true;
            anim.SetInteger("Patton" , Number);

            anim.SetTrigger("ActivePatton");

            isPattonFinish = false;
        }

        public void CheckPositon()
        {
            Debug.Log(transform.position + "현재 애니메이션으로 인한위치 / 원래 위치 : " + OriginPos);
        }

        public void finish()
        {
            isPattonFinish = true;
            agent.isStopped = false;
        }

        public void SlamEffect()
        {
            ParticleSystem newparticle = Instantiate(Patton1Effet);
            newparticle.transform.position = transform.position;
            newparticle.Play();
        }
       
        public void Move()
        {
            if(player != null)
            {
                agent.destination = player.transform.position;
                return;
            }

            Collider[] Object = Physics.OverlapSphere(transform.position, FindDistance, TargetLayer);

            foreach(Collider collider in Object)
            {
                Player.Player player = collider.GetComponent<Player.Player>();

                if (player != null)
                {
                    this.player = player;
                }


            }

            
        }


        public int Choose(float[] prob)
        {
            float total = 0;

            foreach (float item in prob)
            {
                total += item;
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < prob.Length; i++)
            {
                if (randomPoint < prob[i])
                {
                    Debug.Log(i + "뽑은 숫자");
                    return i;

                }
                else
                {
                    total -= prob[i];
                }
            }

            return prob.Length - 1;
        }

        public void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Player.Player player = other.GetComponent<Player.Player>();

                if(timer >= attackDelay)
                {
                    player.Hit(Damage);
                    timer = 0;
                }
            }
        }

        IEnumerator PattonCoroutine()
        {

            float timer = 0;

            float nextPattons = 3f;

            while(true)
            {
                if (health.isDeath)                    
                {

                    Debug.Log("보스죽어있음");
                    break;
                }

                Move();

                    if (isPattonFinish)
                    timer += Time.deltaTime;

                yield return null;

                if(timer > nextPattons)
                {
                    Debug.Log("코루틴실행");

                    StartPatton(Choose(pattonPercentage) + 1);                            
                    timer = 0;
                }
            }


        }


        public override void Hit(int Damage)
        {
            base.Hit(Damage);
        }

        public override void ChageFSM(State state)
        {
            
        }


        
    }
}