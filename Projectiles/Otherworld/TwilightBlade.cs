using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class TwilightBlade : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Blade");
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
			projectile.penetrate = 1;
			projectile.friendly = false;
			projectile.timeLeft = 900;
			projectile.tileCollide = false;
			projectile.melee = true;
			projectile.hostile = false;
			projectile.netImportant = true;
			projectile.alpha = 200;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		Vector2 aimTo = new Vector2(0, 0);
		float rotate = 0;
		bool active = false;
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.5f, 0.65f, 0.75f);
			Player player  = Main.player[projectile.owner];
			BladePlayer bladePlayer = player.GetModPlayer<BladePlayer>();
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (player.dead)
			{
				projectile.Kill();
			}
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if(projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner && proj.timeLeft > 748 && projectile.timeLeft > 748)
				{
					if(proj == projectile)
					{
						found = true;
					}
					if(!found)
						ofTotal++;
					total++;
				}
			}
			if(ofTotal >= bladePlayer.maxBlades || bladePlayer.maxBlades == 0)
			{
				projectile.Kill();
			}
			if (bladePlayer.attackNum > 10 && Main.myPlayer == projectile.owner)
			{
				active = true;
				projectile.netUpdate = true;
			}
			if(!active)
			{
				if (projectile.timeLeft > 720)
				{
					projectile.timeLeft = 750;
				}
				Vector2 toPlayer = player.Center - projectile.Center;
				float distance = toPlayer.Length();
				float speed = distance * 0.1f;
				if(distance > 2000f)
				{
					if(Main.myPlayer == projectile.owner)
					{
						projectile.position = player.position;
						projectile.netUpdate = true;
					}
				}
				Vector2 rotateCenter = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(-modPlayer.orbitalCounter * 1.15f + (ofTotal * 360f / total)));
				rotateCenter += player.Center;
				Vector2 toRotate = rotateCenter - projectile.Center;
				float dist2 = toRotate.Length();
				if (dist2 > 6 + dist2/ 40f)
				{
					dist2 = 6 + dist2 / 40f;
				}
				projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(projectile.Center.Y - rotateCenter.Y, projectile.Center.X - rotateCenter.X));
				if (projectile.owner == Main.myPlayer)
				{
					projectile.ai[0] = Main.MouseWorld.X;
					projectile.ai[1] = Main.MouseWorld.Y;
					projectile.netUpdate = true;
				}
				aimTo = player.Center - new Vector2(projectile.ai[0], projectile.ai[1]);
				projectile.rotation = aimTo.ToRotation() - MathHelper.ToRadians(90);
			}
			else if (projectile.timeLeft > 720)
			{
				if(projectile.alpha > 0)
					projectile.alpha -= 4;
				projectile.friendly = true;
				rotate += 6.25f;
				Vector2 prepareVelo = new Vector2(-3f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				aimTo = aimTo.SafeNormalize(new Vector2(0,1));
				if (projectile.timeLeft == 721)
				{
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 71, 0.7f);
					aimTo *= -12;
					projectile.velocity = aimTo;
				}
				else
				{
					Vector2 newAimTo = aimTo * prepareVelo.X;
					projectile.velocity = -newAimTo;
				}
				if (projectile.owner == Main.myPlayer)
				{
					projectile.netUpdate = true;
				}
			}
			if (projectile.timeLeft < 720)
			{
				int dust = Dust.NewDust(projectile.Center - new Vector2(5, 4), 0, 0, DustID.Electric, 0, 0, projectile.alpha, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
			}
			if (projectile.timeLeft < 700)
			{
				projectile.tileCollide = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			if(timeLeft < 749)
			{
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, 0, 0, projectile.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].velocity += projectile.velocity / 2f;
				}
			}
		}
	}
}
		