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
        //�׽�Ʈ��
        public Skill Telpo;

        //���� 
        private Health health = new Health();
        State state = State.Alive;

        //�̵� ����
        public float speed = 4f;
        private Vector3 lastPosition;
        private bool ismove;

        //��ũ��Ʈ
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
                Debug.Log("�÷��̾ �׾���.");
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
                Debug.Log("��ų ȹ��");
            }
            else
            {
                Debug.Log("��ų ȹ�� ���� �ش� Ű�� �� ��ų�� �ƴ�");
            }
        }


        #region ����
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

        #region �̵�        

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
            // � Ű�� ���ȴ��� Ȯ���մϴ�.
            string key = context.control.name;
            //Debug.Log("Key pressed: " + key);

            movement.Rotation(PlayManager.Instance.MousePosition(transform.position));

            switch (key)
            {
                case "q":
                    // Q Ű�� ������ ���� ����
                    //[KeyCode.Q].Use();
                    skillDic[KeyCode.Q].Use();                    
                    Debug.Log("Q key pressed");
                    break;
                case "w":
                    //TestTelpo();
                    // W Ű�� ������ ���� ����
                    skillDic[KeyCode.W].Use();
                    Debug.Log("W key pressed");
                    break;
                case "e":
                    // E Ű�� ������ ���� ����
                    skillDic[KeyCode.E].Use();
                    Debug.Log("E key pressed");
                    break;
                case "r":
                    // R Ű�� ������ ���� ����
                    skillDic[KeyCode.R].Use();
                    Debug.Log("R key pressed");
                    break;
                case "t":
                    // T Ű�� ������ ���� ����
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