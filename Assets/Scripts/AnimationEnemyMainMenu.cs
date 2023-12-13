using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnemyMainMenu : MonoBehaviour
{
    // animasi naik turun enemy
    
    public float speed = 1f;
    public float height = 0.5f;
    private Vector3 pos;
    
    void Start()
    {
        pos = transform.position;
    }
    
    void Update()
    {
        transform.position = pos + Vector3.up * (Mathf.Sin(Time.time * speed) * height);
    }
}
