using Monster;
using Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;



public struct UnitStat
{
    public int Hp;
    public int Damage;
    public int moveSpeed;

}


namespace Player
{
    [Serializable]
    public static class Stat
    {
        public static int hp;
        public static int mana;

        public static int Damage;
        public static int moveSpeed;

        public static void Setting()
        {
            Stat.hp = 50;
            Stat.mana = 30;
            Stat.Damage = 7;
        }

        public static void Setting(int hp, int mana, int damage)
        {
            Stat.hp = hp;
            Stat.mana = mana;
            Stat.Damage = damage;
        }
    }

    public enum State
    {
        Alive,
        Death,

    }


    public class Player : MonoBehaviour
    {
        //테스트용
        public Skill Telpo;

        //생존 
        private Health health = new Health();
        State state = State.Alive;

        //이동 관련
        public float speed = 4f;
        private Vector3 lastPosition;
        private bool ismove;

        //스크립트
        public PlayerAnim anim;
        public PlayerMovement movement;

        Dictionary<KeyCode, Skill> skillDic = new Dictionary<KeyCode, Skill>();

        // Start is called before the first frame update
        void Start()
        {
            Skill telpo = Instantiate(Telpo);
            Telpo = telpo;

            Stat.Setting();
            health.Init(Stat.hp);

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
        }

        // Update is called once per frame
        void Update()
        {
            
            if(health.isDeath)
            {
                Debug.Log("플레이어가 죽었음.");
                state = State.Death;
                anim.Death();
            }
            if(state == State.Death)
            {
                return;
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


        #region 공격
        public void Attack(bool Actions)
        {
            if(Actions)
                anim.Attack();        
        }

        public void Hit(int Damage)
        {
            health.Hit(Damage);
        }

        public void Heal(int Hp)
        {
            health.Heal(Hp);
        }
        #endregion

        #region 이동        

        public void UsingSkill(bool isStopmove)
        {
            if (isStopmove)
            {
                anim.Move(0);
                movement.Move(transform.position);
            }
            
        }
        #endregion

        public void OnQWERT(InputAction.CallbackContext context)
        {
            if (state == State.Death)
            {
                return;
            }

            //Debug.Log(context.control.valueType.Name);
            // 어떤 키가 눌렸는지 확인합니다.
            string key = context.control.name;
            //Debug.Log("Key pressed: " + key);

            movement.Rotation(PlayManager.Instance.MousePosition(transform.position));

            switch (key)
            {
                case "q":
                    // Q 키가 눌렸을 때의 로직
                    //[KeyCode.Q].Use();
                    skillDic[KeyCode.Q].Use();                    
                    Debug.Log("Q key pressed");
                    break;
                case "w":
                    //TestTelpo();
                    // W 키가 눌렸을 때의 로직
                    skillDic[KeyCode.W].Use();
                    Debug.Log("W key pressed");
                    break;
                case "e":
                    // E 키가 눌렸을 때의 로직
                    skillDic[KeyCode.E].Use();
                    Debug.Log("E key pressed");
                    break;
                case "r":
                    // R 키가 눌렸을 때의 로직
                    skillDic[KeyCode.R].Use();
                    Debug.Log("R key pressed");
                    break;
                case "t":
                    // T 키가 눌렸을 때의 로직
                    //skillDic[KeyCode.Q].Use(this);
                    Debug.Log("T key pressed");
                    break;
                default:
                    Debug.Log("Other key pressed");
                    break;
            }
        }             

        private void Move()
        {
            if(GameData.isPickingItem)
            {
                return;
            }
            movement.StopMove(false);
            movement.Move(PlayManager.Instance.MousePosition(transform.position));
        }               

        public void OnOffInventory(InputAction.CallbackContext context)
        {
            Debug.Log("OnOffInventory");
            if(context.performed)
            {
                ItemManager.Instance.OnOffInventory();

            }
        }
       
    }


}