using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameManager")]

public class GameManagerSO : ScriptableObject
{
    public event Action<int> OnAbrirPuerta;
    public event Action<int> OnCerrarPuerta;
    public event Action<int> OnAudioPuerta;

    public void AbrirPuerta(int idAbrirPuerta)
    {
        OnAbrirPuerta.Invoke(idAbrirPuerta);
    }

    public void CerrarPuerta(int idAbrirPuerta)
    {
        OnCerrarPuerta.Invoke(idAbrirPuerta);
    }

    public void AudioPuerta(int idAbrirPuerta)
    {
        OnAudioPuerta.Invoke(idAbrirPuerta);
    }
}
