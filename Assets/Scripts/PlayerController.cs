﻿using Assets.Scripts.Player_States;
using Assets.Scripts.States.PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController, IAttack {
	public float acceleration = 10f;

    public Transform axeAttack;
    public float attackRadius = 0.5f;
	public LayerMask enemyMask;
    Lady dame;
    public Lady Dame => dame; //TODO: refacotr

    public Animator anim;
    public Animator Animator { get { return anim; } }
    public bool Blocking;
    AudioSource kissSound;
    public AudioSource KissSound => kissSound;

    public CharacterController2D CharacterController;
    public PlayerHealth health;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public readonly Stack<PlayerBaseState> stateStack = new Stack<PlayerBaseState>();
    PlayerBaseState currentState;

    public void PoplastState()//TODO: Get better naming.
	{
        currentState.OnExitState(this);
        currentState = stateStack.Pop();//TODO: nullcheck ?
        currentState.OnEnterState(this);
	}
    public void TransitionState(PlayerBaseState newState)
	{
        currentState.OnExitState(this);
        stateStack.Push(currentState);
        currentState = newState;
        currentState.OnEnterState(this);
	}


    // Use this for initialization
    void Start () {
		//anim = GetComponentInChildren<Animator> ();
        dame = FindObjectOfType<Lady>();
        kissSound = GetComponent<AudioSource>();
        health = GetComponent<PlayerHealth>();
	}

    public bool OnTakeDamage(Vector2 attackedFrom)
	{
        this.CharacterController.m_Rigidbody2D.AddForce(attackedFrom*4, ForceMode2D.Impulse);
        return currentState.OnTakeDamage(this, attackedFrom);
	}

	private void Awake()
	{
        currentState = PlayerBaseState.idleState;
	}
	private void FixedUpdate()
	{
        currentState.FixedUpdate(this);
	}
	// Update is called once per frame
	void Update() {
        currentState.Update(this);
        //TODO: Refactor, test with fixed update ??
        var rb = CharacterController.m_Rigidbody2D;
		if (rb.velocity.y < 0)
		{
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		} else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

    //TODO: Refactor - it is too similar to enemy attack
    public void Attack()
	{
        var collders = Physics2D.OverlapCircleAll(axeAttack.position, attackRadius, enemyMask);
		for (int i = 0; i < collders.Length; i++)
		{
            var enemy = collders[i];
            var attackDelta = enemy.transform.position - this.transform.position; //could cache transform for micro optimization
            enemy.GetComponent<Health>().TakeDamage(attackDelta);
		}
	}
	private void OnDrawGizmosSelected()
	{
        Gizmos.DrawSphere(axeAttack.position, attackRadius);
	}
}
