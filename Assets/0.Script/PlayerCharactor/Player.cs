using Items;
using Monster;
using Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;



namespace Player
{
    [Serializable]
    public class Stat
    {
        public int Level;


        public int MaxHP;
        public int MaxMana;


        public int Damage;
        public int moveSpeed;

        public int hpRegen;
        public int manaRegen;

        public void Setting()
        {
            MaxHP = 50;
            MaxMana = 30;            
            Damage = 7;
            moveSpeed = 4;
            hpRegen = 2;
            manaRegen = 1;
        }

        public void Setting(int hp, int mana, int damage)
        {
            
        }

        public void SettingLoad()
        {

        }
    }

    public enum State
    {
        Alive,
        Death,

    }


    public class Player : MonoBehaviour
    {

        //생존 

        [SerializeField]
        private Health health = new Health();
        State state = State.Alive;
        public Stat stat= new Stat();
        public float regenTimer = 0;

        //공격 딜레이
        private float _attackDelay = 1;
        private float _attackTimer = 0;
        private bool isAttackDelay = false;

        //이동 관련
        public float speed = 4f;
        private Vector3 lastPosition;
        private bool ismove;

        private bool isReady = false;

        //스크립트
        public PlayerAnim anim;
        public PlayerMovement movement;
        public PlayerAttack _attack;

        Dictionary<KeyCode, Skill> skillDic = new Dictionary<KeyCode, Skill>();
        
        public void Init()
        {
            stat.Setting();
            health.Init(stat.MaxHP, stat.MaxMana);

            skillDic.Add(KeyCode.Q, null);
            skillDic.Add(KeyCode.W, null);
            skillDic.Add(KeyCode.E, null);
            skillDic.Add(KeyCode.R, null);

            AddSKill(KeyCode.Q, SkillManager.Instance.GetSkill(SkillManager.SkillName.ActiveGroundHit));
            AddSKill(KeyCode.W, SkillManager.Instance.GetSkill(SkillManager.SkillName.Telpo));
            AddSKill(KeyCode.E, SkillManager.Instance.GetSkill(SkillManager.SkillName.Heal));
            AddSKill(KeyCode.R, SkillManager.Instance.GetSkill(SkillManager.SkillName.Starfall));

            foreach (Skill skill in skillDic.Values)
            {
                skill.Init(this);
            }
            movement.Init(speed);
            _attack.Init(this);
            isReady = true;
        }

        private void FixedUpdate()
        {
            
            if (isReady == false || health.isDeath) return;
            if(regenTimer <=  0)
            {
                regenTimer = 2;
                health.Regen(stat.hpRegen);
                health.manaRegen(stat.manaRegen);
                PlayManager.Instance.playerUI.ChangeBar(true, health.MaxHP, health.HP);
                PlayManager.Instance.playerUI.ChangeBar(false, health.MaxMana, health.Mana);
            }
            if(health.HP < health.MaxHP || health.Mana < health.MaxMana)
                regenTimer -= Time.deltaTime;
            else
            {
                regenTimer = 2;
            }
        }        

        public void HealthInit()
        {
            health.Init(stat.MaxHP, stat.MaxMana);
            PlayManager.Instance.playerUI.ChangeBar(true, health.MaxHP, health.HP);
            PlayManager.Instance.playerUI.ChangeBar(false, health.MaxMana, health.Mana);
        }

        // Update is called once per frame
        void Update()
        {
            if (state == State.Death)
            {
                //Debug.Log("플레이어가 죽었음.");
                return;
            }

            if (health.isDeath)
            {
                //Debug.Log("플레이어가 죽었음.");
                state = State.Death;
                anim.Death();
                return;
            }
            
            _attackTimer -= Time.deltaTime;

            if((Input.GetMouseButton(1) || Input.GetMouseButtonDown(1)) && _attackTimer <= 0)
            {
                isAttackDelay = true;
                movement.Rotation(PlayManager.Instance.MousePositionNoClick(transform.position));
                anim.Attack();
                PlayManager.Instance.SetPoint(transform.position);
                _attack.Attack();
                _attackTimer = _attackDelay;
                StartCoroutine(AttackMoveDelay());
            }

            Move();
            Vector3 currentPosition = transform.position;
            float distanceMoved = Vector3.Distance(lastPosition, currentPosition);

            if (distanceMoved > 0.01f)
            {
                anim.Move(1);

            }
            else
            {
                anim.Move(0);
            }

            lastPosition = currentPosition;
        }

        public void AddSKill(KeyCode keyCode, Skill skill)
        {
            if(skillDic.ContainsKey(keyCode))
            {
                skillDic[keyCode] = skill;
                Debug.Log("스킬 획득");
            }
            else
            {
                Debug.Log("스킬 획득 실패 해당 키에 들어갈 스킬이 아님");
            }
        }


        #region 공격 관련

        IEnumerator AttackMoveDelay()
        {
            float delay = 0.3f;

            yield return new WaitForSeconds(delay);
            isAttackDelay = false;
        }


        public void Hit(int Damage)
        {
            if (health.isDeath) return;

            health.Hit(Damage);
            
            PlayManager.Instance.playerUI.ChangeBar(true, health.MaxHP, health.HP);
            if (health.isDeath)
            {
                //Debug.Log("플레이어가 죽었음.");
                state = State.Death;
                anim.Death();
                StartCoroutine(Death());
            }
        }

        IEnumerator Death()
        {
            float deathtimer = 3;

            yield return new WaitForSeconds(deathtimer);
            SceneLoaderManager.Instance.LoadScene(SceneLoaderManager.SceneName.Town);

        }

