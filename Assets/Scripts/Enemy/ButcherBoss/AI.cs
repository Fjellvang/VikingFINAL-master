﻿using Assets.Scripts.CombatSystem;
using Assets.Scripts.Player_States;
using Assets.Scripts.States;
using Assets.Scripts.States.EnemyStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public abstract class EntityController : MonoBehaviour { } //TODO: Why this shiet

[RequireComponent(typeof(CharacterController2D))]
public class AI : MonoBehaviour, IAttacker, IAttackable, IStunnable {

    public float moveSpeed = 10f;
    [HideInInspector]
    public float stunnedDuration = 2f;
    [HideInInspector]
    public bool playerVisible = false;
    [HideInInspector]
    public CharacterController2D controller;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public Attack weapon;
    [HideInInspector]
    public Animator Animator;

    [HideInInspector]
    public SpriteRenderer SpriteRenderer;


    public FarmerStateMachine StateMachine;

	private void Awake()
	{
        StateMachine = new FarmerStateMachine(this);
        GetComponent<Health>().OnDeath += OnDeath;
    }

    private void OnDeath()
	{
        StateMachine.TransitionState(StateMachine.deadState);
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
        weapon = GetComponentInChildren<Attack>();
        Animator = GetComponentInChildren<Animator>();
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        controller = GetComponent<CharacterController2D>();
    }
	

	private void FixedUpdate()
	{
        StateMachine.currentState.FixedUpdate(this);
	}

    //TODO: get this in a cleaner manner
    IAttackEffect[] attacks = new[]
    {
        new FarmerAttack()
    };

    public void Attack()
	{
        weapon.DoAttack(attacks);
	}

	// Update is called once per frame
	void Update () {
        StateMachine.currentState.Update(this);
	}

	public void PowerFullAttack()
	{
		throw new System.NotImplementedException();
	}

	public void OnTakeDamage(GameObject attacker, IAttackEffect[] attackEffects)
	{
		for (int i = 0; i < attackEffects.Length; i++)
		{
            attackEffects[i].OnSuccessFullAttack(attacker, gameObject);
		}
	}

	public void Stun(float duration)
	{
        this.stunnedDuration = duration;
        this.StateMachine.TransitionState(StateMachine.stunnedState);
	}
}
