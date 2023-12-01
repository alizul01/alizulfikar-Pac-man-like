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
            
            Debug.Log("Pickable list initialized with " + pickableList.Count + " items.");
        }

        private void OnPickablePicked(Pickable pickable)
        {
            pickableList.Remove(pickable);
            if (pickable.type == PickableType.PowerUp)
            {
                player.PickPowerUp();
            }
            if (pickableList.Count > 0) return;
        }
    }
}
