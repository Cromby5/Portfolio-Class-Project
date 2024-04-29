using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] private float speed = 5.0f;
    Rigidbody rigidPlayer;
    // Attack Trails
    [SerializeField] List<GameObject> Trails;

    bool isGrounded = true;
    string currentState;

    int currentAttack = 0;
    float timeLastInput = 0.0f;
    float timeSinceComboStringEnd = 0.0f; // Since the last combo string ended,

    [Header("Adjust Attack Timings")]
    [SerializeField] float timeComboStringEndCheck = 0.5f; // How long to wait after the combo string ends
    [SerializeField] float timeBetweenAttacks = 0.2f; // How long to wait before the next attack can be input
    [SerializeField] float timeBeforeComboEnd = 1f; // How long to wait before the combo cannot be continued


    [SerializeField] List<AttackScriptableObjectTemplate> attacks; // This should be a list of attacks, each attack should have a scriptable object containing the animation that can be swapped out 
    [SerializeField] List<AttackScriptableObjectTemplate> jumpAttack; // Air attacks
    
    [SerializeField] List<BoxCollider> hitBoxes;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI currentAttackDisplay;

    private void Awake()
    {
        rigidPlayer = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        EndAttack();
        currentAttackDisplay.text = "Current Attack: " + currentAttack;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);
        //transform.Translate(movement * speed * Time.deltaTime, Space.World);
        rigidPlayer.AddForce(movement.normalized * speed, ForceMode.Acceleration);

        if (movement != Vector3.zero)
        {
            animator.SetFloat("Speed", 1.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }

    private void Jump()
    {
        // Jump Attack
        CameraShake.instance.ShakeCamera(1f, 1f); // move this to the scriptable object?
    }

    private void Attack()
    {
       if (Time.time - timeSinceComboStringEnd > timeComboStringEndCheck && currentAttack < attacks.Count)
        {
            CancelInvoke("EndCombo"); 
            Debug.Log(Time.time - timeSinceComboStringEnd);
            if (Time.time - timeLastInput >= timeBetweenAttacks)
            {
                animator.runtimeAnimatorController = attacks[currentAttack].animatorOverride; // Change the animation by overriding the controller
                animator.speed = attacks[currentAttack].attackSpeed; // Change the speed of the animation
                Debug.Log(animator.speed);
                animator.Play("Attack",0,0); // Play the attack animation

                hitBoxes[(int)attacks[currentAttack].attackType].enabled = true; // Enable the hitbox for the attack
                Trails[(int)attacks[currentAttack].attackType].SetActive(true); // Play the trail for the attack
                // Maniuplate the time to combo end to allow attacks to be different lengths

                currentAttack++;
                timeLastInput = Time.time;

                if (currentAttack > attacks.Count)
                {
                    //Debug.Log(attacks.Count);
                    currentAttack = 0;
                }
            }
        }
    }
    private void EndAttack()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {       
            // Start the combo end timer, disable all colliders after the animation has finished / gone past a % mark
            Invoke("EndCombo", timeBeforeComboEnd);
            //Trails[(int)attacks[currentAttack-1].attackType].SetActive(false);
            DisableAllColliders();
        }
    }
    private void EndCombo()
    {
        currentAttack = 0;
        animator.speed = 1f;
        DisableTrails();
        timeSinceComboStringEnd = Time.time;
    }

    private void DisableAllColliders() // worst colliders of all time at the moment
    {
        foreach (BoxCollider collider in hitBoxes)
        {
            collider.enabled = false;
        }
    }

    private void DisableTrails()
    {
        foreach (GameObject g in Trails)
        {
            g.SetActive(false);
        }
    }

    // Animation Experiments
    // Moving away from relying on unitys built in animation system, it can be a mess. Though I lose animation blending
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void Observer(string message)
    {
        if (message.Equals("AttackEnd"))
        {
            currentState = "Idle";
        }
    }
}
