using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootmanStateListener : MonoBehaviour
{
    private Animator footmanAnimator;
    private new Rigidbody rigidbody;
    private FootmanStateController.footmanStates previousState = FootmanStateController.footmanStates.idle;
    private FootmanStateController.footmanStates currentState = FootmanStateController.footmanStates.idle;
    public float turnSpeed = 10f,speedMovement=2f,SpeedMovementRun=3f;
    private Vector3 movement, desiredForward;
    private Quaternion rotation = Quaternion.identity;
    
    void OnEnable()
    {
            FootmanStateController.onStateChange += onStateChange;
    }
    //Aquest mètode de MonoBehaviour s'executa cada vegada que es desactiva l'objecte associat a l'script.
    //Es deixa d'escoltar l'event onStateChange
    void OnDisable() { FootmanStateController.onStateChange -= onStateChange; }
    void Start() {
        footmanAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

      
    }
    void FixedUpdate() { onStateCycle();
       // Debug.Log(movement);
    }

    void onStateCycle()
    {
        switch (currentState)
        {
            case FootmanStateController.footmanStates.idle:

                break;
            case FootmanStateController.footmanStates.walk:
                movement.Set(FootmanStateController.horizontal, 0f, FootmanStateController.vertical);
                movement.Normalize();
                desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
                rotation = Quaternion.LookRotation(desiredForward);
              
                break;
            case FootmanStateController.footmanStates.run:
                movement.Set(FootmanStateController.horizontal, 0f, FootmanStateController.vertical);
                movement.Normalize();
                desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
                rotation = Quaternion.LookRotation(desiredForward);
                break;
            case FootmanStateController.footmanStates.attack:
            
                break;
            case FootmanStateController.footmanStates.attack2:
                break;
            case FootmanStateController.footmanStates.defend:
                break;
            case FootmanStateController.footmanStates.death:
                break;
            case FootmanStateController.footmanStates.getHit:
                break;
            case FootmanStateController.footmanStates.victory:
                break;
        }
    }

    public void onStateChange(FootmanStateController.footmanStates newState)
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
            case FootmanStateController.footmanStates.idle:
                footmanAnimator.SetBool("IsWalking", false);
                footmanAnimator.SetBool("IsRunning", false);
                footmanAnimator.SetBool("IsIdle", true);
                footmanAnimator.SetBool("IsAttacking", false);
                footmanAnimator.SetBool("IsDefending", false);
                footmanAnimator.SetBool("IsAttacking2", false);
                break;
            case FootmanStateController.footmanStates.walk:
                footmanAnimator.SetBool("IsWalking", true);
                footmanAnimator.SetBool("IsRunning", false);
               footmanAnimator.SetBool("IsAttacking", false);
                footmanAnimator.SetBool("IsIdle", false);
                footmanAnimator.SetBool("IsDefending", false);
                footmanAnimator.SetBool("IsAttacking2", false);

                break;
            case FootmanStateController.footmanStates.run:
                footmanAnimator.SetBool("IsRunning", true);
                footmanAnimator.SetBool("IsWalking", false);
                break;
            case FootmanStateController.footmanStates.attack:
                footmanAnimator.SetBool("IsAttacking", true);
                footmanAnimator.SetBool("IsWalking", false);
                footmanAnimator.SetBool("IsDefending", false);

                //Debug.Log(FootmanStateController.isWalking + "walkin dentro del onStateChange de attack");
                break;
            case FootmanStateController.footmanStates.attack2:
                footmanAnimator.SetBool("IsAttacking2", true);
                footmanAnimator.SetBool("IsWalking", false);
                footmanAnimator.SetBool("IsDefending", false);
                break;
            case FootmanStateController.footmanStates.defend:
                footmanAnimator.SetBool("IsDefending", true);
                footmanAnimator.SetBool("IsAttacking", false);
                footmanAnimator.SetBool("IsAttacking2", false);
                footmanAnimator.SetBool("IsWalking", false);
                break;
            case FootmanStateController.footmanStates.death:
                break;
            case FootmanStateController.footmanStates.getHit:
                break;
            case FootmanStateController.footmanStates.victory:
                break;
        }
        // Guardar estat actual com a estat previ 
        previousState = currentState;
        // Assignar el nou estat com a estat actual del player 
        currentState = newState;
    }
    bool checkForValidStatePair(FootmanStateController.footmanStates newState)

    {
        bool returnVal = false;
        switch (currentState)
        {
            case FootmanStateController.footmanStates.idle:
                //desde idle podemos pasar a cualquier estado
                returnVal = true;
                break;
            case FootmanStateController.footmanStates.walk:
                if (newState == FootmanStateController.footmanStates.run || newState == FootmanStateController.footmanStates.idle|| newState == FootmanStateController.footmanStates.attack || newState == FootmanStateController.footmanStates.defend || newState == FootmanStateController.footmanStates.attack2)
                    returnVal = true;
                else returnVal = false;
                break;
            case FootmanStateController.footmanStates.run:
                if (newState == FootmanStateController.footmanStates.walk || newState == FootmanStateController.footmanStates.idle)
                    returnVal = true;
                else returnVal = false;
                /*aqui pondremos todo los estados donde se puede pasar, por ejemplo corriendo no se puede atacar pues va aqui*/
                break;
            case FootmanStateController.footmanStates.attack:
                if (newState == FootmanStateController.footmanStates.walk || newState == FootmanStateController.footmanStates.idle || newState == FootmanStateController.footmanStates.run|| newState == FootmanStateController.footmanStates.defend || newState == FootmanStateController.footmanStates.attack2)
                    returnVal = true;
                else returnVal = false;
                break;
            case FootmanStateController.footmanStates.attack2:
                if (newState == FootmanStateController.footmanStates.walk || newState == FootmanStateController.footmanStates.idle || newState == FootmanStateController.footmanStates.run || newState == FootmanStateController.footmanStates.defend || newState == FootmanStateController.footmanStates.attack)
                    returnVal = true;
                else returnVal = false;
                break;
                break;
            case FootmanStateController.footmanStates.defend:
                if (newState == FootmanStateController.footmanStates.walk || newState == FootmanStateController.footmanStates.idle || newState == FootmanStateController.footmanStates.run || newState == FootmanStateController.footmanStates.attack || newState == FootmanStateController.footmanStates.attack2)
                    returnVal = true;
                else returnVal = false;
                break;
            case FootmanStateController.footmanStates.death:
                break;
            case FootmanStateController.footmanStates.getHit:
                break;
            case FootmanStateController.footmanStates.victory:
                break;
        }
        return returnVal;
    }
    // Aquesta funció comprova si hi ha algun motiu que impedeixi passar al nou estat.
    bool checkIfAbortOnStateCondition(FootmanStateController.footmanStates newState)
    {
        bool returnVal = false;
        switch (newState)

        {
            case FootmanStateController.footmanStates.idle:
                break;
            case FootmanStateController.footmanStates.walk:
                break;
            case FootmanStateController.footmanStates.run:
                break;
            case FootmanStateController.footmanStates.attack:
                break;
            case FootmanStateController.footmanStates.attack2:
                break;
            case FootmanStateController.footmanStates.defend:
                break;
            case FootmanStateController.footmanStates.death:
                break;
            case FootmanStateController.footmanStates.getHit:
                break;
            case FootmanStateController.footmanStates.victory:
                break;
        }
        return returnVal;
    }
    void OnAnimatorMove()
    {
        // Debug.Log(movement);
        if (FootmanStateController.isRunning)
        {
            rigidbody.MovePosition(rigidbody.position + movement * (Time.fixedDeltaTime * SpeedMovementRun));
            rigidbody.MoveRotation(rotation);
        }
        if (FootmanStateController.isWalking)
        {
            rigidbody.MovePosition(rigidbody.position + movement * (Time.fixedDeltaTime * speedMovement));
            rigidbody.MoveRotation(rotation);

        }
       
    }
}