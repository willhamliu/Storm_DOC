using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 摄像机控制
/// </summary>
public class Camera_Control : MonoBehaviour
{
    public Action cameraDrag;
    public Action cameraDragEnd;
    public Transform map_Point;
    public float camera_ZoomSpeed = 10f;
    public float camera_MoveSpeed = 1f;
    public float scaleMIN_Y = 40f;//最低高度

    Vector2 map_Border;//地图边界

    Vector3 start_Position;//初始摄像机位置
    Vector3 Single_Touch;//拖动触摸位置
    Vector3 start_Single_Touch;//首次触碰位置
    Vector3 camera_Offset;//摄像机移动时偏移量

    Vector3 lastPoisition;
    Vector3 inertiaDirection;//惯性方向
    Vector2 lastTouch;
    Vector2 currentTouch;

    Vector3 oldTouch1;//初始触摸点1
    Vector3 oldTouch2;//初始触摸点2

    Vector3 newTouch1;//移动触摸点1
    Vector3 newTouch2;//移动触摸点2

    Vector3 scene_Touch1;//在场景中的触摸位置1
    Vector3 scene_Touch2;//在场景中的触摸位置2
    Vector3 touch_Middle;//触摸点间的中点
    Vector3 camera_Zoom_Offset;//相机变焦时的偏移量

    Vector3 mousePosition;
    Coroutine fOV_Away;
    Coroutine fOV_Close;

    float scaleMAX_Y;//最高高度
    float camera_Height;//当前摄像机高度
    float cameraheight_Simulation;


    float halfFOV;//相机视野角度
    float aspect;//摄像机长宽比
    float height;//画面高度（/2）
    float width;//画面宽度（/2）

    float update_X;//更新摄像机x轴位置
    float update_Y;//更新摄像机y轴位置

    float start_Zoom_X;//触摸点在屏幕的占比
    float start_Zoom_Y;

    float zoomT;
    float zoomDamping;
    float moveSpeed_X;
    float moveSpeed_Y;

    int oldDistance;//上一次触摸点1，2之间的距离
    int newDistance; //当前触摸点1，2之间的距离

    bool canera_Lock = false;//检测是否为多指触碰
    bool oldTouch_Update = false;//检测多指触碰时是否更新过旧的触摸点
    bool isHorizontalMove;
    bool isVerticalMove;
    bool isZoomLimit;
    bool isZoomComplete;
    bool isDrag;
    bool isStop;

    void Start()
    {
        isDrag = false;
        isStop = false;
        isZoomLimit = false;
        zoomT = 0;
        camera_Height = cameraheight_Simulation = transform.position.z;
        map_Border = Map_Management.boundary;
        Update_camera_FOV();
        MIX_scale();//必须在视野检测后更新
#if UNITY_ANDROID
        Input.multiTouchEnabled = true;//开启多点触碰
#endif
    }
    
    void LateUpdate()
    {
        CameraMove();
        CameraZoom();

#if UNITY_ANDROID
        if (Input.touchCount == 0)
        {
            if (cameraDragEnd != null) cameraDragEnd.Invoke();
            oldTouch_Update = false;
            canera_Lock = false;
        }
#endif
    }

    void MIX_scale()
    {
        float scaleMAX_height = (map_Border.y - transform.position.y) / Mathf.Tan(halfFOV);//不考虑 Y 轴得到的最远缩放距离
        float scaleMAX_width = ((map_Border.x - transform.position.x) / aspect) / Mathf.Tan(halfFOV);//不考虑x轴得到的最远缩放距离
        scaleMAX_Y = Mathf.Max(scaleMAX_height, scaleMAX_width) + (map_Point.position.z);
        //由于摄像机z坐标为负数，因此取最大值作为极限距离，需要通过地图的位置增加极限距离才是最大高度
    }

