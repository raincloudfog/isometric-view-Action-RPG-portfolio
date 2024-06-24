using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    
    public NavMeshAgent agent;

    private bool _onNavMeshLink = false;

    [SerializeField]
    private float _jumpDuration = 0.8f;

    public UnityEvent OnLand, OnStartJump;
    

    public void Init(float speed)
    {
        //오토메쉬링크 끔
        agent.autoTraverseOffMeshLink = false;
        agent.speed = speed;
    }
    
    public void Move(Vector3 Point)
    {
        if (_onNavMeshLink)
            return;
        agent.destination = Point;
    }

    private void Update()
    {
        if (agent.isOnOffMeshLink
            && _onNavMeshLink == false)
        {
            StartNavMeshLinkMovemnet();
        }
        if (_onNavMeshLink)
        {
            FaceTarget(agent.currentOffMeshLinkData.endPos);
        }
    }

    public void MovePosition(Vector3 point)
    {

        StartCoroutine(movePosition(point));
    }

    IEnumerator movePosition(Vector3 point)
    {        
        agent.enabled = false;

        while (transform.position != point || Vector3.Distance(transform.position, point) > 1)
        {
            transform.position = point;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("위치이동했음.");
        PlayManager.Instance.SetPoint(point);
        agent.enabled = true;
    }


    public void StartNavMeshLinkMovemnet()
    {
        _onNavMeshLink = true;
        NavMeshLink link = (NavMeshLink)agent.navMeshOwner;
        Spline spline = link.GetComponentInChildren<Spline>();

        PerformJump(link, spline);
    }

    private void PerformJump(NavMeshLink link, Spline spline)
    {
        bool reverseDirection = CheckIfJumpingFromendToStart(link);
        StartCoroutine(MoveOnOffMeshLink(spline, reverseDirection));

        OnStartJump?.Invoke();

    }

    private bool CheckIfJumpingFromendToStart(NavMeshLink link)
    {
        Vector3 startPosWorld
            = link.gameObject.transform.TransformPoint(link.startPoint);
        Vector3 endPosWorld
            = link.gameObject.transform.TransformPoint(link.endPoint);

        float distancePlayerToStart
            = Vector3.Distance(agent.transform.position, startPosWorld);
        float distancePlayerToEnd
            = Vector3.Distance(agent.transform.position, endPosWorld);

        return distancePlayerToStart > distancePlayerToEnd;
    }

    private IEnumerator MoveOnOffMeshLink(Spline spline , bool reverseDirection)
    {
        float currenttime = 0;
        Vector3 agentStartPosition = agent.transform.position;

        while(currenttime < _jumpDuration)
        {
            currenttime += Time.deltaTime;

            float amount = Mathf.Clamp01(currenttime / _jumpDuration);
            amount = reverseDirection? 1- amount : amount;

            agent.transform.position = 
                reverseDirection ? 
                spline.CalculatePositionCustomEnd(amount, agentStartPosition) :
                spline.CalculatePositionCstomStart(amount, agentStartPosition);

            yield return new WaitForEndOfFrame();
        }
        agent.CompleteOffMeshLink();

        OnLand?.Invoke();
        yield return new WaitForSeconds(0.1f);
        _onNavMeshLink = false;

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

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));

        transform.rotation =
            Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);

    }
}
