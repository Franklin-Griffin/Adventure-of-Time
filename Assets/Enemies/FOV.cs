using UnityEngine;
using System.Collections;

public class FOV : MonoBehaviour
{
    public float radius = 20;
    [Range(0, 360)]
    public float angle = 120;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    Enemy enemyRef;
    Dragon enemyRefd;

    public Transform head;

    private void Start()
    {
        if (GetComponent<Enemy>())
            enemyRef = GetComponent<Enemy>();
        else
            enemyRefd = GetComponent<Dragon>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(head.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    if (enemyRef)
                        enemyRef.PlayerFound(target);
                    else
                        enemyRefd.PlayerFound(target);
                    Destroy(this);
                }
            }
        }
    }
}