        public void Heal(int Hp)
        {
            health.Heal(Hp);
            PlayManager.Instance.playerUI.ChangeBar(true, health.MaxHP, health.HP);
        }
        #endregion

        #region 이동        

        public void MovePosition(Vector3 point)
        {         
            movement.MovePosition(point);
        }

        

        public void UsingSkill(bool isStopmove)
        {
            if (isStopmove)
            {
                anim.Move(0);
                movement.Move(transform.position);
            }
            
        }

        private void Move()
        {
            if(movement.agent.enabled == false )
            {
                return;
            }

            if (GameData.isPickingItem || GameData.isOpenUI || isAttackDelay)
            {
                Debug.Log(GameData.isPickingItem + "GameData.isPickingItem  / GameData.isOpenUI" + GameData.isOpenUI);
                movement.StopMove(true);
                return;
            }
            movement.StopMove(false);
            movement.Move(PlayManager.Instance.MousePosition(transform.position));
        }

        #endregion

        public void OnQWERT(InputAction.CallbackContext context)
        {
            if (state == State.Death)
            {
                return;
            }

            if (context.performed)
            {

                string key = context.control.name;
                KeyCode keyCode = KeyCode.Escape;
                
                switch (key)
                {
                    case "q":
                        // Q 키가 눌렸을 때의 로직
                        //[KeyCode.Q].Use();
                        keyCode = KeyCode.Q;
                        //Debug.Log("Q key pressed");
                        break;
                    case "w":
                        // W 키가 눌렸을 때의 로직
                        keyCode = KeyCode.W;
                        //Debug.Log("W key pressed");
                        break;
                    case "e":
                        // E 키가 눌렸을 때의 로직
                        keyCode = KeyCode.E;
                        //Debug.Log("E key pressed");
                        break;
                    case "r":
                        // R 키가 눌렸을 때의 로직
                        keyCode = KeyCode.R;
                        //Debug.Log("R key pressed");
                        break;

                    default:
                        Debug.Log("Other key pressed");
                        return;
                }
                if (skillDic[keyCode].isActive)
                {
                    //Debug.Log(keyCode + "의 스킬 사용 가능 하고 지금 현재 쿨타임" + skillDic[keyCode].CoolTimer);
                    if (health.UseSkill(skillDic[keyCode].Manacost))
                    {
                        Vector3 point = PlayManager.Instance.MousePositionNoClick(transform.position);
                        movement.Rotation(point);
                        skillDic[keyCode].Use();
                        PlayManager.Instance.playerUI.ChangeBar(false, health.MaxMana, health.Mana);
                       // Debug.Log(keyCode + "의 스킬 사용 가능한지 확인 및  지금 현재 쿨타임" + skillDic[keyCode].isActive + " /" + skillDic[keyCode].CoolTimer);
                        PlayManager.Instance.SetPoint(transform.position);
                        if (keyCode == KeyCode.W)
                        {
                            PlayManager.Instance.SetPoint(point);
                        }
                    }
                }
            }
        }

        public void OnOffInventory(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                if(GameStateManager.stageManage._optionUi.gameObject.activeSelf == true)
                {
                    return;
                }
                ItemManager.Instance.OnOffInventory();

            }
        }

        #region 캐릭터 스텟 변환

        //마나 추가해줄수도 있음.
        public void EquipChangeStat(eEquipmentType type , int lank)
        {
            if(lank == 0)
            {
                return;
            }

            switch (type)
            {
                case eEquipmentType.Helmet:
                    stat.MaxHP +=  lank * 4;
                    stat.hpRegen += lank * 2;
                    break;
                case eEquipmentType.ChestArmor:
                    stat.MaxHP +=  lank * 7;
                    stat.MaxMana += lank * 4;
                    break;
                case eEquipmentType.LegArmor:
                    stat.MaxHP +=  lank * 5;
                    break;
                case eEquipmentType.Boots:
                    stat.MaxHP += lank * 2;
                    break;
                case eEquipmentType.Weapon:
                    stat.Damage = stat.Damage + (lank * 2);
                    foreach (Skill skill in skillDic.Values)
                    {
                        skill.Init(this);
                    }
                    break;
                default:
                    break;
            }

            health.MaxHP = stat.MaxHP;
            health.MaxMana = stat.MaxMana;

            PlayManager.Instance.playerUI.ChangeBar(true, health.MaxHP, health.HP);
            PlayManager.Instance.playerUI.ChangeBar(false, health.MaxMana, health.Mana);

        }

        public void UnEquipItem(eEquipmentType type, int lank)
        {
            if (lank == 0)
            {
                Debug.Log("장비를 착용하지 않았음.");
                return;
            }

            Debug.Log("장비 해제");

            switch (type)
            {
                case eEquipmentType.Helmet:
                    stat.MaxHP -= lank * 4;
                    stat.hpRegen -= lank * 2;
                    break;
                case eEquipmentType.ChestArmor:
                    stat.MaxHP -= lank * 7;
                    stat.MaxMana -= lank * 4;
                    break;
                case eEquipmentType.LegArmor:
                    stat.MaxHP -= lank * 5;
                    break;
                case eEquipmentType.Boots:
                    stat.MaxHP -= lank * 2;
                    break;
                case eEquipmentType.Weapon:
                    stat.Damage = stat.Damage - (lank * 2);
                    foreach (Skill skill in skillDic.Values)
                    {
                        skill.Init(this);
                    }
                    break;
                default:
                    break;
            }

            health.MaxHP = stat.MaxHP;
            health.MaxMana = stat.MaxMana;

            PlayManager.Instance.playerUI.ChangeBar(true, health.MaxHP, health.HP);
            PlayManager.Instance.playerUI.ChangeBar(false, health.MaxMana, health.Mana);

        }

        #endregion

    }


}