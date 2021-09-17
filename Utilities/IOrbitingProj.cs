using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Utilities
{
	public interface IOrbitingProj
    {
        bool inFront { get; set; }

        void Draw(SpriteBatch spriteBatch, Color lightColor);
    }
}





















