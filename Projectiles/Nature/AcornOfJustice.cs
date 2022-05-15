using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class AcornOfJustice : ModProjectile 
    {	
        public override void SetDefaults()
        {
			Projectile.aiStyle = 1;
			Projectile.width = 20;
			Projectile.height = 20;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
		}
		public override void AI()
        {
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - MathHelper.ToRadians(125);
			Projectile.spriteDirection = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				int Probe = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<GrowTree>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 1);
				Main.projectile[Probe].rotation = (float)Math.Atan2((double)oldVelocity.Y, (double)oldVelocity.X) + MathHelper.ToRadians(90);
				Main.projectile[Probe].spriteDirection = 1;
				Main.projectile[Probe].frame = 3;
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				int Probe = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<GrowTree>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 1);
				Main.projectile[Probe].rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(90);
				Main.projectile[Probe].spriteDirection = 1;
				Main.projectile[Probe].frame = 3;
            }
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 2);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}