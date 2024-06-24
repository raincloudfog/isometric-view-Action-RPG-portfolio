using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem _attackParticle;
    public Player.Player player;

    float attackRange = 3.0f;
    float attackAngle = 45.0f;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Player.Player p)
    {
        player = p;
    }

    public void Attack()
    {
        //_attackParticle.Play();

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 3);

        foreach (Collider target in colliders)
        {
            if(target.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = (target.transform.position - player.transform.position).normalized;

                float angleToTarget = Vector3.Angle(player.transform.forward, directionToTarget);

                if(angleToTarget <= attackAngle)
                {
                    /*GameObject hitparticle = PlayManager.Instance._hitParticlePool.GetValue();
                    hitparticle.transform.position = target.transform.position;
                    hitparticle.GetComponent<ParticleSystem>().Play();*/
                    target.GetComponent<Monster.Monster>().Hit(player.stat.Damage);                    
                }
            }
        }
    }    
    
}


/* 만약 인보크를 사용하고 싶을때
 * public class CustomInvoker : MonoBehaviour
{
    public void CustomInvoke(System.Action method, float delay)
    {
        StartCoroutine(InvokeCoroutine(method, delay));
    }

    private IEnumerator InvokeCoroutine(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);
        method?.Invoke();
    }

    public void CustomInvoke<T>(System.Action<T> method, T param, float delay)
    {
        StartCoroutine(InvokeCoroutine(method, param, delay));
    }

    private IEnumerator InvokeCoroutine<T>(System.Action<T> method, T param, float delay)
    {
        yield return new WaitForSeconds(delay);
        method?.Invoke(param);
    }
}
 */