using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
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
    [SerializeField] private CanvasManager canvasManager;

    [SerializeField] float distanciaX, distanciaZ;

    bool lockCursor = true; //Bloquear el cursor
    private float cameraPitch = 0.0f; //Inclinacion de la camara
    private Rigidbody rb;
    private float hInput, vInput; //Entradas de movimiento
    private Vector3 posicionInicial;
    private float velocidadCorrer, velocidadAndar; //Velocidad al correr y al andar
    private bool seHaCansado, estadoLinterna; //1-Bool para controlar la recuperacion de la energia; 2-Apagar/encender linterna


    void Start()
    {
        posicionInicial = transform.position; //Se guarda la posicion inicial
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
            UsarLinterna();
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

            SFXPasos.SetActive(true);
        }
        else
        {
            SFXPasos.SetActive(false);
        }
    }

    void Correr()
    {
        if (Input.GetKey(KeyCode.LeftShift) && energia>=0 && seHaCansado == false)
        {
            velocidadMovimiento = velocidadCorrer;
            energia -= 10 * Time.deltaTime;
        }
        else
        {
            velocidadMovimiento = velocidadAndar;
        }

        if (energia < 100 && !Input.GetKey(KeyCode.LeftShift))
        {
            energia += 10 * Time.deltaTime;
        }
        
        if (energia <= 0)
        {
            velocidadMovimiento = velocidadAndar;
            seHaCansado = true;
        }
        else if (energia >=30)
        {
            seHaCansado = false;
        }

        canvasManager.BarraEnergia.fillAmount = energia/100;
    }

    void UsarLinterna()
    {
        if (estadoLinterna)
        {
            linterna.SetActive(false);
            estadoLinterna = false;
        }
        else
        {
            linterna.SetActive(true);
            estadoLinterna = true;
        }
    }

    void Morir()
    {
        //El prefab "Mimic" tiene un problema al añadir colliders a la hora de crear tentaculos, por lo que la deteccion de colisiones se realizara artificialmente mediante la distancia del jugador con el alien
        
        Vector3 posJugador = transform.position;
        Vector3 posAlien = Alien.transform.position;

        distanciaX = Math.Abs(posJugador.x - posAlien.x); //El valor tiene que ser absoluto para evitar que una distancia -20 por ejemplo detecte que el alien esta cerca
        distanciaZ = Math.Abs(posJugador.z - posAlien.z);

        if (distanciaX <= 1 && distanciaZ <= 1) //Si se encuentra cerca del jugador se produce la "colision"
        {
            //UnityEngine.Cursor.lockState = CursorLockMode.None;
            //SceneManager.LoadScene(0);
        }
    }
}
