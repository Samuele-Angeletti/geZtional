using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationRenderer : MonoBehaviour
{
    [SerializeField] float maxScale;
    [SerializeField] float animationSpeed;
    [SerializeField] float animationTime;

    float timePassed = 0;
    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (transform.localScale.x >= maxScale)
        {
            animationSpeed = -animationSpeed;
        }
        else if(transform.localScale.x <= 0)
        {
            animationSpeed = Mathf.Abs(animationSpeed);
        }

        transform.localScale += animationSpeed * Time.deltaTime * Vector3.one;

        timePassed += Time.deltaTime;

        if (timePassed >= animationTime)
            Destroy(gameObject);
    }
}
