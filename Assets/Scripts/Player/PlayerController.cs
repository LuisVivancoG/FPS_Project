using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int Health;
    [SerializeField] float RecoverTime;
    bool isinvulnerable;

    [Header("Movement")]
    [SerializeField] float sensitivity;
    [SerializeField] GameObject playerCamera;
    float rotationSpeed = 0f;
    [Space]
    CharacterController playerCC;
    [SerializeField] float moveSpeed;
    [Space]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight;
    Vector3 velocity;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = .4f;
    [SerializeField] LayerMask groundLayerMask;
    bool isGrounded;

    [Header("Projectiles")]
    public GameObject ProjectileSocket;
    public GameObject BulletPrefab;
    [SerializeField] int bulletPoolSize;
    List<GameObject> bulletsPool;
    int currentIndex = 0;

    [Range(0f, 3f)]
    [SerializeField] float shootDelay;
    bool cdTimer = true;

    [SerializeField] Image ImageColorHealth;
    [SerializeField] AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCC = GetComponent<CharacterController>();
        InitializeObjectPool();
    }

    // Update is called once per frame
    void Update()
    {
        // Check collision with ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Gravity();
        CameraRotation();
        Movement();

        // Spawn projectile
        if (Input.GetMouseButtonDown(0) && cdTimer == true && Time.timeScale != 0)
        {
            audioManager.Play("PiranhaCannon");
            GameObject _nextBullet = GetNextObject();
            _nextBullet.SetActive(true);

            cdTimer = false;

            StartCoroutine(ShootingDelay());
        }
    }

    void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationSpeed -= mouseY;
        rotationSpeed = Mathf.Clamp(rotationSpeed, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationSpeed, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        playerCC.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;
        playerCC.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(int Damage)
    {
        if (isinvulnerable == false)
        {
            Vector3 newSizeImage = ImageColorHealth.transform.localScale / (Health - (float)Damage);

            ImageColorHealth.transform.localScale -= newSizeImage;
            Health -= Damage;
            //Debug.Log("Player current hp is " + Health);
            StartCoroutine(Invulnerability());
            HealthCheck();
        }
    }

    void HealthCheck()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene("Gym");
        }
    }

    void InitializeObjectPool()
    {
        bulletsPool = new List<GameObject>(); //Creates a new list same size as the bullets pool

        for (int i = 0; i < bulletPoolSize; i++) //Instantiates a prefab for each index on the list and turn them off
        {
            BulletPrefab = Instantiate(BulletPrefab, ProjectileSocket.transform.position, ProjectileSocket.transform.rotation);

            bulletsPool.Add(BulletPrefab);
            BulletPrefab.SetActive(false);
        }
    }

    public GameObject GetNextObject()
    {
        GameObject nextBullet = NextInactiveBullet();
        currentIndex = (currentIndex + 1) % bulletPoolSize; //Cycle through the pool 
        nextBullet.transform.position = ProjectileSocket.transform.position;
        nextBullet.transform.rotation = ProjectileSocket.transform.rotation;
        nextBullet.SetActive(true); //Everytime next bullet is called set transforms and turn it on

        return nextBullet;
    }

    GameObject NextInactiveBullet()
    {
        GameObject nextBullet;

        var inactiveBullets = bulletsPool.Where(b => b.activeSelf == false).ToList();
        if (inactiveBullets.Count == 0) 
        {
            BulletPrefab = Instantiate(BulletPrefab, ProjectileSocket.transform.position, ProjectileSocket.transform.rotation);

            bulletsPool.Add(BulletPrefab);
            nextBullet = BulletPrefab;
            BulletPrefab.SetActive(false);
        }
        else
        {
            nextBullet = inactiveBullets.First();
        }
        return nextBullet;
    }

    IEnumerator ShootingDelay() //Courutine of 1 second
    {
        yield return new WaitForSeconds(shootDelay);
        //Debug.Log("Ready to shoot");
        cdTimer = true;
    }

    IEnumerator Invulnerability()
    {
        isinvulnerable = true;

        yield return new WaitForSeconds(RecoverTime);

        isinvulnerable = false;
    }
}
