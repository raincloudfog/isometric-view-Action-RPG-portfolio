using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{    
    private bool isPortal;
    

    public bool IsPortal
    {
        get { return isPortal; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameStateManager.isPortal = true;
            isPortal = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameStateManager.isPortal = false;
            isPortal = false;

        }
    }

    

}
