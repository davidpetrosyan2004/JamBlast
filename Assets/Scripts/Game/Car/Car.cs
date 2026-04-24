using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Car : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    [SerializeField] private float speed = 3f;
    public void MoveTo(Vector3 target)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(target));
    }
    public void MoveAlongPath(Vector3 target, Vector3[] path)
    {
        StopAllCoroutines();
        StartCoroutine(MovePath(target, path));
    }
    private IEnumerator MoveRoutine(Vector3 target)
    {
        IsMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            Vector3 direction = (target - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime
            );
            yield return null;
        }

        IsMoving = false;
    }
    private IEnumerator MovePath(Vector3 target, Vector3[] pathPoints)
    {
        IsMoving = true;

        var pathToTarget = GetPathToTarget(target, pathPoints);

        int index = 0;

        while (index < pathToTarget.Length)
        {
            Vector3 nextPoint = pathToTarget[index];

            while (Vector3.Distance(transform.position, nextPoint) > 0.05f)
            {
                Vector3 direction = (nextPoint - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    nextPoint,
                    speed * Time.deltaTime
                );

                yield return null;
            }

            index++;
        }

        IsMoving = false;
    }

    int GetClosestIndex(Vector3 pos, Vector3[] points)
    {
        float minDist = float.MaxValue;
        int index = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float dist = Vector3.Distance(pos, points[i]);
            if (dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        return index;
    }

    public Vector3[] GetPathToTarget(Vector3 target, Vector3[] path)
    {
        var targetIndex = GetClosestIndex(target, path);

        Vector3[] result = new Vector3[targetIndex + 2];

        // сначала путь
        for (int i = 0; i <= targetIndex; i++)
        {
            result[i] = path[i];
        }

        // в конце target
        result[result.Length - 1] = target;

        return result;
    }
}