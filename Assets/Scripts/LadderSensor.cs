using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderSensor : MonoBehaviour
{
    // ��ٸ� �浹 / ������ �̺�Ʈ
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
