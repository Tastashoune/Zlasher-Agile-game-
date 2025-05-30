
namespace MyInterface
{
    public enum EnemyType
    {
        Citizen,        // 0
        Policeman,      // 1
        Spearman,       // 2
        Ufo             // 3
    }
    public enum EnemyState
    {
        Walking,        // 0
        Attacking,      // 1
        Flying          // 2
    }
    public enum EnemyAttack
    {
        NoAttack,       // 0
        MeleeAttack,    // 1
        RangedAttack    // 2
    }
    public interface IEnemyInterface
    {
        void Shoot();
        void Fly();
        void DropHead();
        void Die();
    }
}