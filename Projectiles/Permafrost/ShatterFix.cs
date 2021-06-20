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
			projectile.width = 42;
			projectile.height = 46;
			projectile.penetrate = -1;
			Main.projFrames[projectile.type] = 11;
			projectile.friendly = false;
			projectile.timeLeft = 16;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
		}
		public void Bang(float pos1, float pos2)
		{
			Vector2 atLoc = new Vector2(projectile.Center.X + pos1, projectile.Center.Y + pos2);
			Main.PlaySound(2, (int)(atLoc.X), (int)(atLoc.Y), 30, 0.3f);
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
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.brokenFrigidSword = 3;
			player.heldProj = projectile.whoAmI;

			projectile.position = player.Center + new Vector2(22 * player.direction, -26) - new Vector2(21, 23);
			projectile.spriteDirection = player.direction;
			if (projectile.frame != (int)projectile.ai[0])
			{
				projectile.frame = (int)projectile.ai[0];
				if (projectile.frame == 10)
				{
					projectile.Kill();
					return false;
				}
				Bang(2f * projectile.frame * player.direction, -2f * projectile.frame);
				return true;
			}
			if (projectile.ai[0] == 0)
			{
				Bang(0, 0);
				projectile.Kill();
			}
			return false;
		}
	}
}
		