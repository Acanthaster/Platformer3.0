using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ALR_CustomCharacterController : MonoBehaviour
{
    public GameObject self;
    public GameObject interactionNPC;
    public GameObject interactionNPC1;
    public GameObject interactionAltar;
    public GameObject NPC1;
    public Camera mainCamera;

    // Stock temporairement les points d'origine des Raycasts.
    protected struct RaycastOrigins
    {

        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    // Stock temporairement les données de collisions utilisées pour les opérations
    public struct CollisionInfo
    {

        public bool above, below, left, right;
        public RaycastHit2D hHit, vHit;
        public int groundLayer;
        public bool onGround;
        public bool onWall;


        //En a-t-on vraiment besoin ?
        public float groundAngle; //utile ?
        public float groundDirection; //utile ?

        public void Reset()
        {
            above = false;
            below = false;

            if (!onWall)
            {
                left = false;
                right = false;
            }

            hHit = new RaycastHit2D();
            vHit = new RaycastHit2D();
            onGround = false;
            groundAngle = 0;
            groundDirection = 0;

        }
    }




    /*--------VARIABLES--------*/

    //COMPOSANTS

    private Animator animator;
    public SpriteRenderer sprite;
    public Transform graphicsTransform;
    private Transform myTransform;

    protected BoxCollider2D myCollider;
    protected LayerMask collisionMask;
    protected ALR_PhysicsConfig pConfig; //S'occupe de gérer les layers de collisions et les paramètres de base de la physique
    private ALR_CharacterData cData; // Configuration des paramètres du personnage et de ses actions
    private AXD_PlayerStatus pStatus;
    private ALR_PlayerInputHandler pInput;
    //private ALR_DialogueTrigger dTrigger;
    private ALR_SoundManager soundManager;
    public GameObject altarEnd;


    //COLLISION

    public float raySpacing = 0.125f;
    public float skinWidth = 0.015f;

    protected RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    protected float horizRaySpacing; // Stock l'espace entre les différents ray horizontaux
    protected float horizRayCount; // Stock un compteur pour eux
    protected float vertiRaySpacing; // Même chose qu'au dessus
    protected float vertiRayCount;


    //MOUVEMENT

    [SerializeField]
    protected Vector2 speed = Vector2.zero; // Stock la vitesse de base
    [SerializeField]
    public Vector2 externalForce = Vector2.zero; // Stock les forces ingérentes
    public AnimationCurve knockBackCurveX;
    public AnimationCurve knockBackCurveY;
    public float knockBackTime;
    public bool takingDamage;
    public bool wallJumped;
    public float ponderatedWJSpeed;
    protected float gravityScale = 1; // à set ?
    protected float minimumMoveThreshold = 0.0001f;
    protected float timeSinceFalling; //Pour tenir compte du temps pour le ghost jump
    private bool spawning;
    private bool isOnMovingPlatform = false;
    private bool replaceOnMovingPlatform = false;
    public bool isGhostJumping = false;
    public bool jumped = false;
    public bool pause = false;


    public bool IgnoreFriction { get; set; } // CHECK NEEDED !!
    public bool Immobile { get; set; } // CHECK NEEDED !!
    private CollisionInfo lastSideWJ;

    // Sert à capter les modifications sur la vitesse de base par les forces ingérentes
    public Vector2 TotalSpeed { get { return speed + externalForce; } }

    //ANIMATION

    // Animation attributes and names
    private static readonly string ANIMATION_H_SPEED = "hSpeed";
    private static readonly string ANIMATION_V_SPEED = "vSpeed";
    private static readonly string ANIMATION_JUMP = "Jump";
    private static readonly string ANIMATION_GROUNDED = "Grounded";
    private static readonly string ANIMATION_WALL = "OnWall";
    private static readonly string ANIMATION_FACING = "facingRight";
    private static readonly string ANIMATION_H_MOVING = "hMoving";
    private static readonly string ANIMATION_DAMAGE = "damage";
    //private static readonly string ANIMATION_DEATH = "death";

    public bool FacingRight = true;

    private void Awake()
    {
        knockBackTime = 0;
        wallJumped = false;
        pStatus = GetComponent<AXD_PlayerStatus>();
        animator = GetComponent<Animator>();
        cData = GetComponent<ALR_CharacterData>();
        myCollider = GetComponent<BoxCollider2D>();
        pConfig = GameObject.FindObjectOfType<ALR_PhysicsConfig>();
        pInput = GetComponent<ALR_PlayerInputHandler>();
        soundManager = GetComponent<ALR_SoundManager>();
        myTransform = GetComponent<Transform>();
        pInput.lockInput = true;
        spawning = true;
    }

    public void Start()
    {
        AXD_CheckPoint startPoint = GameObject.Find("StartPoint").GetComponent<AXD_CheckPoint>();
        transform.position = new Vector2(startPoint.transform.position.x,startPoint.GetYAboveGround(myCollider));
        StartCoroutine("Spawn");
        collisionMask = pConfig.characterCollisionMask;

        // Pour initialiser les raycasts en nombres et taille par rapport à la BoxCollider
        CalculateSpacing();
    }

    public void FixedUpdate()
    {
        //Debug.Log("knockback time ? " + (knockBackTime > pStatus.invincibilityCoolDown) + " dead ? " + pStatus.dead);
        if (pStatus.dead || pause)
        {
            pInput.lockInput = true;
        }else if (!takingDamage && !spawning)
        {
            pInput.lockInput = false;
        }
        if ((knockBackTime > pStatus.invincibilityCoolDown || pStatus.dead) && !pause )
        {
            if (!pStatus.dead && !pause)
            {
                pInput.lockInput = false;
            }
            knockBackTime = 0;
            takingDamage = false;
        }
        collisions.Reset();
        //Debug.Log("lockInput ? "+pInput.lockInput + " // Taking damage ? "+takingDamage);
        if (pInput.lockInput)
        {
            speed = externalForce = new Vector2(0, 0);
        }
        Move((TotalSpeed) * Time.fixedDeltaTime);
        PostMove();
        SetAnimations();
        if (wallJumped)
        {
            if (TotalSpeed.x <= cData.wallSpeedThreshhold && TotalSpeed.x >= -cData.wallSpeedThreshhold)
            {
                ponderatedWJSpeed = 0;
            }
            if ((ponderatedWJSpeed > 0 && speed.x > 0 && ponderatedWJSpeed > speed.x) || (ponderatedWJSpeed < 0 && speed.x < 0 && ponderatedWJSpeed < speed.x))
            {
                ponderatedWJSpeed = speed.x;
            }
            if (ponderatedWJSpeed != 0)
            {
                if (Mathf.Sign(pInput.translation) != Mathf.Sign(ponderatedWJSpeed))
                {
                    ponderatedWJSpeed += pInput.translation / 2;
                }
            }
            if (Mathf.Sign(ponderatedWJSpeed) != Mathf.Sign(pInput.translation))
            {
                externalForce.y += pConfig.gravity * Time.fixedDeltaTime * cData.gravityMalusOnWJOnSameSide;
            }
            externalForce.x = ponderatedWJSpeed;
        }
        if (isOnMovingPlatform)
        {
            if (!replaceOnMovingPlatform )
            {
                //Debug.Log("REPLACE");
                transform.localPosition = transform.localPosition +  new Vector3(0, 1.472f - transform.localPosition.y);
                replaceOnMovingPlatform = true;
            }
            if (!CheckMovingPlatform())
            {
                transform.parent = null;
                isOnMovingPlatform = false;
                replaceOnMovingPlatform = false;
            }
        }

        if (collisions.onWall)
        {
            UpdateRaycastOrigins();
            CheckOnWall();
        }

        CheckAhead();
    }



    //COLLISIONS
    //On créé une "grille" de raycast sur le player (sur son collider) pour checker les collisions sur toute sa hauteur et largeur

    //Pour checker les collisions à gauche et à droite sur toute la hauteur du player
    protected void HorizCollisions(ref Vector2 deltaMove)
    {

        float dirX = Mathf.Sign(deltaMove.x);
        float rayLength = Mathf.Abs(deltaMove.x) + skinWidth;

        // On fait un "for" pour créer chaque raycast et checker si il y a une collision à chaque fois
        //i est le nombre de raycast que l'on veut sur toute la hauteur du player (de sa BoxCollider)
        for (int i = 0; i < horizRayCount; i++)
        {

            // En fonction de l'orientation, on choisit un point de départ à Gauche ou à Droite du raycast
            Vector2 rayOrigin = dirX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            // On ajoute la hauteur en y du raycast pour, au final, recouvrir toute la hauteur du player
            rayOrigin += Vector2.up * (horizRaySpacing * i);

            // On fait le raycast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength, Color.red);
            if (hit)
            {


                float angle = Vector2.Angle(hit.normal, Vector2.up);
                //Debug.Log("Collider : " + hit.collider + "\nLayer : " + LayerMask.LayerToName( hit.collider.gameObject.layer));
                if (hit.collider.isTrigger && (hit.collider.CompareTag("Corn") || hit.collider.CompareTag("Cacao") || hit.collider.CompareTag("Checkpoint")))
                {
                    if (hit.collider.CompareTag("Corn"))
                    {
                        soundManager.CollectSound("Corn");
                        pStatus.Corn += 1;
                        Destroy(hit.collider.gameObject);

                    }
                    else if (hit.collider.CompareTag("Cacao"))
                    {
                        soundManager.CollectSound("Cacao");
                        pStatus.Cacao += 1;
                        hit.collider.gameObject.GetComponent<ALR_CacaoAnim>().Collect();
                    }
                    else if (hit.collider.CompareTag("Checkpoint"))
                    {
                        if (!hit.collider.gameObject.GetComponent<AXD_CheckPoint>().activated)
                        {
                            pStatus.TrueLastCheckPoint.GetComponent<AXD_CheckPoint>().Desactivated();
                            pStatus.TrueLastCheckPoint = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<AXD_CheckPoint>().Activate();
                            pStatus.LastCheckpoint = new Vector2(hit.collider.transform.position.x, hit.collider.gameObject.GetComponent<AXD_CheckPoint>().GetYAboveGround(myCollider));

                        }
                       
                    }

                    return;
                }
                else if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Obstacles") && hit.collider.CompareTag("FireRain"))
                {
                    //Debug.Log("Fire rain");
                    if (hit.collider.gameObject.GetComponent<AXD_FireRain>().damaging)
                    {
                        pStatus.TakeDamage();

                    }
                    else
                    {
                        return;
                    }


                }
                else if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Obstacles") && !hit.collider.CompareTag("FireRain"))
                {
                    //Debug.Log("Pas Fire rain");
                    pStatus.TakeDamage();


                }

                if (hit.collider.CompareTag("NPC"))
                    return;

                if (!(i == 100000))
                {

                    deltaMove.x = Mathf.Min(Mathf.Abs(deltaMove.x), (hit.distance - skinWidth)) * dirX;
                    rayLength = Mathf.Min(Mathf.Abs(deltaMove.x) + skinWidth, hit.distance);

                    // Check s'il y a collision horizontale avec un mur 
                    if (!collisions.onGround && angle >= 89f && !collisions.onWall)
                    {
                        if (CheckWall())
                        {
                            collisions.onWall = true;
                            collisions.left = dirX < 0;
                            collisions.right = dirX > 0;
                            speed.x = 0;
                            externalForce.x = 0;
                        }
                    }

                    else
                    {
                        collisions.left = dirX < 0;
                        collisions.right = dirX > 0;
                        collisions.hHit = hit;
                        speed.x = 0;
                        externalForce.x = 0;
                    }

                }
            }
        }
    }


    // La même chose mais pour checker les collisions avec le haut et le bas
    protected void VertiCollisions(ref Vector2 deltaMove)
    {
        float dirY = Mathf.Sign(deltaMove.y);
        float rayLength = Mathf.Abs(deltaMove.y) + skinWidth;

        for (int i = 0; i < vertiRayCount; i++)
        {
            Vector2 rayOrigin = dirY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (vertiRaySpacing * i + deltaMove.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * dirY * rayLength, Color.yellow);

            if (hit)
            {
                //Debug.Log("Collider Layer : " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                if (TotalSpeed.y < 0)
                {
                    jumped = wallJumped = false;
                }
                if (hit.collider.isTrigger && (hit.collider.CompareTag("Corn") || hit.collider.CompareTag("Cacao") || hit.collider.CompareTag("Checkpoint")))
                {
                    if (hit.collider.CompareTag("Corn"))
                    {

                        pStatus.Corn += 1;
                        Destroy(hit.collider.gameObject);

                    }
                    else if (hit.collider.CompareTag("Cacao"))
                    {
                        pStatus.Cacao += 1;
                        hit.collider.gameObject.GetComponent<ALR_CacaoAnim>().Collect();
                        //Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.CompareTag("Checkpoint"))
                    {
                        if (!hit.collider.gameObject.GetComponent<AXD_CheckPoint>().activated)
                        {
                            pStatus.TrueLastCheckPoint.GetComponent<AXD_CheckPoint>().Desactivated();
                            pStatus.TrueLastCheckPoint = hit.collider.gameObject;
                            hit.collider.gameObject.GetComponent<AXD_CheckPoint>().Activate();
                            pStatus.LastCheckpoint = new Vector2(hit.collider.transform.position.x, hit.collider.gameObject.GetComponent<AXD_CheckPoint>().GetYAboveGround(myCollider));

                        }
                    }

                    return;
                }
                else if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Obstacles") && hit.collider.CompareTag("FireRain"))
                {
                    if (hit.collider.gameObject.GetComponent<AXD_FireRain>().damaging)
                    {
                        pStatus.TakeDamage();
                    }
                    else
                    {
                        return;
                    }

                }
                else if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Obstacles") && !hit.collider.CompareTag("FireRain"))
                {
                    pStatus.TakeDamage();
                }
                else if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Death"))
                {
                    //Debug.Log("Death");
                    pStatus.Die();
                }

                if (hit.collider.CompareTag("NPC"))
                    return;

                if (hit.collider.transform.tag == "MovingPlatform" && !isOnMovingPlatform)
                {
                    //Debug.Log("Touching MovingPlatform");

                    if(hit.collider.transform.position.y <= transform.position.y)
                    {
                        transform.parent = hit.collider.transform;
                        ColliderDistance2D dist = myCollider.Distance(hit.collider);
                        transform.position = transform.position + new Vector3(0, Mathf.Abs(dist.distance));
                        isOnMovingPlatform = true;
                        deltaMove.y = 0;
                    }

                    //externalForce.y = 0;
                }
                else
                {
                    deltaMove.y = (hit.distance - skinWidth) * dirY;
                    rayLength = hit.distance;
                }
                collisions.above = dirY > 0;
                collisions.below = dirY < 0;
                collisions.vHit = hit;
            }
        }
    }



    //MOVEMENTS

    // PreMove() est appelé dans la fonction Move()
    // On effectue un ensemble d'update qui sont importantes pour Move()
    protected virtual void PreMove(ref Vector2 deltaMove)
    {
        UpdateRaycastOrigins();
        float xDir = Mathf.Sign(deltaMove.x);
        CheckGround(xDir);
        UpdateExternalForce();
        UpdateGravity();
    }


    // La fonction principale pour le mouvement
    public Vector2 Move(Vector2 deltaMove)
    {
        //Juste le temps de la fonction Move(), le player change de layer pour que les raycasts n'influent pas sur la fonction Move()
        int layer = gameObject.layer;
        gameObject.layer = Physics2D.IgnoreRaycastLayer;

        PreMove(ref deltaMove);

        float xDir = Mathf.Sign(deltaMove.x);

        // Si on a un mouvement à l'horizontal, on check les collisions
        if (deltaMove.x != 0)
        {
            HorizCollisions(ref deltaMove);
        }


        // Si player en contact avec un mur, on check les conditions de WallSlide
        if (collisions.onWall && cData.canWallSlide && TotalSpeed.y <= 0)
        {
            externalForce.y = 0;
            speed.y = -cData.wallSlideSpeed;
        }


        if (deltaMove.y != 0)
        {
            VertiCollisions(ref deltaMove);
        }

        Debug.DrawRay(transform.position, deltaMove * 3f, Color.green);
        if (pStatus.lockedInput)
        {
            speed = new Vector2(0, 0);
        }
        transform.Translate(deltaMove);

        if (collisions.vHit)
        {
            if ((collisions.below && TotalSpeed.y < 0) || (collisions.above && TotalSpeed.y > 0))
            {
                speed.y = 0;
                externalForce.y = 0;

                // Pour le wall jump ?
                externalForce.x = 0;
            }
        }



        // On remet le player dans son layer "Player"
        gameObject.layer = layer;
        return deltaMove;
    }


    // Pour gérer l'ensemble des déplacement horizontaux de base
    public void Walk(float dir)
    {
        if (CanMove())
        {
            if (dir > 0 && !FacingRight)
                FlipIt();
            else if (dir < 0 && FacingRight)
                FlipIt();

            float acc = 0f;
            float dec = 0f;

            // Si cData.advancedAirControl = true (dans inspector), on peut affiner la mobilité in Air
            if (cData.advancedAirControl && !collisions.below)
            {
                acc = cData.airAccelerationTime;
                dec = cData.airDecelerationTime;
            }

            else
            {
                acc = cData.accelerationTime;
                dec = cData.decelerationTime;
            }

            if (acc > 0)
            {
                // Gestion de l'accélération jusqu'à la vitesse max
                if (Mathf.Abs(speed.x) < cData.maxSpeed)
                {
                    speed.x += dir * (1 / acc) * cData.maxSpeed * Time.fixedDeltaTime;
                    speed.x = Mathf.Min(Mathf.Abs(speed.x), cData.maxSpeed * Mathf.Abs(dir)) * Mathf.Sign(speed.x);
                }
            }

            else
            {
                speed.x = cData.maxSpeed * dir;
            }

            // Si on a arrêter de bouger OU qu'on change de sens 
            if (dir == 0 || Mathf.Sign(dir) != Mathf.Sign(speed.x))
            {
                if (dec > 0)
                {
                    speed.x = Mathf.MoveTowards(speed.x, 0, (1 / dec) * cData.maxSpeed * Time.fixedDeltaTime);
                }

                else
                {
                    speed.x = 0;
                }
            }
        }
    }

    public void Jump()
    {
        if (CanMove())
        {
            if (collisions.onGround || (cData.canWallJump && collisions.onWall) || isGhostJumping)
            {
                float height = cData.maxJumpHeight;
                speed.y = Mathf.Sqrt(-2 * pConfig.gravity * height);
                externalForce.y = 0;
                animator.SetTrigger(ANIMATION_JUMP);

                // WALL JUMP
                if (cData.canWallJump && collisions.onWall && !collisions.below)
                {
                    wallJumped = true;
                    soundManager.JumpSound();

                    if (collisions.left)
                    {
                        ponderatedWJSpeed = cData.wallJumpSpeed * 3;
                    }
                    else if (collisions.right)
                    {
                        ponderatedWJSpeed = -cData.wallJumpSpeed * 3;
                    }
                    externalForce.x += ponderatedWJSpeed;

                    collisions.onWall = false;
                }
            }
        }
    }

    public void EndJump()
    {
        //Je sais pas pourquoi ce calcul...
        float yMove = Mathf.Sqrt(-2 * pConfig.gravity * cData.minJumpHeight);

        if (speed.y > yMove)
        {
            speed.y = yMove;
        }
    }


    //On check si on est le GROUND et on update CollisionInfo
    protected void CheckGround(float dir)
    {
        for (int i = 0; i < vertiRayCount; i++)
        {
            Vector2 rayOrigin = dir == 1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += (dir == 1 ? Vector2.right : Vector2.left) * (vertiRaySpacing * i);
            rayOrigin.y += skinWidth * 2;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, skinWidth * 4f, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.down * skinWidth * 4f, Color.cyan);

            if (hit)
            {

                if (TotalSpeed.y < 0)
                {
                    jumped = wallJumped = false;
                }
                if (hit.collider.CompareTag("MovingPlatform"))
                {            
                    /*Debug.Log("Stuck !!");
                    Vector2 transf = transform.position;
                    transf.y = 1.475f;
                    myTransform.position += transf;*/
                }
                if (hit.collider.isTrigger && hit.collider.CompareTag("Checkpoint"))
                {
                    if (!hit.collider.gameObject.GetComponent<AXD_CheckPoint>().activated)
                    {
                        pStatus.TrueLastCheckPoint.GetComponent<AXD_CheckPoint>().Desactivated();
                        pStatus.TrueLastCheckPoint = hit.collider.gameObject;
                        hit.collider.gameObject.GetComponent<AXD_CheckPoint>().Activate();
                        pStatus.LastCheckpoint = new Vector2(hit.collider.transform.position.x, hit.collider.gameObject.GetComponent<AXD_CheckPoint>().GetYAboveGround(myCollider));

                    }
                }
                else if (LayerMask.LayerToName(hit.collider.gameObject.layer).Equals("Death"))
                {
                    //Debug.Log("Death");
                    pStatus.Die();
                }

                else if (!hit.collider.CompareTag("NPC") && !hit.collider.CompareTag("FireRain"))
                {
                    //Debug.Log("Check ground Collider Layer : " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                    collisions.onGround = true;
                    collisions.onWall = false;

                    collisions.groundAngle = Vector2.Angle(hit.normal, Vector2.up);
                    collisions.groundDirection = Mathf.Sign(hit.normal.x);
                    collisions.groundLayer = hit.collider.gameObject.layer;
                    collisions.vHit = hit;
                    collisions.below = true;

                    Debug.DrawRay(rayOrigin, Vector2.down * skinWidth * 2, Color.magenta);
                    break;
                }
            }
        }

    }

    protected bool CheckMovingPlatform()
    {
        int totalCheck = 0;


        for (int i = 0; i < vertiRayCount; i++)
        {
            //Pourquoi on commence par bottomLeft ?? ça me semble plus logique de commencer par bottomRight.....

            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (vertiRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.down * 1f, Color.magenta);
            if (hit.collider == null)
            {
                totalCheck++;
            }
        }

        if (totalCheck == vertiRayCount)
        {
            return false;
        }

        else
        {
            //Debug.Log("JE RESTE !");
            return true;
        }


        //return false;

    }

    protected bool CheckWall()
    {
        float rayLength = 1f;
        float rayCount = 3f;
        float spSpacing = 0.75f;
        float totalCheck = 0f;

        Vector2 saveMiddleRayOrigPos = new Vector2(0, 0);
        float lastSpacing = 0.4f;

        bool belowRay = false;
        bool middleRay = false;
        bool upRay = false;
        bool lastChanceRay = false;

        // En fonction de l'orientation, on choisit un point de départ à Gauche ou à Droite
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft + new Vector2(-0.25f, 0.25f);
            rayOrigin += Vector2.up * (spSpacing * i);

            if (i == 1)
            {
                saveMiddleRayOrigPos += rayOrigin;
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.magenta);

            float angle = Vector2.Angle(hit.normal, Vector2.up);

            if (hit)
            {
                switch (i)
                {
                    case 0:
                        belowRay = true;
                        break;

                    case 1:
                        middleRay = true;
                        break;

                    case 2:
                        upRay = true;
                        break;

                    default:
                        break;
                }

                if (belowRay && middleRay && upRay)
                {
                    return true;
                }
                else if (!belowRay && middleRay && upRay)
                {
                    hit = Physics2D.Raycast(saveMiddleRayOrigPos + new Vector2(0, -lastSpacing), Vector2.right, rayLength, collisionMask);
                    Debug.DrawRay(saveMiddleRayOrigPos + new Vector2(0, -lastSpacing), Vector2.right * rayLength, Color.yellow);

                    if (hit) return true;
                }
            }
        }
        wallJumped = false;
        return false;
    }

    protected void CheckOnWall()
    {
        float rayLength = 1f;
        float rayCount = 3f;
        float spSpacing = 0.5f;
        float totalCheck = 0f;

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft + new Vector2(-0.25f, 0);
            rayOrigin += Vector2.up * (spSpacing * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.cyan);

            float angle = Vector2.Angle(hit.normal, Vector2.up);

            if (hit.collider == null)
            {
                totalCheck++;
            }
        }

        if (totalCheck == 3)
        {
            collisions.onWall = false;
            speed.y = 0;
        }
        wallJumped = false;
    }

    public void CheckAhead()
    {
        float rayLenght = 1f;


        Vector2 direction = FacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLenght, collisionMask);
        Debug.DrawRay(transform.position, direction * rayLenght, Color.white);

        if (hit.collider != null && hit.collider.CompareTag("NPC"))
        {
            pInput.talkingToNPC = true;
            pInput.dTrigger = hit.transform.gameObject.GetComponent<ALR_DialogueTrigger>();

            interactionNPC.SetActive(true);
            interactionNPC1.SetActive(true);
        }
        else
        {
            pInput.talkingToNPC = false;

            interactionNPC.SetActive(false);
            interactionNPC1.SetActive(false);
        }

        if (hit.collider != null && hit.collider.CompareTag("Altar"))
        {
            pInput.makeOffering = true;

            interactionAltar.SetActive(true);
        }
        else
        {
            interactionAltar.SetActive(false);
        }



    }

    // Calcule du nombre et de la taille des raycasts pour faire la "grille" en fonction de la BoxCollider du player 
    void CalculateSpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizRayCount = Mathf.Round(bounds.size.y / raySpacing);
        vertiRayCount = Mathf.Round(bounds.size.x / raySpacing);
        horizRaySpacing = bounds.size.y / (horizRayCount - 1);
        vertiRaySpacing = bounds.size.x / (vertiRayCount - 1);
    }

    public bool CanMove()
    {
        return (!Immobile);
    }

    protected void PostMove()
    {
        IgnoreFriction = false;
    }



    //UPDATES

    private void UpdateGravity()
    {
        float g = pConfig.gravity * gravityScale * Time.fixedDeltaTime;

        if (speed.y > 0)
        {
            speed.y += g;
        }

        else
        {
            externalForce.y += g;
        }

    }

    private void UpdateExternalForce()
    {
        if (IgnoreFriction)
            return;

        float friction = collisions.onGround ? pConfig.groundFriction : pConfig.airFriction;
        externalForce = Vector2.MoveTowards(externalForce, Vector2.zero,
        externalForce.magnitude * friction * Time.fixedDeltaTime);
    }


    //Permet de déterminer les origines des raycasts
    private void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }




    //VISUALS
    private void SetAnimations()
    {
        animator.SetFloat(ANIMATION_H_SPEED, speed.x + externalForce.x);
        animator.SetFloat(ANIMATION_V_SPEED, speed.y + externalForce.y);
        animator.SetBool(ANIMATION_GROUNDED, collisions.onGround);
        animator.SetBool(ANIMATION_WALL, collisions.hHit);
        animator.SetBool(ANIMATION_FACING, FacingRight);
        animator.SetBool(ANIMATION_H_MOVING, speed.x != 0);
    }

    void FlipIt()
    {
        if (Time.timeScale != 0)
        {
            FacingRight = !FacingRight;
            sprite.flipX = !sprite.flipX;

        }

    }
    IEnumerator Spawn()
    {
        animator.Play("Anim_Respawn");
        yield return new WaitForSeconds(2);
        spawning = false;
    }

}

