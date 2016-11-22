﻿using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

using SPICA.Formats.CtrH3D;
using SPICA.Formats.Generic.COLLADA;
using SPICA.Formats.GFL2;
using SPICA.Formats.GFL2.Motion;
using SPICA.Renderer;

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SPICA.WinForms
{
    public partial class FrmMain : GameWindow
    {
        RenderEngine Renderer;

        Vector2 InitialRot;
        Vector2 InitialMov;
        Vector2 FinalMov;

        private float Zoom;
        private float Step;

        private Model Model;

        public FrmMain() : base(800, 600, new GraphicsMode(new ColorFormat(32), 24, 8))
        {
            Title = "SPICA";
        }

        protected override void OnResize(EventArgs e)
        {
            Renderer.UpdateResolution(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.On;

            Renderer = new RenderEngine(Width, Height);

            using (OpenFileDialog OpenDlg = new OpenFileDialog())
            {
                OpenDlg.Filter = "Binary Citra H3D|*.bch|Pokémon SM Character|*.cm;*.bin";

                if (OpenDlg.ShowDialog() == DialogResult.OK)
                {
                    switch (OpenDlg.FilterIndex)
                    {
                        case 1:
                            Model = Renderer.AddModel(H3D.Open(OpenDlg.FileName));
                            break;

                        case 2:
                            using (FileStream FS = new FileStream(OpenDlg.FileName, FileMode.Open))
                            {
                                BinaryReader Reader = new BinaryReader(FS);

                                FS.Seek(4, SeekOrigin.Begin);
                                FS.Seek(Reader.ReadUInt32(), SeekOrigin.Begin);

                                GFModelPack BaseMdl = new GFModelPack(FS);
                                Model = Renderer.AddModel(BaseMdl.ToH3D());

                                FS.Seek(8, SeekOrigin.Begin);
                                FS.Seek(Reader.ReadUInt32(), SeekOrigin.Begin);

                                GFMotion Mot = new GFMotionPack(FS)[1];

                                Model.SkeletalAnimation.SetAnimation(Mot.ToH3DSkeletalAnimation(BaseMdl.Models[0].Skeleton));
                                Model.SkeletalAnimation.Step = 0.3f;
                                Model.SkeletalAnimation.Play();

                                Model.MaterialAnimation.SetAnimation(Mot.ToH3DMaterialAnimation());
                                Model.MaterialAnimation.Step = 0.3f;
                                Model.MaterialAnimation.Play();
                            }
                            break;
                    }
                }
            }

            Tuple<Vector3, float> CenterMax = Model.GetCenterMaxXY();

            Vector3 Center = -CenterMax.Item1;
            float Maximum = CenterMax.Item2;

            Model.TranslateAbs(Center);
            Zoom = Center.Z - Maximum * 2;
            Step = Maximum * 0.05f;

            Renderer.AddLight(new Light
            {
                Position = new Vector3(0, -Center.Y, -Zoom),
                Ambient = new Color4(0f, 0f, 0f, 0f),
                Diffuse = new Color4(1f, 1f, 1f, 1f),
                Specular = new Color4(1f, 1f, 1f, 1f)
            });

            Renderer.SetBackgroundColor(Color.Gray);

            UpdateTranslation();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Model.Animate();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Renderer.RenderScene();
            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left: InitialRot = new Vector2(e.X, e.Y); break;
                case MouseButton.Right: InitialMov = new Vector2(e.X, e.Y); break;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                float X = (InitialMov.X - e.X) + FinalMov.X;
                float Y = (InitialMov.Y - e.Y) + FinalMov.Y;

                FinalMov = new Vector2(X, Y);
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (e.Mouse.LeftButton == OpenTK.Input.ButtonState.Pressed)
            {
                float RY = (float)(((e.X - InitialRot.X) / Width) * Math.PI);
                float RX = (float)(((e.Y - InitialRot.Y) / Height) * Math.PI);

                Model.Rotate(new Vector3(RX, RY, 0));
            }

            if (e.Mouse.RightButton == OpenTK.Input.ButtonState.Pressed)
            {
                float TX = (InitialMov.X - e.X) + FinalMov.X;
                float TY = (InitialMov.Y - e.Y) + FinalMov.Y;

                Renderer.TranslateAbs(new Vector3(-TX, TY, Zoom));
            }

            InitialRot = new Vector2(e.X, e.Y);

            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Mouse.RightButton == OpenTK.Input.ButtonState.Released)
            {
                if (e.Delta > 0)
                    Zoom += Step;
                else
                    Zoom -= Step;

                UpdateTranslation();
            }

            base.OnMouseWheel(e);
        }

        private void UpdateTranslation()
        {
            Renderer.TranslateAbs(new Vector3(-FinalMov.X, FinalMov.Y, Zoom));
        }
    }
}
