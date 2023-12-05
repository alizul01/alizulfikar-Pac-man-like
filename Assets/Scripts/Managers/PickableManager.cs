using System.Collections.Generic;
using Entities.Player;
using Objects;
using UnityEngine;

namespace Managers
{
    public class PickableManager : MonoBehaviour
    {
        [Header("Pickable List")]
        [SerializeField] private List<Pickable> pickableList = new List<Pickable>();
        [SerializeField] private PlayerController player;
        [SerializeField] private ScoreManager scoreManager;
        private void Start()
        {
            InitPickableList();
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        private void InitPickableList()
        {
            var pickables = GameObject.FindObjectsOfType<Pickable>();
            foreach (var pickable in pickables)
            {
                pickableList.Add(pickable);
                pickable.OnPicked += OnPickablePicked;
            }
            
            scoreManager.SetMaxScore(pickableList.Count);
        }

        private void OnPickablePicked(Pickable pickable)
        {
            pickableList.Remove(pickable);
            scoreManager.AddScore(1);
            if (pickable.type == PickableType.PowerUp)
            {
                player.PickPowerUp();
            }
            if (pickableList.Count > 0) return;
        }
    }
}
