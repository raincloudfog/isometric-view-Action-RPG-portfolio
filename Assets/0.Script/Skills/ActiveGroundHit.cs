using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Skills
{


    public class ActiveGroundHit : Skill
    {
        public GameObject GroundHitObj;
        public Transform[] HitPoint;

        public GroundHit[] groundHits;
         

        public override void Use()
        {
            base.Use();
            player.anim.Attack();
            player.UsingSkill(true);
            transform.position = player.transform.position;
            transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles) ;
            StartCoroutine(Skill());
        }

        public override void SetSkill()
        {
            base.SetSkill();
            if(groundHits.Length == 0)
            {
                groundHits = new GroundHit[HitPoint.Length];
            }


            for (int i = 0; i < HitPoint.Length; i++)
            {
                GameObject effect = Instantiate(GroundHitObj, transform);

                effect.transform.position = HitPoint[i].position;

                groundHits[i] = effect.GetComponent<GroundHit>();
                if (groundHits[i] != null)
                {
                    groundHits[i].SetDamage(Damage);

                }
            }            

        }
       /* private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(Skill());
            }
        }*/
        


        IEnumerator Skill()
        {
            float HitTime = 0.3f;
            int Number = 0;
            while (Number < 3)
            {
                groundHits[Number].transform.position = HitPoint[Number].position;
                groundHits[Number].Attack();
                yield return new WaitForSeconds(HitTime);

                Number++;
            }
        }
    }
}