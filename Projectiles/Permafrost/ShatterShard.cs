using Microsoft.Xna.Framework;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class ShatterShard : ModProjectile 
    {
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shatter Shard");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.penetrate = 12;
			Projectile.friendly = true;
			Projectile.timeLeft = 1800;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.hostile = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.timeLeft < 1740)
				Projectile.timeLeft -= 360;
		}
		public override bool PreAI()
		{
			if (rotation == 0)
				SOTSUtils.PlaySound(SoundID.Item50, Projectile.Center);

			rotation++;
			return true;
		}
		public override void AI()
		{
			Player player  = Main.player[Projectile.owner];
			if(player.dead)
			{
				Projectile.Kill();
			}
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Projectile.timeLeft < 1740)
			{
				Vector2 toPlayer = player.Center - Projectile.Center;
				float distance = toPlayer.Length();
				float speed = distance * 0.12f;
				if (speed < 12) speed = 12;

				Projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
				if (distance < 128)
				{
					bool found = false;
					int ofTotal = 0;
					int total = 0;
					for(int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if(Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
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
					if(ofTotal > 60)
					{
						Projectile.Kill();
					}
					Vector2 rotateCenter = new Vector2(96, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
					rotateCenter += player.Center;
					Vector2 toRotate = rotateCenter - Projectile.Center;
					float dist2 = toRotate.Length();
					if(dist2 > 12)
					{
						dist2 = 12;
					}
					Projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - rotateCenter.Y, Projectile.Center.X - rotateCenter.X));
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item50, Projectile.Center);
			for (int i = 0; i < 10; i++)
			{
				int num1 = Dust.NewDust(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
			}
		}
	}
}
		