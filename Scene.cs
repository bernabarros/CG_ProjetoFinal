using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenTKBase
{
    public class Scene
    {
        private List<GameObject> objects = new List<GameObject>();

        public OpenTK.Mathematics.Vector3 ambientLight = new OpenTK.Mathematics.Vector3(1f, 1f, 1f);

        public Scene()
        {
        }

        public void Add(GameObject go)
        {
            objects.Add(go);            
        }

        public List<T> FindObjectsOfType<T>() where T: Component
        {
            List<T> ret = new List<T>();

            foreach (GameObject go in objects)
            {
                ret.AddRange(go.GetComponents<T>());
            }

            return ret;
        }

        public List<GameObject> GetAllObjects() => objects;
    }
}
