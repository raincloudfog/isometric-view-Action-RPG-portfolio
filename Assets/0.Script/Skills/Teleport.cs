using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{


    public class Teleport : Skill
    {
        public float LimitDistance = 5f;
        private Vector3 point;



        public override void Use()
        {
            base.Use();
            player.anim.Attack();
            player.UsingSkill(true);
            point = PlayManager.Instance.MousePosition(transform.position);
            transform.position = point;
            StartCoroutine(TelportActive());
        }
        

        IEnumerator TelportActive()
        {
            float startDelay = 0.3f;



            ParticleSystem particle = Instantiate(skillEffect);
            particle.transform.position = player.transform.position;
            particle.Play();

                yield return new WaitForSeconds(startDelay);
            //한계거리를 넘어가면 한계범위 까지만 텔레포트
            if (Vector3.Distance(player.transform.position, point) > LimitDistance)
            {

                player.transform.position = point + (player.transform.position - point).normalized * LimitDistance;
                Debug.Log("거리 넘어감"  + Vector3.Distance(player.transform.position, point));
            }
            else
            {
                Debug.Log("거리 사정 범위" + Vector3.Distance(player.transform.position, point));

                player.transform.position = point;
            }
            player.movement.Move(player.transform.position);
            particle.transform.position = player.transform.position;

            particle.Play();
        }

        
    }
}
