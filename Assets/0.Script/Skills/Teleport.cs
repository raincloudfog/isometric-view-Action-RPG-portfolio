using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{


    public class Teleport : Skill
    {
        public float LimitDistance = 5f;
        private Vector3 point;

        public override void Init(Player.Player player)
        {
            base.Init(player);
            CoolTime = 5;
            Manacost = 3;

        }

        private void Update()
        {
            if (CoolTimer <= 0)
            {
                CoolTimer = 0;
                isActive = true;
            }
            else
            {
                CoolTimer -= Time.deltaTime;
            }
        }

        public override void Use()
        {
            if (CoolTimer > 0)
            {
                return;
            }

            isActive = false;
            base.Use();
            player.anim.Attack();
            player.UsingSkill(true);
            point = PlayManager.Instance.MousePositionNoClick(transform.position);
            transform.position = point;
            CoolTimer = CoolTime;
            StartCoroutine(PlayManager.Instance.playerUI.SkillCool(SkillManager.SkillName.Telpo, CoolTime));
            StartCoroutine(TelportActive());
        }
        

        IEnumerator TelportActive()
        {
            float startDelay = 0.2f;



            ParticleSystem particle =  SkillManager.Instance._telpoeffect.GetValue().GetComponent<ParticleSystem>();
                //Instantiate(skillEffect);
            particle.transform.position = player.transform.position;
            particle.Play();

                yield return new WaitForSeconds(startDelay);
            //한계거리를 넘어가면 한계범위 까지만 텔레포트
            float limit = Vector3.SqrMagnitude(point - player.transform.position);

            if (limit > LimitDistance)
            {
                Vector3 newpoint = point + (point - player.transform.position).normalized * LimitDistance;
                player.transform.position = newpoint;
                PlayManager.Instance.SetPoint(newpoint);
                Debug.Log("거리 넘어감"  + Vector3.Distance(player.transform.position, point));
            }
            else
            {
                Debug.Log("거리 사정 범위" + Vector3.Distance(player.transform.position, point));

                player.transform.position = point;
            }
            //player.movement.Move(player.transform.position);
            particle.transform.position = player.transform.position;

            particle.Play();

            yield return new WaitForSeconds(2f);

            SkillManager.Instance._telpoeffect.ReturunValue(particle.gameObject);
        }

        
    }
}
