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
			DisplayName.SetDefault("Holo Missile");
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
			projectile.width = 18;
			projectile.height = 32;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 540;
			projectile.tileCollide = true;
			projectile.hostile = true;
			projectile.netImportant = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if(projectile.timeLeft < 30)
			hitbox = new Rectangle((int)(projectile.Center.X - 48), (int)(projectile.Center.Y - 48), 96, 96);
		}
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (projectile.timeLeft >= 30)
				projectile.timeLeft = 31;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture2 = mod.GetTexture("Projectiles/Otherworld/HoloMissileFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			float approaching = ((540f - projectile.timeLeft) / 540f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.75f * approaching;
				float y = Main.rand.Next(-10, 11) * 0.75f * approaching;
				Main.spriteBatch.Draw(texture2, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		bool active = false;
		public override void AI()
		{
			float approaching = ((540f - projectile.timeLeft) / 540f);
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			Lighting.AddLight(projectile.Center, 0.5f, 0.65f, 0.75f);

			Player player  = Main.player[(int)projectile.ai[1]];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int dust = Dust.NewDust(projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Electric, 0, 0, projectile.alpha, default, 1.25f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity *= 0.1f;
			if (player.active)
			{
				float x = Main.rand.Next(-10, 11) * 0.001f * approaching;
				float y = Main.rand.Next(-10, 11) * 0.001f * approaching;
				Vector2 toPlayer = projectile.Center - player.Center;
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
				projectile.velocity += -toPlayer * (0.125f * projectile.timeLeft/540f) + new Vector2(x, y);
			}
			if(projectile.timeLeft == 30)
				Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 14, 0.4f);
			if (projectile.timeLeft < 30)
            {
				projectile.tileCollide = false;
				projectile.velocity *= 0f;
				projectile.alpha = 255;
				for (int i = 0; i < 6; i++)
				{
					int dust2 = Dust.NewDust(projectile.position - new Vector2(7, 0), projectile.width + 7, projectile.height, DustID.Electric, 0, 0, projectile.alpha, default, 1.25f);
					Main.dust[dust2].noGravity = true;
					Main.dust[dust2].velocity *= 2f;
				}
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			projectile.timeLeft = 31;
			projectile.tileCollide = false;
			projectile.velocity *= 0f;
            return false;
        }
    }
}
		