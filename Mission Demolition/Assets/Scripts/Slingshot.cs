using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
        
    // поля, устанавливаемые в инспекторе Unity
        [Header("Set in Inspector")] 
        public GameObject prefabProjectile;
        public float velocityMult = 8f;
    // поля, устанавливаемые динамически
        [Header("Set Dynamically")] 
        public GameObject launchPoint;
        public Vector3 launchPos; 
        public GameObject projectile; // b
        public bool aimingMode;
        private Rigidbody projectileRigidbody;
    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint"); 
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive( false ); 
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter() {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive( true ); 
    }

    void OnMouseExit() {
    //print("Slingshot:OnMouseExit()");
    launchPoint.SetActive( false ); 
    }

    void OnMouseDown() 
    { 
        // Игрок нажал кнопку мыши, когда указатель находился над рогаткой
        aimingMode = true;
        // Создать снаряд
        projectile = Instantiate( prefabProjectile ) as GameObject;
        // Поместить в точку launchPoint
        projectile.transform.position = launchPos;
        // Сделать его кинематическим
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }   
    void Update() 
    {
        
        // Если рогатка не в режиме прицеливания, не выполнять этот код
        if (!aimingMode) return;
        Vector3 mousePos2D = Input.mousePosition; // с
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D );
        // Найти разность координат между launchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D-launchPos;
        // Ограничить mouseDelta радиусом коллайдера объекта Slingshot // d
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
        mouseDelta.Normalize();
        mouseDelta *= maxMagnitude;
        }
        // Передвинуть снаряд в новую позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if ( Input.GetMouseButtonUp(0) ) {
         // Кнопка мыши отпущена
        aimingMode = false;
        projectileRigidbody.isKinematic = false;
        projectileRigidbody.velocity = -mouseDelta * velocityMult;
        FollowCam.POI = projectile;
        projectile = null;

        
        
        }

    }

}
