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
			writer.Write(ofTotal2);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			active = reader.ReadBoolean();
			ofTotal2 = reader.ReadInt32();
		}
		public override void SetDefaults()
        {
			Projectile.width = 18;
			Projectile.height = 32;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.alpha = 200;
			Projectile.ignoreWater = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		Vector2 aimTo = new Vector2(0, 0);
		int ofTotal2 = 0;
		float rotate = 0;
		bool active = false;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.5f, 0.65f, 0.75f);
			Player player  = Main.player[Projectile.owner];
			BladePlayer bladePlayer = player.GetModPlayer<BladePlayer>();
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead)
			{
				Projectile.Kill();
			}
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if(Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner && proj.timeLeft > 748 && Projectile.timeLeft > 748)
				{
					if(proj == Projectile)
					{
						found = true;
					}
					if(!found)
						ofTotal++;
					total++;
				}
			}
			if (Main.myPlayer == player.whoAmI)
				ofTotal2 = ofTotal;
			if (ofTotal2 >= bladePlayer.maxBlades || bladePlayer.maxBlades == 0)
			{
				Projectile.Kill();
			}
			if (bladePlayer.attackNum > 10 && Main.myPlayer == Projectile.owner)
			{
				active = true;
				Projectile.netUpdate = true;
			}
			if(!active)
			{
				if (Projectile.timeLeft > 720)
				{
					Projectile.timeLeft = 750;
				}
				Vector2 toPlayer = player.Center - Projectile.Center;
				float distance = toPlayer.Length();
				float speed = distance * 0.1f;
				if(distance > 2000f)
				{
					if(Main.myPlayer == Projectile.owner)
					{
						Projectile.position = player.position;
						Projectile.netUpdate = true;
					}
				}
				Vector2 rotateCenter = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(-modPlayer.orbitalCounter * 1.15f + (ofTotal2 * 360f / total)));
				rotateCenter += player.Center;
				Vector2 toRotate = rotateCenter - Projectile.Center;
				float dist2 = toRotate.Length();
				if (dist2 > 6 + dist2/ 40f)
				{
					dist2 = 6 + dist2 / 40f;
				}
				Projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - rotateCenter.Y, Projectile.Center.X - rotateCenter.X));
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.ai[0] = Main.MouseWorld.X;
					Projectile.ai[1] = Main.MouseWorld.Y;
					Projectile.netUpdate = true;
				}
				aimTo = player.Center - new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Projectile.rotation = aimTo.ToRotation() - MathHelper.ToRadians(90);
			}
			else if (Projectile.timeLeft > 720)
			{
				if(Projectile.alpha > 0)
					Projectile.alpha -= 4;
				Projectile.friendly = true;
				rotate += 6.25f;
				Vector2 prepareVelo = new Vector2(-3f, 0).RotatedBy(MathHelper.ToRadians(rotate));
				aimTo = aimTo.SafeNormalize(new Vector2(0,1));
				if (Projectile.timeLeft == 721)
				{
					SOTSUtils.PlaySound(SoundID.Item71, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.7f);
					aimTo *= -12;
					Projectile.velocity = aimTo;
				}
				else
				{
					Vector2 newAimTo = aimTo * prepareVelo.X;
					Projectile.velocity = -newAimTo;
				}
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.timeLeft < 720)
			{
				int dust = Dust.NewDust(Projectile.Center - new Vector2(5, 4), 0, 0, DustID.Electric, 0, 0, Projectile.alpha, default, 1f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.1f;
			}
			if (Projectile.timeLeft < 700)
			{
				Projectile.tileCollide = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			if(timeLeft < 749)
			{
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
					Main.dust[dust].velocity += Projectile.velocity / 2f;
				}
			}
		}
	}
}
		