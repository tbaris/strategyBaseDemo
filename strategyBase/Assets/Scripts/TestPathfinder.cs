using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinder : MonoBehaviour
{
   
    void Start()
    {
        List<GridCell> testPath = gameObject.GetComponent<Pathfind>()
            .FindPath(GridManager.Instance.GridCells[0, 0], GridManager.Instance.GridCells[15, 9]);

      //  Debug.Log(testPath[0]);
        for (var index = 0; index < testPath?.Count; index++)
        {
            GridCell cell = testPath[index];
            Debug.Log(cell.GridPos);
        }
    }

 
}
