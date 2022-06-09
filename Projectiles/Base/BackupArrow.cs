using Microsoft.Xna.Framework;
using SOTS.Common.GlobalNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
 
namespace SOTS.Projectiles.Base
{    
    public class BackupArrow : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Backup Arrow");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.VortexBeaterRocket);
            AIType = ProjectileID.VortexBeaterRocket; 
            Projectile.width = 18;
            Projectile.height = 22; 
            Projectile.timeLeft = 1800;
            Projectile.penetrate = 1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Ranged; 
		}
		int bounceCounter = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (Main.myPlayer == Projectile.owner)
				DebuffNPC.SetTimeFreeze(Main.player[Projectile.owner], target, 60);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			bounceCounter++;
			if (bounceCounter > 3)
				return true;
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = -oldVelocity.Y;
			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return false;
		}
	}
}
			