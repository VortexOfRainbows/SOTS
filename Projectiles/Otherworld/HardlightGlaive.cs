using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class HardlightGlaive : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Glaive");
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 19;
			projectile.penetrate = -1;
			projectile.scale = 1.2f;
			projectile.alpha = 0;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/HardlightGlaiveGlow");
			Vector2 drawOrigin = new Vector2(0, 0) + (projectile.spriteDirection != 1 ? new Vector2(48, 0) : Vector2.Zero);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			if (modPlayer.rainbowGlowmasks)
			{
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
				for (int k = 0; k < 3; k++)
				{
					spriteBatch.Draw(texture, drawPos, null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
			else
			{
				Color color = Color.White;
				spriteBatch.Draw(texture, drawPos, null, projectile.GetAlpha(color), projectile.rotation, drawOrigin, projectile.scale, projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			float rotation = projectile.velocity.ToRotation();
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + rotation.ToRotationVector2() * 12, projectile.Center + rotation.ToRotationVector2() * -88, 24f * projectile.scale, ref point))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}
        public float movementFactor 
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}
		Vector2 originalVelo = Vector2.Zero;
		bool runOnce = true;
		int counter = 0;
		bool runOnce2 = true;
		public override void AI()
		{
			if(runOnce)
            {
				originalVelo = projectile.velocity;
				runOnce = false;
            }
			Player player = Main.player[projectile.owner];
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-64, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(counter * 8 * projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			projectile.velocity = new Vector2(-projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			projectile.direction = player.direction;
			player.heldProj = projectile.whoAmI;
			player.itemTime = player.itemAnimation;
			projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
			projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
			if (!player.frozen)
			{
				if (movementFactor == 0f) 
				{
					movementFactor = 10f; 
					projectile.netUpdate = true;
				}
				if (player.itemAnimation < player.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
				{
					movementFactor -= 1.25f;
				}
				else // Otherwise, increase the movement factor
				{
					movementFactor += 0.75f;
				}
			}
			projectile.position += projectile.velocity * movementFactor;
			if (player.itemAnimation == 0)
			{
				projectile.Kill();
			}
			projectile.rotation = (projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(135f);
			projectile.spriteDirection = -projectile.direction;
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= MathHelper.ToRadians(90f);
			}
			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, ModContent.DustType<CopyDust4>(), projectile.velocity.X * .2f, projectile.velocity.Y * .2f, 100, Scale: 1.2f);
				dust.velocity += projectile.velocity * 0.3f;
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 70), new Color(120, 140, 180, 70), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.fadeIn = 0.2f;
			}
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.height, projectile.width, ModContent.DustType<CopyDust4>(), 0, 0, 150, Scale: 0.3f);
				dust.velocity += projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 70), new Color(120, 140, 180, 70), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.fadeIn = 0.2f;
			}
			if (runOnce2 && player.itemAnimation <= player.itemAnimationMax / 3)
			{
				Main.PlaySound(3, (int)projectile.Center.X, (int)projectile.Center.Y, 53, 0.625f);
				runOnce2 = false;
				if(projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, originalVelo.X * 1.3f, originalVelo.Y * 1.3f, mod.ProjectileType("HardlightColumn"), (int)(projectile.damage * 1.6f) + 1, projectile.knockBack * 0.5f, projectile.owner, 3, 0);
			}
		}
		int storeData = -1;
		public override void PostAI()
		{
			if (storeData == -1 && projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("HardlightTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.75f, projectile.owner, 0, projectile.whoAmI);
				projectile.ai[1] = storeData;
			}
		}
	}
}