using UnityEngine;

namespace Entities.Enemy.State
{
    public class PatrolState: IEnemyBaseState
    {
        public bool IsMoving;
        private Vector3 _destination;
        public void EnterState(EnemyController enemy)
        {
            IsMoving = false;
        }

        public void UpdateState(EnemyController enemy)
        {
            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) <= enemy.chaseDistance)
            {
                enemy.TransitionToState(enemy.ChaseState);
            }
            if (!IsMoving)
            {
                IsMoving = true;
                var randomWaypoint = enemy.waypoints[Random.Range(0, enemy.waypoints.Count)];
                _destination = randomWaypoint.position;
                enemy.navMeshAgent.SetDestination(_destination);
            }
            else
            {
                if (Vector3.Distance(_destination, enemy.transform.position) <= 0.5)
                {
                    IsMoving = false;
                    Debug.Log("Reached destination");
                }
            }
        }

        public void ExitState(EnemyController enemy)
        {
            Debug.Log("Exit Patrol");
        }
    }
}