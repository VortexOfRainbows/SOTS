using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SOTS.Projectiles.Pyramid
{
	public class GhostPepper : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Pepper");
			Main.projPet[projectile.type] = true;
			//Main.vanityPet[projectile.type] = true;
		}
		Vector2[] trailPos = new Vector2[13];
		bool runOnce = true;
        public override bool PreAI()
		{
			if (Main.myPlayer != projectile.owner)
				projectile.timeLeft = 20;
			if (runOnce)
			{
				projectile.ai[1] = 80f;
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
			Vector2 curve = new Vector2(110, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
			Vector2 toRotate = new Vector2(0, 2.5f).RotatedBy(MathHelper.ToRadians(curve.X));
			Vector2 current = projectile.Center + new Vector2(0, 17.5f).RotatedBy(projectile.rotation) + projectile.velocity;
			Vector2 velo = projectile.velocity * 0.2f;
			velo.Y += 0.55f;
			velo += toRotate;
			velo = velo.RotatedBy(projectile.rotation);
			velo.Y += 0.55f;
			for (int i = 0; i < trailPos.Length; i++)
			{
				if(trailPos[i] != Vector2.Zero)
					trailPos[i] += velo * (trailPos.Length - i)/(float)trailPos.Length;
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 36;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.penetrate = -1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture2 = mod.GetTexture("Projectiles/Pyramid/GhostPepperTail");
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 current = projectile.Center + new Vector2(0, 17.5f).RotatedBy(projectile.rotation);
			Vector2 previousPosition = current;
			Color color = new Color(75, 30, 75, 0);
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1f;
				if (trailPos[k] == Vector2.Zero)
				{
					return true;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color *= 0.95f;
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j <= 1)
						{
							x = 0;
							y = 0;
						}
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin2, scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
			Texture2D texture3 = mod.GetTexture("Projectiles/Pyramid/GhostPepperShell");
			Vector2 drawPos2 = projectile.Center - Main.screenPosition;
			Vector2 drawOrigin1 = new Vector2(texture3.Width * 0.5f, texture3.Height * 0.5f);
			for (int j = 0; j < 8; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.35f * projectile.scale;
				float y = Main.rand.Next(-10, 11) * 0.35f * projectile.scale;
				Main.spriteBatch.Draw(texture3, drawPos2 + new Vector2(x, y), null, color, projectile.rotation, drawOrigin1, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture1 = mod.GetTexture("Projectiles/Pyramid/GhostPepperGlow");
			Texture2D texture4 = mod.GetTexture("Projectiles/Pyramid/GhostPepperGlowmask");
			Vector2 drawOrigin1 = new Vector2(texture1.Width * 0.5f, texture1.Height * 0.5f);
			Color color = new Color(90, 40, 90, 0);
			Vector2 drawPos2 = projectile.Center - Main.screenPosition;
			for (int j = 0; j < 5; j++)
			{
				float x = Main.rand.Next(-10, 11) * 0.125f * projectile.scale;
				float y = Main.rand.Next(-10, 11) * 0.125f * projectile.scale;
				if (j <= 3)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture1, drawPos2 + new Vector2(x, y), null, color, projectile.rotation, drawOrigin1, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			//Main.spriteBatch.Draw(texture3, drawPos2 + new Vector2(0, projectile.gfxOffY), null, lightColor, projectile.rotation, drawOrigin1, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(texture4, drawPos2 + new Vector2(0, projectile.gfxOffY), null, Color.White, projectile.rotation, drawOrigin1, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			base.PostDraw(spriteBatch, lightColor);
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
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			float orbital = modPlayer.orbitalCounter;
			if (mode == 0)
			{
				bool hasHoloEye = modPlayer.HoloEye;
				projectile.velocity *= 0.625f;
				Vector2 toLocation = projectile.Center;
				toLocation.X = player.Center.X;
				toLocation.Y = player.Center.Y - 64 + (hasHoloEye ? -48 : 0) + Main.player[projectile.owner].gfxOffY;
				Vector2 goTo = toLocation - projectile.Center;
				Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
				float dist = 1.5f + goTo.Length() * 0.01f;
				if (dist > goTo.Length())
					dist = goTo.Length();
				projectile.velocity += newGoTo * dist;
			}
			cooldown++;
			int projId = -1;
			float lastLength = 2000f;
			projectile.velocity *= 0.94f;
			if(mode != 2)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.type == mod.ProjectileType("SoulofLooting") && proj.active && (int)proj.ai[0] == projectile.owner)
					{
						Vector2 toNPC = proj.Center - projectile.Center;
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
						Vector2 goTo = toLocation - projectile.Center;
						Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
						float dist = 0.85f;
						if (dist > goTo.Length())
							dist = goTo.Length();
						projectile.velocity += newGoTo * dist;
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
					projectile.Center = npc.Center + new Vector2(0, -npc.height/2 - 48);
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
			projectile.ai[0] += inc;
			Vector2 curve = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
			Vector2 toRotate = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(curve.X));
			projectile.rotation = toRotate.ToRotation() - MathHelper.ToRadians(90);
			projectile.rotation += projectile.velocity.X * 0.02f;
			#region Animation and visuals
			if(mode != 2 || projectile.ai[0] % (inc * 3) == 0)
			cataloguePos();
			Lighting.AddLight(projectile.Center, new Vector3(75, 30, 75) * 1f / 255f);
			#endregion
		}
	}
}