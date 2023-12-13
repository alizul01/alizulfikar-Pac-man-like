using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    private void Update()
    {
        Transform player;
        (player = transform).Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        var position = player.position;
        position = new Vector3(position.x, Mathf.PingPong(Time.time, 0.5f) + 1, position.z);
        transform.position = position;
    }
}
