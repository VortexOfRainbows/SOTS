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

namespace SOTS.Projectiles.Ores
{    
    public class SoulLock : ModProjectile 
    {	float distance = 30f;  
		int rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Lock");
			
		}
		
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(263);
            AIType = 263; 
			Projectile.height = 2;
			Projectile.width = 2;
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
			if((int)Projectile.ai[0] != -1)
			{
				NPC npc = Main.npc[(int)Projectile.ai[0]];
				if(!npc.friendly && npc.lifeMax > 5 && npc.active)
				{
					Projectile.position.X = npc.Center.X - 1;
					Projectile.position.Y = npc.Center.Y - 1;
				}
				else
				{
					Projectile.Kill();
				}
			}
			Vector2 circularLocation = new Vector2(Projectile.velocity.X -distance, Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(rotation));
			rotation += 15;
			distance -= 0.5f;
			Projectile.scale *= 0.98f;
			Projectile.alpha++;
			
			Player player  = Main.player[Projectile.owner];
			
			
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 16);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			if(Projectile.timeLeft <= 2)
			{
				Projectile.friendly = true;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 0;
			int heal = 1;
			if(Main.rand.NextBool(4)) heal = 2;
			
			if(Main.rand.NextBool(16)) heal = 3;
			
			if(player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 0, 0, player.whoAmI, heal, 0);	
			}
        }
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			for(int i = 5; i > 0; i --)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X , Projectile.position.Y), Projectile.width, Projectile.height, 16);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 1.5f;
			}
		}
	}
}
		