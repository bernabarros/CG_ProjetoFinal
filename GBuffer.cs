using OpenTK.Graphics.OpenGL;
using System;

namespace CG_ProjetoFinal
{
    public class GBuffer
    {
        public int FBO;
        public int NormalTexture;
        public int DepthTexture;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public GBuffer(int w = 1280, int h = 720)
        {
            Width = w;
            Height = h;

            // Create framebuffer
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Normal texture
            NormalTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, NormalTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, Width, Height, 0, PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, NormalTexture, 0);

            // Depth texture
            DepthTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, DepthTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24, Width, Height, 0,
                PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
                TextureTarget.Texture2D, DepthTexture, 0);

            // Set draw buffers
            DrawBuffersEnum[] attachments = { DrawBuffersEnum.ColorAttachment0 };
            GL.DrawBuffers(attachments.Length, attachments);

            // Check completeness
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine($"GBuffer not complete: {status}");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void BindForWriting()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

            // Important: set viewport to match FBO size
            GL.Viewport(0, 0, Width, Height);

            // Make sure OpenGL writes to the right color attachments
            DrawBuffersEnum[] attachments = { DrawBuffersEnum.ColorAttachment0 };
            GL.DrawBuffers(attachments.Length, attachments);

            // Clear color + depth
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest); // needed for proper depth writes
        }

        public void BindForReading()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, NormalTexture);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, DepthTexture);
        }
    }
}
