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
    internal class VoidUI : UIState
    {
		public UIPanel voidUI;
		public static bool visible = false;
        public override void OnInitialize()
		{
			
			UIPanel voidUI = new DragableUIPanel();
			voidUI.Height.Set(30f, 0f);
			voidUI.Width.Set(200f, 0f);
			voidUI.Left.Set(500f, 0f);
			voidUI.Top.Set(30f, 0f);
			voidUI.BackgroundColor = new Color(255, 255, 255, 255);
			voidUI.SetPadding(0);
	
			VoidBar voidAmount = new VoidBar(VoidBarMode.voidAmount, 280, 25);
			voidAmount.Height.Set(30f, 0f);
			voidAmount.Width.Set(200f, 0f);
			voidAmount.Left.Set(0f, 0f);
			voidAmount.Top.Set(0f, 0f);
			voidUI.Append(voidAmount);
			
			Append(voidUI);

		}
		public override void Update(GameTime gameTime) 
		{
			base.Update(gameTime);
			
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Recalculate();
		}

    }
}