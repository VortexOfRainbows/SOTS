using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles 
{    
    public class TridentFlare : ModProjectile 
    {	int enemybore = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TridentFlare");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(1);
            aiType = 1; 
			projectile.penetrate = -1;
			projectile.width = 26;
			projectile.height = 32;


		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			
				target.AddBuff(24, 360, false);
		Projectile.NewProjectile(projectile.Center.X + (projectile.velocity.Y * 4), projectile.Center.Y - (projectile.velocity.X * 4), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 85, projectile.damage, projectile.knockBack, 0);
		Projectile.NewProjectile(projectile.Center.X - (projectile.velocity.Y * 4), projectile.Center.Y + (projectile.velocity.X * 4), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 85, projectile.damage, projectile.knockBack, 0);
		Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 85, projectile.damage, projectile.knockBack, 0);

		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += (int)(target.defense * 0.5f);
			damage += 10;
		}
		public override void AI()
		{
			for(int i = 5; i > 0; i--)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 26, 32, 6);
			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			}
			
		}
	}
}
		
			