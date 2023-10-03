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
using SOTS.Projectiles.Base;

namespace SOTS.Projectiles.Crushers
{    
    public class HealLock : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Heal Lock");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 43;
			Projectile.width = 43;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = false;
			Projectile.alpha = 255;
		}
		public override void AI()
		{
			Vector2 circularLocation = new Vector2(Projectile.velocity.X -distance, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			Projectile.velocity *= 0.98f;
			Projectile.scale *= 0.98f;
			Projectile.alpha++;
			
			Player player  = Main.player[Projectile.owner];
			
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RedTorch);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			Main.dust[num1].scale = 1.5f;
		}
		public override void OnKill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				int heal = 0;
				if(Main.rand.NextBool(5)) heal = 1;
				if(Main.rand.NextBool(20)) heal = 2;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 0, 0, player.whoAmI, (int)Projectile.ai[0] + heal, 1);
			}
			for(int i = 5; i > 0; i --)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, DustID.RedTorch);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.5f;
				Main.dust[num1].scale = 1.5f;
			}
		}
	}
}
		