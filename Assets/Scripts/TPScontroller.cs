using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPScontroller : MonoBehaviour {

    //记录--------------------------------------------------------
    Vector3 movetoPos = Vector3.zero;//当前移动方向
    [Header("移动速度")]
    public float Speed = 1.0f;
    [Header("视角灵敏度")]
    public float roFlag = 0.1f;
    [Header("跳跃力度")]
    public float jumpforce = 200;
    [Header("重力")]
    public float gravity = 20;
    [Header("跳跃高度")]
    public float jumphigh = 5;
    [Header("摄像机")]
    public GameObject mycamera;
    CharacterController controller;
    Animator animator;
    int isAboveHash = Animator.StringToHash("IsAbove");

    private new Rigidbody rigidbody;
    //内部调用----------------------------------------------------

    Vector3 speedDir;
    Vector3 worldSpeedDir;
    Vector2 nowPos;
    Vector2 LastPos;
    float nowAngle_y;

    void cmaDirUpdateSet()//根据鼠标设置Cma的位置
    {

        if (!Cursor.visible)//鼠标不可见时
        {
            nowPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            float rotate_x = (nowPos.x - LastPos.x) * roFlag;
            float rotate_y = (LastPos.y - nowPos.y) * roFlag;
            LastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //完成Player x_横向旋转
            gameObject.transform.Rotate(new Vector3(0, rotate_x, 0));
            if ((nowAngle_y + rotate_y) > 30f || nowAngle_y + rotate_y < -90f) {
                return;
            }
            nowAngle_y += rotate_y;
            mycamera.transform.Rotate(new Vector3(rotate_y, 0, 0));
            //视角旋转 摄像机位置变换数据


            //非FPS型
            if (nowAngle_y > -30f && nowAngle_y <= 30f) {
                mycamera.transform.localPosition = new Vector3(0, -0.75f + 3.75f * ((nowAngle_y + 30f) / 60f), -5f + 1f * ((nowAngle_y + 30f) / 60f));
            }
            else {
                mycamera.transform.localPosition = new Vector3(0, -0.75f * ((nowAngle_y + 90f) / 60f), -0.75f - 4.25f * ((nowAngle_y + 90f) / 60f));
            }

            //FPS型

            //if (nowAngle_y > -60f && nowAngle_y <= 30f)
            //{
            //	mycamera.transform.localPosition = new Vector3(0, -2f + 5f * ((nowAngle_y + 60f) / 90f), -3f - 1f * ((nowAngle_y + 60f) / 90f));
            //}
            //else
            //{
            //	mycamera.transform.localPosition = new Vector3(0, -2f * ((nowAngle_y + 90f) / 30f), -0.75f - 3.25f * ((nowAngle_y + 90f) / 30f));
            //}
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                Cursor.visible = false;
                LastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }

    Vector3 basePos0;
    Vector3 basePos1;
    Vector3 basePos2;
    Vector3 basePos3;
    Vector3 basePos4;
    Vector3 basePos5;
    Vector3 basePos6;
    Vector3 cmaPos;
    Ray baser0;
    Ray baser1;
    Ray baser2;
    Ray baser3;
    Ray baser4;
    Ray baser5;
    Ray baser6;
    GameObject onCmaObj;
    List<GameObject> onCmaCollision;
    List<GameObject> onRemoveCollision;
    LayerMask mask;
    LayerMask mask2;
    LayerMask mask3;
    RaycastHit onrcHit;
    Color lastColor;
    void shelterCheckUpdate()//遮挡透明化处理
    {
        basePos0 = gameObject.transform.position;
        basePos1 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z);
        basePos2 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.8f, gameObject.transform.position.z);
        basePos3 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
        basePos4 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
        basePos5 = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
        basePos6 = new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);

        cmaPos = mycamera.transform.position;

        baser0 = new Ray(cmaPos, basePos0 - cmaPos);
        baser1 = new Ray(cmaPos, basePos1 - cmaPos);
        baser2 = new Ray(cmaPos, basePos2 - cmaPos);
        baser3 = new Ray(cmaPos, basePos3 - cmaPos);
        baser4 = new Ray(cmaPos, basePos4 - cmaPos);
        baser5 = new Ray(cmaPos, basePos5 - cmaPos);
        baser6 = new Ray(cmaPos, basePos6 - cmaPos);
        //checkOnCmaCollision
        for (int i = 0; i < onCmaCollision.Count; i++) {
            onCmaObj = onCmaCollision[i];
            if (null == onCmaObj) {
                onRemoveCollision.Add(onCmaObj);
                continue;
            }
            onCmaObj.layer = 11;
            if (Physics.Raycast(baser0, out onrcHit, Vector3.Distance(cmaPos, basePos0), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser1, out onrcHit, Vector3.Distance(cmaPos, basePos1), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser2, out onrcHit, Vector3.Distance(cmaPos, basePos2), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser3, out onrcHit, Vector3.Distance(cmaPos, basePos3), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser4, out onrcHit, Vector3.Distance(cmaPos, basePos4), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser5, out onrcHit, Vector3.Distance(cmaPos, basePos5), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser6, out onrcHit, Vector3.Distance(cmaPos, basePos6), mask3)) {
                onCmaObj.layer = 10;
                continue;
            }

            onCmaObj.layer = 9;
            onRemoveCollision.Add(onCmaObj);
        }
        for (int i = 0; i < onRemoveCollision.Count; i++) {
            onCmaObj = onRemoveCollision[i];
            if (null == onCmaObj) {
                onCmaCollision.Remove(onCmaObj);
                continue;
            }
            lastColor = onCmaObj.GetComponent<MeshRenderer>().material.color;
            onCmaObj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Legacy Shaders/Diffuse");
            onCmaObj.GetComponent<MeshRenderer>().material.color = new Color(lastColor.r, lastColor.g, lastColor.b, 1f);
            onCmaCollision.Remove(onCmaObj);
        }
        onRemoveCollision.Clear();

        //addin
        checkCmaCollision(baser0, cmaPos, basePos0);
        checkCmaCollision(baser1, cmaPos, basePos1);
        checkCmaCollision(baser2, cmaPos, basePos2);
        checkCmaCollision(baser3, cmaPos, basePos3);
        checkCmaCollision(baser4, cmaPos, basePos4);
        checkCmaCollision(baser5, cmaPos, basePos5);
        checkCmaCollision(baser6, cmaPos, basePos6);
    }
    void checkCmaCollision(Ray baser0, Vector3 cmaPos, Vector3 basePos0)//检测是否产生遮挡
    {
        for (; ; )
        {
            if (Physics.Raycast(baser0, out onrcHit, Vector3.Distance(cmaPos, basePos0), mask)) {
                if (onrcHit.collider.gameObject.name == "Plane") {
                    break;
                }
                objAddToOnCmaCollision(onrcHit.collider.gameObject);
            }
            else {
                break;
            }
        }

    }
    void objAddToOnCmaCollision(GameObject obj)//添加遮挡物体
    {
        obj.layer = 10;
        Material objm = obj.GetComponent<MeshRenderer>().material;
        lastColor = objm.color;
        objm.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        objm.color = new Color(lastColor.r, lastColor.g, lastColor.b, 0.3f);
        onCmaCollision.Add(obj);
    }
    public void outPCOnDragBegin() {
        LastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        nowAngle_y = mycamera.transform.eulerAngles.x;
        if (nowAngle_y > 180f || nowAngle_y < -180f) {
            for (; ; )
            {
                if (nowAngle_y > 180f) { nowAngle_y -= 360; }
                else if (nowAngle_y < -180) { nowAngle_y += 360; }

                if (nowAngle_y < 180f && nowAngle_y > -180f) { break; }
            }
        }
    }
    //Behaviour---------------------------------------------------

    //Start is called before the first frame update
    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        outPCOnDragBegin();//角色选择初始设置
        Cursor.visible = false;
        if (!mycamera) {
            mycamera = Camera.main.gameObject;
        }

        onCmaCollision = new List<GameObject>();
        onRemoveCollision = new List<GameObject>();
        mask = 1 << (LayerMask.NameToLayer("Collision"));
        mask2 = 1 << (LayerMask.NameToLayer("out"));
        mask3 = 1 << (LayerMask.NameToLayer("check"));

        animator = gameObject.GetComponentInChildren<Animator>();

        rigidbody = GetComponent<Rigidbody>();
    }

    Vector3 moveDirection = Vector3.zero;

    // Update is called once per frame
    void Update() {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        animator.SetFloat("Velocity X", x);
        animator.SetFloat("Velocity Z", z);
        bool isMoving = (x != 0f || z != 0f); //&& controller.isGrounded
        if (isMoving) {
            animator.SetBool("Moving", true);
        }
        else {
            animator.SetBool("Moving", false);
        }

        if (controller.isGrounded) {
            animator.SetInteger("Jumping", 0);
            moveDirection = new Vector3(x * Speed , 0, z * Speed );
            //moveDirection = new Vector3(0, 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            //moveDirection *= Speed;
            
            if (Input.GetKey(KeyCode.Space)) {
                Debug.Log("jump");
                animator.SetTrigger("JumpTrigger");
                //moveDirection.y = jumpforce;
                for (float timer = 1.5f; timer >= 0; timer -= Time.deltaTime) {
                    moveDirection.y += jumpforce * Time.deltaTime;
                }
                moveDirection.x = x * Speed;
                moveDirection.z = z * Speed;
                moveDirection = transform.TransformDirection(moveDirection);
            }
            
        }
        else
            animator.SetInteger("Jumping", 1);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void LateUpdate() {
        cmaDirUpdateSet();//摄像机位置设置
        shelterCheckUpdate();//遮挡透明化处理
    }

}