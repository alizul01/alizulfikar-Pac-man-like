namespace Entities.Enemy
{
    public interface IEnemyBaseState
    {
        public void EnterState(EnemyController enemy);
        public void UpdateState(EnemyController enemy);
        public void ExitState(EnemyController enemy);
    }
}