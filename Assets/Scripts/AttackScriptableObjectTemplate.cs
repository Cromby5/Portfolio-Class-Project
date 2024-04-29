using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    // What hitbox to enable, this is dumb
    LeftPunch,
    RightPunch,
    LeftKick,
    RightKick,
}

[CreateAssetMenu()]
public class AttackScriptableObjectTemplate : ScriptableObject
{
   public AnimatorOverrideController animatorOverride; // the animaton 
   public AttackType attackType; 

   public float attackSpeed = 1f; // To change the animator speed
   public float attackTime = 1f; // How long the attack lasts for, used for timing
   public float animationExit = 0.9f; // When to exit the animation, used for timing
}
