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
			Projectile.width = 36;
			Projectile.height = 22;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false;
			Projectile.melee = false;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 0.9f;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			writer.Write(ofTotalId);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
			ofTotalId = reader.ReadInt32();
		}
		bool returnH = false;
		int counter = 0;
		int ofTotalId = 0;
		int shootingDelay = 0;
		Vector2 offset = Vector2.Zero;
		public override void AI()
		{
			Player player  = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(counter > 5)
            {
				if((Projectile.ai[0] < 0) && !returnH)
                {
					if(player.itemAnimation <= 0 && !player.channel)
					{
						Projectile.ai[0]++;
					}
					else
                    {
						Projectile.ai[0] = -2;
                    }
                }
				else
				{
					Projectile.ai[0]--;
					if (Projectile.ai[0] <= 0)
						returnH = true;
				}
			}
			counter++;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.Projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner && Math.Abs(proj.ai[0] - Projectile.ai[0]) <= 1)
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
				Projectile.netUpdate = true;
            }
			Vector2 toLocation = player.Center;
			Projectile.velocity *= 0.05f;
			toLocation.Y += Main.player[Projectile.owner].gfxOffY;
			float rotation = modPlayer.orbitalCounter * 1.5f + (float)ofTotalId / total * 360f;
			Vector2 circular = new Vector2(40, 0).RotatedBy(MathHelper.ToRadians(rotation));
			circular.Y *= 1.1f;
			Vector2 goTo = toLocation + circular;
			goTo -= Projectile.Center;
			float dist = 7.5f + goTo.Length() * 0.1f;
			if (returnH)
			{
				goTo -= circular;
				if(goTo.Length() <= 12)
                {
					Projectile.Kill();
                }
			}
			Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
			if (dist > goTo.Length())
				dist = goTo.Length();
			Projectile.velocity = newGoTo * dist;
			if (!returnH)
			{
				if(counter >= 6)
                {
					shootingDelay--;
					if (shootingDelay <= 0)
					{
						shootingDelay = (int)Projectile.ai[1];
						FireBullet();
						if (Projectile.ai[1] < 16)
							Projectile.ai[1] += 1.5f;
						if (Projectile.ai[1] > 16)
						{
							Projectile.ai[1] = 16;
                        }
					}
				}
				Projectile.timeLeft = 11;
			}
			if(Projectile.timeLeft < 11)
            {
				Projectile.alpha += 25;
            }
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 center = new Vector2(Projectile.Center.X, Projectile.Center.Y + 4);
				Vector2 playerCursor = Main.MouseWorld;
				Projectile.rotation = (center - playerCursor).ToRotation();
				Projectile.netUpdate = true;
				if ((playerCursor - Projectile.Center).X > 0)
				{
					Projectile.rotation -= MathHelper.Pi;
					Projectile.spriteDirection = -1;
				}
				else
				{
					Projectile.spriteDirection = 1;
				}
			}
			offset *= 0.8f;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture, Projectile.Center + offset - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
			return false;
        }
        public void FireBullet()
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 center = new Vector2(Projectile.Center.X, Projectile.Center.Y + 5);
				Vector2 playerCursor = Main.MouseWorld;
				Vector2 rotateArea = new Vector2(6, 0).RotatedBy((playerCursor - center).ToRotation());
				Projectile.NewProjectile(center, rotateArea, ModContent.ProjectileType<FriendlyPolarBullet>(), (int)(Projectile.damage * 0.25f), Projectile.knockBack, Main.myPlayer);
			}
			offset = new Vector2(8 * Projectile.spriteDirection, 0).RotatedBy(Projectile.rotation);
		}
	}
}
		