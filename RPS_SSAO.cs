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
        private GBuffer gBuffer;
        private Material geometryMaterial;

        private int fullscreenVAO = 0;

        private Shader debugNormalShader = Shader.Find("Shaders/debug_normal");

        public RPS_SSAO()
        {
            fullscreenVAO = GL.GenVertexArray();

            gBuffer = new GBuffer();
            geometryMaterial = new Material(Shader.Find("Shaders/geometry"));
        }


        public override void Render(Scene scene)
        {
            if (scene == null) return;

            var cameras = scene.FindObjectsOfType<Camera>();
            var renderables = scene.FindObjectsOfType<Renderable>();

            foreach (var camera in cameras)
            {
                GeometryPass(scene, camera, renderables);

                DebugDrawNormals(1280,720);
            }
        }

        private void GeometryPass(Scene scene, Camera camera, List<Renderable> renderables)
        {
            gBuffer.BindForWriting();

            GL.Viewport(0,0,1280,720);

            GL.ClearColor(0f,0f,0f,0f);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Shader.SetMatrix(Shader.MatrixType.Camera, camera.transform.worldToLocalMatrix);
            Shader.SetMatrix(Shader.MatrixType.Projection, camera.projection);

            
            foreach (var renderable in renderables)
            {
                var meshRenderer = renderable.GetComponent<MeshRenderer>();

                if (meshRenderer != null && meshRenderer.material != null)
                {
                    OpenTK.Mathematics.Color4 mainColor = meshRenderer.material.GetColor("Color");

                    geometryMaterial.SetColor("Color", mainColor);
                }
                
                renderable.RenderWithMaterial(camera, geometryMaterial);
            }
            
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