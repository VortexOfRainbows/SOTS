using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class PetPutridPinkyCrystal : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinky Pet");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(fireToX);
			writer.Write(fireToY);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			fireToX = reader.ReadSingle();
			fireToY = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
		public override void SetDefaults()
        {
			projectile.netImportant = true;
            projectile.width = 26;
            projectile.height = 46; 
            projectile.timeLeft = 20000;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
			projectile.alpha = 70;
		}
		private int shader = 0;
		public float fireToX = 0;
		public float fireToY = 0;
		public float eyeReset = 2.5f;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.hornet = false; // Relic from aiType
			if (Main.myPlayer != projectile.owner)
				projectile.timeLeft = 20;
			for(int i = 0; i < Main.projectile.Length; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.type == projectile.type && proj.owner == projectile.owner && proj.active && proj.whoAmI != projectile.whoAmI)
                {
					projectile.Kill();
                }
			}
			shader = player.cPet;
			return true;
        }
		bool runOnce = true;
		int[] hooks = new int[6];
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			}
			return base.PreDraw(spriteBatch, lightColor);
        }
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if((modPlayer.petPinky + 1) != projectile.damage)
            {
				projectile.Kill();
            }
			if (runOnce)
            {
				runOnce = false;
				for(int i = 0; i < hooks.Length; i++)
                {
					if(Main.myPlayer == projectile.owner)
						hooks[i] = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<FluxSlimeBall>(), projectile.damage, projectile.knockBack, Main.myPlayer, projectile.identity, i * 60);
                }
            }
			bool hasHooked = false;
			if(Main.myPlayer == projectile.owner && projectile.damage > 0)
            {
				#region Find target
				float distanceFromTarget = 400f;
				int decidedHook = -1;
				int targetID = -1;
				List<int> excluded = new List<int>();
				for (int i = 0; i < hooks.Length; i++)
				{
					Projectile hook = Main.projectile[hooks[i]];
					FluxSlimeBall slimeBall = hook.modProjectile as FluxSlimeBall;
					if (hook.ai[0] == (int)projectile.whoAmI && hook.active && hook.type == ModContent.ProjectileType<FluxSlimeBall>())
					{
						if (slimeBall != null)
						{
							excluded.Add(slimeBall.targetID);
							if (slimeBall.targetID != -1) 
								hasHooked = true;
						}
					}
				}
				for (int i = 0; i < hooks.Length; i++)
				{
					Projectile hook = Main.projectile[hooks[i]];
					FluxSlimeBall slimeBall = hook.modProjectile as FluxSlimeBall;
					if ((int)hook.ai[0] == projectile.whoAmI && hook.active && hook.type == ModContent.ProjectileType<FluxSlimeBall>() && (hook.Center - projectile.Center).Length() < 48)
					{
						if (slimeBall != null)
						{
							if (slimeBall.targetID == -1)
							{
								for (int j = 0; j < Main.maxNPCs; j++)
								{
									NPC npc = Main.npc[j];
									if (npc.CanBeChasedBy() && !excluded.Contains(npc.whoAmI))
									{
										float between = Vector2.Distance(npc.Center, hook.Center);
										bool inRange = between < distanceFromTarget;
										bool lineOfSight = Collision.CanHitLine(hook.position, hook.width, hook.height, npc.position, npc.width, npc.height);

										bool closeThroughWall = between < 100f; //should attack semi-reliably through walls
										if (inRange && (lineOfSight || closeThroughWall) && between < distanceFromTarget)
										{
											distanceFromTarget = between;
											targetID = j;
											decidedHook = i;
										}
									}
								}
							}
						}
								
					}
					if(!hook.active || hook.type != ModContent.ProjectileType<FluxSlimeBall>())
					{
						hooks[i] = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<FluxSlimeBall>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.whoAmI, i * 60);
					}
				}
				if(decidedHook != -1)
				{
					Projectile hook = Main.projectile[hooks[decidedHook]];
					FluxSlimeBall slimeBall = hook.modProjectile as FluxSlimeBall;
					if(slimeBall != null)
                    {
						if(slimeBall.targetID == -1)
                        {
							slimeBall.targetID = targetID;
							hook.netUpdate = true;
						}
                    }
				}
				#endregion
			}
			if(projectile.owner == Main.myPlayer)
			{
				Vector2 toLocation = projectile.Center;
				projectile.velocity *= 0.1f;
				projectile.ai[0] += 1 * player.direction;
				toLocation.X = player.Center.X;
				toLocation.Y = player.Center.Y - 120 + Main.player[projectile.owner].gfxOffY;
				Vector2 circular = new Vector2(48, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
				circular.Y *= 0.3f;
				Vector2 goTo = toLocation + circular;
				Vector2 hookPositionAverage = Vector2.Zero;
				for (int i = 0; i < hooks.Length; i++)
				{
					Projectile hook = Main.projectile[hooks[i]];
					if ((int)hook.ai[0] == projectile.whoAmI && hook.active && hook.type == ModContent.ProjectileType<FluxSlimeBall>())
					{
						hookPositionAverage += hook.Center;
					}
				}
				fireToX = Main.MouseWorld.X;
				fireToY = Main.MouseWorld.Y;
				if(hasHooked)
				{
					fireToX = hookPositionAverage.X / 6f;
					fireToY = hookPositionAverage.Y / 6f;
				}
				goTo += hookPositionAverage;
				goTo /= 7f;
				goTo -= projectile.Center;
				Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
				float dist = 9f + goTo.Length() * 0.02f;
				if (dist > goTo.Length())
					dist = goTo.Length();
				projectile.velocity = newGoTo * dist;
				projectile.netUpdate = true;
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Minions/PetPutridPinkyEye");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			float shootToX = fireToX - projectile.Center.X;
			float shootToY = fireToY - projectile.Center.Y;
			Vector2 shootTo = new Vector2(shootToX, shootToY).SafeNormalize(Vector2.Zero);
			drawPos += shootTo * 2;
			spriteBatch.Draw(texture, drawPos, null, drawColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
		}
	}
}
