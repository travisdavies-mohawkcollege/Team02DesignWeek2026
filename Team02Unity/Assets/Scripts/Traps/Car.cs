using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool directionSet;
    public CarDirection direction;
    public Vector2 directionVector;
    public float speed = 3;
    public float timerMax = 3f;
    public float timer = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = 3f;
    }

    void Update()
    {
        if (!directionSet) return;
        rb.AddForce(directionVector * speed, ForceMode2D.Impulse);
        timer -= Time.deltaTime;
        if (timer < timerMax)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(CarDirection _direction)
    {
        switch (_direction)
        {
            case CarDirection.up:
                direction = _direction;
                directionVector.Set(0, 1);
                directionSet = true;
                return;
            case CarDirection.down:
                direction = _direction;
                directionVector.Set(0, -1);
                directionSet = true;
                break;
            case CarDirection.left:
                direction = _direction;
                directionVector.Set(-1, 0);
                directionSet = true;
                break;
            case CarDirection.right:
                direction = _direction;
                directionVector.Set(1, 0);
                directionSet = true;
                break;

        }
            
    }
}
