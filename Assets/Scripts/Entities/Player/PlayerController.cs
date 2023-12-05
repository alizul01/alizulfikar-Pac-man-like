using System;
using System.Collections;
using Entities.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Properties")] [SerializeField]
        private float speed = 100f;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float powerUpDuration = 5f;

        #region Private Variables

        private Coroutine _powerUpCoroutine;
        private bool _isPowerUpActive;
        private Rigidbody _rigidbody;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private int health;

        #endregion

        #region Public Events

        public Action OnPowerUpStart;
        public Action OnPowerUpEnd;

        #endregion

        #region UI Events

        [SerializeField] private TMP_Text healthText;

        #endregion

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UpdateUI();
        }

        private void UpdateUI()
        {
            healthText.text = "Health: " + health;
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
            _isPowerUpActive = true;
            OnPowerUpStart?.Invoke();
            yield return new WaitForSeconds(powerUpDuration);
            _isPowerUpActive = false;
            OnPowerUpEnd?.Invoke();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy")) return;
            if (_isPowerUpActive)
            {
                other.gameObject.GetComponent<EnemyController>().Dead();
            }
            else
            {
                Dead();
            }
        }

        public void Dead()
        {
            transform.position = spawnPoint.position;
            health--;
            UpdateUI();
            if (health <= 0)
            {
                Debug.Log("Game Over");
            }
        }
    }
}