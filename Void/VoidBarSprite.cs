using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Void
{
    class VoidBarSprite : UIElement
    {
        public Color backgroundColor = Color.Gray;
        private static Texture2D _backgroundTexture;

        public VoidBarSprite()
        {
            if (_backgroundTexture == null)
                _backgroundTexture = ModContent.GetTexture("SOTS/Void/VoidBarSprite");
			
        }
		protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), backgroundColor);
        }
    }
}