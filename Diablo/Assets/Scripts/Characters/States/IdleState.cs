using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditorInternal;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    bool isPatrol = false;
    private float minIdleTime = 0.0f;
    private float maxIdleTime = 3.0f;
    private float idleTime = 0.0f;

    private Animator animator;
    private CharacterController controller;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
    }

    public override void OnEnter()
    {
        //? -> Nullable
        animator?.SetBool(hashMove, false);
        animator.SetFloat(hashMoveSpeed, 0);
        controller?.Move(Vector3.zero);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy(); //대기 상태에서 적 찾기
        if (enemy) //적을 발견했다.
        {
            if (context.IsAvailableAttack) //공격가능한가(거리)
            {
                // check attack cool time
                // and transition to attack state
                stateMachine.ChangeState<AttackState>();
            }
            else
            {
                stateMachine.ChangeState<MoveState>();
            }
        }
        else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)
        {
            stateMachine.ChangeState<MoveToWaypointState>();
        }

    }

    public override void OnExit()
    {
    }
}
