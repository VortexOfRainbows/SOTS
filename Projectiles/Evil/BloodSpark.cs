using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Evil
{    
    public class BloodSpark : ModProjectile 
    {
		bool runOnce = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Spark");
		}
        public override void SetDefaults()
        {
			Projectile.height = 96;
			Projectile.width = 96;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.timeLeft = 5;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = Projectile.ai[1] == -1;
        }
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void AI()
        {
			if(runOnce)
			{
				//Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62, 0.4f, -0.1f);
				float randomDirection = Projectile.ai[0];
				for(int i = 20; i < 340; i += 10)
				{
					Vector2 circularLocation = new Vector2(-5, 0).RotatedBy(MathHelper.ToRadians(randomDirection + i));
					bool blood = Main.rand.NextBool(5);
					int type = DustID.Torch;
					if (blood)
					{
						type = DustID.Blood;
					}
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, type);
					float speedMult = 1f;
					if (blood)
					{
						dust.noGravity = false;
						dust.scale = 1.6f + Main.rand.NextFloat(-0.1f, 0.1f); 
						speedMult = 0.5f;
					}
					else
					{
						dust.noGravity = true;
						dust.scale = 2.0f + Main.rand.NextFloat(-0.1f, 0.1f);
					}
					dust.velocity *= 0.2f;
					dust.velocity += circularLocation * 0.5f * Main.rand.NextFloat(0.9f, 1.1f) * speedMult;
				}
				for(int j = -1; j <= 1; j += 2)
                {
					for(int i = 0; i < 10; i++)
					{
						Vector2 circularLocation = new Vector2(-6 * j, 0).RotatedBy(MathHelper.ToRadians(randomDirection));
						bool blood = Main.rand.NextBool(5);
						int type = DustID.Torch;
						if (blood)
						{
							type = DustID.Blood;
						}
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, type);
						float speedMult = 1f;
						if (blood)
						{
							dust.noGravity = false;
							dust.scale = 2f + Main.rand.NextFloat(-0.1f, 0.1f);
							speedMult = 0.5f;
						}
						else
						{
							dust.noGravity = true;
							dust.scale = 2.4f + Main.rand.NextFloat(-0.1f, 0.1f);
						}
						dust.velocity *= 0.2f;
						dust.velocity += circularLocation * 0.1f * (i + 0.2f) * speedMult;
					}
                }
				runOnce = false;
			}
        }
	}
}