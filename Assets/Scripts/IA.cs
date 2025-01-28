using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA : MonoBehaviour
{
    [SerializeField] NavMeshAgent nMA;
    [SerializeField] private GameObject jugador;

    void Start()
    {

    }

    void Update()
    {
        nMA.SetDestination(jugador.transform.position);
    }
}
