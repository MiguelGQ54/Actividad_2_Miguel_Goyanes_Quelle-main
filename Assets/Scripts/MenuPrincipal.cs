using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private GameObject menuPrincipalPanel;
    [SerializeField] private GameObject ajustesPanel;
    [SerializeField] private AudioMixer audioMixer;

    public void onJugar()
    {
        SceneManager.LoadScene(1);
    }
    
    public void onAjustes()
    {
        menuPrincipalPanel.SetActive(false);
        ajustesPanel.SetActive(true);
    }

    public void onVolverMenu()
    {
        menuPrincipalPanel.SetActive(true);
        ajustesPanel.SetActive(false);
    }

    public void VolumenGeneral(float volumen)
    {
        audioMixer.SetFloat("volumenGeneral", volumen);
    }

    public void VolumenMusica(float volumen)
    {
        audioMixer.SetFloat("volumenMusica", volumen);
    }

    public void VolumenSFX(float volumen)
    {
        audioMixer.SetFloat("volumenSFX", volumen);
    }
}
