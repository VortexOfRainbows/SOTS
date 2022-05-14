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
			Projectile.height = 26;
			Projectile.width = 26;
			Projectile.friendly = false;
			Projectile.timeLeft = 7200;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 5;
			Projectile.netImportant = true;
		}
		public override void AI()
		{
			Projectile.ai[1]--;
			Projectile.alpha -= Projectile.alpha > 0 ? 1 : 0;
			Projectile.rotation += 0.11f;
			Projectile.alpha = Projectile.timeLeft <= 255 ? 200 - Projectile.timeLeft : Projectile.alpha;
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 26, 26, ModContent.DustType<CurseDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = Projectile.alpha;
			
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
				Projectile.velocity *= 0.97f;
				Projectile.scale *= 0.98f;
				if(Projectile.scale < 0.4f)
				{
					Projectile.Kill();
				}
				return;
			}
			NPC curse = Main.npc[pIndex];
			
			if(Projectile.ai[1] >= 0)
			{
				Projectile.ai[0]++;
				Vector2 rotatePos = new Vector2(-64, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
				Projectile.position.X = curse.Center.X + rotatePos.X - Projectile.width/2;
				Projectile.position.Y = curse.Center.Y + rotatePos.Y - Projectile.height/2;
				Projectile.timeLeft = 2700;
			}
			else if(Projectile.ai[1] >= -5)
			{
				Vector2 rotateVelocity = new Vector2(0, -2.4f).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
				Projectile.velocity.X = rotateVelocity.X;
				Projectile.velocity.Y = rotateVelocity.Y;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		