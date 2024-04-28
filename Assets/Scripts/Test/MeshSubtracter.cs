//using System.Collections;
//using System.Collections.Generic;
//using Meta.XR.MRUtilityKit;
//using UnityEditor;
//using UnityEngine;
//using Parabox.CSG; 
//public class MeshSubtracter : MonoBehaviour
//{
//    [SerializeField] private GameObject window;
//    public void GetRoomObjectAndDrillHole()
//    {
//        MRUKRoom mrukComponent = FindObjectOfType<MRUKRoom>();
//        GameObject mrukObject = mrukComponent.gameObject;
//        Debug.Log(mrukObject.name);
//        ApplyWindow(mrukObject);
//    }

//    private void ApplyWindow(GameObject obj)
//    {
//        // ApplyWindowSingle(obj);
//        foreach (Transform child in obj.transform)
//        {

//            foreach (Transform grandchild in child.gameObject.transform)
//            {
//                ApplyWindowSingle(grandchild.gameObject);
//            }
//        }
        
//    }

//    private void ApplyWindowSingle(GameObject obj)
//    {
//        if (!obj.GetComponent<MeshFilter>())
//        {
//            return;
//        }
        
//        // check if wall and window collider
//        Model result = CSG.Subtract(obj, window);
//        var composite = new GameObject();
//        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
//        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
//        var newcollider = composite.AddComponent<MeshCollider>();
//        newcollider.convex = true;
//        // composite.transform.parent = obj.transform.parent;

//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        // Model result = CSG.Subtract(wall, window);
//        // var composite = new GameObject();
//        // composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
//        // composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
