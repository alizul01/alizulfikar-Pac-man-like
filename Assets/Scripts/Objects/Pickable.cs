using System;
using UnityEngine;

namespace Objects
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] public PickableType type;
        public Action<Pickable> OnPicked;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnPicked?.Invoke(this);
            Debug.Log($"Player picked up {type}");
            Destroy(gameObject);
        }
    }
}
