using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    using Monster;
    public class GroundHit : MonoBehaviour
    {
        public ParticleSystem particle;
        [SerializeField]
        int damage;

        [SerializeField]
        float radius;

        public LayerMask TargetLayer;

        public void Attack()
        {
            Debug.Log("어택!!");
            Collider[] collider = Physics.OverlapSphere(transform.position, radius, TargetLayer);
            foreach (Collider other in collider)
            {
                Debug.Log(other);
                other.GetComponent<Monster>().Hit(damage);
            }

            particle.Play();
        }

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, radius);
        }

        public void OnParticleCollision(GameObject other)
        {
            /*if (other != null)
            {
                
                if (other.CompareTag("Enemy"))
                {
                    Debug.Log("땅 치기 맞은 대상 " + other.name);
                    Monster enemy = other.GetComponent<Monster>();
                    enemy.Hit(damage);
                }
            }*/
        }

    }

}
