using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    
    public NavMeshAgent agent;



    public void Init(float speed)
    {
        agent.speed = speed;
    }
    
    public void Move(Vector3 Point)
    {
        agent.destination = Point;
    }

    public void StopMove(bool isStop)
    {
        agent.isStopped = isStop;
    }

    public void Rotation(Vector3 point)
    {
        Vector3 direction = (point - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
    }
}
