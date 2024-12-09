using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
   public float fadeTime = 0.5f;
   private float timeElapsed = 0f;
   SpriteRenderer spriteRenderer;
   GameObject objToRemove;
   Color startColor;
   
   // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      timeElapsed = 0f;
      spriteRenderer = animator.GetComponent<SpriteRenderer>();
      startColor = spriteRenderer.color;
      objToRemove = animator.gameObject;
   }

   // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      timeElapsed += Time.deltaTime;

      float newAlpha = startColor.a * (1 - (timeElapsed / fadeTime));

      spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

      if (timeElapsed > fadeTime) Destroy(objToRemove);
   }
}
