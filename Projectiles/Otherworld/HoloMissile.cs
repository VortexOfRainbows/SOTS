using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld;
using Steamworks;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class HoloMissile : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Holo Missile");
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(active);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			active = reader.ReadBoolean();
		}
		public override void SetDefaults()
        {
			Projectile.width = 18;
			Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 540;
			Projectile.tileCollide = true;
			Projectile.hostile = true;
			Projectile.netImportant = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if(Projectile.timeLeft < 30)
			hitbox = new Rectangle((int)(Projectile.Center.X - 48), (int)(Projectile.Center.Y - 48), 96, 96);
		}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (Projectile.timeLeft >= 30)
				Projectile.timeLeft = 31;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			return true;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/HoloMissileFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			float approaching = ((540f - Projectile.timeLeft) / 540f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.75f * approaching;
				float y = Main.rand.Next(-10, 11) * 0.75f * approaching;
				Main.spriteBatch.Draw(texture2, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		bool active = false;
		public override void AI()
		{
			float approaching = ((540f - Projectile.timeLeft) / 540f);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			Lighting.AddLight(Projectile.Center, 0.5f, 0.65f, 0.75f);

			Player player  = Main.player[(int)Projectile.ai[1]];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int dust = Dust.NewDust(Projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.1f;
			if (player.active)
			{
				float x = Main.rand.Next(-10, 11) * 0.001f * approaching;
				float y = Main.rand.Next(-10, 11) * 0.001f * approaching;
				Vector2 toPlayer = Projectile.Center - player.Center;
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
				Projectile.velocity += -toPlayer * (0.125f * Projectile.timeLeft/540f) + new Vector2(x, y);
			}
			if(Projectile.timeLeft == 30)
				SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f);
			if (Projectile.timeLeft < 30)
            {
				Projectile.tileCollide = false;
				Projectile.velocity *= 0f;
				Projectile.alpha = 255;
				for (int i = 0; i < 6; i++)
				{
					int dust2 = Dust.NewDust(Projectile.position - new Vector2(7, 0), Projectile.width + 7, Projectile.height, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].velocity *= 2f;
				}
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.timeLeft = 31;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
            return false;
        }
    }
}
		