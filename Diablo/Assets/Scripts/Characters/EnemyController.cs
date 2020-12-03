using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Variables
    public float viewRadius = 5f; //시야 거리
    public float attackRange = 1.5f; //공격 거리
    public float CalcAttackRange => attackRange + 0.5f;

    protected Transform target;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected CharacterController controller;

    protected int hashMoveSpeed = Animator.StringToHash("Move");
    protected int hashAttack = Animator.StringToHash("Attack");
    protected int hasAttackIndex = Animator.StringToHash("AttackIndex");

    #endregion Variables
    #region Unity Methods
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.updatePosition = false;
        agent.updateRotation = true;

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (target)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= viewRadius)
            {
                agent.SetDestination(target.position);
            }

            if (agent.remainingDistance > agent.stoppingDistance)
            {
                controller.Move(agent.velocity * Time.deltaTime);
            }
            else
            {
                controller.Move(Vector3.zero);
            }

            animator.SetFloat(hashMoveSpeed, agent.velocity.magnitude / agent.speed, .1f, Time.deltaTime);

            if (distance <= agent.stoppingDistance)
            {
                // Attack the target
                animator.SetTrigger(hashAttack);

                // Face the target
                FaceTarget();
            }
            else
            {
            }
        }
        
    }

    void FaceTarget()
    {
        //현재 위치에서 타켓 방향으로 향하는 방향벡터
        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    private void OnAnimatorMove()
    {
        Vector3 position = agent.nextPosition;
        animator.rootPosition = agent.nextPosition;
        transform.position = position;
    }
    #endregion Unity Methods

    #region Helper Methods
    public virtual bool IsAvailableAttack => false; //공격 가능한가를 나타내는 변수

    public virtual Transform SearchEnemy()
    {
        return null;
    }
    #endregion Helper Methods
}
