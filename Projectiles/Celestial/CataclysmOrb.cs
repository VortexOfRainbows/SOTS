using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Projectiles.Lightning;

namespace SOTS.Projectiles.Celestial
{    
    public class CataclysmOrb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Bomb");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(48);
            AIType = 48; 
			// Projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// Projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// Projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Color color = new Color(100, 155, 100, 0);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2.5f, 4f), 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = Color.White;
			spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public override void AI()
		{
			if(Projectile.timeLeft >= 800)
			{
				Projectile.timeLeft = Main.rand.Next(32, 48);
			}
		}
		public override void Kill(int timeLeft)
        {
			SoundEngine.PlaySound(3, (int)Projectile.Center.X, (int)Projectile.Center.Y, 53, 0.625f);
			for (int i = 0; i < 10; i++)
			{
				var num371 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, Mod.Find<ModDust>("CopyDust4").Type, 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += Projectile.velocity * 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(210, 255, 205, 100), new Color(30, 150, 60, 100), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = Projectile.alpha;
			}
			SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 94, 0.55f, 0.1f);
			if (Projectile.owner == Main.myPlayer)
			{
				int amt = 1;
				if (Main.rand.NextBool(3))
					amt++;
				if (Main.rand.NextBool(3))
					amt++;
				for (int i = 0; i < amt; i++)
				{
					Vector2 circular = -Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<CataclysmLightning>(), (int)(Projectile.damage * 0.9f + 0.5f), 0, Projectile.owner, 0);
				}
			}
		}
	}
}
			