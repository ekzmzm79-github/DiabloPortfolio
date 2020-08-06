using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomStateMachineBehaviour : StateMachineBehaviour
{

    #region Variables
    public int numberOfStates = 3; //애니메이션 개수
    public float minNormTime = 0f; 
    public float maxNormTime = 5f;

    public float randomNormalTime;

    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");
    #endregion Variables

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //트랜지션에 필요한 시간을 랜덤하게 결정
        randomNormalTime = Random.Range(minNormTime, maxNormTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //만약 베이스 레이어에 있는 상황이라면 -1로 초기화해서 아무런 랜덤idle 실행안함
        if(animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        {
            animator.SetInteger(hashRandomIdle, -1);
            
        }

        //베이스 레이어에 있는 상황이 아니고 현재 재생중인 애니메이션이 randomNormalTime보다 크다면?
        //-> 해당 idle 애니메이션 재생 시간을 초과했으므로 교체해야함
        if (!animator.IsInTransition(0) && stateInfo.normalizedTime > randomNormalTime)
        {
            animator.SetInteger(hashRandomIdle, Random.Range(0, numberOfStates));
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
