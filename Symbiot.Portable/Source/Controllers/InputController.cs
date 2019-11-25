using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbiot.Portable.Source.Controllers
{
    public class InputController
    {
        public static InputController Instance;

        public delegate void Delegate_OnClick(Point point);

        public Delegate_OnClick OnClick;

        public InputController()
        {
            Instance = this;
        }

        private ButtonState lastMouseState = ButtonState.Released;

        public void OnUpdade()
        {
            if (GameRoot.CurrentPlatform == GameRoot.Platform.PC)
            {
                if (Mouse.GetState().LeftButton != lastMouseState)
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        OnClick(Mouse.GetState().Position);

                    lastMouseState = Mouse.GetState().LeftButton;
                }
            }
            else
            {
                TouchCollection touchCollection = TouchPanel.GetState();

                foreach (TouchLocation touch in touchCollection)
                    if (touch.State == TouchLocationState.Pressed)
                        OnClick(touch.Position.ToPoint());
            }
        }
    }
}
