using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SOTS.Projectiles.Pyramid.GhostPepper
{
	public class GhostPepper : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Pepper");
			Main.projPet[Projectile.type] = true;
			Main.projFrames[Projectile.type] = 7;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 36;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
		}
		Vector2[] trailPos = new Vector2[13];
		bool runOnce = true;
        public override bool PreAI()
		{
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			if (runOnce)
			{
				Projectile.ai[1] = 80f;
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			return base.PreAI();
		}
		public void cataloguePos()
		{
			Vector2 curve = new Vector2(100, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			Vector2 toRotate = new Vector2(0, 2.5f).RotatedBy(MathHelper.ToRadians(curve.X));
			Vector2 current = Projectile.Center + new Vector2(0, 10f).RotatedBy(Projectile.rotation) + Projectile.velocity;
			Vector2 velo = Projectile.velocity * 0.2f;
			velo.Y += 0.6f;
			velo += toRotate;
			velo = velo.RotatedBy(Projectile.rotation);
			velo.Y += 0.6f;
			for (int i = 0; i < trailPos.Length; i++)
			{
				if(trailPos[i] != Vector2.Zero)
					trailPos[i] += velo * (trailPos.Length - i)/(float)trailPos.Length;
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return true;
			Color color = new Color(70, 60, 70, 0);
			Texture2D texture3 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepper").Value;
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			Vector2 drawOrigin1 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f / 7);
			for (int j = 0; j < 8; j++)
			{
				Vector2 circular = new Vector2(1.75f, 0).RotatedBy(j * MathHelper.PiOver4);
				Main.spriteBatch.Draw(texture3, drawPos2 + circular, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			return true;
		}
        public override void PostDraw(Color lightColor)
		{
			if (runOnce)
				return;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperTail").Value;
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 current = Projectile.Center + new Vector2(0, 10f).RotatedBy(Projectile.rotation);
			Vector2 previousPosition = current;
			Color color = new Color(45, 40, 60, 0);
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1f;
				if (trailPos[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color *= 0.945f;
				float max = betweenPositions.Length() / (5f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.03f * scale * j;
						float y = Main.rand.Next(-10, 11) * 0.03f * scale * j;
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin2, scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
			Texture2D texture1 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperGlow").Value;
			Texture2D tFront = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperFront").Value;
			Texture2D texture4 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/GhostPepper/GhostPepperGlowmask").Value;
			Vector2 drawOrigin1 = new Vector2(texture1.Width * 0.5f, texture1.Height * 0.5f);
			color = new Color(40, 25, 35, 0);
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(tFront, drawPos2 + new Vector2(0, Projectile.gfxOffY), new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), lightColor, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			for (int j = 0; j < 8; j++)
			{
				Vector2 circular = new Vector2(0.5f, 0).RotatedBy(j * MathHelper.PiOver4);
				Main.spriteBatch.Draw(texture1, drawPos2 + circular, null, color, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			//Main.spriteBatch.Draw(texture3, drawPos2 + new Vector2(0, Projectile.gfxOffY), null, lightColor, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(texture4, drawPos2 + new Vector2(0, Projectile.gfxOffY), null, Color.White, Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
        }
        public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return false;
		}
		int mode = 0; //0 = follow, 1 = find souls, 2 = targetting
		public float cooldown = 30;
		public int npcTargetId = -1;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float orbital = modPlayer.orbitalCounter;
			if (mode == 0)
			{
				bool hasHoloEye = modPlayer.HoloEye;
				Projectile.velocity *= 0.625f;
				Vector2 toLocation = Projectile.Center;
				toLocation.X = player.Center.X;
				toLocation.Y = player.Center.Y - 64 + (hasHoloEye ? -48 : 0) + Main.player[Projectile.owner].gfxOffY;
				Vector2 goTo = toLocation - Projectile.Center;
				Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
				float dist = 1.5f + goTo.Length() * 0.01f;
				if (dist > goTo.Length())
					dist = goTo.Length();
				Projectile.velocity += newGoTo * dist;
			}
			cooldown++;
			int projId = -1;
			float lastLength = 2000f;
			Projectile.velocity *= 0.94f;
			if(mode != 2)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.type == ModContent.ProjectileType<SoulofLooting>() && proj.active && (int)proj.ai[0] == Projectile.owner)
					{
						Vector2 toNPC = proj.Center - Projectile.Center;
						if (toNPC.Length() < lastLength)
						{
							lastLength = toNPC.Length();
							projId = proj.whoAmI;
						}
					}
				}
				if (projId != -1)
				{
					mode = 1;
					Projectile proj = Main.projectile[projId];
					if (proj.active)
					{
						cooldown = 0;
						Vector2 toLocation = proj.Center;
						Vector2 goTo = toLocation - Projectile.Center;
						Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
						float dist = 0.85f;
						if (dist > goTo.Length())
							dist = goTo.Length();
						Projectile.velocity += newGoTo * dist;
					}
				}
				else if(cooldown > 20)
				{
					mode = 0;
				}
			}

			if(npcTargetId != -1)
            {
				NPC npc = Main.npc[npcTargetId];
				if (npc.active)
				{
					mode = 2;
					Projectile.Center = npc.Center + new Vector2(0, -npc.height/2 - 48);
					if(cooldown >= 60)
                    {
						mode = 0;
						npcTargetId = -1;
                    }
				}
				else
                {
					npcTargetId = -1;
					mode = 0;
				}
            }

			float inc = 2.75f;
			Projectile.ai[0] += inc;
			Vector2 curve = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			Vector2 toRotate = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(curve.X));
			Projectile.rotation = toRotate.ToRotation() - MathHelper.ToRadians(90);
			Projectile.rotation += Projectile.velocity.X * 0.02f;
			#region Animation and visuals
			if(mode != 2 || Projectile.ai[0] % (inc * 3) == 0)
			cataloguePos();
			Projectile.frameCounter++;
			if(Projectile.frameCounter > 5)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 7;
            }
			Lighting.AddLight(Projectile.Center, new Vector3(75, 30, 75) * 1f / 255f);
			#endregion
		}
	}
}