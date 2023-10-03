using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceCluster : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Cluster");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(48);
            AIType = 48;
			Projectile.DamageType = DamageClass.Ranged;
			// Projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.penetrate = 1;
			Projectile.width = 18;
			Projectile.height = 18;
		}
		bool runOnce = true;
		bool alt = false;
		int counter = 0;
        public override bool PreAI()
        {
			if(runOnce)
            {
				if (Projectile.ai[0] != -1)
					alt = true;
				runOnce = false;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			counter++;
			if(counter == 4 && !alt)
            {
				for(float j = 0; j <= 0.1f; j += 0.1f)
					for(int i = 0; i < 360; i += 10)
					{
						Vector2 circular = new Vector2(2 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
						circular.X *= 0.5f;
						circular = circular.RotatedBy(Projectile.velocity.ToRotation());
						int snow = Dust.NewDust(Projectile.Center + Projectile.velocity * j + circular - new Vector2(5), 0, 0, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
						Main.dust[snow].noGravity = true;
						Main.dust[snow].velocity *= 0f;
						Main.dust[snow].velocity += 0.6f * circular;
					}
            }
			if(Main.rand.NextBool(2) && (counter >= 5 || alt))
			{
				int snow = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
				Main.dust[snow].noGravity = true;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = alt ? Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/IceClusterAlt").Value : Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void OnKill(int timeLeft)
        {
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<IcePulse>(), Projectile.damage, 0, Projectile.owner, alt ? -1 : 0);
				for (int i = 0; i < 15; i++)
				{
					int snow = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, alt ? ModContent.DustType<ModIceDust>() : ModContent.DustType<ModSnowDust>());
					Main.dust[snow].noGravity = true;
					Main.dust[snow].velocity *= 2;
					Main.dust[snow].scale *= 1.2f;
				}
			}
		}
	}
}
		
			