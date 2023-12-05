using System;
using System.Collections;
using UnityEngine;

namespace Entities.Enemy.State
{
    public class RetreatState: IEnemyBaseState
    {
        private static readonly int RetreatState1 = Animator.StringToHash("RetreatState");

        public void EnterState(EnemyController enemy)
        {
            Debug.Log("Start Retreat");
            enemy.animator.SetTrigger(RetreatState1);
        }

        public void UpdateState(EnemyController enemy)
        {
            if (enemy.player != null)
            {
                enemy.navMeshAgent.destination = enemy.transform.position - enemy.player.transform.position;
            }
        }

        public void ExitState(EnemyController enemy)
        {
            Debug.Log("Exit Retreat");
        }
    }
}