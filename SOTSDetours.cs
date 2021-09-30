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
		public static RenderTarget2D TargetProj;
		public static void Initialize()
		{
			On.Terraria.Main.DrawProjectiles += Main_DrawProjectiles;
			On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
			On.Terraria.Main.DrawPlayers += Main_DrawPlayers;
			Main.OnPreDraw += Main_OnPreDraw;
			if (!Main.dedServ)
				ResizeTargets();
		}

		public static void ResizeTargets()
		{
			TargetProj = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
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
			if (Main.spriteBatch != null && !Main.dedServ)
			{
				if (SOTS.primitives != null)
				{
					SOTS.primitives.DrawTrailsProj(Main.spriteBatch, Main.graphics.GraphicsDevice);
					SOTS.primitives.DrawTrailsNPC(Main.spriteBatch, Main.graphics.GraphicsDevice);
				}
				DrawProjToTarget();
			}
		}
		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			if (!Main.dedServ)
			{
				SOTS.primitives.DrawTargetProj(Main.spriteBatch);
			}
			orig(self);
			if (!Main.dedServ)
				DrawProjTarget();
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
			PreDrawPlayers();
			orig(self);
			PostDrawPlayers();
		}
		private static void PreDrawPlayers()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				for (int i = 0; i < Main.player.Length; i++)
				{
					Player player = Main.player[i];
					if (player.active)
					{
						CurseHelper.DrawPlayerFoam(Main.spriteBatch, player);
					}
				}
				Main.spriteBatch.End();
			}
		}
		private static void PostDrawPlayers()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
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
		public static void DrawProjToTarget()
		{
			GraphicsDevice gD = Main.instance.GraphicsDevice;
			SpriteBatch spriteBatch = Main.spriteBatch;
			if (TargetProj == null || gD == null || Main.dedServ || spriteBatch == null)
				return;

			RenderTargetBinding[] bindings = gD.GetRenderTargets();
			gD.SetRenderTarget(TargetProj);
			gD.Clear(Color.Transparent);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			for (int i = 0; i < Main.projectile.Length; i++)
            {
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.modProjectile is IPixellated modProj)
                {
					modProj.Draw(spriteBatch, proj.GetAlpha(Color.White));
                }
            }
			spriteBatch.End();
			gD.SetRenderTargets(bindings);
		}
		public static void DrawProjTarget()
        {
			GraphicsDevice gD = Main.instance.GraphicsDevice;
			SpriteBatch spriteBatch = Main.spriteBatch;
			if (TargetProj == null || gD == null || Main.dedServ || spriteBatch == null)
				return;

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
			spriteBatch.Draw(TargetProj, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
			spriteBatch.End();
		}
	}
}
