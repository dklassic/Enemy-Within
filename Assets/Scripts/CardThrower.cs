using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;

public class CardThrower : MonoBehaviour
{
    [SerializeField] GameObject card = null;
    [SerializeField] Transform targetLocation = null;
    [SerializeField] float throwStrength = 4f;
    float explosionStrength = 250f;
    float explosionRange = 20f;

    [ExposeMethodInEditor]
    void Throw()
    {
        Rigidbody rb;
        if (card.TryGetComponent<Rigidbody>(out rb))
        {
            rb.GetComponent<BoxCollider>().enabled = true;
            rb.AddForce((targetLocation.position - card.transform.position).normalized * throwStrength, ForceMode.Impulse);
        }
        else
        {
            card.AddComponent<Rigidbody>();
            rb.GetComponent<BoxCollider>().enabled = true;
            rb = card.GetComponent<Rigidbody>();
            rb.AddForce((targetLocation.position - card.transform.position).normalized * throwStrength, ForceMode.Impulse);
        }
    }

    [ExposeMethodInEditor]
    void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRange);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosionStrength, explosionPos, explosionRange, 0.5f);
        }
    }
}
