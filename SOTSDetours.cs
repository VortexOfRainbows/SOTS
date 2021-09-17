using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using SOTS.Prim;
using SOTS.Utilities;
using System;
using System.Linq;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using System.Collections.Generic;
using Terraria.Localization;

namespace SOTS
{
	public static class SOTSDetours
	{
		public static void Initialize()
		{
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers += Main_DrawPlayers;
			Main.OnPreDraw += Main_OnPreDraw;
		}

		public static void Unload()
		{
			On.Terraria.Main.DrawProjectiles -= Main_DrawProjectiles;
			On.Terraria.Main.DrawNPCs -= Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers -= Main_DrawPlayers;
			Main.OnPreDraw -= Main_OnPreDraw;
		}

		private static void Main_OnPreDraw(GameTime obj)
		{
			if (Main.spriteBatch != null && SOTS.primitives != null) 
			{
				SOTS.primitives.DrawTrailsProj(Main.spriteBatch, Main.graphics.GraphicsDevice);
				SOTS.primitives.DrawTrailsNPC(Main.spriteBatch, Main.graphics.GraphicsDevice);
			}
		}
		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			if (!Main.dedServ)
			{
				SOTS.primitives.DrawTargetProj(Main.spriteBatch);
			}
			orig(self);
		}

		private static void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (!Main.dedServ) 
			{
				SOTS.primitives.DrawTargetNPC(Main.spriteBatch);
			}
			orig(self, behindTiles);
		}

		private static void Main_DrawPlayers(On.Terraria.Main.orig_DrawPlayers orig, Main self)
		{
			orig(self);
			PostDrawPlayers();
		}

		private static void PostDrawPlayers()
        {
			Main.spriteBatch.Begin();
			for (int i = 0; i < Main.projectile.Length; i++)
            {
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.modProjectile is IOrbitingProj modProj && modProj.inFront)
                {
					modProj.Draw(Main.spriteBatch, Color.White); //change later
                }
            }
			Main.spriteBatch.End();
        }
	}
}
