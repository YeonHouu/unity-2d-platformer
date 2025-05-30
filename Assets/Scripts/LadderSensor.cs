using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSensor : MonoBehaviour
{
    // 사다리 충돌 / 떨어짐 이벤트
    public Action<GameObject> OnEnter;
    public Action<GameObject> OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExit?.Invoke(collision.gameObject);
    }
}
