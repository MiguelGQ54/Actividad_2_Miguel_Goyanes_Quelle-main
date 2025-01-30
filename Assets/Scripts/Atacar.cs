using System;
using UnityEngine;

public class Atacar : Patrulla
{
    [SerializeField] Patrulla patrulla;

    [SerializeField] private float contador; //tiempo en el que el jugador tiene que pasar fuera de la vision del alien para perderlo de vista
    [SerializeField] private bool veAlJugador; //Bool para controlar si el alien esta viendo al jugador o no

    private void Awake()
    {
        contador = 5f; //El contador de la vision del jugador se inicializa en 5 para que no se desactive el script si ya se desactivo anteriormente
        veAlJugador = true; //Si se inicia el script es porque esta viendo al jugador   
    }

    void Update()
    {
        alien.SetDestination(jugador.transform.position); //El alien se dirige al jugador

        if (veAlJugador == false)
        {
            contador -= Time.deltaTime; //Si pierde de vista al jugador empieza a bajar el contador
        }
        else { contador = 5f;} //Si sigue viendo al jugador el contador esta en su maximo valor

        if (contador <= 0) //Si el contador llega a 0 se desactiva el modo de ataque y vuelve a patrullar
        {
            patrulla.enabled = true; //Se inicia el script de patrulla
            enabled = false; //Se desactiva este script
        }
    }

    private void FixedUpdate()
    {
        Vector3 posAlien = transform.position;
        Vector3 posJugador = jugador.transform.position;

        distanciaX = Math.Abs(posAlien.x - posJugador.x);
        distanciaZ = Math.Abs(posAlien.z - posJugador.z);

        if (distanciaX <= 30 && distanciaZ <= 30) //Si se encuentra cerca del jugador se produce la "colision"
        {
            veAlJugador = true;
        }
        else {  veAlJugador = false;}
    }
}
