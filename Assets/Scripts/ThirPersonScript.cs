using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ThirPersonScript : MonoBehaviour
{
    public enum States
    {
       idle,
       walk,
        attack,
        Dying, 
    }
    float horizontal;
    float vertical;
    Vector3 direction;
    public States state;
    public CharacterController controller; // referencia pro controle
    public Transform cam;
    public int lifesPlayer = 7;
   
    public Animator anim;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private void Start()
    {
        StartCoroutine(Idle());
    }
    
    void Update()
    {

        

        horizontal = Input.GetAxisRaw("Horizontal");
        
        vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("walking", true);
        }
            if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Attack());
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
       
       if (collision.collider.CompareTag("EnemyWeapon"))
       {
           Debug.Log("TomouDano");
           lifesPlayer--;
           StartCoroutine(Dying());
       }
        
    }
    IEnumerator Idle()
    {
        //equivalente ao Start 
        state = States.idle;


        //
        while (state == States.idle)
        {
            //equivalente ao update
            anim.SetBool("walking", false);








            if (direction.magnitude >= 0.1f)
            {
                StartCoroutine(Walk());
            }
            //
            yield return new WaitForEndOfFrame();
        }
        //saida do estado
    }

    IEnumerator Walk()
    {
        //equivalente ao Start 
        state = States.walk;


        //
        while (state == States.walk)
        {
            //equivalente ao update
            
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            anim.SetBool("walking", true);

            if (direction.magnitude < 0.1f)
            {
                StartCoroutine(Idle());
            }
            //
            yield return new WaitForEndOfFrame();
        }
        //saida do estado
    }


    IEnumerator Attack()
    {
        //equivalente ao Start 
        state = States.attack;
        anim.SetTrigger("Attack");
    
        yield return new WaitForSeconds(.2f);
        StartCoroutine(Idle());
    }
    IEnumerator Dying()
    {
        
        yield return new WaitForSeconds(.1f);
        if (lifesPlayer <= 0)
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene("Derrota");
        }
        
    }
}
