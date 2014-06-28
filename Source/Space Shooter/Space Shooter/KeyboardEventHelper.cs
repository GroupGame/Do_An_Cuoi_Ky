using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Shooter
{
    public class KeyboardEventHelper : InvisibleGameEntity
    {
        protected KeyboardState CurrentState;
        protected KeyboardState PreviousState;
        public KeyboardEventHelper()
        {

        }
        public static KeyboardEventHelper Instance = new KeyboardEventHelper();
        public static KeyboardEventHelper GetInstance()
        {
            return Instance;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ProcessNewState(Keyboard.GetState());
        }

        public void ProcessNewState(KeyboardState keyboardState)
        {
            PreviousState = CurrentState;
            CurrentState = keyboardState;
        }

        public bool HasKeyUpEvent(Keys keys)
        {
            try
            {
                if (PreviousState.IsKeyDown(keys) &&
                    CurrentState.IsKeyUp(keys))
                    return true;
            }
            catch (Exception)
            {
                
            }
            return false;
        }
    }
}
