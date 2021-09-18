using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AttackScoreController : MonoBehaviour
{
    [SerializeField] public Direction4 direction4;
    [SerializeField] private Color defaultColor = Color.black;
    private Text text;
    private List<AttackScoreColor> attackScoreColors;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void SetAttackScoreColors( List<AttackScoreColor>  colors)
    {
        attackScoreColors = colors;
    }
    // Start is called before the first frame update
    public void SetAttackScore(int attackPoints)
    {
        text.color = defaultColor;
        foreach (var score in attackScoreColors)
        {
            if (attackPoints <= score.highestScore)
            {
                text.color = score.color;
                break;

            }
        }
        text.text = attackPoints.ToString();
    }
}

[Serializable]
public class AttackScoreColor
{
    public int highestScore;
    public Color color;

    public static int CompareByScore(AttackScoreColor asc1, AttackScoreColor asc2)
    {
        return asc1.highestScore.CompareTo(asc2.highestScore);
    }
}