    void Update_camera_FOV()//更新摄像机视野
    {
        halfFOV = (Camera.main.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        aspect = Camera.main.aspect;
        
        height = Mathf.Abs(camera_Height - map_Point.position.z) * Mathf.Tan(halfFOV);
        width = height * aspect;
    }
    void JudgeBorder()
    {
        if (transform.position.y + height > -map_Border.y)//上边溢出
        {
            //Debug.Log(transform.position.y+"            偏移量"+ (transform.position.y + height)+"减去偏移量"+ (Mathf.Abs(map_Border.y - Map_Management.instance.Innerradius))+"    单一"+ height+"   单一y"+ transform.position.y);
            transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y + height) - (Mathf.Abs(map_Border.y))), camera_Height);
        }
        else if (transform.position.y - height < map_Border.y)//下边溢出
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y - height) + (Mathf.Abs(map_Border.y))), camera_Height);
        }


        if (transform.position.x + width > -map_Border.x)//左边溢出
        {
            transform.position = new Vector3(transform.position.x - ((transform.position.x + width) - (Mathf.Abs(map_Border.x))), transform.position.y, camera_Height);
        }
        else if (transform.position.x - width < map_Border.x)//右边溢出
        {
            transform.position = new Vector3(transform.position.x - ((transform.position.x - width) + (Mathf.Abs(map_Border.x))), transform.position.y, camera_Height);
        }
    }

    void CameraMove()//移动,改变视野
    {
#if UNITY_STANDALONE
        mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (mousePosition.x < 0.01f || mousePosition.x > 1 - 0.01f)
        {
            isHorizontalMove = true;
        }
        if (mousePosition.y < 0.01f || mousePosition.y > 1 - 0.01f)
        {
            isVerticalMove = true;
        }

        if (isVerticalMove && isHorizontalMove) camera_MoveSpeed = 0.8f;

        if (mousePosition.x < 0.01f && transform.position.x - width > map_Border.x)
        {
            transform.Translate(-Vector3.right * Mathf.Abs(camera_Height) * camera_MoveSpeed * Time.deltaTime, Space.World);
        }
        if (mousePosition.x > 1 - 0.01f && transform.position.x + width < -map_Border.x)
        {
            transform.Translate(Vector3.right * Mathf.Abs(camera_Height) * camera_MoveSpeed * Time.deltaTime, Space.World);
        }
        if (mousePosition.y < 0.01f && transform.position.y - height > map_Border.y)
        {
            transform.Translate(-Vector3.up * Mathf.Abs(camera_Height) * camera_MoveSpeed * Time.deltaTime, Space.World);
        }
        if (mousePosition.y > 1 - 0.01f && transform.position.y + height < -map_Border.y)
        {
            transform.Translate(Vector3.up * Mathf.Abs(camera_Height) * camera_MoveSpeed * Time.deltaTime, Space.World);
        }
        camera_MoveSpeed = 1;
        isVerticalMove = false;
        isHorizontalMove = false;
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1 && !canera_Lock)//单指触碰
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)//在开始触摸时要更新固定点(首次触摸位置)
            {
                isStop = true;
                start_Single_Touch = Single_Touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y,Mathf.Abs(camera_Height)));
                start_Single_Touch = Single_Touch = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(camera_Height)));
                lastPoisition = transform.position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved) //手指在屏幕上移动         
            {
                isDrag = true;
                if (cameraDrag != null) cameraDrag.Invoke();
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Mathf.Abs(camera_Height)));
                camera_Offset = transform.position - mousePosition; ;//偏移量
                currentTouch = mousePosition;

                update_X = Mathf.Clamp(Single_Touch.x + camera_Offset.x, map_Border.x + (width), -map_Border.x - (width));
                update_Y = Mathf.Clamp(Single_Touch.y + camera_Offset.y, map_Border.y + (height), -map_Border.y - (height));

                if ((update_X == map_Border.x + (width) || update_X == -map_Border.x - (width) ||
                  update_Y == map_Border.y + (height) || update_Y == -map_Border.y - (height)))
                {
                    Single_Touch = start_Single_Touch;
                    update_X = Mathf.Clamp(Single_Touch.x + camera_Offset.x, map_Border.x + (width), -map_Border.x - (width));
                    update_Y = Mathf.Clamp(Single_Touch.y + camera_Offset.y, map_Border.y + (height), -map_Border.y - (height));
                }
                transform.position = new Vector3(update_X, update_Y, camera_Height);
                start_Single_Touch = transform.position - camera_Offset;

                inertiaDirection = Vector3.ClampMagnitude((transform.position - lastPoisition) / Time.deltaTime,600);
                lastPoisition = transform.position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended&& isDrag)//在开始触摸时要更新固定点(首次触摸位置)      
            {
                if (isDrag)
                {
                    isStop = false;
                    isDrag = false;
                    StartCoroutine(Inertia());
                }
            }
        }
