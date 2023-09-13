using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{
	public class AtlantisGlaive : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hardlight Glaive");
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1.0f;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(0, 0) + (Projectile.spriteDirection != 1 ? new Vector2(94, 0) : Vector2.Zero);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            for (int i = 0; i < 6; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(i * MathHelper.TwoPi / 6f + MathHelper.ToRadians(SOTSWorld.GlobalCounter * -1.5f));
                Main.spriteBatch.Draw(texture, drawPos + circular, null, new Color(40, 50, 120, 0), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
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
		public override bool PreAI()
		{
			if(runOnce)
            {
                SOTSUtils.PlaySound(SoundID.DD2_GhastlyGlaivePierce, Projectile.Center, 0.9f, 0.2f);
                originalVelo = Projectile.velocity;
				runOnce = false;
            }
			Player player = Main.player[Projectile.owner];
			Projectile.direction = Math.Sign(originalVelo.X);
			player.direction = Projectile.direction;
			counter++;

			Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
			Vector2 mousePosition = new Vector2(-54, 0).RotatedBy(originalVelo.ToRotation());
			mousePosition += new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(counter * 8 * Projectile.direction) + originalVelo.ToRotation());
			Vector2 toMouse = mousePosition;
			Projectile.velocity = new Vector2(-Projectile.velocity.Length(), 0).RotatedBy(toMouse.ToRotation());

			player.heldProj = Projectile.whoAmI;
			player.itemTime = player.itemAnimation;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			if (!player.frozen)
			{
				if (movementFactor == 0f) 
				{
					movementFactor = 10f; 
					if (Main.myPlayer == Projectile.owner)
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
			if (player.itemAnimation <= 1 && counter > 5 && Main.myPlayer == Projectile.owner)
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
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<CopyDust4>(), Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 30, Scale: 1.2f);
				dust.velocity += Projectile.velocity * 0.3f;
				dust.velocity *= 0.2f;
				dust.noGravity = true;
				dust.color = ColorHelpers.AtlantisColor;
				dust.fadeIn = 0.2f;
			}
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<CopyDust4>(), 0, 0, 30, Scale: 0.75f);
				dust.velocity += Projectile.velocity * 0.5f;
				dust.velocity *= 0.25f;
				dust.noGravity = true;
				dust.color = ColorHelpers.AtlantisColor;
                dust.fadeIn = 0.2f;
			}
			if (runOnce2 && player.itemAnimation <= player.itemAnimationMax / 3)
			{
				runOnce2 = false;
				if(Projectile.owner == Main.myPlayer)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, originalVelo.SafeNormalize(Vector2.Zero) * 0.5f, ModContent.ProjectileType<WaterShark>(), Projectile.damage, Projectile.knockBack * 0.5f, Projectile.owner, 3, 0);
			}
			return false;
		}
	}
}