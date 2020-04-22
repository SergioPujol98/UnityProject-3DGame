using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanStateControllerSecondPlayer : MonoBehaviour
{

    public static float horizontal1, vertical1, run, attack;
    private float health = 100f;
    private bool hasHorizontalInput, hasVerticalInput;
    public static bool isWalking, isRunning, isAttacking;
    public enum footmanStates { idle = 0, walk, run, attack, attack2, defend, death, getHit, victory };
    public delegate void footmanStateHandler(FootmanStateControllerSecondPlayer.footmanStates newState);
    public static event footmanStateHandler onStateChange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontal1 = Input.GetAxis("Horizontal1");
        vertical1 = Input.GetAxis("Vertical1");
        run = Input.GetAxis("Run");
        attack = Input.GetAxis("Attack1");
        isAttacking = !Mathf.Approximately(attack, 0f);
        isRunning = !Mathf.Approximately(run, 0f);
        hasHorizontalInput = !Mathf.Approximately(horizontal1, 0f);
        hasVerticalInput = !Mathf.Approximately(vertical1, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;
        //animator.SetBool("IsWalking", isWalking);


        if (!isWalking && !isRunning && !isAttacking)
        {
            onStateChange(footmanStates.idle);

        }

        if (isWalking && !isRunning)
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


        Debug.Log(isAttacking + " isAttacking " + isWalking + " iswalking " + isRunning + " isRunning");

    }

}
