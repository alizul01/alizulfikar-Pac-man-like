﻿using System;
using System.Collections.Generic;
using Entities.Enemy.State;
using Entities.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Entities.Enemy
{
    public class EnemyController: MonoBehaviour
    {
        // getter and setter for waypoints
        [field:Header("Enemy Properties")]
        public List<Transform> waypoints { get; set; } = new List<Transform>();

        private IEnemyBaseState _currentState;
        
        public PatrolState PatrolState = new PatrolState();
        public ChaseState ChaseState = new ChaseState();
        public RetreatState RetreatChaseState = new RetreatState();
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        
        [SerializeField] public float chaseDistance = 10f;
        [SerializeField] public PlayerController player;

        private void Awake()
        {
            _currentState = PatrolState;
            _currentState.EnterState(this);
            navMeshAgent = GetComponent<NavMeshAgent>();

            InitializeWaypoints();
        }

        private void Start()
        {
            Debug.Log("Player reference: " + player);
            player.OnPowerUpStart += StartRetreat;
            player.OnPowerUpEnd += EndRetreat;
        }


        private void InitializeWaypoints()
        {
            var waypointsTransform = GameObject.FindGameObjectsWithTag("Waypoint");
            foreach (var waypointTransform in waypointsTransform)
            {
                waypoints.Add(waypointTransform.transform);
            }
        }
        
        public void TransitionToState(IEnemyBaseState state)
        {
            if (_currentState == null) return;
            _currentState.ExitState(this);
            _currentState = state;
            _currentState.EnterState(this);
        }

        private void FixedUpdate()
        {
            _currentState?.UpdateState(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var waypoint in waypoints)
            {
                Gizmos.DrawSphere(waypoint.position, 0.5f);
            }
        }
        
        private void StartRetreat()
        {
            Debug.Log("Start Retreat");
            TransitionToState(RetreatChaseState);
        }

        private void EndRetreat()
        {
            Debug.Log("End Retreat");
            TransitionToState(PatrolState);
        }

        
        public void Dead()
        {
            _currentState.ExitState(this);
            _currentState = null;
            Destroy(gameObject);
        }
    }
}