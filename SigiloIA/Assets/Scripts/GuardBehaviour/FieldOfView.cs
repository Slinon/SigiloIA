using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    // @IGM ----------------------------------------------------------------
    // Estructura para almacenar la información de los raycast que lancemos.
    // ---------------------------------------------------------------------
    public struct ViewCastInfo
    {

        public bool hit;                        // Booleano para saber si se ha golpeado algo
        public Vector3 point;                   // Punto en el que se ha golpeado
        public float dist;                      // Distancia a la que ha llegado el raycast
        public float angle;                     // Angulo del raycast

        // @IGM -------------------
        // Constructor de la clase.
        // ------------------------
        public ViewCastInfo(bool hit, Vector3 point, float dist, float angle)
        {

            this.hit = hit;
            this.point = point;
            this.dist = dist;
            this.angle = angle;

        }

    }

    // @IGM ---------------------------------------
    // Estructura con la informacion de la esquina.
    // --------------------------------------------
    public struct EdgeInfo
    {

        public Vector3 pointA;              // Punto A de la esquina
        public Vector3 pointB;              // Punto B de la esquina

        // @IGM -------------------
        // Constructor de la clase.
        // ------------------------
        public EdgeInfo(Vector3 pointA, Vector3 pointB)
        {

            this.pointA = pointA;
            this.pointB = pointB;

        }
    }

    [Header("Principal")]
    public float viewRadius;               // Radio de vision del enemigo
    [Range(0, 360)]
    public float viewAngle;                 // Angulo de vision del enemigo
    [HideInInspector]
    public Transform player;                // Posición del jugador
    public LayerMask playerMask;            // Capa del jugador
    public LayerMask obstacleMask;          // Capa de los obstaculos
    [Header("Malla")]
    public MeshFilter viewMeshFilter;       // Filtro de malla de la vista del enemigo
    public MeshRenderer meshRenderer;       // Malla de renderizado del campo de vision
    public float meshResolution;            // Resolución de la malla de visualización
    public int edgeResolveIterations;       // Veces que se ejecutará el bucle de encontrar la esquina de un obstaculo
    public float edgeDistThreshold;         // Distancia limite del vertice
    
    private Mesh viewMesh;                  // Malla de la vista del enemigo

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {
        // Creamos la malla
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";

        // Asignamos la malla al filtro de la malla
        viewMeshFilter.mesh = viewMesh;

        // Lanzamos la corrutina cada 0.2 segundos
        StartCoroutine("FindPlayerWithDelay", 0.2f);

    }

    // @IGM ---------------------------
    // Update is called once per frame.
    // --------------------------------
    private void LateUpdate()
    {

        // Dibujamos el campo de vision
        DrawFieldOfView();

    }

    // @IGM -------------------------------------------
    // Corrutina para buscar al jugador no tan seguido.
    // ------------------------------------------------
    IEnumerator FindPlayerWithDelay(float delay)
    {

        while (true)
        {

            // Esperamos el tiempo marcado y lanzamos el metodo
            yield return new WaitForSeconds(delay);
            FindPlayer();

        }

    }

    // @IGM ---------------------------------------------
    // Metodo para encontrar al jugador dentro del rango.
    // --------------------------------------------------
    private void FindPlayer()
    {

        player = null;

        // Buscamos al jugador en la capa del jugador
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, 
            viewRadius, playerMask);

        // Recorremos el array por si hay más de un jugador
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {

            // Asignamos la posición del jugador
            Transform playeInRange = targetsInViewRadius[i].transform;

            // Buscamos la dirección del jugador
            Vector3 dirToPlayer = (playeInRange.position - transform.position).normalized;

            // Comprobamos que el angulo del jugador está dentro del ángulo de vision
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {

                // Buscamos la distancia entre el enemigo y el jugador
                float distToPlayer = Vector3.Distance(transform.position, playeInRange.position);

                // Comprobamos que no hay ningún obstáculo por el medio
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {

                    // Asignamos la variable jugador
                    player = playeInRange;

                }

            }

        }

    }

    // @IGM ----------------------------------------------
    // Metodo para dibujar el campo de visión del guardia.
    // ---------------------------------------------------
    private void DrawFieldOfView()
    {

        // Calculamos las veces que hay que dibujar las lineas verticales de la malla
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);

        // Calculamos el tamaño de los pasos
        float stepAngleSize = viewAngle / stepCount;

        // Creamos la lista de puntos de los pasos de la malla
        List<Vector3> viewPoints = new List<Vector3>();

        // Creamos la variable de la informacion antigua del raycast
        ViewCastInfo oldViewCast = new ViewCastInfo();

        // Recorremos los pasos
        for (int i = 0; i <= stepCount; i++)
        {

            // Creamos los raycast en el angulo correcto por cada paso
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);


            // Comprobamos si no es la primera iteracion
            if (i > 0)
            {

                // Comprobamos si se ha excedido el limite
                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dist -
                    newViewCast.dist) > edgeDistThreshold;

                // Comprobamos si el raycast anterior golpeo o si el nuevo no golpeo
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded))
                {

                    // Calculamos un nuevo vertice
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    // Comprobamos si el puntoA no es el origen
                    if (edge.pointA != Vector3.zero)
                    {

                        // Lo añadimos a la lista
                        viewPoints.Add(edge.pointA);

                    }

                    // Comprobamos si el puntoB no es el origen
                    if (edge.pointB != Vector3.zero)
                    {

                        // Lo añadimos a la lista
                        viewPoints.Add(edge.pointB);

                    }

                }

            }

            // Guardamos la posición en la lista
            viewPoints.Add(newViewCast.point);

            // Actualizamos la información anterior
            oldViewCast = newViewCast;

        }

        // Preparamos las variables para construir la malla
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        // Asignamos el primer vertice al origen
        vertices[0] = Vector3.zero;

        // Recorremos el arrey de vertices
        for (int i = 0; i < vertexCount - 1; i++)
        {

            // Asignamos el vertice con la posición relativa del enemigo
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            // Comprobamos que no hemos llegado al final
            if (i < vertexCount - 2)
            {

                // Añadimos los tres vertices del triangulo
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;

            }

        }
        
        // Limpiamos la malla
        viewMesh.Clear();

        // Recalculamos la malla con nuestros parametros
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    // @IGM -------------------------------------------------------
    // Metodo para averiguar la dirección dado el angulo de vision.
    // ------------------------------------------------------------
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {

        // Comprobamos si el ángulo está en la posición global
        if (!angleIsGlobal)
        {

            // Añadimos la rotacion del objeto
            angleInDegrees += transform.eulerAngles.y;

        }

        // Devolvemos la direccion del angulo
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, 
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }

    // @IGM ----------------------------------------------------------
    // Función para encotrar el vértice de la esquina de un obstaculo.
    // ---------------------------------------------------------------
    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {

        // Establecemos los angulos y los puntos
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        // Repetimos la busqueda el numero de veces que indiquemos
        for (int i = 0; i < edgeResolveIterations; i++)
        {

            // Conseguimos la bisectriz de los dos angulos
            float angle = (minAngle + maxAngle) / 2;

            // Lanzamos el raycast
            ViewCastInfo newViewCast =  ViewCast(angle);

            // Comprobamos si se ha excedido el limite
            bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dist -
                    newViewCast.dist) > edgeDistThreshold;

            // Comprobamos si el nuevo raycast ha golpeado el obstaculo igual que el minimo
            if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceeded)
            {
                // Lo convertimos en el nuevo minimo
                minAngle = angle;
                minPoint = newViewCast.point;

            }
            else
            {

                // Comprobamos si el nuevo raycast ha golpeado el obstaculo igual que el maximo
                // Lo convertimos en el nuevo maximo
                maxAngle = angle;
                maxPoint = newViewCast.point;

            }

        }

        // Devolvemos la información de la esquina
        return new EdgeInfo(minPoint, maxPoint);

    }

    // @IGM ------------------------------------------------------------
    // Función para saber si se colisiona con algo al lanzar un raycast.
    // -----------------------------------------------------------------
    private ViewCastInfo ViewCast(float globalAngle)
    {

        // Establecemos la direccion
        Vector3 dir = DirFromAngle(globalAngle, true);

        // Comprobamos si el raycast ha golpeado algo
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, viewRadius,
            obstacleMask))
        {

            // Devolvemos la informacion
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);

        }
        else
        {

            // Devolvemos la informacion
            return new ViewCastInfo(false, transform.position + dir * viewRadius,
                viewRadius, globalAngle);

        }

    }

}
