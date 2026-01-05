using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKBase;

namespace CG_ProjetoFinal
{
    public class RPS_SSAO : RenderPipeline
    {
        // Handle to the G-Buffer object
        private GBuffer gBuffer;

        // The material used specifically for the Geometry Pass
        private Material geometryMaterial;

        // An empty Vertex Array Object used for rendering full-screen quads
        private int fullscreenVAO = 0;

        // Shader used for debugging
        private Shader debugNormalShader = Shader.Find("Shaders/debug_normal");

        public RPS_SSAO()
        {
            // Generate a new Vertex Array Object ID
            fullscreenVAO = GL.GenVertexArray();

            // Start of GBuffer
            gBuffer = new GBuffer();
            geometryMaterial = new Material(Shader.Find("Shaders/geometry"));
        }

        // The main for the rendering pipeline
        public override void Render(Scene scene)
        {
            if (scene == null) return;

            // Retrieve all cameras and renderable objects from the scene
            var cameras = scene.FindObjectsOfType<Camera>();
            var renderables = scene.FindObjectsOfType<Renderable>();

            foreach (var camera in cameras)
            {
                // Perform the Geometry Pass
                GeometryPass(scene, camera, renderables);

                // Perform the Debug Pass
                DebugDrawNormals(1280,720);
            }
        }

        private void GeometryPass(Scene scene, Camera camera, List<Renderable> renderables)
        {
            gBuffer.BindForWriting();

            // Set the viewport size
            GL.Viewport(0,0,1280,720);

            // Set the clear color to black
            GL.ClearColor(0f,0f,0f,0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Pass the Camera's View and Projection matrices to the shaders
            Shader.SetMatrix(Shader.MatrixType.Camera, camera.transform.worldToLocalMatrix);
            Shader.SetMatrix(Shader.MatrixType.Projection, camera.projection);

            
            foreach (var renderable in renderables)
            {
                var meshRenderer = renderable.GetComponent<MeshRenderer>();

                // This ensures the G-Buffer records the correct albedo
                if (meshRenderer != null && meshRenderer.material != null)
                {
                    OpenTK.Mathematics.Color4 mainColor = meshRenderer.material.GetColor("Color");

                    geometryMaterial.SetColor("Color", mainColor);
                }
                
                //Render the object using the Geometry Material
                renderable.RenderWithMaterial(camera, geometryMaterial);
            }
            //Unbind the G-Buffer FBO
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        
        private void DebugDrawNormals(int windowWidth, int windowHeight)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0,0, 1280,720);

            GL.Disable(EnableCap.DepthTest);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(debugNormalShader.ProgramHandle);

            GL.BindVertexArray(fullscreenVAO);

            // Bind the normal texture
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, gBuffer.NormalTexture);
            
            int loc = GL.GetUniformLocation(debugNormalShader.ProgramHandle, "gNormal");
            if (loc != -1) GL.Uniform1(loc, 0);

            // Draw fullscreen triangle
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
        }
    }
}