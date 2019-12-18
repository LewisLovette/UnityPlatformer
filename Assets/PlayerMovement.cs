using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public BoxCollider2D playerBox;

    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool crouch = false;
    bool platformDrop = false;
    float fallTime = 0;

    //FPS
    float deltaTime = 0.0f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;


        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        //Set the animator (as we moved it to player movement) Speed that we've set a condition for so that it changed to a run animation
        //Mathf.Abs makes sure it's always positive

        //when holding crouch, can go into platform drop with jump
        if (Input.GetButton("Crouch"))
        {
            if (Input.GetButtonDown("Jump"))  //fall through platforms. -will only work when grounded
            {
                platformDrop = true;
                controller.UpdatePlatformDrop(platformDrop);
            }
        }

        if (Input.GetButtonDown("Jump") && Input.GetButtonDown("Crouch"))  //fall through platforms. -will only work when grounded
        {
            platformDrop = true;
            controller.UpdatePlatformDrop(platformDrop);
        }
        else if (Input.GetButtonDown("Jump") && !platformDrop) //doesn't trigger jump animation for crouhc then delayed jump
        {
            jump = true;
            animator.SetBool("isJump", true);

            playerBox.enabled = false;

            //disable platform layer FOR BOX collider during jump.

        }

        if (platformDrop && fallTime > 0.3) //so the player can only fall for a *set" period (stop player being pushed up but tapping and being middle of collision box)
        {
            platformDrop = false;
            controller.UpdatePlatformDrop(platformDrop);
            fallTime = 0;
            crouch = false;
        }
        else if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;

        }
        else if (Input.GetButtonUp("Crouch") && !platformDrop) //so player doesn't idle if tapping platform drop combo
        {
            crouch = false;

        }
        
        //Debug.Log(platformDrop + " Time: " + fallTime);

    }
    
    //For FPS counter
    void OnGUI()    
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    public void OnLanding()
    {
        animator.SetBool("isJump", false);
    }

    public void OnCrouch(bool isCrouch)
    {
        animator.SetBool("isCrouch", isCrouch);
    }

    void FixedUpdate()
    {
        //Moving the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        //Time.fixedDeltaTime means consistency no matter how many times this is called per second
        if(platformDrop) fallTime += Time.fixedDeltaTime;
        jump = false;

    }
}
