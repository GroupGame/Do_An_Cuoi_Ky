using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public class My2DSprite : MyModel
    {
        private float _Depth = 0;

        public float Depth
        {
            get { return _Depth; }
            set { _Depth = value; }
        }
        private float _Left;

        public float Left
        {
            get { return _Left; }
            set { _Left = value; }
        }
        private float _Top;

        public float Top
        {
            get { return _Top; }
            set { _Top = value; }
        }
        private int _Width;   

        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        private int _Height;

        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        private List<Texture2D> _Textures;

        public List<Texture2D> Textures
        {
            get { return _Textures; }
            set { 
                _Textures = value;
                _nTextures = _Textures.Count;
                _iTexture = 0;
                _Width = _Textures[0].Width;
                _Height = _Textures[0].Height;
            }
        }
        private int _nTextures;

        public int nTextures
        {
            get { return _nTextures; }
            set { _nTextures = value; }
        }
        private int _iTexture;

        public int iTexture
        {
            get { return _iTexture; }
            set { _iTexture = value; }
        }
       
        public My2DSprite(float left, float top, List<Texture2D> textures)
        {
            Left = left;
            Top = top;
            Textures = textures;
        }

        int d1 = 0;
        int d2 = 1;

        public override void Update(GameTime gameTime)
        {
            _iTexture = (_iTexture + 1) % _nTextures;
            if (State == 1)
            {
                if (Math.Abs(d1) == 10)
                    d2 *= -1;
                d1 += d2;
            }
        }
        //public override void Draw(GameTime gameTime, object obj)
        //{
        //    InternalDraw(gameTime, (SpriteBatch)obj);
        //}
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(State == 0)
                spriteBatch.Draw(_Textures[_iTexture],new Vector2(_Left,_Top),Color.White);
            else
            {
                Rectangle r = new Rectangle((int)_Left - d1, (int)_Top - d1, (int)_Width + 2 * d1, (int)_Height + 2 * d1);
                spriteBatch.Draw(_Textures[_iTexture], r, Color.LightGreen);
            }
        }

        private int _State = 0;

        public int State
        {
            get { return _State; }
            set { _State = value; }
        }
        public override bool IsSelected(object obj)
        {
            Vector2 pos = (Vector2)obj;
            if (pos.X >= _Left && pos.X <= _Left + _Width &&
                pos.Y >= _Top && pos.Y <= _Top + _Height)
                return true;
            return false;
        }

        public override void Select(bool bSelected)
        {
            if (bSelected)
                State = 1;
            else
                State = 0;
        }
    }
}
