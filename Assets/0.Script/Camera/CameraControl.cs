using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    CinemachineBrain cameraBrain;


    public Vector3 camOffset = new Vector3(0,28,0);
    public Transform playerTransform;
    public CinemachineVirtualCamera playervirCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        playervirCam.transform.position = Vector3.Lerp(playervirCam.transform.position, playerTransform.position + camOffset, 0.7f);
        playervirCam.transform.position = playerTransform.position + camOffset;
    }
}
