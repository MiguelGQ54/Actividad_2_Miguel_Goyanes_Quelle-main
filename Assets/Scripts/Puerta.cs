using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Puerta : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private int idPuerta;

    private bool abrir = false;
    private bool puertaAbierta = false; //Bool para evitar que se siga abriendo la puerta mas de la cuenta (antes se
                                        //podia abrir infinitas veces haciendo que la puerta atraviese algunos pasillos)
    private bool cerrar = false;
    private float contador; //Contador para controlar que la puerta no se siga abriendo indefinidamente

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        gM.OnAbrirPuerta += Abrir;
        gM.OnCerrarPuerta += Cerrar;
        gM.OnAudioPuerta += Audio;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (abrir && puertaAbierta == false) //Solo se puede abrir si la puerta esta cerrada
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime);
            contador += Time.deltaTime;

            if (contador >= 0.8)
            {
                contador = 0;
                abrir = false;
                puertaAbierta = true;
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
        if(idAbrirPuerta == idPuerta)
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
