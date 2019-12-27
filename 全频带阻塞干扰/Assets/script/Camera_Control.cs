using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Control : MonoBehaviour
{
    public Transform map_point;

    private Vector3 start_position;//初始摄像机位置
    private Vector3 start_Single_Touch;//首次触碰位置
    private Vector3 Single_Touch;//拖动触摸位置
    private Vector3 map_border;//地图边界


    private Vector3 oldTouch1;//初始触摸点1
    private Vector3 oldTouch2;//初始触摸点2

    private Vector3 newTouch1;//移动触摸点1
    private Vector3 newTouch2;//移动触摸点2

    private Vector3 scene_Touch1;//在场景中的触摸位置1
    private Vector3 scene_Touch2;//在场景中的触摸位置2
    private Vector3 touch_middle;//触摸点间的中点
    private Vector3 camera_offset;//摄像机移动时偏移量
    private Vector3 camera_zoom_offset;//相机变焦时的偏移量

    public float scaleMIN_Y;//最低高度
    float scaleMAX_Y;//最高高度
    float camera_Height;//当前摄像机高度

    float halfFOV;//相机视野角度
    float aspect;//摄像机长宽比
    float height;//画面高度（/2）
    float width;//画面宽度（/2）

    float update_x;//拖动时x轴位置
    float update_y;//拖动时y轴位置

    float start_zoom_x;//触摸点在屏幕的占比
    float start_zoom_y;

    int oldDistance;//上一次触摸点1，2之间的距离
    int newDistance; //当前触摸点1，2之间的距离

    bool canera_lock = false;//检测是否为多指触碰
    bool oldTouch_update = false;//检测多指触碰时是否更新过旧的触摸点


    //public Dialogue_Management Dialogue_management;
    float currentScale;

    void Start()
    {
        camera_Height = transform.position.z;
        map_border =Map_Management.map_Management.boundary;
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
        float scaleMAX_height = (map_border.y - transform.position.y) / Mathf.Tan(halfFOV);//不考虑 Y 轴得到的最远缩放距离
        float scaleMAX_width = ((map_border.x - transform.position.x) / aspect) / Mathf.Tan(halfFOV);//不考虑x轴得到的最远缩放距离
        scaleMAX_Y = Mathf.Max(scaleMAX_height, scaleMAX_width);//由于摄像机z坐标为负数，因此取最大值作为极限高度
    }

    private void Update_camera_FOV()//更新摄像机视野
    {
        halfFOV = (Camera.main.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        aspect = Camera.main.aspect;
        height = Mathf.Abs(camera_Height - map_point.position.z) * Mathf.Tan(halfFOV);
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
                camera_offset = transform.position - hit.point;//偏移量

                update_x = Mathf.Clamp(Single_Touch.x + camera_offset.x, map_border.x + (width), -map_border.x - (width));
                update_y = Mathf.Clamp(Single_Touch.y + camera_offset.y, map_border.y + (height), -map_border.y - (height) + Map_Management.map_Management.innerRadius);

                if ((update_x == map_border.x + (width) || update_x == -map_border.x - (width) ||
                  update_y == map_border.y + (height) || update_y == -map_border.y - (height) + Map_Management.map_Management.innerRadius))
                {
                    Single_Touch = start_Single_Touch;
                    update_x = Mathf.Clamp(Single_Touch.x + camera_offset.x, map_border.x + (width), -map_border.x - (width));
                    update_y = Mathf.Clamp(Single_Touch.y + camera_offset.y, map_border.y + (height), -map_border.y - (height) + Map_Management.map_Management.innerRadius);
                }
                if (Vector3.Distance(transform.position, new Vector3(update_x, update_y, transform.position.z)) > 1)
                {
                    Unit_Management.Unit_management.order_lock = true;
                }
                transform.position = new Vector3(update_x, update_y, transform.position.z);
            }
            start_Single_Touch = transform.position - camera_offset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Unit_Management.Unit_management.order_lock = false;
        }

        currentScale = Input.GetAxis("Mouse ScrollWheel") * 50f;

        Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit3;
        if (Physics.Raycast(ray3, out hit3))
        {
            start_position = this.transform.position;
            touch_middle = hit3.point;
            start_zoom_x = (touch_middle.x - start_position.x) / width;
            start_zoom_y = (touch_middle.y - start_position.y) / height;
        }

        if (currentScale < 0)
        {
            camera_Height = Mathf.Clamp(camera_Height - 1f, scaleMAX_Y, scaleMIN_Y);
            Update_camera_FOV();//更新模拟高度
            transform.position = new Vector3(touch_middle.x - (width * start_zoom_x), touch_middle.y - (height * start_zoom_y), camera_Height);


            if (transform.position.y + height >= -map_border.y)//上边溢出
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y + height) - (Mathf.Abs(map_border.y) + Map_Management.map_Management.innerRadius)), camera_Height);
            }
            else if (transform.position.y - height <= map_border.y)//下边溢出
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y - height) + (Mathf.Abs(map_border.y))), camera_Height);
            }


            if (transform.position.x + width >= -map_border.x)//左边溢出
            {
                transform.position = new Vector3(transform.position.x - ((transform.position.x + width) - (Mathf.Abs(map_border.x))), transform.position.y, camera_Height);
            }
            else if (transform.position.x - width <= map_border.x)//右边溢出
            {
                transform.position = new Vector3(transform.position.x - ((transform.position.x - width) + (Mathf.Abs(map_border.x))), transform.position.y, camera_Height);
            }
        }
        if (currentScale > 0 && camera_Height < scaleMIN_Y)
        {

            camera_Height = Mathf.Clamp(camera_Height + 1, scaleMAX_Y, scaleMIN_Y);
            Update_camera_FOV();
            transform.position = new Vector3(touch_middle.x - (width * start_zoom_x), touch_middle.y - (height * start_zoom_y), camera_Height);
        }



