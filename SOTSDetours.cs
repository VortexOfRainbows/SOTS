using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.ArtificialDebuffs;
using SOTS.Projectiles.Chaos;
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

			//order of updates: player, NPC, gore, projectile, item, dust, time
			On.Terraria.Player.Update += Player_Update;
			On.Terraria.NPC.UpdateNPC += NPC_UpdateNPC;
			On.Terraria.Gore.Update += Gore_Update;
            On.Terraria.Projectile.Update += Projectile_Update;
			On.Terraria.Item.UpdateItem += Item_UpdateItem;
			On.Terraria.Dust.UpdateDust += Dust_UpdateDust;
			On.Terraria.Main.UpdateTime += Main_UpdateTime;
			//On.Terraria.NPC.UpdateCollision += NPC_UpdateCollision;
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

			//order of updates: player, NPC, gore, projectile, item, dust, time
			On.Terraria.Player.Update -= Player_Update;
			On.Terraria.NPC.UpdateNPC -= NPC_UpdateNPC;
			On.Terraria.Gore.Update -= Gore_Update;
			On.Terraria.Projectile.Update -= Projectile_Update;
			On.Terraria.Item.UpdateItem -= Item_UpdateItem;
			On.Terraria.Dust.UpdateDust -= Dust_UpdateDust;
			On.Terraria.Main.UpdateTime -= Main_UpdateTime;

			Main.OnPreDraw -= Main_OnPreDraw;
		}
		private static void Player_Update(On.Terraria.Player.orig_Update orig, Player self, int i)
        {
			if(SOTSWorld.IsFrozenThisFrame && self.active)
            {
				SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(self);
				sPlayer.oldHeldProj = self.heldProj;
				if (!sPlayer.TimeFreezeImmune)
                {
					return;
                }
            }
			orig(self, i);
		}
		/*private static void NPC_UpdateCollision(On.Terraria.NPC.orig_UpdateCollision orig, NPC self)
		{
			if (self.active)
			{
				if (SOTSWorld.IsFrozenThisFrame)
				{
					return;
				}
			}
			orig(self);
		}*/
		private static void NPC_UpdateNPC(On.Terraria.NPC.orig_UpdateNPC orig, NPC self, int i)
		{
			if (self.active)
			{
				if (SOTSWorld.IsFrozenThisFrame)
				{
					/*for (int index = 0; index < 256; ++index)
					{
						if (self.immune[index] > 0)
							--self.immune[index];
					}*/
					return;
				}
				else
				{
					bool freeze = DebuffNPC.UpdateWhileFrozen(self, i);
					if (freeze)
					{
						return;
					}
				}
			}
			orig(self, i);
		}
		private static void Gore_Update(On.Terraria.Gore.orig_Update orig, Gore self)
		{
			if (SOTSWorld.IsFrozenThisFrame && self.active)
			{
				return;
			}
			orig(self);
		}
		private static void Projectile_Update(On.Terraria.Projectile.orig_Update orig, Projectile self, int i)
		{
			if(self.active)
			{
				if (SOTSWorld.IsFrozenThisFrame)
				{
					bool dragIntoFreeze = SOTSProjectile.GlobalFreezeSlowdown(self);
					if (dragIntoFreeze && SOTSProjectile.CanBeTimeFrozen(self))
					{
						self.Damage(); //turn on hitboxes
						return;
					}
				}
				else
				{
					bool freeze = SOTSProjectile.UpdateWhileFrozen(self, i);
					if (freeze)
						return;
				}
			}
			orig(self, i);
		}
		private static void Item_UpdateItem(On.Terraria.Item.orig_UpdateItem orig, Item self, int i)
		{
			if (SOTSWorld.IsFrozenThisFrame && self.active)
			{
				return;
			}
			orig(self, i);
		}
		private static void Dust_UpdateDust(On.Terraria.Dust.orig_UpdateDust orig)
		{
			if (SOTSWorld.IsFrozenThisFrame)
			{
				return;
			}
			orig();
		}
		private static void Main_UpdateTime(On.Terraria.Main.orig_UpdateTime orig)
		{
			if (SOTSWorld.IsFrozenThisFrame)
			{
				return;
			}
			orig();
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
				PreDrawNPCs();
				SOTS.primitives.DrawTargetNPC(Main.spriteBatch);
			}
			if (self != null && orig != null)
            {
				orig(self, behindTiles);
			}
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
					if (proj.active && proj.modProjectile is ChaosDiamondLaser dLaser)
					{
						dLaser.DrawBlack(Main.spriteBatch); //change later
					}
				}
				Main.spriteBatch.End();
			}
		}
		private static void PreDrawNPCs()
		{
			if (!Main.dedServ)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active)
					{
						DebuffNPC instancedNPC = npc.GetGlobalNPC<DebuffNPC>();
						if (instancedNPC.timeFrozen != 0)
							instancedNPC.DrawTimeFreeze(npc, Main.spriteBatch, Color.White);
					}
				}
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
