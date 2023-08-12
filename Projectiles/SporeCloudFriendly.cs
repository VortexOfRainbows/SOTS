using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace SOTS.Projectiles
{    
    public class SporeCloudFriendly : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spore Cloud");
		}
        public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 38;
			Projectile.timeLeft = 70;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.scale = 1.01f;
			Projectile.alpha = 45;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Confused, 120, false);
			Projectile.damage = (int)(Projectile.damage * 0.75f);
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 32;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        bool runOnce = true;
		float randMod = 1f;
		public override bool PreAI()
		{
			if(runOnce)
			{
				randMod = Main.rand.NextFloat(0.6f, 1.1f);
				Projectile.scale = 0.1f * randMod;
				runOnce = false;
			}
			Projectile.alpha += 3;
			Projectile.velocity *= 0.95f - 0.05f * randMod;
			Projectile.rotation += (Projectile.velocity.X + Projectile.direction) * 0.1f / randMod;
			if(Projectile.scale < 1 * randMod)
				Projectile.scale += 0.02f * randMod;
			if (Main.rand.NextBool(140))
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 41, 0, 0, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			return true;
		}
	}
}
		