using UnityEngine;
using Doublsb.Dialog;
using UnityEngine.SceneManagement;

// Ensure the component is present on the gameobject the script is attached to
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControllerTD : MonoBehaviour
{
    // Local rigidbody variable to hold a reference to the attached Rigidbody2D component
    new Rigidbody2D rigidbody2D;

    public float movementSpeed = 1000.0f;

    public float PlayerHealth = 10f;

    public DialogManager DialogManager;

    public Animator PlayerAnim;
    public Animator AttackAnims;
    public Animator ParticleAnims;

    void Awake()
    {
        DialogManager = GameObject.FindGameObjectWithTag("DDmanager").GetComponent<DialogManager>();
        DialogManager.Printer.SetActive(false);

        // Setup Rigidbody for frictionless top down movement and dynamic collision
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.isKinematic = false;
        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 0.0f;
    }

    void FixedUpdate()
    {
        // Handle user input
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Move(targetVelocity);
    }

    void Move(Vector2 targetVelocity)
    {
        if (DialogManager.Printer.activeInHierarchy == false)
        {
            // Set rigidbody velocity
            rigidbody2D.velocity = (targetVelocity * movementSpeed) * Time.deltaTime; // Multiply the target by deltaTime to make movement speed consistent across different framerates
        }
    }

    private void Update()
    {
        if (PlayerHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DialogManager.Click_Window();
        }

        if(DialogManager.Printer.activeInHierarchy == true)
        {
            rigidbody2D.velocity = new Vector2(0, 0);
        }

        if(rigidbody2D.velocity.magnitude >= 0.1f)
        {
            PlayerAnim.SetBool("isWalking", true);
        }
        else
        {
            PlayerAnim.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackAnims.SetTrigger("Attack");
            ParticleAnims.SetTrigger("ShootMelee");
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            AttackAnims.SetTrigger("StopSwing");
            ParticleAnims.SetTrigger("StopMeleeShoot");
        }
    }

    private void Start()
    {
        DialogManager.Hide();
    }

    public void TakeDamage(float damageAmount)
    {
        PlayerHealth = PlayerHealth - damageAmount;
    }
}