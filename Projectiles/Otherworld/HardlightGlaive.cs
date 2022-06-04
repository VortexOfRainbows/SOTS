using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
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
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1.2f;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/HardlightGlaiveGlow");
			Vector2 drawOrigin = new Vector2(0, 0) + (Projectile.spriteDirection != 1 ? new Vector2(48, 0) : Vector2.Zero);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			if (modPlayer.rainbowGlowmasks)
			{
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
				for (int k = 0; k < 3; k++)
				{
					Main.spriteBatch.Draw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
			else
			{
				Color color = Color.White;
				Main.spriteBatch.Draw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			float rotation = Projectile.velocity.ToRotation();
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + rotation.ToRotationVector2() * 12, Projectile.Center + rotation.ToRotationVector2() * -88, 24f * Projectile.scale, ref point))
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}
        public float movementFactor 
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		Vector2 originalVelo = Vector2.Zero;
		bool runOnce = true;
		int counter = 0;
		bool runOnce2 = true;
		public override void AI()
		{
			if(runOnce)
            {
				originalVelo = Projectile.velocity;
				runOnce = false;
            }
			Player player = Main.player[Projectile.owner];
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-64, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(counter * 8 * Projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			Projectile.velocity = new Vector2(-Projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			Projectile.direction = player.direction;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = player.itemAnimation;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			if (!player.frozen)
			{
				if (movementFactor == 0f) 
				{
					movementFactor = 10f; 
					Projectile.netUpdate = true;
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
			Projectile.position += Projectile.velocity * movementFactor;
			if (player.itemAnimation == 0)
			{
				Projectile.Kill();
			}
			Projectile.rotation = (Projectile.Center - player.Center).ToRotation() + MathHelper.ToRadians(135f);
			Projectile.spriteDirection = -Projectile.direction;
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= MathHelper.ToRadians(90f);
			}
			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<CopyDust4>(), Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, Scale: 1.2f);
				dust.velocity += Projectile.velocity * 0.3f;
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 70), new Color(120, 140, 180, 70), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.fadeIn = 0.2f;
			}
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<CopyDust4>(), 0, 0, 150, Scale: 0.3f);
				dust.velocity += Projectile.velocity * 0.5f;
				dust.velocity *= 0.5f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 70), new Color(120, 140, 180, 70), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.fadeIn = 0.2f;
			}
			if (runOnce2 && player.itemAnimation <= player.itemAnimationMax / 3)
			{
				SOTSUtils.PlaySound(SoundID.NPCHit53, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.625f);
				runOnce2 = false;
				if(Projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, originalVelo.X * 1.3f, originalVelo.Y * 1.3f, ModContent.ProjectileType<HardlightColumn>(), (int)(Projectile.damage * 1.6f) + 1, Projectile.knockBack * 0.5f, Projectile.owner, 3, 0);
			}
		}
		int storeData = -1;
		public override void PostAI()
		{
			if (storeData == -1 && Projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<HardlightTrail>(), (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.75f, Projectile.owner, 0, Projectile.whoAmI);
				Projectile.ai[1] = storeData;
				Projectile.netUpdate = true;
			}
		}
	}
}