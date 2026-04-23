using UnityEngine;
using System.Collections; 
public class Car : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    public void MoveTo(Vector3 target)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(target));
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        IsMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                5f * Time.deltaTime
            );
            yield return null;
        }

        IsMoving = false;
    }
}