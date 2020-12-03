using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 상태를 나타내는 추상 클래스
/// </summary>
/// <typeparam name="T">상태</typeparam>
public abstract class State<T>
{
    protected StateMachine<T> stateMachine;
    protected T context;

    public State()
    {

    }
    //internal - 같은 어셈블리에서는 public, 외부에서는 private
    //(어셈블리라는 것은 한 프로젝트가 뽑아내는 결과물)
    internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        OnInitialized();
    }

    public virtual void OnInitialized()
    { }
    public virtual void OnEnter()
    { }
    public abstract void Update(float deltaTime);

    public virtual void OnExit()
    { }
}

/// <summary>
/// 상태 표현 클래스 cur, pre 상태를 전환시간 및 여러 상태를 모아두는 dic으로 구성
/// State 추상클래스를 사용하는 클래스
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class StateMachine<T>
{
    private T context;
    private State<T> currentState;
    public State<T> Currentstate => currentState;

    private State<T> previousState;
    public State<T> PreviousState => previousState;

    private float elapsedTimeInstate = 0.0f;
    public float ElapsedTimeInState => elapsedTimeInstate;

    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

    public StateMachine(T context, State<T> initialState)
    {
        this.context = context;

        //초기상태 지정
        AddState(initialState);
        currentState = initialState;
        currentState.OnEnter();
    }
    
    public void AddState(State<T> state)
    {
        state.SetStateMachineAndContext(this, context);
        states[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        elapsedTimeInstate += deltaTime;
        currentState.Update(deltaTime);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);
        if(currentState.GetType() == newType)
        {
            return currentState as R;
        }

        if(currentState != null)
        {
            currentState.OnExit();
        }

        previousState = currentState;
        currentState = states[newType];
        currentState.OnEnter();
        elapsedTimeInstate = 0.0f;

        return currentState as R;
    }

}
