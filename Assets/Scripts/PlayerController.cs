using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D theRB;

    public Transform gunArm;

    public Animator anim;

    /*public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;*/

    public SpriteRenderer[] playerSR;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLenght = .5f, dashCooldown = 1f, dashInvincibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float  dashCoolCounter;

    public GameObject[] dashIndicators;
    public GameObject dashIndicatorsHolder;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> avaliableGuns = new List<Gun>();
    
    [HideInInspector]
    public int currentGun;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //theCam = Camera.main;
        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = avaliableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = avaliableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {

        if (canMove && !LevelManager.instance.isPaused)
        {
            //player movement
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            theRB.velocity = moveInput * activeMoveSpeed;

            //aiming

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.transform.localScale = new Vector3(-1f, -1f, 1f);
                dashIndicatorsHolder.transform.localScale = new Vector3(-1f, 1f, 1f);

            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.transform.localScale = Vector3.one;
                dashIndicatorsHolder.transform.localScale = Vector3.one;

            }

            // rotate gun arm
            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            /*
            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0 && dashCounter <= 0)
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(12);
                }
            }*/

            //dash
            if (Input.GetKeyDown(KeyCode.Tab) && avaliableGuns.Count > 0)
            {
                currentGun++;
                if (currentGun >= avaliableGuns.Count) currentGun = 0;
                SwitchGun();
            }

            if (Input.GetKeyDown(KeyCode.Space) && dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLenght;
                AudioManager.instance.PlaySFX(8);
                anim.SetTrigger("dash");
                PlayerHealthController.instance.MakeInvincavle(dashInvincibility);

            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                    StartCoroutine(WaitForPointTwentyFive());
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;

            }
            //animation
            if (moveInput != Vector2.zero) anim.SetBool("isMoving", true);
            else anim.SetBool("isMoving", false);
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }


    //dash reload indicator
    IEnumerator WaitForPointTwentyFive()
    {

        for (int i = 0; i < dashIndicators.Length; i++)
        {
            dashIndicators[i].SetActive(true);
        }

        for (int i = 0; i < dashIndicators.Length; i++)
        {
            
            yield return new WaitForSeconds(.25f);
            dashIndicators[i].SetActive(false);
        }
    }

    public void SwitchGun()
    {
        foreach (Gun theGun in avaliableGuns) theGun.gameObject.SetActive(false);
        avaliableGuns[currentGun].gameObject.SetActive(true);

        UIController.instance.currentGun.sprite = avaliableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = avaliableGuns[currentGun].weaponName;
    }

   
}
