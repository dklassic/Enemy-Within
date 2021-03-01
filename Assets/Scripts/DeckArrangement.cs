using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatchCo;
using Cinemachine;

[ExecuteAlways]
public class DeckArrangement : MonoBehaviour
{
    [Header("Deck Layout")]
    [SerializeField] private int currentCard = -10;
    [SerializeField] [Range(0f, 1f)] private float zOffset = 0f;
    [SerializeField] [Range(0f, 1f)] private float xOffset = 0f;
    [SerializeField] [Range(0f, 5f)] private float selectionModifier = 2f;
    [SerializeField] private bool autoArrange = true;
    [SerializeField] private float yRotation = -20f;
    [SerializeField] private Transform table = null;
    [Header("Throw")]
    [SerializeField] Transform throwTarget = null;
    [SerializeField] float throwStrength = 4f;
    [SerializeField] float aimFuzziness = 10f;
    [Header("Explosion")]
    [SerializeField] Transform explosionTarget = null;
    float explosionStrength = 500f;
    float explosionRange = 20f;
    [SerializeField] CinemachineImpulseSource impulse = null;

    // Update is called once per frame
    void Update()
    {
        if (!autoArrange)
            return;
        if (transform.childCount == 0)
            return;
        List<Vector3> offset = OffsetDetermination();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localPosition = Vector3.Lerp(child.localPosition, offset[i], 0.5f);
            child.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(throwTarget.position, aimFuzziness);
    }

    private List<Vector3> OffsetDetermination()
    {
        List<Vector3> result = new List<Vector3>();
        float totalXOffset = 0;
        if (currentCard == -10)
            totalXOffset = (transform.childCount - 1) * xOffset;
        else if (currentCard == 0 || currentCard == transform.childCount)
            totalXOffset = (transform.childCount - 2) * xOffset + xOffset * selectionModifier;
        else
            totalXOffset = (transform.childCount - 3) * xOffset + xOffset * selectionModifier * 2f;
        float totalZOffset = (transform.childCount - 1) * zOffset;
        float actualXOffset = -totalXOffset / 2;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i > 0)
            {
                if (currentCard == i - 1 || currentCard == i)
                    actualXOffset += xOffset * selectionModifier;
                else
                    actualXOffset += xOffset;
            }
            result.Add(Vector3.right * actualXOffset + Vector3.back * (zOffset * i - totalZOffset / 2));
        }
        return result;
    }

    [ExposeMethodInEditor]
    void Throw()
    {
        if (currentCard < 0 || currentCard > transform.childCount - 1)
            return;
        Transform card = transform.GetChild(currentCard);
        Rigidbody rb;
        card.parent = table;
        Vector3 targetPosition = throwTarget.position + Vector3.up * Random.Range(-aimFuzziness, aimFuzziness) + Vector3.right * Random.Range(-aimFuzziness, aimFuzziness);
        if (card.TryGetComponent<Rigidbody>(out rb))
        {
            card.GetChild(0).GetComponent<BoxCollider>().enabled = true;
            rb.isKinematic = false;
            rb.AddForce((targetPosition - card.transform.position).normalized * throwStrength, ForceMode.Impulse);
        }
        if (currentCard > transform.childCount - 1)
        {
            currentCard = transform.childCount - 1;
        }
    }
    [ExposeMethodInEditor]
    void GetBack()
    {
        if (table.childCount == 0)
            return;
        Transform child = table.GetChild(table.childCount - 1);
        child.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        child.GetComponent<Rigidbody>().isKinematic = true;
        child.parent = transform;
        currentCard = transform.childCount - 1;
    }

    [ExposeMethodInEditor]
    void Explosion()
    {
        Vector3 explosionPos = explosionTarget.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRange);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.transform.parent.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosionStrength, explosionPos, explosionRange, 0.2f);
        }
        if (impulse == null)
            return;
        impulse.GenerateImpulse();
    }

    public Transform CurrentFocus()
    {
        if (currentCard < 0 || currentCard > transform.childCount - 1)
            return transform;
        else
            return transform.GetChild(currentCard);
    }
}
