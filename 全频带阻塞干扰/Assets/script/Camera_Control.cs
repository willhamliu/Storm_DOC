using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Control : MonoBehaviour
{
    public Transform map_Point;

    private Vector3 start_Position;//初始摄像机位置
    private Vector3 start_Single_Touch;//首次触碰位置
    private Vector3 Single_Touch;//拖动触摸位置
    private Vector3 map_Border;//地图边界


    private Vector3 oldTouch1;//初始触摸点1
    private Vector3 oldTouch2;//初始触摸点2

    private Vector3 newTouch1;//移动触摸点1
    private Vector3 newTouch2;//移动触摸点2

    private Vector3 scene_Touch1;//在场景中的触摸位置1
    private Vector3 scene_Touch2;//在场景中的触摸位置2
    private Vector3 touch_Middle;//触摸点间的中点
    private Vector3 camera_Offset;//摄像机移动时偏移量
    private Vector3 camera_Zoom_Offset;//相机变焦时的偏移量

    public float scaleMIN_Y;//最低高度
    float scaleMAX_Y;//最高高度
    float camera_Height;//当前摄像机高度

    float halfFOV;//相机视野角度
    float aspect;//摄像机长宽比
    float height;//画面高度（/2）
    float width;//画面宽度（/2）

    float update_X;//拖动时x轴位置
    float update_Y;//拖动时y轴位置

    float start_Zoom_X;//触摸点在屏幕的占比
    float start_Zoom_Y;

    int oldDistance;//上一次触摸点1，2之间的距离
    int newDistance; //当前触摸点1，2之间的距离

    bool canera_Lock = false;//检测是否为多指触碰
    bool oldTouch_Update = false;//检测多指触碰时是否更新过旧的触摸点


    //public Dialogue_Management Dialogue_management;
    float currentScale;

    void Start()
    {
        camera_Height = transform.position.z;
        map_Border =Map_Management.map_Management.boundary;
        Update_camera_FOV();
        MIX_scale();

#if UNITY_ANDROID
        Input.multiTouchEnabled = true;//开启多点触碰
#endif
    }

    void LateUpdate()
    {
        Control();
    }

    public void MIX_scale()
    {
        float scaleMAX_height = (map_Border.y - transform.position.y) / Mathf.Tan(halfFOV);//不考虑 Y 轴得到的最远缩放距离
        float scaleMAX_width = ((map_Border.x - transform.position.x) / aspect) / Mathf.Tan(halfFOV);//不考虑x轴得到的最远缩放距离
        scaleMAX_Y = Mathf.Max(scaleMAX_height, scaleMAX_width) + (map_Point.position.z);
        //由于摄像机z坐标为负数，因此取最大值作为极限距离，需要通过地图的位置增加极限距离才是最大高度
    }

    private void Update_camera_FOV()//更新摄像机视野
    {
        halfFOV = (Camera.main.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        aspect = Camera.main.aspect;
        
        height = Mathf.Abs(camera_Height - map_Point.position.z) * Mathf.Tan(halfFOV);
        width = height * aspect;
    }

    private void Control()//移动,改变视野(模拟量)
    {
#if UNITY_EDITOR_WIN
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Single_Touch = hit.point;
                start_Single_Touch = hit.point;//避免在边缘处再次点击时old_moveTouch为上次结束触摸的位置
            }
        }
        if (Input.GetMouseButton(0))//单指触碰
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                camera_Offset = transform.position - hit.point;//偏移量

                update_X = Mathf.Clamp(Single_Touch.x + camera_Offset.x, map_Border.x + (width), -map_Border.x - (width));
                update_Y = Mathf.Clamp(Single_Touch.y + camera_Offset.y, map_Border.y + (height), -map_Border.y - (height) + Map_Management.map_Management.Innerradius);

                if ((update_X == map_Border.x + (width) || update_X == -map_Border.x - (width) ||
                  update_Y == map_Border.y + (height) || update_Y == -map_Border.y - (height) + Map_Management.map_Management.Innerradius))
                {
                    Single_Touch = start_Single_Touch;
                    update_X = Mathf.Clamp(Single_Touch.x + camera_Offset.x, map_Border.x + (width), -map_Border.x - (width));
                    update_Y = Mathf.Clamp(Single_Touch.y + camera_Offset.y, map_Border.y + (height), -map_Border.y - (height) + Map_Management.map_Management.Innerradius);
                }
                if (Vector3.Distance(transform.position, new Vector3(update_X, update_Y, transform.position.z)) > 1)
                {
                    Unit_Management.unit_Management.Order_lock = true;
                }
                transform.position = new Vector3(update_X, update_Y, transform.position.z);
            }
            start_Single_Touch = transform.position - camera_Offset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Unit_Management.unit_Management.Order_lock = false;
        }

        currentScale = Input.GetAxis("Mouse ScrollWheel") * 50f;

        Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit3;
        if (Physics.Raycast(ray3, out hit3))
        {
            start_Position = this.transform.position;
            touch_Middle = hit3.point;
            start_Zoom_X = (touch_Middle.x - start_Position.x) / width;
            start_Zoom_Y = (touch_Middle.y - start_Position.y) / height;
        }

        if (currentScale < 0)
        {
            camera_Height = Mathf.Clamp(camera_Height - 1f, scaleMAX_Y, scaleMIN_Y);
            Update_camera_FOV();//更新模拟高度
            transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);


            if (transform.position.y + height >= -map_Border.y)//上边溢出
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y + height) - (Mathf.Abs(map_Border.y) + Map_Management.map_Management.Innerradius)), camera_Height);
            }
            else if (transform.position.y - height <= map_Border.y)//下边溢出
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y - height) + (Mathf.Abs(map_Border.y))), camera_Height);
            }


            if (transform.position.x + width >= -map_Border.x)//左边溢出
            {
                transform.position = new Vector3(transform.position.x - ((transform.position.x + width) - (Mathf.Abs(map_Border.x))), transform.position.y, camera_Height);
            }
            else if (transform.position.x - width <= map_Border.x)//右边溢出
            {
                transform.position = new Vector3(transform.position.x - ((transform.position.x - width) + (Mathf.Abs(map_Border.x))), transform.position.y, camera_Height);
            }
        }
        if (currentScale > 0 && camera_Height < scaleMIN_Y)
        {

            camera_Height = Mathf.Clamp(camera_Height + 1, scaleMAX_Y, scaleMIN_Y);
            Update_camera_FOV();
            transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);
        }



