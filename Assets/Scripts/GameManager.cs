using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerHealth playerHealth;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
   
    public void GameOver()
    {
        Debug.Log("게임오버");
        StopCamera();
    }

    public void StopCamera()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_XDamping = 5f;
        transposer.m_YDamping = 5f;
        virtualCamera.Follow = null;
    }
}
