using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SOTS.Projectiles.Chaos
{    
    public class ChimeraFireball : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
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
        public override void SetDefaults()
        {
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 330;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.netImportant = true;
		}
		public override void OnKill(int timeLeft)
        {
            if(Main.myPlayer == Projectile.owner)
            {
                for(int i = 0; i < 8; i ++)
                {
                    Vector2 circular = new Vector2(2, 0).RotatedBy(i * MathHelper.PiOver4);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<ChimeraFireballSmall>(), Projectile.damage, 1f, Main.myPlayer, Projectile.ai[0]);
                }
            }
		}
		public override void AI()
        {
            int target = (int)Projectile.ai[0];
            Player player = Main.player[target];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
            }
            else
            {
                float speedMult = Projectile.ai[1] / 60f;
                if (speedMult > 1)
                    speedMult = 1;
                Vector2 hoverPosition = player.Center - new Vector2(0, 200);
                Vector2 toPlayer = hoverPosition - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayer.SafeNormalize(Vector2.Zero) * (2 + speedMult + Projectile.velocity.Length()), 0.02f * (1 + speedMult));
                Projectile.ai[1]++;
                Projectile.rotation += MathHelper.ToRadians(4) * Math.Sign(Projectile.velocity.X) * MathF.Sqrt(Math.Abs(Projectile.velocity.X));
            }
        }
    }
    public class ChimeraFireballSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 18;
            Projectile.width = 18;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 330;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(4);
        }
    }
}
		