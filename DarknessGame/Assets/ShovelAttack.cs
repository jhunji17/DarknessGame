using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAttack : MonoBehaviour
{
    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode right;
    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode attackKey;

    public GameObject shovel;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        PlayerMovement.somethingGotHit += destroyThisShovel;
    }

    private void OnDisable()
    {
        PlayerMovement.somethingGotHit -= destroyThisShovel;
    }

    public void Start()
    {
        animator.GetComponent<Animator>();
        spriteRenderer.GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        HandleAttackBoxMovement();
        HandleAnimation();
    }

    private void HandleAttackBoxMovement()
    {
        Vector2 newPos = new Vector2();

        if (Input.GetKey(left))
        {
            newPos = new Vector2(-0.4f, 0.55f);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 90);
            spriteRenderer.flipX = true;
            
        }
        if (Input.GetKey(right))
        {
            newPos = new Vector2(-0.4f, -0.9f);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 270);
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(down))
        {
            newPos = new Vector2(0.5f, 0);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 180);
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(up))
        {
            newPos = new Vector2(-1, 0);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipX = false;
        }

    }

    private void HandleAnimation()
    {
        if (Input.GetKeyDown(attackKey))
        {
            animator.SetTrigger("attack");
        }
    }

    public void destroyThisShovel()
    {
        // wait for some seconds / play an animation
        Destroy(shovel);
        Destroy(this);
    }
}
