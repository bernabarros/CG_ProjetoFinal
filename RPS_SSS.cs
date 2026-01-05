using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenTKBase
{
    public class RPS_SSS : RenderPipeline
    {
        private int _fbo = -1;
        private int _depthTexture = -1;
        private int _resolutionWidth = 1280;
        private int _resolutionHeight = 720;
        private int _quadVAO = -1;
        private int _quadVBO;

        // Simple shader
        public int DepthShaderId; 

        // Complex shader
        public int SSSShaderId;   

        public override void Render(Scene scene)
        {
            if (scene == null) return;

            // Lazy initialization
            if (_fbo == -1) InitializeResources();

            var allCameras = scene.FindObjectsOfType<Camera>();
            var allRender = scene.FindObjectsOfType<Renderable>();

            foreach (var camera in allCameras)
            {
      
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);
                GL.Viewport(0, 0, _resolutionWidth, _resolutionHeight);

                // Clear only the Depth Buffer
                GL.Clear(ClearBufferMask.DepthBufferBit);

                GL.UseProgram(DepthShaderId);

                foreach (var render in allRender)
                {
                    SetupMatrices(render, camera, DepthShaderId); 
                    render.Render(camera);
                }

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                GL.Viewport(0, 0, _resolutionWidth, _resolutionHeight);
                
                // Clear the screen with the camera's background color
                GL.ClearColor(camera.GetClearColor());

                GL.UseProgram(SSSShaderId);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _depthTexture);
                GL.Uniform1(GL.GetUniformLocation(SSSShaderId, "screenDepthMap"), 0);

                // Define a light direction
                Vector3 lightDirWorld = new Vector3(-0.5f, -1.0f, -0.5f); 
                Matrix4 viewMatrix = camera.transform.worldToLocalMatrix;
                Vector3 lightDirView = (new Vector4(lightDirWorld, 0.0f) * viewMatrix).Xyz.Normalized();
                Matrix4 projMatrix = camera.projection;

                // Pass the Projection Matrix and View-Space Light Direction to the shader
                GL.UniformMatrix4(GL.GetUniformLocation(SSSShaderId, "projection"), false, ref projMatrix);
                GL.Uniform3(GL.GetUniformLocation(SSSShaderId, "lightDirView"), lightDirView);

                DrawQuad();
            }
        }

        private void SetupMatrices(Renderable render, Camera cam, int shaderId)
        {
            Matrix4 modelMatrix = render.transform.localToWorldMatrix;
            int modelLoc = GL.GetUniformLocation(shaderId, "model");
            GL.UniformMatrix4(modelLoc, false, ref modelMatrix);

            Matrix4 viewMatrix = cam.transform.worldToLocalMatrix;
            int viewLoc = GL.GetUniformLocation(shaderId, "view");
            GL.UniformMatrix4(viewLoc, false, ref viewMatrix);

            Matrix4 projMatrix = cam.projection;
            int projLoc = GL.GetUniformLocation(shaderId, "projection");
            GL.UniformMatrix4(projLoc, false, ref projMatrix);
        }

        private void InitializeResources()
        {
            _fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fbo);

            _depthTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _depthTexture);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, _resolutionWidth, _resolutionHeight, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _depthTexture, 0);
            
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            // Setup the Full-Screen Quad geometry
            float[] quadVertices = { 
                -1.0f,  1.0f,  0.0f, 1.0f,
                -1.0f, -1.0f,  0.0f, 0.0f,
                 1.0f, -1.0f,  1.0f, 0.0f,

                -1.0f,  1.0f,  0.0f, 1.0f,
                 1.0f, -1.0f,  1.0f, 0.0f,
                 1.0f,  1.0f,  1.0f, 1.0f
            };

            _quadVAO = GL.GenVertexArray();
            _quadVBO = GL.GenBuffer();
            GL.BindVertexArray(_quadVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _quadVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);
            
            // Position
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            // UV Coordinates
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            
            GL.BindVertexArray(0);
        }

        private void DrawQuad()
        {
            GL.BindVertexArray(_quadVAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }
    }
}