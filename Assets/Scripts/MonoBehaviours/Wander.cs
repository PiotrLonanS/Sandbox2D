using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    public float pursiutSpeed;
    public float wanderSpeed;
    float currentSpeed;

    public float directionChangeInterval;

    public bool followPlayer;

    Coroutine moveCoroutine;

    Rigidbody2D rb2d;
    Animator animator;
    readonly Transform targetTransform = null;

    Vector3 endPosition;

    float currentAngle = 0;
    void Start()
    {
        animator = GetComponent<Animator>();

        currentSpeed = wanderSpeed;

        rb2d = GetComponent<Rigidbody2D>();

        StartCoroutine(WanderRoutine());

        IEnumerator WanderRoutine()
        {
            while (true)
            {
                ChooseNewEndpoint();

                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }
                moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

                yield return new WaitForSeconds(directionChangeInterval);
            }
        }
    }
    void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);

        currentAngle = Mathf.Repeat(currentAngle, 360);

        endPosition += Vector3FromAngle(currentAngle);
    }
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadiants = inputAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(inputAngleRadiants),
                            Mathf.Sin(inputAngleRadiants), 0);
    }
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while (remainingDistance > float.Epsilon)
        {
            if (targetTransform != null)
            {
                endPosition = targetTransform.position;
            }
            if(rigidBodyToMove != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position,
                    endPosition, speed * Time.deltaTime);

                rb2d.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("isWalking", false);
    }
    void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }
}
