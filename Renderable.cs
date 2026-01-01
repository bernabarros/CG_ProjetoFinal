namespace OpenTKBase
{
    public abstract class Renderable : Component
    {
        public abstract void Render(Camera camera);
        public virtual void RenderWithMaterial(Camera camera, Material material)
        {
            Render(camera);
        }
    }
}
