using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Player : MonoBehaviour
{
    // Movement Attributes
    [Header("General")]
    [SerializeField]
    protected AnimationCurve animateCurve;
    [SerializeField]
    public CinemachineFreeLook cinemachineCamera;

    [Header("Health & Mana")]
    [SerializeField]
    public RectTransform healthBar;
    [SerializeField]
    public RectTransform manaBar;

    [Header("Weapons")]
    [SerializeField]
    protected PlayerWeapon weapon;
    [SerializeField]
    protected GameObject specialProjectile, explosion, blood;
    [SerializeField]
    protected Transform projectileSpawnpoint;

    // General
    protected float coefficient = 30f;
    protected Animator animator;
    protected Rigidbody rb;
    private float speed;
    private float ySpeed = 0;
    protected float rotationSpeed = 300f;
    private Transform floatingHealth;

    // Child Attributes (Will be overrided)
    protected float jumpSpeed = 5f;
    protected float walkingSpeed = 120f;
    protected float runningSpeed = 180f;
    protected float hp = 350f;
    protected float maxHp = 350f;
    protected float maxMana = 50f;
    protected float mana = 50f;
    protected float basicDamage = 0f;
    protected bool isFps = false;
    protected string walkingTerrain = "wet";

    // Time Intervals
    private static float inventoryTime = 0;
    protected float animateTime = 0.1f;
    protected float walkTime = 0.1f;

    // Movement Attributes
    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;
    private bool isGrounded = false;
    private bool isActive = false;
    private bool isOnStone = false;
    private bool isOnWood = false;
    private bool isRunning = false;

    // Inventory
    [HideInInspector]
    public float buffInterval = 0f;
    [HideInInspector]
    public bool speedBuff = false;
    [HideInInspector]
    public float buffAmount = 0f;

    protected AudioManager audioManager;
    private Audio3D audio3D;

    // A* Attributes
    private Vector3 prevPos = new Vector3(-10000, -10000, -10000);
    private PlayerBot botMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioManager = AudioManager.Instance;
        audio3D = gameObject.GetComponent<Audio3D>();
        speed = walkingSpeed;
        floatingHealth = transform.Find("Healthbar");
        botMovement = gameObject.GetComponent<PlayerBot>();
        StartChildren();
    }

    void FixedUpdate()
    {
        if (MenuConfig.Instance != null && MenuConfig.Instance.IsShown)
            return;

        if (isActive)
        {
            if (!isFps)
            {
                SurfaceAlignment();
            }
            Movement();
        }

        FixedUpdateChildren();

        // Gravity
        if (ySpeed >= 0) ySpeed += Physics.gravity.y;
        else ySpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuConfig.Instance != null && MenuConfig.Instance.IsShown)
            return;

        // If player is active
        if (isActive)
        {
            CheckMovement();
            CheckJump();
        }
        // Else if bot
        else
        {
            botMovement.BotMovement();
            AnimateBotMovement();
        }

        Buff();
        UpdateChildren();
        AttackListener();
    }

    private void AttackListener()
    {
        if (Input.GetMouseButton(0) && isActive)
        {
            BasicAttack();
        }
        if (Input.GetMouseButton(1) && isActive)
        {
            HeavyAttack();
        }
        if (Input.GetKeyDown(KeyCode.X) && isActive)
        {
            SpecialAttack();
        }

        if (Input.GetKeyDown(KeyCode.I) && Time.time - inventoryTime > 0.2)
        {
            inventoryTime = Time.time;
            Inventory.Instance.ShowInventory(true);
        }
    }

    protected abstract void StartChildren();
    protected abstract void UpdateChildren();
    protected abstract void FixedUpdateChildren();
    protected abstract void BasicAttack();
    protected abstract void HeavyAttack();
    protected abstract void SpecialAttack();
    public abstract void AttackSound(int variation);

    private void CheckMovement()
    {
        // Check for running and walking events
        IsMoving();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (wPressed || aPressed || sPressed || dPressed)
        {
            animator.SetBool("isWalking", true);
            if (isRunning)
            {
                animator.SetBool("isRunning", true);
                speed = runningSpeed;
            }
            if (isGrounded)
            {
                if (isOnWood) 
                    audioManager.Walk("wood", speed);
                else if (isOnStone) 
                    audioManager.Walk("stone", speed);
                else
                    audioManager.Walk(walkingTerrain, speed);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("isRunning", false);
            speed = walkingSpeed;
            isRunning = false;
        }

        if (speed == runningSpeed)
        {
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.m_Lens.FieldOfView, 70, animateCurve.Evaluate(animateTime));
        }
        else if (cinemachineCamera.m_Lens.FieldOfView > 60)
        {
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineCamera.m_Lens.FieldOfView, 60, animateCurve.Evaluate(animateTime));
        }

    }

    private void CheckJump()
    {
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded && this.jumpSpeed > 0)
        {
            ySpeed = jumpSpeed;
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
        rb.AddForce(0, ySpeed, 0);
    }

    private void IsMoving()
    {
        // If keydown
        if (Input.GetKeyDown(KeyCode.W)) wPressed = true;
        if (Input.GetKeyDown(KeyCode.A)) aPressed = true;
        if (Input.GetKeyDown(KeyCode.S)) sPressed = true;
        if (Input.GetKeyDown(KeyCode.D)) dPressed = true;
        // If keyup
        if (Input.GetKeyUp(KeyCode.W)) wPressed = false;
        if (Input.GetKeyUp(KeyCode.A)) aPressed = false;
        if (Input.GetKeyUp(KeyCode.S)) sPressed = false;
        if (Input.GetKeyUp(KeyCode.D)) dPressed = false;
    }

    private void Movement()
    {
        // Get movement axises
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 relativeX = x * Camera.main.transform.right;
        Vector3 relativeY = y * Camera.main.transform.forward;

        // Move player
        Vector3 direction = relativeX + relativeY;
        Vector3 counterDirection = new Vector3(-rb.velocity.x, 0f, -rb.velocity.z);

        direction.y = 0;

        rb.AddForce(direction * speed);
        rb.AddForce(counterDirection * coefficient);

        // Rotate player according to direction
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion surfaceRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, toRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, surfaceRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SurfaceAlignment()
    {
        if (!isGrounded) return;

        Ray raycast = new Ray(transform.position, -transform.up);
        RaycastHit rayInfo;

        if (Physics.Raycast(raycast, out rayInfo, 1 << LayerMask.GetMask("Ground")))
        {
            Vector3 direction = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Keep current forward direction if direction is too small
            if (direction.magnitude < 0.01f)
            {
                direction = transform.forward;
            }
            else direction.Normalize();

            Vector3 surfaceRight = Vector3.Cross(rayInfo.normal, direction).normalized;
            Vector3 surfaceDirection = Vector3.Cross(surfaceRight, rayInfo.normal);
            Quaternion targetRotation = Quaternion.LookRotation(surfaceDirection, rayInfo.normal);

            // Rotate player based on targetRotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, animateCurve.Evaluate(animateTime));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;

        if (layer == 6 || layer == 12 || layer == 13 || layer == 8)
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isOnWood = false;
            isOnStone = false;

            if (layer == 12) isOnWood = true;
            else if (layer == 13) isOnStone = true;
        }
    }

    public void UpdateHealth()
    {
        if (isActive)
        {
            floatingHealth.gameObject.SetActive(false);
            healthBar.localScale = new Vector3((hp / maxHp) * 0.1f, 0.022f, 0.7f);
            if (hp > maxHp * 2 / 3)
            {
                healthBar.GetComponent<Image>().color = Color.green;
            } 
            else if (hp > maxHp * 1 / 3)
            {
                healthBar.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                healthBar.GetComponent<Image>().color = Color.red;
            }
            healthBar.GetComponent<Image>().CrossFadeAlpha(0.1f, 0f, false); ;
        }
        else
        {
            floatingHealth.gameObject.SetActive(true);
            floatingHealth.Find("Health").localScale = new Vector3((hp / maxHp) * 0.218f, 0.029f, 1f);
            if (hp > maxHp * 2 / 3)
            {
                floatingHealth.Find("Health").GetComponent<Image>().color = Color.green;
            }
            else if (hp > maxHp * 1 / 3)
            {
                floatingHealth.Find("Health").GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                floatingHealth.Find("Health").GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void UpdateMana()
    {
        if (!isActive) return;

        manaBar.localScale = new Vector3((mana / maxMana) * 0.07f, 0.022f, 0.7f);
    }

    public void Buff()
    {
        if (speedBuff)
        {
            speed = runningSpeed * buffAmount;

            if (buffInterval > Time.time)
            {
                speedBuff = false;
            }
        }
    }

    public void DeductHp(float damage)
    {
        if (damage <= 0) return;

        audio3D.Gore();
        Instantiate(blood, transform.position, transform.rotation);
        hp -= damage;
        UpdateHealth();

        if (hp <= 0)
        {
            if (isActive)
                PlayerManager.Instance.SwitchPlayer();
            PlayerManager.Instance.Remove(this);
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    public void AddHp(float amount)
    {
        hp += amount;

        if (hp > maxHp) hp = maxHp;

        UpdateHealth();
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    public void AddMana(float amount)
    {
        mana += amount;

        if (mana > maxMana) mana = maxMana;

        UpdateMana();
    }

    public bool DeductMana(float amount)
    {
        if (mana < amount) return false;

        mana -= amount;
        UpdateMana();
        return true;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }


    // Setter getter
    public bool GetActive()
    {
        return isActive;
    }
    public virtual void SetPlayerActive(bool x)
    {
        isActive = x;
        ToggleBot(!x);
    }
    public void ToggleBot(bool x)
    {
        // If set to bot
        if (x)
        {
            animator.SetBool("basic1", false);
            animator.SetBool("basic2", false);
            animator.SetBool("basic3", false);
        }
        // If player
        else
        {
            animator.SetBool("botAttack", false);
        }
    }
    private void AnimateBotMovement()
    {
        if (Time.time - walkTime > 0.3)
        {
            walkTime = Time.time + 0.3f;

            // If following path, animate move
            if (Vector3.Distance(prevPos, transform.position) > 0.1)
            {
                prevPos = transform.position;
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", true);
                animator.SetBool("botAttack", false);
                weapon.SetDamage(0f);
                animator.SetBool("basic1", false);
            }
            // Else if close enough, attack
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("botAttack", true);
                weapon.SetDamage(basicDamage);
            }
        }

        if (animator.GetBool("isRunning"))
        {
            audio3D.Walk(speed / 85);
        }
    }

    public bool GetFps()
    {
        return isFps;
    }
    public void SetFps(bool x)
    {
        isFps = x;
    }

    
}
