using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanStateController : MonoBehaviour
{
   
    public static float horizontal,vertical; 
    private float health = 100f, run, attack1,attack22,defense;
    private bool hasHorizontalInput, hasVerticalInput;
    public static bool isWalking,isRunning,isAttacking,isDefensing,isAttacking2;
    public enum footmanStates { idle = 0, walk, run, attack, attack2, defend, death, getHit, victory };
    public delegate void footmanStateHandler(FootmanStateController.footmanStates newState);
    public static event footmanStateHandler onStateChange;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        run = Input.GetAxis("Run");
        attack1 = Input.GetAxis("Attack1");
        defense = Input.GetAxis("Defense");
        attack22 = Input.GetAxis("Attack2");
        isAttacking = !Mathf.Approximately(attack1, 0f);
        isRunning = !Mathf.Approximately(run, 0f);
        isDefensing = !Mathf.Approximately(defense, 0f);
        isAttacking2= !Mathf.Approximately(attack22, 0f);
        hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
         hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;
       
        //animator.SetBool("IsWalking", isWalking);

        Debug.Log(isDefensing);
        if (!isWalking && !isRunning && !isAttacking&&!isDefensing&&!isAttacking2)
          {
              onStateChange(footmanStates.idle);
          }
       
        if (isWalking&&!isRunning)
        {
            onStateChange(footmanStates.walk);
        }
        if (isRunning)
        {
            isWalking = false;
            onStateChange(footmanStates.run);
        }
        if (isAttacking)
        {
            isWalking = false;
            onStateChange(footmanStates.attack);
        }
        if (isAttacking2)
        {
            isWalking = false;
            onStateChange(footmanStates.attack2);
        }
        if (isDefensing)
        {
            isWalking = false;
            onStateChange(footmanStates.defend);
        }
       // Debug.Log(isAttacking + " isAttacking "+isWalking + " iswalking " + isRunning+" isRunning");

    }
  
}
