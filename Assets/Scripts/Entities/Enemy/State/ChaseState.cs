using UnityEngine;

namespace Entities.Enemy.State
{
    public class ChaseState: IEnemyBaseState
    {
        public void EnterState(EnemyController enemy)
        {
            Debug.Log("Start Chase");
        }

        public void UpdateState(EnemyController enemy)
        {
            if (enemy.player == null) return;
            
            enemy.navMeshAgent.SetDestination(enemy.player.transform.position);
            
            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) > enemy.chaseDistance)
            {
                enemy.TransitionToState(enemy.PatrolState);
            }
        }

        public void ExitState(EnemyController enemy)
        {
            Debug.Log("Exit Chase");
        }
    }
}