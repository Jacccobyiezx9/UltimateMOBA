using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 5f;
    private bool isStunned = false;
    private float originalSpeed;
    public Material originalMaterial;
    public float lineLength = 5f; // Length of the Z-axis line
    public Color lineColor = Color.red; // Color of the line

    private void Start()
    {
        originalSpeed = speed;
        originalMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (!isStunned)
        {
            Walk();
        }
    }

    private void Walk()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Stun(float duration)
    {
        isStunned = true;
        GetComponent<Renderer>().material.color = Color.grey;
        StartCoroutine(RemoveStunAfterTime(duration));
    }

    private IEnumerator RemoveStunAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStunned = false;
        GetComponent<Renderer>().material = originalMaterial;
    }

    public void Slow(float slowAmount, float duration)
    {
        speed *= (1 - slowAmount);
        StartCoroutine(RemoveSlowAfterTime(duration));
    }

    private IEnumerator RemoveSlowAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * lineLength;

        Gizmos.DrawLine(start, end);
    }
}
