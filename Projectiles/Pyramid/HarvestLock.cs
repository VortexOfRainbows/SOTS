using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.NPCs.ArtificialDebuffs;

namespace SOTS.Projectiles.Pyramid
{    
    public class HarvestLock : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harvest Lock");
			
		}
        public override void SetDefaults()
        {
			projectile.height = 2;
			projectile.width = 2;
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.timeLeft = 80;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
		}
		bool runOnce = true;
		float width = -1;
		float ogWidth = -1;
		Vector2[] trailPos = new Vector2[10];
		Vector2[] trailPos2 = new Vector2[10];
		Vector2[] trailPos3 = new Vector2[10];
		Vector2[] trailPos4 = new Vector2[10];
		public void cataloguePos(Vector2 catalogue, Vector2[] trialArray)
		{
			Vector2 current = catalogue;
			for (int i = 0; i < trialArray.Length; i++)
			{
				Vector2 previousPosition = trialArray[i];
				trialArray[i] = current;
				current = previousPosition;
			}
		}
		bool runOnce2 = true;
		public override void AI()
		{
			if(projectile.timeLeft >= 32)
				for (int j = 0; j < Main.projectile.Length; j++)
				{
					Projectile proj = Main.projectile[j];
					if (proj.active && proj.owner == projectile.owner && proj.type == projectile.type && proj.whoAmI != projectile.whoAmI)
					{
						if (proj.timeLeft <= projectile.timeLeft && proj.timeLeft >= 32)
						{
							projectile.Kill();
							return;
						}
					}
				}
			if ((int)projectile.ai[0] != -1)
			{
				NPC npc = Main.npc[(int)projectile.ai[0]];
				if (npc.lifeMax > 5 && npc.active)
				{
					if(runOnce2 && projectile.timeLeft < 32)
					{
						DebuffNPC debuffNPC = (DebuffNPC)mod.GetGlobalNPC("DebuffNPC");
						debuffNPC = (DebuffNPC)debuffNPC.Instance(npc);
						if (debuffNPC.HarvestCurse < 99 && Main.myPlayer == projectile.owner)
						{
							debuffNPC.OnHitByProjectile(npc, projectile, 0, 0, false);
						}
						runOnce2 = false;
						for (int i = 0; i < 360; i += 4)
						{
							Vector2 rotationalPos = new Vector2(7, 0).RotatedBy(MathHelper.ToRadians(i));
							int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(20, 20) - new Vector2(5), 40, 40, mod.DustType("CopyDust4"));
							Dust dust = Main.dust[num1];
							dust.noGravity = true;
							dust.velocity *= 0.1f;
							dust.position += rotationalPos;
							dust.velocity += rotationalPos * 0.33f;
							dust.scale *= 2.2f;
							dust.fadeIn = 0.1f;
							dust.alpha = 0;
							dust.color = VoidPlayer.soulLootingColor;
						}
					}
					if(projectile.timeLeft > 40)
						for (int j = 0; j < Main.projectile.Length; j++)
						{
							Projectile proj = Main.projectile[j];
							if (proj.active && proj.owner == projectile.owner && proj.type == mod.ProjectileType("GhostPepper"))
							{
								GhostPepper pepper = (GhostPepper)proj.modProjectile;
								if (pepper.npcTargetId != npc.whoAmI)
									Main.PlaySound(2, (int)proj.Center.X, (int)proj.Center.Y, 8, 1.4f);
								pepper.npcTargetId = npc.whoAmI;
								pepper.cooldown = 40;
								proj.netUpdate = true;
							}
						}
					projectile.position.X = npc.Center.X - 1;
					projectile.position.Y = npc.Center.Y - 1;
				}
				else
				{
					projectile.Kill();
				}
				if (width == -1)
				{
					width = (float)Math.Sqrt((double)npc.width * npc.height);
					ogWidth = width *= 0.75f;
				}
			}
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
            }
			Player player = Main.player[projectile.owner];
			Vector2 circularLocation = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation));
			cataloguePos(circularLocation + projectile.Center, trailPos);
			circularLocation = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation + 90));
			cataloguePos(circularLocation + projectile.Center, trailPos2);
			circularLocation = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation + 180));
			cataloguePos(circularLocation + projectile.Center, trailPos3);
			circularLocation = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation + 270));
			cataloguePos(circularLocation + projectile.Center, trailPos4);
			rotation += 7;
			if(projectile.timeLeft >= 30)
			{
				distance -= 30f / 55f;
				width -= ogWidth / 55f;
			}
			else
            {
				distance = 0;
				width = 0;
            }
		}
        public override void Kill(int timeLeft)
		{
			base.Kill(timeLeft);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 circularLocation = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation)) + projectile.Center;
			if (runOnce)
				return false;
			Vector2 current = circularLocation;
			Draw(spriteBatch, trailPos, current);
			current = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation + 90)) + projectile.Center;
			Draw(spriteBatch, trailPos2, current);
			current = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation + 180)) + projectile.Center;
			Draw(spriteBatch, trailPos3, current);
			current = new Vector2(distance + width, 0).RotatedBy(MathHelper.ToRadians(rotation + 270)) + projectile.Center;
			Draw(spriteBatch, trailPos4, current);
			return false;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2[] trailArray, Vector2 current)
		{
			Texture2D texture2 = mod.GetTexture("Projectiles/Pyramid/GhostPepperTail");
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 previousPosition = current;
			Color color = new Color(VoidPlayer.soulLootingColor.R, VoidPlayer.soulLootingColor.G, VoidPlayer.soulLootingColor.B, 0);
			color *= (projectile.timeLeft - 30) / 90f;
			for (int k = 0; k < trailArray.Length; k++)
			{
				if (k >= projectile.timeLeft / 3 - 10)
					return;
				float scale = projectile.scale * (trailArray.Length - k) / (float)trailArray.Length;
				scale *= 1f;
				if (trailArray[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailArray[k] - Main.screenPosition;
				Vector2 currentPos = trailArray[k];
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
		}
	}
}
		