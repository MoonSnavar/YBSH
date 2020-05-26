using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Атрибуты персонажа:")]
    public float movementBaseSpeed = 4.0f;
    public float currentSpeed;
    [Space]
    [Header("Статистика персонажа:")]
    public Vector2 movementDirection;
    public float movementSpeed;
    [Header("Ссылка на джойстик")]
    public VariableJoystick joyStick;
    [Space]
    [Header("Ссылка на аниматор игрока")]    
    public Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private AudioSource walkingSound;
    private int soundState;


    // Start is called before the first frame update
    void Start()
    {
        soundState = PlayerPrefs.GetInt("Sounds");
        walkingSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();        
        currentSpeed = movementBaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();            
        Move();
        CheckFlip();
        Animations();    
    }


    private void Animations()
    {
        if (!PlayerManager.isGetProducts)
        {
            if (movementDirection.y > 0)
            {
                PlayWalkSound();
                animator.SetInteger("Walk", 1);
            }
            else if (movementDirection.y < 0)
            {
                PlayWalkSound();
                animator.SetInteger("Walk", 2);
            }
            else if (movementDirection.x != 0)
            {
                PlayWalkSound();
                animator.SetInteger("Walk", 0);
            }
            else
            {
                walkingSound.Stop();
                animator.SetInteger("Walk", 3);
            }
        }
        else
        {
            if (movementDirection.y > 0)
            {
                PlayWalkSound();
                animator.SetInteger("Walk", 4);
            }
            else if (movementDirection.y < 0)
            {
                PlayWalkSound();
                animator.SetInteger("Walk", 5);
            }
            else if (movementDirection.x != 0)
            {
                PlayWalkSound();
                animator.SetInteger("Walk", 6);
            }
            else
            {
                walkingSound.Stop();
                animator.SetInteger("Walk", 7);
            }
        }
    }

    public void ProcessInputs()
    {
        movementDirection = new Vector2(joyStick.Horizontal, joyStick.Vertical);
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();
    }
    public void ForciblySetAnimation()
    {        
        animator.SetTrigger("IdleDown");
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
    void PlayWalkSound()
    {
        if (!walkingSound.isPlaying)
        {
            if (soundState == 0)
                walkingSound.Play();
        }
    }
}
