using System;
using System.Collections;
using UnityEngine;

namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float powerUpDuration = 5f;
        private Coroutine _powerUpCoroutine;
        public Action OnPowerUpStart;
        public Action OnPowerUpEnd;

        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }
        
        private void MovePlayer()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var horizontalDirection = cameraTransform.right * horizontal;
            var verticalDirection = cameraTransform.forward * vertical;

            // komponen Y dijadiin 0 biar gaada pergerakan vertikal
            verticalDirection.y = 0f;
            horizontalDirection.y = 0f;

            Vector3 direction = (horizontalDirection + verticalDirection).normalized;
            _rigidbody.velocity = direction * speed * Time.fixedDeltaTime;
        }

        public void PickPowerUp()
        {
            if (_powerUpCoroutine != null)
            {
                StopCoroutine(_powerUpCoroutine);
            }
            
            _powerUpCoroutine = StartCoroutine(StartPowerUp());
        }

        private IEnumerator StartPowerUp()
        {
            OnPowerUpStart?.Invoke();
            yield return new WaitForSeconds(powerUpDuration);
            OnPowerUpEnd?.Invoke();
        }
    }
}