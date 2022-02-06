using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Minions;
using SOTS.Utilities;
using Terraria;
using Terraria.ID;

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
			On.Terraria.NPC.UpdateNPC += NPC_UpdateNPC;
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
			On.Terraria.NPC.UpdateNPC -= NPC_UpdateNPC;
			Main.OnPreDraw -= Main_OnPreDraw;
		}
		private static void NPC_UpdateNPC(On.Terraria.NPC.orig_UpdateNPC orig, NPC self, int i)
		{
			if (self.active)
			{
				NPC realNPC = self;
				if (self.realLife != -1)
					realNPC = Main.npc[self.realLife];
				DebuffNPC debuffNPC = realNPC.GetGlobalNPC<DebuffNPC>();
				if (debuffNPC.timeFrozen > 0)
				{
					if(self.immune[Main.myPlayer] > 0)
						self.immune[Main.myPlayer]--;
					debuffNPC.timeFrozen--;
					if (debuffNPC.timeFrozen == 0 && Main.netMode == NetmodeID.Server)
					{
						debuffNPC.netUpdateTime = true;
					}
					self.whoAmI = i;
					return;
				}
			}
			orig(self, i);
		}

		private static void Main_OnPreDraw(GameTime obj)
		{
			if (Main.spriteBatch != null && !Main.dedServ)
			{
				if (SOTS.primitives != null && Main.spriteBatch != null)
				{
					SOTS.primitives.DrawTrailsProj(Main.spriteBatch, Main.graphics.GraphicsDevice);
					SOTS.primitives.DrawTrailsNPC(Main.spriteBatch, Main.graphics.GraphicsDevice);
				}
			}
		}
		private static void Main_DrawProjectiles(On.Terraria.Main.orig_DrawProjectiles orig, Main self)
		{
			if (SOTS.primitives != null && Main.spriteBatch != null)
			{
				SOTS.primitives.DrawTargetProj(Main.spriteBatch);
			}
			if(self != null && orig != null)
            {
				orig(self); 
				PostDrawProjectiles();
			}
		}

		private static void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
		{
			if (SOTS.primitives != null && Main.spriteBatch != null)
			{
				SOTS.primitives.DrawTargetNPC(Main.spriteBatch);
			}
			if (self != null && orig != null)
				orig(self, behindTiles);
		}

		private static void Main_DrawPlayers(On.Terraria.Main.orig_DrawPlayers orig, Main self)
		{
			PreDrawPlayers();
			orig(self);
			PostDrawPlayers();
		}
		private static void PostDrawProjectiles()
		{
			if (!Main.dedServ)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.modProjectile is HoloPlatform hPlatform)
					{
						hPlatform.Draw(Main.spriteBatch); //change later
					}
				}
				Main.spriteBatch.End();
			}
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
						modProj.Draw(Main.spriteBatch, Color.White);
					}
					if (proj.active && proj.modProjectile is IncineratorGloveProjectile modProj2)
					{
						modProj2.Draw(Main.spriteBatch, Color.White); 
					}
				}
				Main.spriteBatch.End();
			}
		}
	}
}
