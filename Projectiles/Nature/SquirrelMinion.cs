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

namespace SOTS.Projectiles.Nature
{
	public class SquirrelMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Squirrel");
			Main.projFrames[projectile.type] = 6;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.CloneDefaults(393); //pirate
            aiType = 393; 
			projectile.width = 50;
			projectile.height = 32;
			projectile.tileCollide = true;
			drawOriginOffsetY = 2;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override bool MinionContactDamage()
		{
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			if (projectile.velocity.X > 0) 
			{
				projectile.velocity.X += 2.4f;
			}
			if(projectile.velocity.X < 0) 
			{
				projectile.velocity.X -= 2.4f;
			}
			if(Math.Abs(projectile.velocity.X) > 20f)
			{
				projectile.velocity.X *= 0.5f;
			}
			if(projectile.velocity.X == 0)
			{
				projectile.velocity.X += Main.rand.Next(-5,6);
				projectile.velocity.X *= 1.15f;
			}
		}
		int frame = 0;
		int counter = 0;
		int ai1 = 0;
		public override void AI() 
		{
			projectile.netUpdate = true;
			float veloX = projectile.velocity.X;
			float veloY = projectile.velocity.Y;
			
			if(Math.Abs(veloX) > 20f)
			{
				projectile.velocity.X *= 0.9f;
			}
			Player player = Main.player[projectile.owner];
			counter++;
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(mod.BuffType("SquirrelBuff"));
			}
			if (player.HasBuff(mod.BuffType("SquirrelBuff")))
			{
				projectile.timeLeft = 2;
			}
			#endregion
			if(Math.Abs(veloX) > 0.3f)
			{
				int frameSpeed = 7;
				counter++;
				if (counter >= frameSpeed) {
					counter = 0;
					frame++;
					if (frame >= Main.projFrames[projectile.type]) {
						frame = 1;
					}
				}
			}
			else
			{
				frame = 0;
			}
			
			projectile.frame = frame;
		}	
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.ai[0] = 0;
			return false;
		}
	}
}