using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    [Space]
    [Header("Constantes Mouvement")]
    public float VITTESSE_DEPLACEMENT_BASE = 10f;

    [Space]
    [Header("Constantes Attaques")]
    private float VITESSE_DE_BASE_FLECHE = 20f;
    public Camera CAMERA_DE_BASE;

    [Space]
    [Header("MouvementPerso")]
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 Movement;
    Vector2 Idle;


    [Space]
    [Header("Attaques")]
    public GameObject arrowPrefab;
    Vector2 PositionSourie;



    private void Awake()
    {
      
    }


    // Update is called once per frame
    void Update()
    {

        //Entrée de mouvement (horizontale et verticale (WASD ou flèches))
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

        //Garder en mémoire la dernière dirrection pour qu le Héros s'arrete dans le bon sens
        if (Movement != Vector2.zero)
            Idle = Movement;


        // Foncction ou sont appelé les Animations
        Animation();


        //Attaques mélée 
        Attaquer();


        //Attaque Distance
        PositionSourie = CAMERA_DE_BASE.ScreenToWorldPoint(Input.mousePosition);
        //PositionSourie = new Vector2( Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        PositionSourie = PositionSourie - rb.position;
        PositionSourie.Normalize();
        Tirer();



    }


    void FixedUpdate()
    {
        //Sorties
        rb.MovePosition(rb.position + Movement * VITTESSE_DEPLACEMENT_BASE * Time.fixedDeltaTime);

    }

    void Animation()
    {
        animator.SetFloat("Horizontal", Movement.x);
        animator.SetFloat("Vertical", Movement.y);
        animator.SetFloat("Speed", Movement.sqrMagnitude);


        animator.SetFloat("IdleH", Idle.x);
        animator.SetFloat("IdleV", Idle.y);

    }

    void Tirer()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject Arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            Arrow.GetComponent<Rigidbody2D>().velocity = PositionSourie * VITESSE_DE_BASE_FLECHE;
            Arrow.transform.Rotate(0, 0, Mathf.Atan2(PositionSourie.y, PositionSourie.x) * Mathf.Rad2Deg - 90f);
            Physics2D.IgnoreCollision(Arrow.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());

            Destroy(Arrow, 2f);


        }
    }

    void Attaquer()
    {
        if (Input.GetButtonDown("Fire1"))
            animator.SetBool("Attaque", true);
        else
            animator.SetBool("Attaque", false);

    }
}
