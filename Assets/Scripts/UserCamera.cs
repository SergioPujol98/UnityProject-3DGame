using UnityEngine;
using System.Collections;

public class UserCamera : MonoBehaviour
{

    public Transform target;                            // Target 
    public float targetHeight = 1.7f;                       // Ajust vertical
    public float distance = 12.0f;                          // Distancia inicio
    public float offsetFromWall = 0.1f;                     // Mantener camara alejada de objetos
    public float maxDistance = 20;                      // Vision
    public float minDistance = 0.6f;                        // Vision
    public float xSpeed = 200.0f;                           // Velocidad rotacion
    public float ySpeed = 200.0f;                           // Velocidad rotacion
    public float yMinLimit = -80;                           // Velocidad vision arriba
    public float yMaxLimit = 80;                            // Velocidad vision arriba
    public float zoomRate = 40;                             // Vision velocidad
    public float rotationDampening = 0.0f;              // Rotacion
    public float zoomDampening = 5.0f;                  // Velocidad vista
    LayerMask collisionLayers = -1;     // Choque de camara con objeto

    public bool lockToRearOfTarget;
    public bool allowMouseInputX = true;
    public bool allowMouseInputY = true;

    

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    public float desiredDistance;
    private float correctedDistance;
    private bool rotateBehind;

    public GameObject userModel;
    public bool inFirstPerson;

    void Start()
    {

        Vector3 angles = transform.eulerAngles;
        xDeg = angles.x;
        yDeg = angles.y;
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;
        
        if (lockToRearOfTarget)
            rotateBehind = true;
    }

    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            if (inFirstPerson == true)
            {

                minDistance = 10;
                desiredDistance = 15;
                userModel.SetActive(true);
                inFirstPerson = false;
            }
        }

        if (desiredDistance == 10)
        {

            minDistance = 0;
            desiredDistance = 0;
            userModel.SetActive(false);
            inFirstPerson = true;
        }
    }

    //Solo mover camera cuando se haya actualziado
    void LateUpdate()
    {

        // No hacer nada
        if (!target)
            return;

        Vector3 vTargetOffset3;

        // Boton del raton  pulsado
        if (GUIUtility.hotControl == 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {

            }
            else
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    //Comprueba que el raton este en la pantalla
                    if (allowMouseInputX)
                        xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    if (allowMouseInputY)
                        yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                }
            }
        }
        ClampAngle(yDeg);

        // Rotacion
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);

        // Calculamos  la distancia deseada.
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        // Calculamos posicion de la camara
        Vector3 vTargetOffset = new Vector3(0, -targetHeight, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset);
        // Checkeamos la colision usando el punto de registro deseado del objetivo que nosotros escogemos según lo establecido por el usuario usando la altura
                RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z);

        // Calculamos la posicion de la camara en caso de que colisione contra un objeto
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
        {
            correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
            isCorrected = true;
        }

        //No entiendo para que sirve esto, creo que suaviza el error de distancia con el salto
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // Mantenemos dentro de los limites de vision preestablecidos
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Recalculamos posicion
        position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset);

        //Insertar posicion y rotacion de la camara
        transform.rotation = rotation;
        transform.position = position;
    }

    void ClampAngle(float angle)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        yDeg = Mathf.Clamp(angle, -60, 80);
    }

}