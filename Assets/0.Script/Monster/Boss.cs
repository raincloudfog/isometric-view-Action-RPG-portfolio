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
                Debug.LogError(" NavMeshAgent�� ���� NavMesh ���� �ùٸ��� ��ġ�Ǿ� ���� ����.");

                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    Debug.LogError("NavMesh ���� ���� ������Ʈ�� �� ��ġ!!");

                    agent.Warp(hit.position);
                }
                else
                {
                    Debug.LogError("NavMesh ���� ���� ������Ʈ�� ��ġ�� �� �����ϴ�.");
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
            Debug.Log(Number + "���� ����");

            pattonDamage = (int)(Damage * 1.5f);
            agent.isStopped = true;
            anim.SetInteger("Patton" , Number);

            anim.SetTrigger("ActivePatton");

            isPattonFinish = false;
        }

        public void CheckPositon()
        {
            Debug.Log(transform.position + "���� �ִϸ��̼����� ������ġ / ���� ��ġ : " + OriginPos);
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
                Debug.Log("�÷��̾ ����");
            else Debug.Log("�÷��̾� �߰�");

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
            //��Ÿ�Ȯ��
            if (sqrDistance <= Range)
            {
                agent.isStopped = true;
                anim.SetTrigger("attack");
                Debug.Log("�Ѵ붧��");
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

                Debug.Log("���� �̺�Ʈ ����");
            }
            else
            {
                Debug.Log("���� �̺�Ʈ�� ���� �ƴ�" + _attackEvent.ToString());
            }
        }

        public void AttackEventStop() 
        {
            Debug.Log("���� �̺�Ʈ ���߱�");
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
                Debug.Log("���� ���ݼ�������.");
                if (Attack())
                {
                    Debug.Log("������ ���� ��");

                    break;
                }
            }

            Debug.Log("���� ���Ϲ� ���.");
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
                    Debug.Log(i + "���� ����");
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
                    Debug.Log("�����׾�����");
                    break;
                }
                

                Move();

                if (player == null)
                    continue;
                if (isPattonFinish)
                    timer += Time.deltaTime;


                if(timer > nextPattons)
                {
                    Debug.Log("�ڷ�ƾ����");

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

            //Debug.Log(gameObject.name + "�� ü�� �� �� / �� : " + beforeHP + "/" + health.HP);
            if (health.isDeath)
            {
                pattonDamage = 0;
                GameStateManager.stageManage.MonsterDead();
                Debug.Log(gameObject.name + "�׾���..");
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

                //���� ��ġ�� ����� ���� �ٸ� �����۵� ��ġ�� ��ø�ɰ�� �ٽ� ��ġ ����
                Vector3 randpos = transform.position + new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2));
                Vector3 checkpos = System.Array.Find(vector3s, pos => pos == randpos);

                if(randpos == checkpos)
                {
                    while (randpos == checkpos)
                    {
                        yield return null;
                        Debug.Log("���������  ���� ����  ��ø�ǹǷ� �ٽ� �������");
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