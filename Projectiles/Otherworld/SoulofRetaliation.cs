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
    public class SoulofRetaliation : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Retaliation");
		}
        public override void SetDefaults()
        {
			projectile.width = 22;
			projectile.height = 22;
			projectile.penetrate = 1;
			Main.projFrames[projectile.type] = 4;
			projectile.friendly = false;
			projectile.timeLeft = 900;
			projectile.tileCollide = false;
			projectile.melee = true;
			projectile.hostile = false;
			projectile.netImportant = true;
			projectile.alpha = 100;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(active);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			active = reader.ReadBoolean();
		}
		Vector2 aimTo = new Vector2(0, 0);
		float scaleGrow = 0;
		float counter = 0;
		int ofTotal = 0;
		int total = 0;
		bool active = false;
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.5f, 0.5f, 0.5f);
			Player player  = Main.player[projectile.owner];
			BeadPlayer beadPlayer = player.GetModPlayer<BeadPlayer>();
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int maxCounter = 230;
			if(counter < maxCounter)
			counter += 2;
			projectile.frameCounter++;
			if(projectile.frameCounter >= 8)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 4;
			}
			if (player.dead)
			{
				projectile.Kill();
				return;
			}
			if (beadPlayer.attackNum > 10 && counter >= maxCounter && projectile.owner == Main.myPlayer)
			{
				active = true;
				projectile.netUpdate = true;
			}
			if(!active)
			{
				bool found = false; 
				ofTotal = 0;
				total = 0;
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner && proj.timeLeft > 748 && projectile.timeLeft > 748)
					{
						if (proj == projectile)
						{
							found = true;
						}
						if (!found)
							ofTotal++;
						total++;
					}
				}
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
				Vector2 rotateCenter = new Vector2(maxCounter + 40 - counter, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total)));
				rotateCenter += player.Center - new Vector2(0, counter/3);
				Vector2 toRotate = rotateCenter - projectile.Center;
				float dist2 = toRotate.Length();
				if (dist2 > 9 + dist2 / 40f)
				{
					dist2 = 9 + dist2 / 40f;
				}
				projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(projectile.Center.Y - rotateCenter.Y, projectile.Center.X - rotateCenter.X));
				if (projectile.owner == Main.myPlayer)
				{
					projectile.ai[0] = Main.MouseWorld.X;
					projectile.ai[1] = Main.MouseWorld.Y;
					projectile.netUpdate = true;
				}
				Vector2 rotateAdd = new Vector2(maxCounter + 40 - counter, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total)));
				aimTo = projectile.Center - (new Vector2(projectile.ai[0], projectile.ai[1]) + rotateAdd);
				scaleGrow += 4;
				Vector2 cycleScale = new Vector2(0.2f, 0).RotatedBy(MathHelper.ToRadians(scaleGrow));
				projectile.scale = 1f + cycleScale.X;
			}
			else if (projectile.timeLeft > 720) //prelaunch effects
			{
				if (projectile.owner == Main.myPlayer)
				{
					projectile.netUpdate = true;
				}
				projectile.velocity *= 0f;
				projectile.alpha -= 2;
				if(projectile.timeLeft > 748)
				{
					Main.PlaySound(SoundID.NPCDeath39, player.Center);
					projectile.scale = 1f;
				}
				projectile.scale += 0.02f;
			}
			if (projectile.timeLeft < 720) //launch effects
			{
				Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 94);
				aimTo = aimTo.SafeNormalize(new Vector2(0, 1));
				aimTo *= -12;
				projectile.velocity = aimTo;
				if(Main.myPlayer == projectile.owner)
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("DoubleLaser"), projectile.damage, 1f, player.whoAmI, modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total));
				projectile.Kill();
			}
		}
		public override void Kill(int timeLeft)
		{
			if(timeLeft < 749)
			{
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 91, 0, 0, projectile.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
	}
}
		