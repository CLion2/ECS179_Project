using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideGate : MonoBehaviour
{
    [SerializeField] public bool opening = false;
    [SerializeField] public bool closing = false;
    [SerializeField] private float speed = 1f;
    [SerializeField] Vector3 targetPosition;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        this.startPosition = transform.position;
    }
    public void SetOpening()
    {
        this.opening = true;
    }
    public void SetClosing()
    {
        this.closing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.opening)
        {
            transform.position = Vector3.MoveTowards(transform.position, this.targetPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, this.targetPosition) < 0.01f)
            {
                this.opening = false;
            }
        }
        if (this.closing)
        {
            transform.position = Vector3.MoveTowards(transform.position, this.startPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, this.startPosition) < 0.01f)
            {
                this.closing = false;
            }
        }
    }
}
