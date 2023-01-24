using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class IlluminantAxe : ModProjectile 
    {
        public float AI3 = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AI3);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI3 = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Axe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Earth/Glowmoth/IlluminantAxeGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            AI3 = 5;
            Projectile.netUpdate = true;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
            height = 24;
            return true;
        }
        int counter = 0;
        public override bool PreAI()
        {
            if(AI3 == 5)
            {
                Projectile.tileCollide = false;
                Projectile.velocity *= 0.1f;
                AI3 = 4;
            }
            else if (AI3 == 4)
            {
                Projectile.tileCollide = false;
                Projectile.velocity *= 0.9f;
            }
            counter++;
            if (counter % 3 != 0)
                Projectile.soundDelay++;
            Projectile.rotation -= 0.12f * (float)Projectile.direction;
            if (Main.rand.NextBool(12))
            {
                int num1 = Dust.NewDust(new Vector2(Projectile.Hitbox.X, Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, DustID.GlowingMushroom, 0f, 0f, 150, default(Color), 1.3f);
                Main.dust[num1].velocity *= 0.2f;
                Main.dust[num1].scale = 0.8f;
            }
            else if(Main.rand.NextBool(6))
            {
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num2];
                dust.color = VoidPlayer.VibrantColorAttempt(Main.rand.NextFloat(360));
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.44f;
                dust.velocity += Projectile.velocity;
            }
            return base.PreAI();
        }
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 46; 
            Projectile.timeLeft = 7200;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Melee; 
            Projectile.aiStyle = 3; 
			Projectile.alpha = 0;
            Projectile.extraUpdates = 1;
		}
	}
}