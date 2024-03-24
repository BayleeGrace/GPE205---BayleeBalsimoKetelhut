using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject targetPlayer;
    private Transform targetTransform;
    [HideInInspector] public GameObject playerCamera;
    public Vector3 offset;
    
    public void Start()
    {

    }
    public void FixedUpdate()
    {

        if (GameManager.instance != null)
        {
            if (GameManager.instance.players != null)
            {
                foreach (var player in GameManager.instance.players)
                {
                    targetPlayer = player.pawn.gameObject;
                    playerCamera = GameManager.instance.newCamera;
                }
            }
        }
            targetTransform = targetPlayer.transform;

            if (playerCamera != null)
            {
                playerCamera.transform.LookAt(targetTransform);
                playerCamera.transform.position = targetPlayer.transform.position + offset;
                //playerCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
    }
}
