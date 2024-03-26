using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int CurrentHP { get; set; }
    public int AttackDamage { get; set; }
    public int AttackSpeed { get; set; }
    public int MovSpeed { get; set; }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        Debug.Log(CurrentHP);
    }

    /* public void TakeDamage(int damage, bool clamp)
    {
        if (clamp)
        {
            CurrentHP = Math.Max(0, CurrentHP);
        }

        else
        {
            TakeDamage(damage);
        }
    }*/
}
