using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using SOTS.Helpers;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceStorm : ModProjectile 
    {	
		private float distance = 30f;  
		private int rotation = 0;
		private int size = 1;
        public override bool ShouldUpdatePosition()
        {
            return false;
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
            Color c = ColorHelper.PermafrostColor * 0.5f;
            c.A = 0;
            if (rotation == 0)
			{
				Vector2 toPlayer = player.Center - Projectile.Center;
                Vector2 safe = -toPlayer.SNormalize();
                float length = toPlayer.Length();
                for (int i = 32; i < length - 32; i += 8)
                {
                    float percent = i / length;
                    Vector2 position = Projectile.Center + toPlayer * percent;
					if(Main.rand.NextBool(3))
					{
                        Dust d = Dust.NewDustDirect(position - new Vector2(4), 0, 0, ModContent.DustType<CopyIceDust>());
                        d.noGravity = true;
                        d.velocity = d.velocity * 0.5f + safe * Main.rand.NextFloat(1, 2);
                        d.scale = 1.05f + 0.6f * percent;
                    }
					else
						PixelDust.Spawn(position, 0, 0, safe * Main.rand.NextFloat(1, 2) + Main.rand.NextVector2Circular(.5f, .5f) * (0.5f + 0.5f * percent), c * (1f + 0.15f * Main.rand.NextFloat(percent)), 8).scale = 1f + 0.5f * percent;
                }
            }
			rotation += 15;
			distance -= 0.525f + 0.1f * modPlayer.shardSpellExtra;
			if(distance > 1)
            {
				for(int i = -1; i <= 1; i += 2)
                {
                    Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(4) + circularLocation * i, 0, 0, ModContent.DustType<CopyIceDust>());
                    d.noGravity = true;
                    d.velocity *= 0.05f;
					d.scale = d.scale * 0.2f + 1.0f;
                }

                Vector2 circular = Main.rand.NextVector2CircularEdge(distance + 8, distance + 8) * 1.25f;
                PixelDust.Spawn(Projectile.Center + circular, 0, 0, circular * -Main.rand.NextFloat(0.1f), c, 8).scale = Main.rand.NextFloat(1, 1.25f);
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
                Dust d = Dust.NewDustDirect(atLoc - new Vector2(4), 0, 0, ModContent.DustType<CopyIceDust>());
				d.noGravity = true;
				d.velocity = 0.09f * circularLocation;
                d.scale = 1.1f + 0.2f * size;
			}
			Color c = ColorHelper.PermafrostColor * 0.8f;
			c.A = 0;
            for (int i = 0; i < 15; i++)
			{
				PixelDust.Spawn(atLoc, 0, 0, Main.rand.NextVector2Circular(7, 7), c, 5).scale = Main.rand.NextFloat(1, 1.5f);
			}
			size++;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width), (int)(Projectile.position.Y - Projectile.height), Projectile.width * 3, Projectile.height * 3);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.velocity *= 0.5f; //this is really weird and dumb. not sure I Should change it, though
            target.immune[Projectile.owner] = 6;
        }
	}
}
		