#endif
    }

    IEnumerator Inertia()
    {
        moveSpeed_X = Mathf.Abs(inertiaDirection.x);
        moveSpeed_Y = Mathf.Abs(inertiaDirection.y);
        while (!isStop|| (moveSpeed_X <= 0&& moveSpeed_Y <= 0))
        {
            update_X = Mathf.Clamp(transform.position.x + inertiaDirection.normalized.x * moveSpeed_X * Time.deltaTime, map_Border.x + (width), -map_Border.x - (width));
            update_Y = Mathf.Clamp(transform.position.y + inertiaDirection.normalized.y * moveSpeed_Y * Time.deltaTime, map_Border.y + (height), -map_Border.y - (height));

            transform.position = new Vector3(update_X, update_Y, camera_Height);
            moveSpeed_X -= moveSpeed_X * 0.15f;
            moveSpeed_Y -= moveSpeed_Y * 0.15f;
            yield return null;
        }
    }

    void CameraZoom()
    {
#if UNITY_STANDALONE
        start_Position = this.transform.position;
        touch_Middle = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(camera_Height)));
        start_Zoom_X = (touch_Middle.x - start_Position.x) / width;
        start_Zoom_Y = (touch_Middle.y - start_Position.y) / height;

        if (Input.mouseScrollDelta.y > 0)
        {
            isZoomComplete = false;
            isZoomLimit = false;
            zoomT = 0;
            zoomDamping = camera_ZoomSpeed;
            cameraheight_Simulation = Mathf.Clamp(camera_Height + 60, scaleMAX_Y, scaleMIN_Y);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            isZoomComplete = false;
            isZoomLimit = false;
            zoomT = 0;
            zoomDamping = camera_ZoomSpeed;
            cameraheight_Simulation = Mathf.Clamp(camera_Height - 60, scaleMAX_Y, scaleMIN_Y);
        }
      
        if(!isZoomLimit)
        {
            zoomDamping -= Time.deltaTime;
            zoomT += Time.deltaTime * zoomDamping;
            camera_Height = Mathf.Lerp(camera_Height, cameraheight_Simulation, zoomT);

            Update_camera_FOV();
            transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);
            JudgeBorder();
        }
        if (Mathf.Abs(camera_Height - cameraheight_Simulation)<0.01f)
        {
            isZoomLimit = true;
            camera_Height = cameraheight_Simulation;
            if (!isZoomComplete)
            {
                Update_camera_FOV();
                transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);
                JudgeBorder();
                isZoomComplete = true;
            }
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 2)//多指触碰
        {
            canera_Lock = true;
            if (cameraDrag != null) cameraDrag.Invoke();

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                scene_Touch1 = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, Mathf.Abs(camera_Height)));
                scene_Touch2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(1).position.x, Input.GetTouch(1).position.y, Mathf.Abs(camera_Height)));

                touch_Middle = (scene_Touch1 + scene_Touch2) / 2;

                start_Position = this.transform.position;
                start_Zoom_X = (touch_Middle.x - start_Position.x) / width;
                start_Zoom_Y = (touch_Middle.y - start_Position.y) / height;


                newTouch1 = Input.GetTouch(0).position;
                newTouch2 = Input.GetTouch(1).position;

                oldDistance = (int)Vector2.Distance(oldTouch1, oldTouch2);
                newDistance = (int)Vector2.Distance(newTouch1, newTouch2);

                if (oldTouch_Update == true)
                {
                    camera_Height = Mathf.Clamp(camera_Height - ((oldDistance - newDistance) * 0.09f), scaleMAX_Y, scaleMIN_Y);

                    Update_camera_FOV();
                    transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);
                    JudgeBorder();
                }
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;

                oldTouch_Update = true;
            }
        }
#endif
    }
}

    
    

