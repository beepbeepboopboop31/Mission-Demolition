using UnityEngine;
using System.Collections;
public class FollowCam : MonoBehaviour {
        static public GameObject POI; // Ссылка на интересующий объект // а
        public float easing = 0.05f;
        public Vector2 minXY = Vector2.zero;


        [Header("Set Dynamically")]
        public float camZ; // Желаемая координата Z камеры
        void Awake() {
        camZ = this.transform.position.z;
        }
        void FixedUpdate () {
        // Однострочная версия if не требует фигурных скобок
        if (POI == null) return; // выйти, если нет интересующего объекта 
        // Получить позицию интересующего объекта
        Vector3 destination = POI.transform.position;
        // Определить точку между текущим местоположением камеры и destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Ограничить X и Y минимальными значениями
        destination.x = Mathf.Max( minXY.x, destination.x );
        destination.y = Mathf.Max( minXY.y, destination.y );
        // Принудительно установить значение destination.z равным camZ, чтобы
        // отодвинуть камеру подальше
        destination.z = camZ;
        // Поместить камеру в позицию destination
        transform.position = destination;
        // Изменить размер orthographicSize камеры., чтобы земля
        // оставалась в поле зрения
        Camera.main.orthographicSize = destination.y + 10;
        }
}
