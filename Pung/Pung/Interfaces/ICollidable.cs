﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pung
{
    public interface ICollidable
    {

        void CheckCollisions();

        void UpCollision(GameObject target);

        void DownCollision(GameObject target);

        void LeftCollision(GameObject target);

        void RightCollision(GameObject target);
    }
}
