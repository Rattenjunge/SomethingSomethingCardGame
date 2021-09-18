using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Creature", menuName = "SomethingSomethingCardGame/Creature Card")]
public class CreatureCard : PlayableCard {
	public int NorthAttack;
	public int EastAttack;
	public int SouthAttack;
	public int WestAttack;

	public int GetAttackPointsByDirection(Direction4 direction4)
	{
		switch (direction4)
		{
			case Direction4.North:
				return this.NorthAttack;
			case Direction4.South:
				return SouthAttack;
			case Direction4.East:
				return EastAttack;
			case Direction4.West:
				return WestAttack;
			default:
				throw new ArgumentOutOfRangeException(nameof(direction4), direction4, null);
		}
	}
}
