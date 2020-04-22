using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanStateListenerSecondPlayer : MonoBehaviour
{
    private Animator footmanAnimator;
    private new Rigidbody rigidbody;
    private FootmanStateControllerSecondPlayer.footmanStates previousState = FootmanStateControllerSecondPlayer.footmanStates.idle;
    private FootmanStateControllerSecondPlayer.footmanStates currentState = FootmanStateControllerSecondPlayer.footmanStates.idle;
    public float turnSpeed = 10f, speedMovement = 2f, SpeedMovementRun = 3f;
    private Vector3 movement, desiredForward;
    private Quaternion rotation = Quaternion.identity;

    void OnEnable()
    {
        FootmanStateControllerSecondPlayer.onStateChange += onStateChange;
    }
    //Aquest mètode de MonoBehaviour s'executa cada vegada que es desactiva l'objecte associat a l'script.
    //Es deixa d'escoltar l'event onStateChange
    void OnDisable() { FootmanStateControllerSecondPlayer.onStateChange -= onStateChange; }
    void Start()
    {
        footmanAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();


    }
    void FixedUpdate()
    {
        onStateCycle();
        // Debug.Log(movement);
    }

    void onStateCycle()
    {
        switch (currentState)
        {
            case FootmanStateControllerSecondPlayer.footmanStates.idle:

                break;
            case FootmanStateControllerSecondPlayer.footmanStates.walk:
                movement.Set(FootmanStateControllerSecondPlayer.horizontal1, 0f, FootmanStateControllerSecondPlayer.vertical1);
                movement.Normalize();
                desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
                rotation = Quaternion.LookRotation(desiredForward);

                break;
            case FootmanStateControllerSecondPlayer.footmanStates.run:
                movement.Set(FootmanStateControllerSecondPlayer.horizontal1, 0f, FootmanStateControllerSecondPlayer.vertical1);
                movement.Normalize();
                desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
                rotation = Quaternion.LookRotation(desiredForward);
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack:
                /*Debug.Log(FootmanStateController.isWalking + " dentro del footmanstatesAttack");
                if (FootmanStateController.isWalking)
                {

                }*/
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack2:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.defend:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.death:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.getHit:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.victory:
                break;
        }
    }

    public void onStateChange(FootmanStateControllerSecondPlayer.footmanStates newState)
    {
        // Si l'estat actual i el nou són el mateix, no cal fer res 
        if (newState == currentState) return;
        // Comprovar que no hi hagi condicions per abortar l'estat 
        if (checkIfAbortOnStateCondition(newState)) return;

        // Comprovar que el pas de l'estat actual al nou estat està permès. Si no ho està, no es continua. 
        if (!checkForValidStatePair(newState)) return;
        // Realitzar les accions necessàries en cada cas per canviar l'estat.
        switch (newState)
        {

            case FootmanStateControllerSecondPlayer.footmanStates.idle:
                footmanAnimator.SetBool("IsWalking", false);
                footmanAnimator.SetBool("IsRunning", false);
                footmanAnimator.SetBool("IsIdle", true);
                footmanAnimator.SetBool("IsAttacking", false);
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.walk:
                footmanAnimator.SetBool("IsWalking", true);
                footmanAnimator.SetBool("IsRunning", false);
                footmanAnimator.SetBool("IsAttacking", false);
                footmanAnimator.SetBool("IsIdle", false);

                break;
            case FootmanStateControllerSecondPlayer.footmanStates.run:
                footmanAnimator.SetBool("IsRunning", true);
                footmanAnimator.SetBool("IsWalking", false);
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack:
                footmanAnimator.SetBool("IsAttacking", true);
                footmanAnimator.SetBool("IsWalking", false);

                /* footmanAnimator.SetBool("IsWalking", false);
                  footmanAnimator.SetBool("IsRunning", false);
                  footmanAnimator.SetBool("IsIdle", false);*/
                Debug.Log(FootmanStateControllerSecondPlayer.isWalking + "walkin dentro del onStateChange de attack");
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack2:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.defend:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.death:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.getHit:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.victory:
                break;
        }
        // Guardar estat actual com a estat previ 
        previousState = currentState;
        // Assignar el nou estat com a estat actual del player 
        currentState = newState;
    }
    bool checkForValidStatePair(FootmanStateControllerSecondPlayer.footmanStates newState)

    {
        bool returnVal = false;
        switch (currentState)
        {
            case FootmanStateControllerSecondPlayer.footmanStates.idle:
                //desde idle podemos pasar a cualquier estado
                returnVal = true;
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.walk:
                if (newState == FootmanStateControllerSecondPlayer.footmanStates.run || newState == FootmanStateControllerSecondPlayer.footmanStates.idle || newState == FootmanStateControllerSecondPlayer.footmanStates.attack)
                    returnVal = true;
                else returnVal = false;
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.run:
                if (newState == FootmanStateControllerSecondPlayer.footmanStates.walk || newState == FootmanStateControllerSecondPlayer.footmanStates.idle)
                    returnVal = true;
                else returnVal = false;
                /*aqui pondremos todo los estados donde se puede pasar, por ejemplo corriendo no se puede atacar pues va aqui*/
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack:
                if (newState == FootmanStateControllerSecondPlayer.footmanStates.walk || newState == FootmanStateControllerSecondPlayer.footmanStates.idle || newState == FootmanStateControllerSecondPlayer.footmanStates.run)
                    returnVal = true;
                else returnVal = false;
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack2:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.defend:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.death:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.getHit:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.victory:
                break;
        }
        return returnVal;
    }
    // Aquesta funció comprova si hi ha algun motiu que impedeixi passar al nou estat.
    bool checkIfAbortOnStateCondition(FootmanStateControllerSecondPlayer.footmanStates newState)
    {
        bool returnVal = false;
        switch (newState)

        {
            case FootmanStateControllerSecondPlayer.footmanStates.idle:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.walk:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.run:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.attack2:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.defend:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.death:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.getHit:
                break;
            case FootmanStateControllerSecondPlayer.footmanStates.victory:
                break;
        }
        return returnVal;
    }
    void OnAnimatorMove()
    {
        // Debug.Log(movement);
        if (FootmanStateControllerSecondPlayer.isRunning)
        {
            rigidbody.MovePosition(rigidbody.position + movement * (Time.fixedDeltaTime * SpeedMovementRun));
            rigidbody.MoveRotation(rotation);
        }
        if (FootmanStateControllerSecondPlayer.isWalking)
        {
            rigidbody.MovePosition(rigidbody.position + movement * (Time.fixedDeltaTime * speedMovement));
            rigidbody.MoveRotation(rotation);

        }

    }
}