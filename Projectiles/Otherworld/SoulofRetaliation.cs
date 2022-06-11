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
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.penetrate = 1;
			Main.projFrames[Projectile.type] = 4;
			Projectile.friendly = false;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.alpha = 100;
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
			Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);
			Player player  = Main.player[Projectile.owner];
			BeadPlayer beadPlayer = player.GetModPlayer<BeadPlayer>();
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int maxCounter = 230;
			if(counter < maxCounter)
			counter += 2;
			Projectile.frameCounter++;
			if(Projectile.frameCounter >= 8)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 4;
			}
			if (player.dead)
			{
				Projectile.Kill();
				return;
			}
			if (beadPlayer.attackNum > 10 && counter >= maxCounter && Projectile.owner == Main.myPlayer)
			{
				active = true;
				Projectile.netUpdate = true;
			}
			if(!active)
			{
				bool found = false; 
				ofTotal = 0;
				total = 0;
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner && proj.timeLeft > 748 && Projectile.timeLeft > 748)
					{
						if (proj == Projectile)
						{
							found = true;
						}
						if (!found)
							ofTotal++;
						total++;
					}
				}
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
				Vector2 rotateCenter = new Vector2(maxCounter + 40 - counter, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total)));
				rotateCenter += player.Center - new Vector2(0, counter/3);
				Vector2 toRotate = rotateCenter - Projectile.Center;
				float dist2 = toRotate.Length();
				if (dist2 > 9 + dist2 / 40f)
				{
					dist2 = 9 + dist2 / 40f;
				}
				Projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - rotateCenter.Y, Projectile.Center.X - rotateCenter.X));
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.ai[0] = Main.MouseWorld.X;
					Projectile.ai[1] = Main.MouseWorld.Y;
					Projectile.netUpdate = true;
				}
				Vector2 rotateAdd = new Vector2(maxCounter + 40 - counter, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total)));
				aimTo = Projectile.Center - (new Vector2(Projectile.ai[0], Projectile.ai[1]) + rotateAdd);
				scaleGrow += 4;
				Vector2 cycleScale = new Vector2(0.2f, 0).RotatedBy(MathHelper.ToRadians(scaleGrow));
				Projectile.scale = 1f + cycleScale.X;
			}
			else if (Projectile.timeLeft > 720) //prelaunch effects
			{
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
				}
				Projectile.velocity *= 0f;
				Projectile.alpha -= 2;
				if(Projectile.timeLeft > 748)
				{
					Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath39, player.Center);
					Projectile.scale = 1f;
				}
				Projectile.scale += 0.02f;
			}
			if (Projectile.timeLeft < 720) //launch effects
			{
				SOTSUtils.PlaySound(SoundID.Item94, Projectile.Center);
				aimTo = aimTo.SafeNormalize(new Vector2(0, 1));
				aimTo *= -12;
				Projectile.velocity = aimTo;
				if(Main.myPlayer == Projectile.owner)
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<DoubleLaser>(), Projectile.damage, 1f, player.whoAmI, modPlayer.orbitalCounter * 1.25f + (ofTotal * 360f / total));
				Projectile.Kill();
			}
		}
		public override void Kill(int timeLeft)
		{
			if(timeLeft < 749)
			{
				for (int i = 0; i < 20; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 91, 0, 0, Projectile.alpha, default, 1.25f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.5f;
				}
			}
		}
	}
}
		