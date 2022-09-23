using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    AIMovement aIMovement;                          // Gestión de movimiento de la IA

    // @IGM -----------------------------------------
    // Start is called before the first frame update.
    // ----------------------------------------------
    private void Start()
    {

        aIMovement = GetComponent<AIMovement>();
        Pathfinding.Instance = new Pathfinding(10, 10);
        
    }

    // @IGM ---------------------------
    // Update is called once per frame.
    // --------------------------------
    private void Update()
    {

        // Movemos la IA
        aIMovement.MoveAI();

        // Comprobamos que hemos pulsado el click izquierdo
        if (Input.GetMouseButtonDown(0))
        {

            // Accedemos al punto en el mundo donde hemos clicado
            Vector3 mouseWorldPosition = mouseWorldPoint();

            // Establecemos el camino
            aIMovement.SetTargetPosition(mouseWorldPosition);

        }

    }

    // @IGM ----------------------------------------------
    // Fución para sacar la posición del ratón en el mapa.
    // ---------------------------------------------------
    private Vector3 mouseWorldPoint()
    {

        Vector3 worldPosition = new Vector3();

        // Lanzamos un raycast desde la posición del raton
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        // Comprobamos si ha colisionado con algo
        if (Physics.Raycast(ray, out hitData, 1000))
        {

            // Guardamos la posicion
            worldPosition = hitData.point;
            worldPosition.y = 1f;

        }
        else
        {

            // No ha colisionado con nada
            worldPosition = new Vector3(-1, -1, -1);

        }

        return worldPosition;

    }

}
