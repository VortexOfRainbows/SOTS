using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class ShatterFix : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shatter Fix");
		}
        public override void SetDefaults()
        {
			Projectile.width = 42;
			Projectile.height = 46;
			Projectile.penetrate = -1;
			Main.projFrames[Projectile.type] = 11;
			Projectile.friendly = false;
			Projectile.timeLeft = 16;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
		public void Bang(float pos1, float pos2)
		{
			Vector2 atLoc = new Vector2(Projectile.Center.X + pos1, Projectile.Center.Y + pos2);
			SOTSUtils.PlaySound(SoundID.Item30, (int)(atLoc.X), (int)(atLoc.Y), 0.3f);
			for (int i = 0; i < 360; i += 5)
			{
				Vector2 circularLocation = new Vector2(30, 0).RotatedBy(MathHelper.ToRadians(i));
				if (i < 90)
				{
					circularLocation -= new Vector2(30, 30);
					int num1 = Dust.NewDust(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = 0.1f * -circularLocation;
				}
				else if (i < 180)
				{
					circularLocation -= new Vector2(-30, 30);
					int num1 = Dust.NewDust(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = 0.1f * -circularLocation;
				}
				else if (i < 270)
				{
					circularLocation -= new Vector2(-30, -30);
					int num1 = Dust.NewDust(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = 0.1f * -circularLocation;
				}
				else
				{
					circularLocation -= new Vector2(30, -30);
					int num1 = Dust.NewDust(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = 0.1f * -circularLocation;
				}
			}
		}
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.brokenFrigidSword = 3;
			player.heldProj = Projectile.whoAmI;

			Projectile.position = player.Center + new Vector2(22 * player.direction, -26) - new Vector2(21, 23);
			Projectile.spriteDirection = player.direction;
			if (Projectile.frame != (int)Projectile.ai[0])
			{
				Projectile.frame = (int)Projectile.ai[0];
				if (Projectile.frame == 10)
				{
					Projectile.Kill();
					return false;
				}
				Bang(2f * Projectile.frame * player.direction, -2f * Projectile.frame);
				return true;
			}
			if (Projectile.ai[0] == 0)
			{
				Bang(0, 0);
				Projectile.Kill();
			}
			return false;
		}
	}
}
		