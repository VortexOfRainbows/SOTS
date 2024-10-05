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
using SOTS.Helpers;

namespace SOTS.Projectiles.Inferno
{    
    public class BlazingArrow : ModProjectile 
    {
		float helixRot = 0;
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * ((255 - Projectile.alpha) / 255f);
        }
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blazing Arrow");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				float alpha = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = new Color(200, 70, 10, 0) * alpha;
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color * 0.8f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return !stop;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(1);
            AIType = 1;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = 4;
			Projectile.alpha = 0;
			Projectile.width = 14;
			Projectile.height = 46;
			Projectile.arrow = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			stop = true;
			return false;
        }
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.OnFire, 300, false);
			if(Projectile.owner == Main.myPlayer && Projectile.penetrate != 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PlasmaStar>(), (int)(0.15f * Projectile.damage + 0.9f), 0, Main.myPlayer, -1, Main.rand.Next(360));
			}
		}
        public override bool? CanHitNPC(NPC target)
        {
            return (!stop && Projectile.penetrate > 2)? base.CanHitNPC(target) : false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) 
		{
			width = 12;
			height = 12;
			fallThrough = true;
			return true;
		}
		Vector2 initialVelo = Vector2.Zero;
		bool stop = false;
        public override bool ShouldUpdatePosition()
        {
            return !stop;
        }
        public override void AI()
		{
			if(Projectile.timeLeft < 30)
            {
				stop = true;
			}
			if(Projectile.penetrate <= 2 || stop)
			{
				if (Projectile.timeLeft > 30)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PlasmaStar>(), (int)(0.15f * Projectile.damage + 0.9f), 0, Main.myPlayer, -1, Main.rand.Next(360));
					Projectile.timeLeft = 30;
				}
				stop = true;
				Projectile.tileCollide = false;
				Projectile.rotation = initialVelo.ToRotation() + MathHelper.PiOver2;
				return;
            }
			if (initialVelo == Vector2.Zero)
				initialVelo = Projectile.velocity;
			Projectile.velocity = initialVelo; //cancel out arrow normal gravity
			
			for(float i = 0; i < 1; i += 0.2f)
			{
				float rad = Projectile.velocity.ToRotation();
				float sinusoid = (float)Math.Sin(MathHelper.ToRadians(helixRot * 6));
				float sinusoid2 = (float)Math.Cos(MathHelper.ToRadians(helixRot * 9));
				helixRot += 0.2f;
				Vector2 helixPos1 = Projectile.Center + Projectile.velocity * i + new Vector2(sinusoid * 8, 0).RotatedBy(rad + MathHelper.ToRadians(90));
				Vector2 helixPos2 = Projectile.Center + Projectile.velocity * i + new Vector2(sinusoid2 * 4, 0).RotatedBy(rad - MathHelper.ToRadians(90));

				Dust dust = Dust.NewDustDirect(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.scale = 0.5f;
				dust.fadeIn = 0.1f;
				dust.color = ColorHelper.InfernoColorGradientDegrees(MathHelper.ToRadians(helixRot * 2));

				dust = Dust.NewDustDirect(new Vector2(helixPos2.X - 4, helixPos2.Y - 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.scale = 0.5f;
				dust.fadeIn = 0.1f;
				dust.color = ColorHelper.InfernoColorGradientDegrees(MathHelper.ToRadians(helixRot * 3));
			}
		}
	}
}
		
			