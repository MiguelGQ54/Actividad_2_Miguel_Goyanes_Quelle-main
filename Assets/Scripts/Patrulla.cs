using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrulla : MonoBehaviour
{
    [SerializeField] private Transform ruta;
    [SerializeField] private float tiempoEspera;
    [SerializeField] private float rangoVision;
    [SerializeField] private float anguloVision;
    [SerializeField] private LayerMask objetivo;
    [SerializeField] private LayerMask obstaculo;

    private List<Vector3> puntosDeRuta = new List<Vector3>();
    private NavMeshAgent alien;
    private int puntoActual = 0;
    private Vector3 destinoActual;

    private void Awake()
    {
        foreach (Transform punto in ruta)
        {
            puntosDeRuta.Add(punto.position);
        }
        alien.GetComponent<NavMeshAgent>();

        destinoActual = puntosDeRuta[puntoActual];
        StartCoroutine(PatrullarYEsperar());
    }

    private IEnumerator PatrullarYEsperar()
    {
        alien.SetDestination(destinoActual);
        yield return new WaitUntil( ()=> !alien.pathPending && alien.remainingDistance <= 0.2f);
        yield return new WaitForSeconds(tiempoEspera);
        CalcularNuevoDestino();
    }

    private void CalcularNuevoDestino()
    {
        puntoActual++;
        puntoActual = puntoActual % (puntosDeRuta.Count);
        destinoActual = puntosDeRuta[puntoActual];
    }

    private void FixedUpdate()
    {
        Collider[] collDetectados = Physics.OverlapSphere(transform.position, rangoVision, objetivo);
        if (collDetectados.Length > 0)
        {
            Vector3 directionAtTarget = (collDetectados[0].transform.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, directionAtTarget, rangoVision, obstaculo))
            {
                if (Vector3.Angle(transform.forward, directionAtTarget) <= anguloVision/2)
                {
                    enabled = false;
                }
            }
        }
    }
}
