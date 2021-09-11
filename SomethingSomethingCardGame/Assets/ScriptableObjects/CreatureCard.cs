using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Creature", menuName = "SomethingSomethingCardGame/CreatureCard")]
public class CreatureCard : ScriptableObject {
	public int Id;
	public string Name;
	public Sprite FaceImage;
	public string FlavourText;

	public int NorthAttack;
	public int EastAttack;
	public int SouthAttack;
	public int WestAttack;
}
