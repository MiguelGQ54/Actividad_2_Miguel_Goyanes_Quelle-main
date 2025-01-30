using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null; //Camara del jugador
    [SerializeField] private float mouseSensitivity = 2.0f; //Sensibilidad del raton
    [SerializeField] private float velocidadMovimiento; //Velocidad base del jugador
    [SerializeField] private float energia; //Energia para poder correr
    [SerializeField] private GameObject linterna;
    [SerializeField] private GameObject SFXPasos; //Audio source que contiene el clip de los pasos y se ejecuta en bucle mientras esta activo
    [SerializeField] private GameObject Alien;
    [SerializeField] private CanvasManager canvasManager; //Canvas manager para actualizar la barra de energia

    float distanciaX, distanciaZ;
    bool lockCursor = true; //Bloquear el cursor
    private float cameraPitch = 0.0f; //Inclinacion de la camara
    private Rigidbody rb;
    private float hInput, vInput; //Entradas de movimiento
    private float velocidadCorrer, velocidadAndar; //Velocidad al correr y al andar
    private bool seHaCansado, estadoLinterna; //1-Bool para controlar la recuperacion de la energia; 2-Apagar/encender linterna


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (lockCursor)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

            velocidadAndar = velocidadMovimiento; //La velocidad al andar es la velocidad base
            velocidadCorrer = velocidadAndar + 7; //La velocidad al correr es superior a la de andar
        }
    }

    void Update()
    {
        //Actualizar la vista de la camara con el raton
        UpdateMouseLook();

        //Leer entradas de movimiento
        hInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        vInput = UnityEngine.Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Con el click izquierdo se enciende o se apaga la linterna
        {
            ActivarLinterna();
        }
    }

    private void FixedUpdate()
    {
        //Obtener la direccion hacia adelante y derecha segun la camara
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        //Asegurarse de que forward y right no incluyan el eje Y
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        //Calcular la direccion final del movimiento
        Vector3 moveDirection = forward * vInput + right * hInput;

        Movimiento();
        Correr();
        Morir();
    }

    void UpdateMouseLook()
    {
        Vector2 mouseDelta = new Vector2(UnityEngine.Input.GetAxis("Mouse X"), UnityEngine.Input.GetAxis("Mouse Y"));

        //Control de rotacion de la camara en el eje vertical
        cameraPitch -= mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        //Rotar al jugador en el eje horizontal
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    void Movimiento()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 velocidad = Vector3.zero;

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 direccion = (transform.forward * vertical + transform.right * horizontal).normalized;
            velocidad = direccion * velocidadMovimiento;

            velocidad.y = rb.velocity.y;
            rb.velocity = velocidad;

            SFXPasos.SetActive(true); //Al moverse se activa el audio source de los pasos
        }
        else
        {
            SFXPasos.SetActive(false);
        }
    }

    void Correr()
    {
        if (Input.GetKey(KeyCode.LeftShift) && energia>=0 && seHaCansado == false) //Al pulsar shift, si queda energia y el jugador no se ha cansado recientemente se puede correr
        {
            velocidadMovimiento = velocidadCorrer; //Se aumenta la velocidad de movimiento
            energia -= 10 * Time.deltaTime; //Se gasta energia al correr
        }
        else
        {
            velocidadMovimiento = velocidadAndar; //Si no se esta corriendo la velocidad es la normal
        }

        if (energia < 100 && !Input.GetKey(KeyCode.LeftShift)) //Si la energia es inferior a 100 (energia maxima) y no se esta corriendo se recupera con el tiempo
        {
            energia += 20 * Time.deltaTime;
        }
        
        if (energia <= 0) //Si se gasta toda la energia se entra en el estado de cansado, en el que no se puede correr hasta salir de el
        {
            velocidadMovimiento = velocidadAndar;
            seHaCansado = true;
        }
        else if (energia >=30 && seHaCansado == true) //Al superar el valor de 30 de energia el jugador pierde el estado de cansado si lo tenia activo
        {
            seHaCansado = false;
        }

        canvasManager.BarraEnergia.fillAmount = energia/100; //Se actualiza la barra de energia en todo momento
    }

    void ActivarLinterna()
    {
        if (estadoLinterna) //Si la linterna estaba encendida, se apaga
        {
            linterna.SetActive(false);
            estadoLinterna = false;
        }
        else //Si la linterna estaba apagada, se enciende
        {
            linterna.SetActive(true);
            estadoLinterna = true;
        }
    }

    void Morir()
    {
        //El prefab "Mimic" tiene un problema al añadir colliders a la hora de crear tentaculos, por lo que la deteccion de colisiones se realizara artificialmente mediante la distancia del jugador con el alien
        
        Vector3 posJugador = transform.position; //Se toma la posicion del jugador
        Vector3 posAlien = Alien.transform.position; //Se toma la posicion del alien

        distanciaX = Math.Abs(posJugador.x - posAlien.x); //El valor tiene que ser absoluto para evitar que una distancia -20 por ejemplo detecte que el alien esta cerca
        distanciaZ = Math.Abs(posJugador.z - posAlien.z);

        if (distanciaX <= 1 && distanciaZ <= 1) //Si se encuentra cerca del jugador se produce la "colision"
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0); //Se vuelve al menu principal; El jugador ha perdido
        }
    }
}
