using System;
using UnityEngine;

namespace Objects
{
    public class Pickable : MonoBehaviour
    {
        [SerializeField] public PickableType type;
        [SerializeField] private new GameObject particleSystem;
        public Action<Pickable> OnPicked;
        public AudioClip audioClip;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            audioSource = GameObject.FindWithTag("Audio").GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("<color=red>AudioSource not found!</color>");
            } 
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnPicked?.Invoke(this);
            audioSource.PlayOneShot(audioClip);
            particleSystem.SetActive(true);
            particleSystem.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }
}
