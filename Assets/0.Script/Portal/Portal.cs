using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{    
    public bool isPortal;

    [SerializeField]
    private bool isBossroomportal = false;

    [SerializeField]
    Transform _start, _end;

    public Transform EndPos
    {
        get
        {
            return _end;
        }
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if(isBossroomportal)
            {
                GameStateManager.isBossroomPortal = true;
                isPortal = true;
            }
            else
            {
                GameStateManager.isPortal = true;
                isPortal = true;
            }
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(isBossroomportal)
            {
                GameStateManager.isBossroomPortal = false;
                isPortal = false;
            }
            else
            {
                GameStateManager.isPortal = false;
                isPortal = false;
            }
            

        }
    }

    private void OnDrawGizmos()
    {
        if(_start != null && _end != null) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_start.position, 0.5f);
            Gizmos.DrawSphere(_end.position, 0.5f);
            
        }
    }

}
