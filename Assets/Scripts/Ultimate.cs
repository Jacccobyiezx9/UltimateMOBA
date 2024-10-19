using UnityEngine;
using UnityEngine.UI;

public class StunAbility : MonoBehaviour
{
    public float abilityRange = 10f;
    public float stunDuration = 2f;
    public float slowDuration = 3f;
    public float slowAmount = 0.5f;
    public float coneAngle = 90f;
    public Image cone;
    private bool isAbilityActive = false;

    private void Start()
    {
        cone.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            cone.enabled = true;
            isAbilityActive = true;
        }

        if (isAbilityActive && Input.GetMouseButtonDown(0))
        {
            ExecuteAbility();
            isAbilityActive = false;
            cone.enabled = false;
        }
    }

    private void ExecuteAbility()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, abilityRange);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                Vector3 enemyForward = enemy.transform.forward;

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                Debug.Log($"Enemy Forward: {enemyForward}, Direction to Player: {directionToEnemy}");
                Debug.Log($"Angle to Enemy: {angleToEnemy}");

                if (angleToEnemy <= coneAngle / 2 && distanceToEnemy <= abilityRange)
                {
                    if (IsFacingOpposite(enemyForward, directionToEnemy))
                    {
                        enemy.Stun(stunDuration);
                        Debug.Log("Enemy Stunned");
                    }
                    else
                    {
                        enemy.Slow(slowAmount, slowDuration);
                        Debug.Log("Enemy Slowed");
                    }
                }
            }
        }
    }

    private bool IsFacingOpposite(Vector3 enemyForward, Vector3 directionToPlayer)
    {
        float dot = Vector3.Dot(enemyForward.normalized, directionToPlayer);
        return dot < -0.9f; 
    }

    private void OnDrawGizmos()
    {
        if (isAbilityActive) 
        {
            Gizmos.color = Color.red;
            Vector3 position = transform.position;
            Vector3 forward = transform.forward;


            float halfAngle = coneAngle / 2;
            float radius = abilityRange;

            for (float i = -halfAngle; i <= halfAngle; i += 1f)
            {
                Quaternion rotation = Quaternion.Euler(0, i, 0); 
                Vector3 direction = rotation * forward;
                Gizmos.DrawLine(position, position + direction * radius);
            }

            Vector3 leftEdge = Quaternion.Euler(0, -halfAngle, 0) * forward;
            Vector3 rightEdge = Quaternion.Euler(0, halfAngle, 0) * forward;

            Gizmos.DrawLine(position + leftEdge * radius, position + rightEdge * radius);
        }
    }
}