#endif

#if UNITY_ANDROID
        if (Input.touchCount == 1&& canera_lock == false)//单指触碰
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
                Unit_Management.Unit_management.order_lock = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    camera_offset = transform.position - hit.point;//偏移量

                    update_x = Mathf.Clamp(Single_Touch.x + camera_offset.x, map_border.x + (width), -map_border.x - (width));
                    update_y = Mathf.Clamp(Single_Touch.y + camera_offset.y, map_border.y + (height), -map_border.y - (height) + Map_Management.innerRadius);

                    if ((update_x == map_border.x + (width) || update_x == -map_border.x - (width) ||
                      update_y == map_border.y + (height) || update_y == -map_border.y - (height) + Map_Management.innerRadius))
                    {
                        Single_Touch = start_Single_Touch;
                        update_x = Mathf.Clamp(Single_Touch.x + camera_offset.x, map_border.x + (width), -map_border.x - (width));
                        update_y = Mathf.Clamp(Single_Touch.y + camera_offset.y, map_border.y + (height), -map_border.y - (height) + Map_Management.innerRadius);
                    }

                    transform.position = new Vector3(update_x, update_y, transform.position.z);
                }
                start_Single_Touch = transform.position - camera_offset;
            }
        }
        if (Input.touchCount == 2)//多指触碰
        {
            canera_lock = true;
            Unit_Management.Unit_management.order_lock = true;
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
                touch_middle = (scene_Touch1 + scene_Touch2) / 2;

                start_position = this.transform.position;
                start_zoom_x = (touch_middle.x - start_position.x) / width;
                start_zoom_y = (touch_middle.y - start_position.y) / height;


                newTouch1 = Input.GetTouch(0).position;
                newTouch2 = Input.GetTouch(1).position;

                oldDistance = (int)Vector2.Distance(oldTouch1, oldTouch2);
                newDistance = (int)Vector2.Distance(newTouch1, newTouch2);

                if (oldTouch_update==true)
                {
                    camera_Height = Mathf.Clamp(camera_Height - ((oldDistance - newDistance) * 0.09f), scaleMAX_Y, scaleMIN_Y);
                    Update_camera_FOV();

                    if (oldDistance > newDistance)
                    {
                        transform.position = new Vector3(touch_middle.x - (width * start_zoom_x), touch_middle.y - (height * start_zoom_y), camera_Height);

                        if (transform.position.y + height >= -map_border.y)//上边溢出
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y + height) - (Mathf.Abs(map_border.y) + Map_Management.innerRadius)), camera_Height);
                        }
                        else if (transform.position.y - height <= map_border.y)//下边溢出
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y - ((transform.position.y - height) + (Mathf.Abs(map_border.y))), camera_Height);
                        }


                        if (transform.position.x + width >= -map_border.x)//左边溢出
                        {
                            transform.position = new Vector3(transform.position.x - ((transform.position.x + width) - (Mathf.Abs(map_border.x))), transform.position.y, camera_Height);
                        }
                        else if (transform.position.x - width <= map_border.x)//右边溢出
                        {
                            transform.position = new Vector3(transform.position.x - ((transform.position.x - width) + (Mathf.Abs(map_border.x))), transform.position.y, camera_Height);
                        }
                    }
                    else if (oldDistance < newDistance)
                    {
                        transform.position = new Vector3(touch_middle.x - (width * start_zoom_x), touch_middle.y - (height * start_zoom_y), camera_Height);
                    }
                }
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;

                oldTouch_update = true;
            }
        }
        if (Input.touchCount == 0)
        {
            canera_lock = false;
            oldTouch_update = false;
            Unit_Management.Unit_management.order_lock = false;
        }
#endif

    }
}
    
    

