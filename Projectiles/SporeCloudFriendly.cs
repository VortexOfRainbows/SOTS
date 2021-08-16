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
			DisplayName.SetDefault("Spore Cloud");
		}
        public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 38;
			projectile.timeLeft = 70;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.scale = 1.01f;
			projectile.alpha = 45;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ranged = true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(10))
				target.AddBuff(BuffID.Confused, 120, false);
			projectile.damage = (int)(projectile.damage * 0.75f);
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 32;
			hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width / 2, width, width);
        }
        bool runOnce = true;
		float randMod = 1f;
		public override bool PreAI()
		{
			if(runOnce)
			{
				randMod = Main.rand.NextFloat(0.6f, 1.1f);
				projectile.scale = 0.1f * randMod;
				runOnce = false;
			}
			projectile.alpha += 3;
			projectile.velocity *= 0.95f - 0.05f * randMod;
			projectile.rotation += (projectile.velocity.X + projectile.direction) * 0.1f / randMod;
			if(projectile.scale < 1 * randMod)
				projectile.scale += 0.02f * randMod;
			if (Main.rand.NextBool(140))
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 41, 0, 0, 250, new Color(100, 100, 100, 250), 0.8f);
			}
			return true;
		}
	}
}
		