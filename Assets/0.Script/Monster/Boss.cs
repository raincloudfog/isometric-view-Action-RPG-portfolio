using Skills;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.TextCore.LowLevel;

namespace Monster
{
    public class Boss : Monster
    {
        public float[] pattonPercentage = new float[3]
        {
            30,30,30
        };

        [SerializeField]
        protected Player.Player player;

        public ParticleSystem Patton1Effet;

        public Fsm[] pattons;

        Vector3 OriginPos;

        public float timer;

        public float attackDelay = 1f;

        Coroutine _attackEvent;

        private bool isPattonFinish;

        public int pattonDamage;

        public UnityEvent OnDeath;
       
        public override void Init()
        {
            health.Init(Hp, 0);
            if (!agent.isOnNavMesh)
            {
                Debug.LogError(" NavMeshAgent가 현재 NavMesh 위에 올바르게 배치되어 있지 않음.");

                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    Debug.LogError("NavMesh 영역 내에 오브젝트를 재 배치!!");

                    agent.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("NavMesh 영역 내에 오브젝트를 배치할 수 없습니다.");
                }
            }
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
            Collider[] Object = Physics.OverlapSphere(transform.position, FindDistance, TargetLayer);            

            foreach (Collider collider in Object)
            {
                Player.Player player = collider.GetComponent<Player.Player>();

                if (player != null)
                {
                    this.player.Hit(Damage);
                }


            }
        }
       
        public void Move()
        {
            

            if(player != null)
            {
                agent.destination = player.transform.position;
                return;
            }

            Collider[] Object = Physics.OverlapSphere(transform.position, FindDistance, TargetLayer);

            if (Object == null)
                Debug.Log("플레이어가 없음");
            else Debug.Log("플레이어 발견");

            foreach(Collider collider in Object)
            {
                Player.Player player = collider.GetComponent<Player.Player>();

                if (player != null)
                {
                    this.player = player;
                }


            }

            
        }
        
        public bool Attack()
        {
            float sqrDistance = Vector3.SqrMagnitude(transform.position - player.transform.position);
            //사거리확인
            if (sqrDistance <= Range)
            {
                agent.isStopped = true;
                anim.SetTrigger("attack");
                Debug.Log("한대때림");
                player.Hit(Damage);
                return true;
            }
            else
            {
                agent.isStopped = false;
                return false;

            }
        }

        public void AttackEvent()
        {
            if(_attackEvent == null)
            {
                _attackEvent = StartCoroutine(AttackRogic());

                Debug.Log("어택 이벤트 실행");
            }
            else
            {
                Debug.Log("어택 이벤트가 널이 아님" + _attackEvent.ToString());
            }
        }

        public void AttackEventStop() 
        {
            Debug.Log("어택 이벤트 멈추기");
            if(_attackEvent != null ) 
                StopCoroutine(_attackEvent);
            _attackEvent = null;

        }

        IEnumerator AttackRogic()
        {
            float attackTimer = 0;
            float attackDuration = 2;

            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9)
            {
                yield return new WaitForEndOfFrame();
                Debug.Log("아직 공격성공못함.");
                if (Attack())
                {
                    Debug.Log("보스가 공격 함");

                    break;
                }
            }

            Debug.Log("공격 와일문 벗어남.");
            _attackEvent = null;
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
                yield return null;

                
                if (GameData.isEnterthebossroom == false)
                    continue;
                    

                if (health.isDeath)                    
                {
                    anim.SetTrigger("Death");
                    Debug.Log("보스죽어있음");
                    break;
                }
                

                Move();

                if (player == null)
                    continue;
                if (isPattonFinish)
                    timer += Time.deltaTime;


                if(timer > nextPattons)
                {
                    Debug.Log("코루틴실행");

                    StartPatton(Choose(pattonPercentage) + 1);                            
                    timer = 0;
                }
            }

            agent.isStopped = true;
        }


        public override void Hit(int Damage)
        {
            if (health.isDeath)
                return;
            int beforeHP = health.HP;


            health.Hit(Damage);

            //Debug.Log(gameObject.name + "의 체력 비교 전 / 후 : " + beforeHP + "/" + health.HP);
            if (health.isDeath)
            {
                pattonDamage = 0;
                GameStateManager.stageManage.MonsterDead();
                Debug.Log(gameObject.name + "죽었다..");
                agent.isStopped = true;

                OnDeath?.Invoke();
            }
        }

        public override void ChageFSM(State state)
        {
            
        }

        public override void Drop()
        {
            StartCoroutine(DropRogic());
        }

        IEnumerator DropRogic()
        {
            DropItem[] drops = new DropItem[3];
            Vector3[] vector3s = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                drops[i] = Instantiate(SettingManager.Instance.ADropitem.GetComponent<DropItem>());
                drops[i].Init();
                vector3s[i] = Vector3.zero;

                //랜덤 위치를 만들고 만약 다른 아이템들 위치와 중첩될경우 다시 위치 선정
                Vector3 randpos = transform.position + new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2));
                Vector3 checkpos = System.Array.Find(vector3s, pos => pos == randpos);

                if(randpos == checkpos)
                {
                    while (randpos == checkpos)
                    {
                        yield return null;
                        Debug.Log("드랍아이템  랜덤 포스  중첩되므로 다시 만들겠음");
                        randpos = transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));                        
                    }
                }

                
                vector3s[i] = randpos;

                drops[i].transform.position = randpos;
            }
            gameObject.SetActive(false);
        }
    }
}