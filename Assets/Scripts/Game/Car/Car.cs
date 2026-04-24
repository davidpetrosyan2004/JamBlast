using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
public class Car : MonoBehaviour
{
    public bool IsMoving { get; private set; }
    public int capacity { get; private set; }
    public List<MeshRenderer> meshes;
    public CarData.CarColor carColor; 

    [SerializeField] private float speed = 5f;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private TextMeshProUGUI capcityText;

    public void SetColor(Material material, CarData.CarColor color)
    {
        foreach (var mesh in meshes)
        {
            mesh.material = material;
        }
        carColor = color;
    }

    public void SetCapacityText(int number)
    {
        capacity = number;
        capcityText.text = "0/" + capacity.ToString();
    }
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
        smokeEffect.Play();
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
        smokeEffect.Stop();
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

        for (int i = 0; i <= targetIndex; i++)
        {
            result[i] = path[i];
        }

        result[result.Length - 1] = target;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == this) return;
        if (other.CompareTag("ScaleCar"))
        {
            transform.DOScale(1.5f, 0.2f).SetLoops(1, LoopType.Yoyo);
        } 
    }
}