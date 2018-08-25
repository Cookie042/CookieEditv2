using System;
using System.Diagnostics;
using Flaxen.SlimDXControlLib;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D10;
using SlimDX.Direct3D9;
using Buffer = SlimDX.Direct3D10.Buffer;
using Effect = SlimDX.Direct3D10.Effect;
using Format = SlimDX.DXGI.Format;
using ShaderFlags = SlimDX.D3DCompiler.ShaderFlags;
using Viewport = SlimDX.Direct3D10.Viewport;

namespace CookieEdit2
{
    /// <summary>
    ///     This is an example rendering engine which demonstrates a basic rendering operation,
    ///     with time-based animation.  The Initialize() method is provided to implement the effect,
    ///     and Reinitialize() is not needed since nothing need change when the window size changes.
    /// </summary>
    public class RenderEngine : SimpleRenderEngine
    {
        private Matrix _machineOrientation;

        private Matrix _machineOrigin;

        private Vector3 _machPos;
        private Vector3 _machRot;

        private Matrix _proj;
        private Matrix _view;
        private Matrix _world;
        private long lastTime;
        private bool m_invert;
        private readonly bool m_perspective = true;
        private Effect m_sampleEffect;
        private InputLayout m_sampleLayout;
        private DataStream m_sampleStream;
        private Buffer m_sampleVertices;
        private Vector3 m_worldUp;

        private const double pi2 = Math.PI * 2;

        private readonly float orbitDia = 10;

        private SlimDXControl parentControl;

        private float t;

        private readonly Stopwatch theTime;

        /// <summary>
        ///     Initializes a new instance of the TriangleRenderEngine class.  It supports two styles of display, based on the
        ///     invert parameter.
        /// </summary>
        /// <param name="invert">changes the style of the triangle displayed</param>
        public RenderEngine(bool invert, SlimDXControl control)
        {
            m_invert = invert;

            parentControl = control;

            CameraPosition = new Vector3(4f, 4f, -10);
            TargetPosition = new Vector3(0, 0, 0);

            theTime = new Stopwatch();
            theTime.Start();
        }


        /// <summary>
        ///     Gets or sets the camera's postion.  The values are in world coordinates.
        /// </summary>
        public Vector3 CameraPosition { get; set; }

        /// <summary>
        ///     Gets or sets the camera target's postion.  The values are in world coordinates.
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        ///     Implements the rendering pipeline.
        /// </summary>
        public override void Render()
        {
            Device.OutputMerger.SetTargets(SampleDepthView, SampleRenderView);
            Device.Rasterizer.SetViewports(new Viewport(0, 0, WindowWidth, WindowHeight, 0.0f, 1.0f));

            Device.ClearDepthStencilView(SampleDepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil,
                1.0f, 0);

            var deltaTime = (theTime.ElapsedMilliseconds - lastTime) / 1000.0f;

            // set c to be in the range [0..1] based on current tick

            //input

            t += deltaTime / 10f;

            if (t > 1f)
                t = 0f;

            CameraPosition = new Vector3(orbitDia * (float) Math.Cos(t * Math.PI + Math.PI), 2,
                orbitDia * (float) Math.Sin(t * Math.PI + Math.PI));

            Device.ClearRenderTargetView(SampleRenderView, new Color4(1.0f, .4f, .2f, .2f));

            Device.InputAssembler.SetInputLayout(m_sampleLayout);
            Device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
            Device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(m_sampleVertices, 32, 0));

            var technique = m_sampleEffect.GetTechniqueByIndex(0);

            var pass = technique.GetPassByIndex(0);

            SetTransforms();

            for (var i = 0; i < technique.Description.PassCount; ++i)
            {
                pass.Apply();
                Device.Draw(3, 0);
            }

            lastTime = theTime.ElapsedMilliseconds;

            Device.Flush();
        }

        /// <summary>
        ///     Implements the creation of the effect and the sets up the vertices.
        /// </summary>
        /// <param name="control">the associated SlimDXControl object</param>
        public override void Initialize(SlimDXControl control)
        {
            base.Initialize(control);

            m_sampleEffect = Effect.FromFile(Device, "resources\\FX\\MiniTri.fx", "fx_4_0", ShaderFlags.None,
                EffectFlags.None, null, null, null);
            var technique = m_sampleEffect.GetTechniqueByIndex(0);
            var pass = technique.GetPassByIndex(0);
            InputElement[] inputElements =
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
            };
            m_sampleLayout = new InputLayout(Device, pass.Description.Signature, inputElements);

            m_sampleStream = new DataStream(3 * 32, true, true);
            m_sampleStream.WriteRange(
                new[]
                {
                    new Vector4(0.0f, 1f, 0f, 1f), new Vector4(1f, .2f, .2f, 1.0f),
                    new Vector4(1f, -1f, 0f, 1f), new Vector4(.2f, 1f, .2f, 1.0f),
                    new Vector4(-1f, -1f, 0f, 1f), new Vector4(.2f, .2f, 1f, 1.0f)
                });

            m_sampleStream.Position = 0;

            var bufferDesc = new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 3 * 32,
                Usage = ResourceUsage.Default
            };
            m_sampleVertices = new Buffer(Device, m_sampleStream, bufferDesc);
            Device.Flush();

            lastTime = theTime.ElapsedMilliseconds;
        }

        /// <summary>
        ///     Disposes of our managed resources.
        /// </summary>
        protected override void DisposeManaged()
        {
            if (m_sampleVertices != null)
            {
                m_sampleVertices.Dispose();
                m_sampleVertices = null;
            }

            if (m_sampleLayout != null)
            {
                m_sampleLayout.Dispose();
                m_sampleLayout = null;
            }

            if (m_sampleEffect != null)
            {
                m_sampleEffect.Dispose();
                m_sampleEffect = null;
            }

            if (m_sampleStream != null)
            {
                m_sampleStream.Dispose();
                m_sampleStream = null;
            }

            if (SharedTexture != null)
            {
                SharedTexture.Dispose();
                SharedTexture = null;
            }

            base.DisposeManaged();
        }

        private void SetTransforms()
        {
            // world transfrom: from local coordinates to world coordinates
            // in our case, from the range [0..100] to [0..1]
            _world = Matrix.Scaling(1, 1, 1);

            // view transform: from world coordinates to view (camera, eye) coordinates
            // the "up" direction is the Y axis
            m_worldUp = new Vector3(0, 1, 0);
            _view = Matrix.LookAtLH(CameraPosition, TargetPosition, m_worldUp);

            var rot = Quaternion.RotationYawPitchRoll(0, 0, 0);

            _machineOrientation = Matrix.Transformation(new Vector3(1), Quaternion.Identity, new Vector3(1),
                Vector3.Zero, rot, _machPos);

            // projection transform: from view coordinates to perspective space
            var znear = 0.0001f; // in view space
            var zfar = 10000.0f; // in view space
            if (m_perspective)
            {
                var fovY = (float) (Math.PI * 0.25); // radians, 45 deg
                var aspect = (float) WindowWidth / WindowHeight;
                _proj = Matrix.PerspectiveFovLH(fovY, aspect, znear, zfar);
            }
            else
            {
                var width = 2.0f;
                var height = 2.0f;
                _proj = Matrix.OrthoLH(width, height, znear, zfar);
            }

            // compute the summary transform, and push it into the gpu
            var wvp = _world * _view * _proj;
            var wvpTransform = m_sampleEffect.GetVariableByName("gWVP").AsMatrix();
            wvpTransform.SetMatrix(wvp);
        }
    }
}