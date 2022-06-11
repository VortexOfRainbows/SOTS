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
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 34;
			Projectile.width = 34;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Vector2 circularLocation = new Vector2(-distance, 4).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.525f + 0.1f * modPlayer.shardSpellExtra;
			if(distance > 1)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType< CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;

				num1 = Dust.NewDust(new Vector2(Projectile.Center.X - circularLocation.X - 4, Projectile.Center.Y - circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
			else
			{
				Projectile.friendly = false;
				if (Projectile.timeLeft % 9 == 0)
				{
					Bang(Main.rand.Next(-24, 25), Main.rand.Next(-24, 25));
					Projectile.friendly = true;
				}
			}
		}
		public void Bang(int pos1, int pos2)
		{
			Vector2 atLoc = new Vector2(Projectile.Center.X + pos1, Projectile.Center.Y + pos2);
			SOTSUtils.PlaySound(SoundID.Item30, (int)atLoc.X, (int)atLoc.Y, 0.9f, -0.2f);
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
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width), (int)(Projectile.position.Y - Projectile.height), Projectile.width * 3, Projectile.height * 3);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.velocity *= 0.5f;
            target.immune[Projectile.owner] = 6;
        }
	}
}
		