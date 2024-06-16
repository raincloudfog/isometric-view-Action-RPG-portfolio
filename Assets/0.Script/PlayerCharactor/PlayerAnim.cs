using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerAnim : MonoBehaviour
    {
        /*Helmet = 0,
        ChestArmor ,
        LegArmor,
        Boots,*/

        public Animator anim;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }        

        public void Move(float speed)
        {
            anim.SetFloat("speed", speed);
        }

        public void Attack()
        {
            anim.SetTrigger("attack");
        }

        public void Death()
        {
            anim.SetTrigger("death");
        }
    }
}