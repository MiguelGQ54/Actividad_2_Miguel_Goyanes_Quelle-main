using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;    

public class Puerta2 : MonoBehaviour //Mismo script que Puerta.cs pero cambiando la direccion en la que se abre
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private int idPuerta;

    private bool abrir = false;
    private bool puertaAbierta = false;
    
    private bool cerrar = false;
    private float contador;

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
        if (abrir && puertaAbierta == false)
        {
            transform.Translate(Vector3.right * 5 * Time.deltaTime);
            contador += Time.deltaTime;

            if (contador >= 0.8)
            {
                contador = 0;
                abrir = false;
                puertaAbierta = true;
            }
        }

        if (cerrar && puertaAbierta)
        {
            transform.Translate(Vector3.left * 5 * Time.deltaTime);
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
