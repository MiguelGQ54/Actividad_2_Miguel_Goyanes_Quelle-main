using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AbrirPuerta : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private int idAbrirPuerta;

    private bool puertaAbierta = false;

    private void OnTriggerStay(Collider other)
    {
        if(((other.transform.TryGetComponent(out Jugador jugador) && Input.GetKey(KeyCode.E)) || other.CompareTag("Alien")) && puertaAbierta == false) //Si el jugador pulsa la tecla E o el alien entra se abre la puerta si esta esta cerrada 
        {
            gM.AbrirPuerta(idAbrirPuerta);
            gM.AudioPuerta(idAbrirPuerta);
            puertaAbierta = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.transform.TryGetComponent(out Jugador jugador) || other.CompareTag("Alien")) && puertaAbierta == true)
        {
            gM.CerrarPuerta(idAbrirPuerta);
            gM.AudioPuerta(idAbrirPuerta);
            puertaAbierta = false;
        }
    }
}
