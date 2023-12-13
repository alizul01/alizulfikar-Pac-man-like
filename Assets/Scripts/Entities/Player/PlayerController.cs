using System;
using System.Collections;
using Entities.Enemy;
using Managers;
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
        [SerializeField] private GameObject playerModel;
        [SerializeField] private AudioClip[] sfxClips;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] public MainMenuManager mainMenuManager;

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

            // player rotation follow camera
            if (direction == Vector3.zero) return;
            var toRotation = Quaternion.LookRotation(direction);
            toRotation.eulerAngles = new Vector3(-90, toRotation.eulerAngles.y, 87);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, toRotation,
                Time.fixedDeltaTime * 5f);
        }

        public void PickPowerUp()
        {
            Debug.Log("PowerUp Picked");
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
                audioSource.PlayOneShot(sfxClips[1]);
                other.gameObject.GetComponent<EnemyController>().Dead();
            }
            else
            {
                Dead();
            }
        }

        public void Dead()
        {
            audioSource.PlayOneShot(sfxClips[0]);
            transform.position = spawnPoint.position;
            health--;
            UpdateUI();
            if (health <= 0)
            {
                mainMenuManager.Play("Lose");
            }
        }
    }
}