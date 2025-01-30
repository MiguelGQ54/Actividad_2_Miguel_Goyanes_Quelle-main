using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrulla : MonoBehaviour
{
    [SerializeField] private Transform ruta;
    //[SerializeField] protected LayerMask objetivo; //No se pudo implementar la deteccion de obstaculos por lo que no son necesarias estas variables
    //[SerializeField] protected LayerMask obstaculo;
    [SerializeField] protected NavMeshAgent alien;
    [SerializeField] private Atacar atacar; //Script del estado de ataque
    [SerializeField] protected GameObject jugador;

    private List<Vector3> puntosDeRuta = new List<Vector3>(); //Lista con las coordenadas de cada punto de la patrulla
    private int puntoActual = 0;
    private Vector3 destinoActual;
    protected float distanciaX, distanciaZ; //Distancia del jugador

    private void Start()
    {
        foreach (Transform punto in ruta)
        {
            puntosDeRuta.Add(punto.position); //Se guarda cada uno de los puntos anidados en el objeto RutaPatrulla
        }

        destinoActual = puntosDeRuta[puntoActual];
    }

    private void Awake()
    {
        StartCoroutine(PatrullarYEsperar()); //Se inicia la corrutina de la patrulla
    }

    private void FixedUpdate()
    {
        //No detecta al jugador correctamente
        /*Collider[] collsDetectados = Physics.OverlapSphere(transform.position, 360, objetivo);
        if (collsDetectados.Length > 0)
        {
            Vector3 directionAtTarget = (collsDetectados[0].transform.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, directionAtTarget, 360, obstaculo))
            {
                StopCoroutine(PatrullarYEsperar());
                atacar.enabled = true;
                enabled = false;
            }
        }*/

        //Como la deteccion no funciona correctamente mediante el metodo anterior se realizara mediante la distancia con el jugador, al igual que en el script Jugador.cs
        //El inconveniente de este metodo es que el alien podra detectar al jugador a traves de obstaculos
        Vector3 posAlien = transform.position;
        Vector3 posJugador = jugador.transform.position;

        distanciaX = Math.Abs(posAlien.x - posJugador.x); //El valor tiene que ser absoluto para evitar que una distancia -20 por ejemplo detecte que el jugador esta cerca
        distanciaZ = Math.Abs(posAlien.z - posJugador.z);

        if (distanciaX <= 40 && distanciaZ <= 40) //Si se encuentra cerca del jugador se produce la "colision"
        {
            StopCoroutine(PatrullarYEsperar()); //Se detiene la corrutina de patrullar
            atacar.enabled = true; //Se inicia el script de ataque
            enabled = false; //Se desactiva este script
        }
    }

    private IEnumerator PatrullarYEsperar()
    {
        while (true)
        {
            alien.SetDestination(destinoActual); //Se dirige al punto actual
            yield return new WaitUntil( ()=> !alien.pathPending && alien.remainingDistance <= 0.2f); //Se espera a que llegue al punto actual
            CalcularNuevoDestino(); //Se calcula el siguiente punto
        }
    }

    private void CalcularNuevoDestino()
    {
        puntoActual++; //Se pasa al siguiente punto del array
        puntoActual = puntoActual % (puntosDeRuta.Count); //Si se llega al ultimo punto el punto actual pasa a ser 0 para repetir el bucle
        destinoActual = puntosDeRuta[puntoActual]; //El nuevo destino es el nuevo punto calculado
    }
}
