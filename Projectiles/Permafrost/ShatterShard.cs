using Microsoft.Xna.Framework;
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
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 12;
			projectile.width = 12;
			projectile.penetrate = 12;
			projectile.friendly = true;
			projectile.timeLeft = 1800;
			projectile.tileCollide = false;
			projectile.melee = true;
			projectile.hostile = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.timeLeft < 1740)
				projectile.timeLeft -= 360;
		}
		public override bool PreAI()
		{
			if (rotation == 0)
				Main.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));

			rotation++;
			return true;
		}
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			if(player.dead)
			{
				projectile.Kill();
			}
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (projectile.timeLeft < 1740)
			{
				Vector2 toPlayer = player.Center - projectile.Center;
				float distance = toPlayer.Length();
				float speed = distance * 0.12f;
				if (speed < 12) speed = 12;

				projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
				if (distance < 128)
				{
					bool found = false;
					int ofTotal = 0;
					int total = 0;
					for(int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if(projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner)
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
					if(ofTotal > 60)
					{
						projectile.Kill();
					}
					Vector2 rotateCenter = new Vector2(96, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
					rotateCenter += player.Center;
					Vector2 toRotate = rotateCenter - projectile.Center;
					float dist2 = toRotate.Length();
					if(dist2 > 12)
					{
						dist2 = 12;
					}
					projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(projectile.Center.Y - rotateCenter.Y, projectile.Center.X - rotateCenter.X));
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			for (int i = 0; i < 10; i++)
			{
				int num1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 67);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}
		