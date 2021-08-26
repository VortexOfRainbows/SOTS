using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceStorm : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		int size = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Storm");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(263);
            aiType = 263; 
			projectile.height = 34;
			projectile.width = 34;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.alpha = 255;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			Vector2 circularLocation = new Vector2(-distance, 4).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.525f + 0.1f * modPlayer.shardSpellExtra;
			if(distance > 1)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType< CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;

				num1 = Dust.NewDust(new Vector2(projectile.Center.X - circularLocation.X - 4, projectile.Center.Y - circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			else
			{
				projectile.friendly = false;
				if (projectile.timeLeft % 9 == 0)
				{
					Bang(Main.rand.Next(-24, 25), Main.rand.Next(-24, 25));
					projectile.friendly = true;
				}
			}
		}
		public void Bang(int pos1, int pos2)
		{
			Vector2 atLoc = new Vector2(projectile.Center.X + pos1, projectile.Center.Y + pos2);
			Main.PlaySound(SoundID.Item, (int)atLoc.X, (int)atLoc.Y, 30, 0.9f, -0.2f);
			for (int i = 0; i < 360; i += 5)
			{
				Vector2 circularLocation = new Vector2(30 * (1 + 0.2f * size), 0).RotatedBy(MathHelper.ToRadians(i));
				if (i < 90)
				{
					circularLocation -= new Vector2(30 * (1 + 0.2f * size), 30 * (1 + 0.2f * size));
				}
				else if (i < 180)
				{
					circularLocation -= new Vector2(-30 * (1 + 0.2f * size), 30 * (1 + 0.2f * size));
				}
				else if (i < 270)
				{
					circularLocation -= new Vector2(-30 * (1 + 0.2f * size), -30 * (1 + 0.2f * size));
				}
				else
				{
					circularLocation -= new Vector2(30 * (1 + 0.2f * size), -30 * (1 + 0.2f * size));
				}
				int num1 = Dust.NewDust(new Vector2(atLoc.X + circularLocation.X - 4, atLoc.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = 0.1f * -circularLocation;
				Main.dust[num1].scale = 1 + 0.1f * size;
			}
			size++;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)(projectile.position.X - projectile.width), (int)(projectile.position.Y - projectile.height), projectile.width * 3, projectile.height * 3);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.velocity *= 0.5f;
            target.immune[projectile.owner] = 6;
        }
	}
}
		