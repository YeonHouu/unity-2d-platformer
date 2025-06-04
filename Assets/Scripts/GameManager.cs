using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerHealth playerHealth;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
            return;
        }

        playerHealth = GetComponent<PlayerHealth>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
   
    public void GameOver()
    {
        Debug.Log("���ӿ���");
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
