using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAttack : MonoBehaviour
{
    PlayerMovement shovelMovement;

    public GameObject shovel;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    public bool noShovel = false;

    private void OnEnable()
    {
        PlayerMovement.shovelBreaker += destroyThisShovel;
    }

    private void OnDisable()
    {
        PlayerMovement.shovelBreaker -= destroyThisShovel;
    }

    public void Start()
    {
        animator.GetComponent<Animator>();
        spriteRenderer.GetComponent<SpriteRenderer>();
        shovelMovement = GetComponentInParent<PlayerMovement>();
    }

    public void Update()
    {
        HandleAttackBoxMovement();
        HandleAnimation();
    }

    private void HandleAttackBoxMovement()
    {
        Vector2 newPos = new Vector2();

        if (Input.GetKey(shovelMovement.left))
        {
            newPos = new Vector2(-0.4f, 0.55f);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 90);
            spriteRenderer.flipX = true;
            
        }
        if (Input.GetKey(shovelMovement.right))
        {
            newPos = new Vector2(-0.4f, -0.9f);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 270);
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(shovelMovement.down))
        {
            newPos = new Vector2(0.5f, 0);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 180);
            spriteRenderer.flipX = false;
        }

        if (Input.GetKey(shovelMovement.up))
        {
            newPos = new Vector2(-1, 0);
            shovel.transform.localPosition = newPos;
            shovel.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteRenderer.flipX = false;
        }

    }

    private void HandleAnimation()
    {
        if (Input.GetKeyDown(shovelMovement.attackKey))
        {
            animator.SetTrigger("attack");
        }
    }

    public void destroyThisShovel(bool isRed)
    {
        Debug.Log("destroy shovel called");
        // wait for some seconds / play an animation
        if(isRed && transform.parent.parent.name == "red")
        {
            noShovel = true;
            Destroy(shovel);
            Destroy(this);
        }

        else if(!isRed && transform.parent.parent.name == "blue")
        {
            noShovel = true;
            Destroy(shovel);
            Destroy(this);
        }

        else
        {
            return;
        }
        
    }
}
