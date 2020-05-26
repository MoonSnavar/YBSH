using UnityEngine;

public class FreeWalking : MonoBehaviour
{
    [Header("Атрибуты персонажа:")]
    public float currentSpeed = 2.0f;
    [Space]
    [Header("Статистика персонажа:")]
    public Vector2 movementDirection;
    public float movementSpeed;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    [Space]
    [Header("Ссылка на аниматор")]
    private Animator animator;
    private bool isStop;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isStop = false;
        Invoke("Walking", 2f);
        InvokeRepeating("Stop", 4f, 4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Walking();
    }
    private void Stop()
    {
        isStop = true;
        Walking();
        CancelInvoke("Walking");
        Invoke("Walking", 2f);
    }
    private void Walking()
    {
        if (!isStop)
            ChangeDirection(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        else
            ChangeDirection(0, 0);
        Move();
        CheckFlip();
        isStop = false;
        Animations();
    }

    private void Animations()
    {
        float tempY=0;
        if (movementDirection.y != 0)
                tempY = 1 - movementDirection.y;
        float tempX=0;
        if (movementDirection.x != 0)
            tempX = 1 - movementDirection.x;

        if (tempY != 0 && tempX != 0)
        {
            if (tempX < tempY)
                animator.SetInteger("Walk", 0);
            else if (movementDirection.y > 0)
            {
                animator.SetInteger("Walk", 1);
            }
            else if (movementDirection.y < 0)
            {
                animator.SetInteger("Walk", 2);
            }
        }
        else
            animator.SetInteger("Walk", 3);        
    }

    private void ChangeDirection(float directionX, float directionY)
    {
        movementDirection = new Vector2(directionX, directionY);
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();
    }
    private void Move()
    {
        rb.velocity = movementDirection * movementSpeed * currentSpeed;
    }

    private void CheckFlip()
    {
        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if (movementDirection.x > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (movementDirection.x < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
    }
}
