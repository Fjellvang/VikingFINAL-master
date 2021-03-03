﻿using Assets.Scripts.Enemy.ButcherBoss;
using Assets.Scripts.Player_States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.States.EnemyStates.ButcherBossStates
{
	public abstract class ButcherBossBaseState : BaseState<ButcherBossController>
	{
		protected float directionalForce = 0;
		//TODO: This is identical to to enemy base state.... Refactor.
		public override void FixedUpdate(ButcherBossController controller)
		{
			controller.controller.Move(directionalForce * Time.fixedDeltaTime, false, false);
		}

	}
}