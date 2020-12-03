using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// EnemyController를 상속받아서 Enemy의 상태머신을 구현하는 클래스
/// </summary>
public class EnemyController_StateMachine : EnemyController
{
    #region NewVariables
    protected StateMachine<EnemyController> stateMachine;

    public LayerMask targetMask;
    public Collider weaponCollider;

    public GameObject hitEffetct;

    #endregion NewVariables

    #region Unity Methods
    protected override void Start()
    {
        base.Start();

        //각 적마다 대기, 이동, 공격 상태를 가지며 각 상태에 따라서 행동

        //최초 대기 상태로 상태머신 생성
        stateMachine = new StateMachine<EnemyController>(this, new IdleState());

        //이동, 공격 상태 추가
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
    }

    void Update()
    {
        stateMachine.Update(Time.deltaTime);

        if (!(stateMachine.CurrentState is MoveState))
        {
            FaceTarget();
        }
    }

    #endregion Unity Methods
}
