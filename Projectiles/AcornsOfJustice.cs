using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class AcornsOfJustice : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acorns Of Justice");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.width = 30;
			projectile.height = 30;
            projectile.magic = true;
			projectile.penetrate = 1;
			projectile.ranged = false;
			projectile.alpha = 0; 
			projectile.friendly = true;
		}
		public override void AI()
        {
			projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
			projectile.spriteDirection = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 5; i++)
				{
				int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("GrowTree"), projectile.damage, projectile.knockBack, player.whoAmI, 0, 12);
				Main.projectile[Probe].ai[0] = 1;
				Main.projectile[Probe].rotation = (float)Math.Atan2((double)oldVelocity.Y, (double)oldVelocity.X) + MathHelper.ToRadians(72 * i);
				Main.projectile[Probe].spriteDirection = 1;
				Main.projectile[Probe].frame = 3;
				}
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < 5; i++)
				{
				int Probe = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("GrowTree"), projectile.damage, projectile.knockBack, player.whoAmI, 0, 12);
				Main.projectile[Probe].ai[0] = 1;
				Main.projectile[Probe].rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(72 * i);
				Main.projectile[Probe].spriteDirection = 1;
				Main.projectile[Probe].frame = 3;
				}
            }
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 24; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}