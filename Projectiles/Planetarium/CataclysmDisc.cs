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

namespace SOTS.Projectiles.Planetarium
{    
    public class CataclysmDisc : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cataclysm Disc");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;    
		}
        public override void SetDefaults()
        {
			Projectile.height = 48;
			Projectile.width = 48;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 715;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
		}
		int helixRot = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.rotation += 0.125f;
			if(Projectile.timeLeft < 700)
			{
				if(Projectile.timeLeft > 610)
				{
					Projectile.velocity *= 0.91f;
				}
				else
				{
					Projectile.velocity = new Vector2(-22.5f, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
					Vector2 toPlayer = player.Center - Projectile.Center;
					float distance = toPlayer.Length();
					if(distance < 20)
					{
						Projectile.Kill();
					}
				}
				if(Projectile.timeLeft == 650)
				{
					if (Projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<CataclysmDamage>(), (int)(3f * Projectile.damage) + 1, Projectile.knockBack, Main.myPlayer);
					}
				}
			}

			Projectile.localAI[0] += 10f;
			float rad = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			helixRot++;

			Vector2 helixPos3 = new Vector2(22f, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			Vector2 helixPos4 = new Vector2(22f, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			helixPos3 *= Projectile.scale;
			helixPos4 *= Projectile.scale;
			int num2 = Dust.NewDust(new Vector2(Projectile.Center.X + helixPos3.X - 4, Projectile.Center.Y + helixPos3.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
			Dust dust = Main.dust[num2];
			dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(Projectile.localAI[0])).X + 0.5f);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos3 * 0.065f + Projectile.velocity * 0.35f;
			dust.alpha = Projectile.alpha;

			num2 = Dust.NewDust(new Vector2(Projectile.Center.X + helixPos4.X - 4, Projectile.Center.Y + helixPos4.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
			dust = Main.dust[num2];
			dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), -new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(Projectile.localAI[0])).X + 0.5f);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos4 * 0.065f + Projectile.velocity * 0.35f;
			dust.alpha = Projectile.alpha;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = 21;
			if (Projectile.timeLeft < 690)
			{
				if(Projectile.timeLeft > 610)
				{
					Projectile.localNPCImmunity[target.whoAmI] = 6;
				}
			}
			if(Main.rand.NextBool(10))
				target.AddBuff(BuffID.OnFire, 900, false);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.timeLeft = Projectile.timeLeft > 705 ? 705 : Projectile.timeLeft;
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			Projectile.tileCollide = false;
			return false;
		}
	}
}
		