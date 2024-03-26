using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] Canvas pauseCanvas;
    [SerializeField] Canvas gameplayCanvas;
    [SerializeField] Canvas galleryCanvas;
    [SerializeField] UIControls uIControls;

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
            GameObject _nextBullet = GetNextObject();
            _nextBullet.SetActive(true);

            cdTimer = false;

            StartCoroutine(ShootingDelay());
        }
        Pause();
    }

    void CameraRotation()
    {
        float _mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float _mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationSpeed -= _mouseY;
        rotationSpeed = Mathf.Clamp(rotationSpeed, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationSpeed, 0, 0);

        transform.Rotate(Vector3.up * _mouseX);
    }

    void Movement()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        Vector3 _move = transform.right * _horizontalInput + transform.forward * _verticalInput;

        playerCC.Move(_move * moveSpeed * Time.deltaTime);

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
        GameObject _nextBullet = NextInactiveBullet();
        currentIndex = (currentIndex + 1) % bulletPoolSize; //Cycle through the pool 
        _nextBullet.transform.position = ProjectileSocket.transform.position;
        _nextBullet.transform.rotation = ProjectileSocket.transform.rotation;
        _nextBullet.SetActive(true); //Everytime next bullet is called set transforms and turn it on

        return _nextBullet;
    }

    GameObject NextInactiveBullet()
    {
        GameObject _nextBullet;

        var _inactiveBullets = bulletsPool.Where(b => b.activeSelf == false).ToList();
        if (_inactiveBullets.Count == 0) 
        {
            BulletPrefab = Instantiate(BulletPrefab, ProjectileSocket.transform.position, ProjectileSocket.transform.rotation);

            bulletsPool.Add(BulletPrefab);
            _nextBullet = BulletPrefab;
            BulletPrefab.SetActive(false);
        }
        else
        {
            _nextBullet = _inactiveBullets.First();
        }
        return _nextBullet;
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

    private void Pause()
    {
        if (Input.GetButton("Cancel"))
        {
            if (uIControls.InGallery == true)
            {
                galleryCanvas.GetComponent<Canvas>().enabled = false;
            }
            if (uIControls.InGameplay == true)
            {
                gameplayCanvas.GetComponent<Canvas>().enabled = false;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseCanvas.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0f;
        }
    }
}
