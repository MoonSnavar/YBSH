using UnityEngine;

public class HorizontalWalking : MonoBehaviour
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
    public Animator animator;
    [Header("Ссылка на префаб следа вируса")]
    public GameObject virusMark;
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();        
    }
    private void OnEnable()
    {
        if (transform.localScale.x > 0)
            flag = true;
        else
            flag = false;
        InvokeRepeating("SpawnVirusMark", 1f, 0.7f);
    }
    private void OnDisable()
    {
        CancelInvoke("SpawnVirusMark");
        CancelInvoke("WalkRight");
        CancelInvoke("WalkLeft");
    }
    
    void Update()
    {
        if (transform.localPosition.x >= -3 && transform.localPosition.x <= -2.5f)
        {
            if (flag)
            {
                CancelInvoke("SpawnVirusMark");
                CancelInvoke("WalkLeft");
                animator.SetInteger("Walk", 3);
                InvokeRepeating("SpawnVirusMark", 2f, 0.7f);
                InvokeRepeating("WalkRight", 2f, 0.1f);
                flag = false;
            }
        }
        else if(transform.localPosition.x <= 3 && transform.localPosition.x >= 2.5f)
        {
            if (!flag)
            {
                CancelInvoke("SpawnVirusMark");
                CancelInvoke("WalkRight");
                animator.SetInteger("Walk", 3);
                InvokeRepeating("SpawnVirusMark", 2f, 0.7f);
                InvokeRepeating("WalkLeft", 2f, 0.1f);
                flag = true;
            }
        }
    }

    private void SpawnVirusMark()
    {
        Instantiate(virusMark,new Vector3(transform.position.x, transform.position.y + 0.2f),virusMark.transform.localRotation);
    }

    private void WalkRight()
    {
        Walking();
        ChangeDirection(1);
    }

    private void WalkLeft()
    {
        Walking();
        ChangeDirection(-1);
    }
    private void Walking()
    {
        Move();
        CheckFlip();
        Animations();
    }

    private void Animations()
    {
        if (movementDirection.x != 0)
        {
            animator.SetInteger("Walk", 0);
        }
        else
        {
            animator.SetInteger("Walk", 3);
        }
    }

    void ChangeDirection(int direction)
    {
        movementDirection = new Vector2(1 * direction, 0);
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();
    }
    void Move()
    {
        rb.velocity = movementDirection * movementSpeed * currentSpeed;
    }

    void CheckFlip()
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
