using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private int idPuerta; //ID de la puerta que debe de coincidir con el del collider que se ha activado para poder abrirse

    private bool puertaAbierta = false; //Bool para evitar que se siga abriendo la puerta mas de la cuenta (antes se
                                        //podia abrir infinitas veces al pulsar repetidamente la tecla E o entrando el 
                                        //alien cuando la puerta estaba abierta, haciendo que la puerta atraviese algunos pasillos)
    private bool abrir = false;
    private bool cerrar = false;
    private float contador; //Contador para controlar que la puerta no se siga abriendo indefinidamente

    [SerializeField] private AudioSource audioSource; //Audio source que contiene el clip del sonido
    [SerializeField] private AudioClip audioClip; //Audioclip del sonido de abrir la puerta

    void Start()
    {
        gM.OnAbrirPuerta += Abrir; //Al activarse el evento se ejecuta el metodo Abrir
        gM.OnCerrarPuerta += Cerrar;
        gM.OnAudioPuerta += Audio;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (abrir && puertaAbierta == false) //Solo se puede abrir si la puerta esta cerrada
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime); //Se abre gradualmente la puerta
            contador += Time.deltaTime; //Comienza la cuenta atras en la que se abre

            if (contador >= 0.8) //Despues de 0,8s la puerta esta abierta del todo
            {
                contador = 0; //Se reinicia el contador
                abrir = false; //Se deja de abrir la puerta
                puertaAbierta = true; //La puerta esta abierta, por lo que no se puede volver a abrir hasta que se cierre
            }
        }

        if (cerrar && puertaAbierta) //Solo se puede cerrar si la puerta esta abierta
        {
            transform.Translate(Vector3.right * 5 * Time.deltaTime);
            contador += Time.deltaTime;

            if (contador >= 0.8)
            {
                contador = 0;
                cerrar = false;
                puertaAbierta = false;
            }
        }
    }

    private void Abrir(int idAbrirPuerta)
    {
        if(idAbrirPuerta == idPuerta) //La puerta solo se abre si coincide su ID con el del collider que se ha activado
        {
            abrir = true;
        }
    }

    private void Cerrar(int idAbrirPuerta)
    {
        if (idAbrirPuerta == idPuerta)
        {
            cerrar = true;
        }
    }

    private void Audio(int idAbrirPuerta)
    {
        if (idAbrirPuerta == idPuerta)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
