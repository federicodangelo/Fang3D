using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fang3D
{
    public enum Texture2DFormat
    {
        RGB,
        RGBA
    }

    public class Texture2D : Resource
    {
        internal Texture2DFormat format;
        internal byte[] data;
        internal int width;
        internal int height;
        internal Bitmap bitmap;

        public Texture2DFormat Format
        {
            get { return format; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        public Texture2D()
        {
            Name = "Texture";
        }

        protected override void OnFileNameUpdated()
        {
            UnloadTexture();
            LoadNewTexture();

            base.OnFileNameUpdated();
        }

        public override void OnFileChanged()
        {
            UnloadTexture();
            LoadNewTexture();

            base.OnFileChanged();
        }

        public override void OnFileDeleted()
        {
            UnloadTexture();

            base.OnFileDeleted();
        }

        private void UnloadTexture()
        {
            data = null;
            bitmap = null;
            width = height = 0;
            missing = true;
        }

        private void LoadNewTexture()
        {
            Name = Path.GetFileNameWithoutExtension(FileName);
            if (Texture2DImporter.LoadFromFile(this, FileName))
                missing = false;
        }

        public override bool CanAssignTo(object obj)
        {
            if (data != null && obj is Entity)
                return true;

            return false;
        }

        public override object AssignTo(object obj)
        {
            if (obj is Entity)
            {
                Entity ent = (Entity)obj;

                MeshRender render = (MeshRender) ent.FindChildComponent(typeof(MeshRender));

                if (render != null)
                    render.Texture = this;

                return this;
            }

            return null;
        }
    }
}
