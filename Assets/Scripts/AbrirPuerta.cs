using UnityEngine;

public class AbrirPuerta : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private int idAbrirPuerta; //Id del collider de la puerta

    private bool puertaAbierta = false;

    private void OnTriggerStay(Collider other)
    {
        if(((other.transform.TryGetComponent(out Jugador jugador) && Input.GetKey(KeyCode.E)) || other.CompareTag("Alien")) && puertaAbierta == false) //Si el jugador pulsa la tecla E dentro del collider o el alien entra, se abre la puerta solo si esta esta cerrada 
        {
            gM.AbrirPuerta(idAbrirPuerta); //Se abre la puerta
            gM.AudioPuerta(idAbrirPuerta); //Suena el sonido de la puerta
            puertaAbierta = true; //La puerta esta abierta, por lo que se puede cerrar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.transform.TryGetComponent(out Jugador jugador) || other.CompareTag("Alien")) && puertaAbierta == true) //Si el jugador o el alien sale del collider se cierra la puerta
        {
            gM.CerrarPuerta(idAbrirPuerta); //Se cierra la puerta
            gM.AudioPuerta(idAbrirPuerta); //Suena el audio otra vez
            puertaAbierta = false; //La puerta esta cerrada, por lo que se puede abrir
        }
    }
}
