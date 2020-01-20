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

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseSpike : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Spike");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 26;
			projectile.width = 26;
			projectile.friendly = false;
			projectile.timeLeft = 7200;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = 5;
			projectile.netImportant = true;
		}
		public override void AI()
		{
			projectile.ai[1]--;
			projectile.alpha -= projectile.alpha > 0 ? 1 : 0;
			projectile.rotation += 0.11f;
			projectile.alpha = projectile.timeLeft <= 255 ? 200 - projectile.timeLeft : projectile.alpha;
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 26, 26, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = projectile.alpha;
			
			int pIndex = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC npc1 = Main.npc[i];
				if(npc1.type == mod.NPCType("PharaohsCurse") && npc1.active)
				{
					pIndex = i;
					break;
				}
			}
			if(pIndex == -1)
			{
				projectile.velocity *= 0.97f;
				projectile.scale *= 0.98f;
				if(projectile.scale < 0.4f)
				{
					projectile.Kill();
				}
				return;
			}
			NPC curse = Main.npc[pIndex];
			
			if(projectile.ai[1] >= 0)
			{
				projectile.ai[0]++;
				Vector2 rotatePos = new Vector2(-64, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
				projectile.position.X = curse.Center.X + rotatePos.X - projectile.width/2;
				projectile.position.Y = curse.Center.Y + rotatePos.Y - projectile.height/2;
				projectile.timeLeft = 2700;
			}
			else if(projectile.ai[1] >= -5)
			{
				Vector2 rotateVelocity = new Vector2(0, -2.4f).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
				projectile.velocity.X = rotateVelocity.X;
				projectile.velocity.Y = rotateVelocity.Y;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		