#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1&& canera_Lock == false)//单指触碰
        {
            if (Input.touches[0].phase == TouchPhase.Began)//在开始触摸时要更新固定点(首次触摸位置)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Single_Touch = hit.point;
                    start_Single_Touch = hit.point;//避免在边缘处再次点击时old_moveTouch为上次结束触摸的位置
                }
            }
            if (Input.touches[0].phase == TouchPhase.Moved) //手指在屏幕上移动
            {
                Unit_Management.unit_Management.Order_lock = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    camera_Offset = transform.position - hit.point;//偏移量

                    update_X = Mathf.Clamp(Single_Touch.x + camera_Offset.x, map_Border.x + (width), -map_Border.x - (width));
                    update_Y = Mathf.Clamp(Single_Touch.y + camera_Offset.y, map_Border.y + (height), -map_Border.y - (height) + Map_Management.map_Management.Innerradius);

                    if ((update_X == map_Border.x + (width) || update_X == -map_Border.x - (width) ||
                      update_Y == map_Border.y + (height) || update_Y == -map_Border.y - (height) + Map_Management.map_Management.Innerradius))
                    {
                        Single_Touch = start_Single_Touch;
                        update_X = Mathf.Clamp(Single_Touch.x + camera_Offset.x, map_Border.x + (width), -map_Border.x - (width));
                        update_Y = Mathf.Clamp(Single_Touch.y + camera_Offset.y, map_Border.y + (height), -map_Border.y - (height) + Map_Management.map_Management.Innerradius);
                    }

                    transform.position = new Vector3(update_X, update_Y, transform.position.z);
                }
                start_Single_Touch = transform.position - camera_Offset;
            }
        }
        if (Input.touchCount == 2)//多指触碰
        {
            canera_Lock = true;
            Unit_Management.unit_Management.Order_lock = true;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Ray Touch1_ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit Touch1_hit;
                if (Physics.Raycast(Touch1_ray, out Touch1_hit))
                {
                    scene_Touch1 = Touch1_hit.point;
                }
                Ray Touch2_ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
                RaycastHit Touch2_hit;
                if (Physics.Raycast(Touch2_ray, out Touch2_hit))
                {
                    scene_Touch2 = Touch2_hit.point;
                }
                touch_Middle = (scene_Touch1 + scene_Touch2) / 2;

                start_Position = this.transform.position;
                start_Zoom_X = (touch_Middle.x - start_Position.x) / width;
                start_Zoom_Y = (touch_Middle.y - start_Position.y) / height;


                newTouch1 = Input.GetTouch(0).position;
                newTouch2 = Input.GetTouch(1).position;

                oldDistance = (int)Vector2.Distance(oldTouch1, oldTouch2);
                newDistance = (int)Vector2.Distance(newTouch1, newTouch2);

                if (oldTouch_Update==true)
                {
                    camera_Height = Mathf.Clamp(camera_Height - ((oldDistance - newDistance) * 0.09f), scaleMAX_Y, scaleMIN_Y);
                    Update_camera_FOV();

                    if (oldDistance > newDistance)
                    {
                        transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);

                        if (transform.position.y + height >= -map_Border.y)//上边溢出
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y + height) - (Mathf.Abs(map_Border.y) + Map_Management.map_Management.Innerradius)), camera_Height);
                        }
                        else if (transform.position.y - height <= map_Border.y)//下边溢出
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y - height) + (Mathf.Abs(map_Border.y))), camera_Height);
                        }


                        if (transform.position.x + width >= -map_Border.x)//左边溢出
                        {
                            transform.position = new Vector3(transform.position.x - ((transform.position.x + width) - (Mathf.Abs(map_Border.x))), transform.position.y, camera_Height);
                        }
                        else if (transform.position.x - width <= map_Border.x)//右边溢出
                        {
                            transform.position = new Vector3(transform.position.x - ((transform.position.x - width) + (Mathf.Abs(map_Border.x))), transform.position.y, camera_Height);
                        }
                    }
                    else if (oldDistance < newDistance)
                    {
                        transform.position = new Vector3(touch_Middle.x - (width * start_Zoom_X), touch_Middle.y - (height * start_Zoom_Y), camera_Height);
                    }
                }
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;

                oldTouch_Update = true;
            }
        }
        if (Input.touchCount == 0)
        {
            canera_Lock = false;
            oldTouch_Update = false;
            Unit_Management.unit_Management.Order_lock = false;
        }
#endif
    }
}
    
    

