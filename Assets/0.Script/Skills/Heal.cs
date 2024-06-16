using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class Heal : Skill
    {


        public override void Use()
        {
            base.Use();
            
            player.UsingSkill(true);
            //float heal = Damage * 0.7f;
            transform.position = player.transform.position;
            skillEffect.Play();
        }

        public override void OnParticleCollision(GameObject other)
        {
            Player.Player player = other.GetComponent<Player.Player>();
            if(player != null)
            {
                player.Heal(Damage);
                Debug.Log("Èú!!");
            }
        }
    }

}

