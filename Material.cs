
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenTKBase
{
    public class Material
    {
        public Shader                       shader;
        public Dictionary<string, object>   properties;

        public Material(Shader shader)
        {
            this.shader = shader;
            properties = new Dictionary<string, object>();
        }

        public void SetVector2(string name, Vector2 v)
        {
            properties[name] = new Vector2(v.X, v.Y);
        }
        public void SetVector3(string name, Vector3 v)
        {
            properties[name] = new Vector3(v.X, v.Y, v.Z);
        }
        public void SetVector4(string name, Vector4 v)
        {
            properties[name] = new Vector4(v.X, v.Y, v.Z, v.W);
        }

        public void SetColor(string name, Color4 color)
        {
            properties[name] = new Vector4(color.R, color.G, color.B, color.A);
        }
        public Color4 GetColor(string name)
        {
            if (properties.ContainsKey(name))
            {
                if (properties[name] is Vector4 v)
                {
                    return new Color4(v.X, v.Y, v.Z, v.W);
                }
            }
            return Color4.White;
        }

        public T Get<T>(string name) => (T)properties[name];
    }
}
