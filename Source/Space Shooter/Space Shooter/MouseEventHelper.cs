using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public class MouseEventHelper : InvisibleGameEntity
    {
        protected MouseState CurrentState;
        protected MouseState PreviousState;
        public MouseEventHelper()
        {

        }
        public static MouseEventHelper Instance = new MouseEventHelper();
        public static MouseEventHelper GetInstance()
        {
            return Instance;
        }
        public override void Update(GameTime gameTime)
        {
 	         base.Update(gameTime);
            ProcessNewState(Mouse.GetState());
        }
        public void ProcessNewState(MouseState mouseState)
        {
            PreviousState = CurrentState;
            CurrentState = mouseState;
        }
        public bool HasLeftButtonDownEvent()
        {
            try
            {
                if (PreviousState.LeftButton == ButtonState.Released &&
                CurrentState.LeftButton == ButtonState.Pressed)
                    return true;
            }
            catch (Exception)
            {

            }
            return false;
        }

        public bool HasLeftButtonUpEvent()
        {
            try
            {
                if (PreviousState.LeftButton == ButtonState.Pressed &&
                CurrentState.LeftButton == ButtonState.Released)
                    return true;
            }
            catch (Exception)
            {

            }
            return false;

        }

        public bool IsLeftButtonDown()
        {
            return CurrentState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonUp()
        {
            return CurrentState.LeftButton == ButtonState.Released;
        }
        internal Vector2 GetMousePosDifference()
        {
            try
            {
                return new Vector2(CurrentState.X - PreviousState.X,
                    CurrentState.Y - PreviousState.Y);
            }
            catch
            {

            }
            return Vector2.Zero;
        }

        internal Vector2 GetMousePosition()
        {

            return new Vector2(CurrentState.X, CurrentState.Y);
        }
        public bool HasRightButtonUpEvent()
        {
            try
            {
                if (PreviousState.RightButton == ButtonState.Pressed &&
                    CurrentState.RightButton == ButtonState.Released)
                    return true;

            }
            catch (Exception)
            {

            }

            return false;

        }
    }
}
