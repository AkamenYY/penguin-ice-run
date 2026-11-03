using UnityEngine;
using UnityEngine.InputSystem;

// Пингвин: бег, прыжок, снежки, анимации.
// Ввод (Input System): компонент PlayerInput сам вызывает OnMove / OnJump / OnFire.
public class PenguinController : MonoBehaviour
{
    public float speed = 5.5f;     // скорость бега
    public float jumpSpeed = 7f;   // сила прыжка
    public Animator anim;          // анимации модели: Idle / Run / Jump / Wave
    public AudioClip jumpClip;     // звук прыжка

    Rigidbody rb;
    Vector2 move;              // куда жмут стик (x — влево/вправо, y — вперёд/назад)
    bool grounded;             // стоим на льду?
    MovingPlatform platform;   // плывущая льдина под ногами

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ===== Ввод =====

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (!grounded) return;   // прыгаем только со льда
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpSpeed, rb.linearVelocity.z);
        GetComponent<AudioSource>().PlayOneShot(jumpClip);
    }

    public void OnFire()
    {
        anim.SetTrigger("Wave");          // взмах крылом
        GetComponent<Thrower>().Throw();  // снежок
    }

    // ===== Физика =====

    void FixedUpdate()
    {
        // 1. Стоим на льду? Смотрим, есть ли что-то в маленьком шаре у ног.
        Collider[] under = Physics.OverlapSphere(transform.position + Vector3.up * 0.2f, 0.3f,
                                                 Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        grounded = under.Length > 0 && rb.linearVelocity.y < 0.5f;
        platform = grounded ? under[0].GetComponent<MovingPlatform>() : null;

        // 2. Направление относительно камеры: стик вверх = «от камеры»
        Transform cam = Camera.main.transform;
        Vector3 forward = cam.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = cam.right; right.y = 0f; right.Normalize();
        Vector3 dir = forward * move.y + right * move.x;
        if (GameManager.I.Ending) dir = Vector3.zero;   // смерть / победа — стоим

        // 3. Скорость по полу (+ скорость льдины, если едем на ней). Вертикальную не трогаем.
        Vector3 vel = dir * speed;
        if (platform != null) vel += platform.Velocity;
        rb.linearVelocity = new Vector3(vel.x, rb.linearVelocity.y, vel.z);

        // 4. Поворачиваемся туда, куда бежим
        if (dir != Vector3.zero) rb.MoveRotation(Quaternion.LookRotation(dir));
    }

    void Update()
    {
        anim.SetBool("Run", move != Vector2.zero);
        anim.SetBool("Grounded", grounded);

        if (transform.position.y < -2.5f) GameManager.I.Restart();   // упал в воду
    }
}
