using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class MiniPolarisCannon : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polar Cannon");
		}
        public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 22;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.melee = false;
			projectile.hostile = false;
			projectile.netImportant = true;
			projectile.ignoreWater = true;
			projectile.scale = 0.9f;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(ofTotalId);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			ofTotalId = reader.ReadInt32();
		}
		bool returnH = false;
		int counter = 0;
		int ofTotalId = 0;
		int shootingDelay = 0;
		Vector2 offset = Vector2.Zero;
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(counter > 5)
            {
				if((projectile.ai[0] < 0) && !returnH)
                {
					if(player.itemAnimation <= 0 && !player.channel)
					{
						projectile.ai[0]++;
					}
					else
                    {
						projectile.ai[0] = -2;
                    }
                }
				else
				{
					projectile.ai[0]--;
					if (projectile.ai[0] <= 0)
						returnH = true;
				}
			}
			counter++;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && proj.owner == projectile.owner && Math.Abs(proj.ai[0] - projectile.ai[0]) <= 1)
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
			if (Main.myPlayer == player.whoAmI)
			{
				ofTotalId = ofTotal;
				projectile.netUpdate = true;
            }
			Vector2 toLocation = player.Center;
			projectile.velocity *= 0.05f;
			toLocation.Y += Main.player[projectile.owner].gfxOffY;
			float rotation = modPlayer.orbitalCounter * 0.8f + (float)ofTotalId / total * 360f;
			Vector2 circular = new Vector2(40, 0).RotatedBy(MathHelper.ToRadians(rotation));
			circular.Y *= 1.1f;
			Vector2 goTo = toLocation + circular;
			goTo -= projectile.Center;
			float dist = 7.5f + goTo.Length() * 0.1f;
			if (returnH)
			{
				goTo -= circular;
				if(goTo.Length() <= 12)
                {
					projectile.Kill();
                }
			}
			Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
			if (dist > goTo.Length())
				dist = goTo.Length();
			projectile.velocity = newGoTo * dist;
			if (!returnH)
			{
				if(counter >= 6)
                {
					shootingDelay--;
					if (shootingDelay <= 0)
					{
						shootingDelay = (int)projectile.ai[1];
						FireBullet();
						if (projectile.ai[1] < 16)
							projectile.ai[1] += 1.5f;
						if (projectile.ai[1] > 16)
						{
							projectile.ai[1] = 16;
                        }
					}
				}
				projectile.timeLeft = 10;
			}
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 center = new Vector2(projectile.Center.X, projectile.Center.Y + 4);
				Vector2 playerCursor = Main.MouseWorld;
				projectile.rotation = (center - playerCursor).ToRotation();
				projectile.netUpdate = true;
				if ((playerCursor - projectile.Center).X > 0)
				{
					projectile.rotation -= MathHelper.Pi;
					projectile.spriteDirection = -1;
				}
				else
				{
					projectile.spriteDirection = 1;
				}
			}
			offset *= 0.8f;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture, projectile.Center + offset - Main.screenPosition, null, lightColor, projectile.rotation, origin, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
			return false;
        }
        public void FireBullet()
		{
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 center = new Vector2(projectile.Center.X, projectile.Center.Y + 5);
				Vector2 playerCursor = Main.MouseWorld;
				Vector2 rotateArea = new Vector2(6, 0).RotatedBy((playerCursor - center).ToRotation());
				Projectile.NewProjectile(center, rotateArea, ModContent.ProjectileType<FriendlyPolarBullet>(), (int)(projectile.damage * 0.25f), projectile.knockBack, Main.myPlayer);
			}
			offset = new Vector2(8 * projectile.spriteDirection, 0).RotatedBy(projectile.rotation);
		}
	}
}
		