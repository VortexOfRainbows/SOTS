using System;
using Microsoft.Xna.Framework;
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
			Main.projFrames[Projectile.type] = 6;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.CloneDefaults(393); //pirate
            AIType = 393; 
			Projectile.width = 50;
			Projectile.height = 32;
			Projectile.tileCollide = true;
			DrawOriginOffsetY = 2;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
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
			Player player = Main.player[Projectile.owner];
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			if (Projectile.velocity.X > 0) 
			{
				Projectile.velocity.X += 2.4f;
			}
			if(Projectile.velocity.X < 0) 
			{
				Projectile.velocity.X -= 2.4f;
			}
			if(Math.Abs(Projectile.velocity.X) > 20f)
			{
				Projectile.velocity.X *= 0.5f;
			}
			if(Projectile.velocity.X == 0)
			{
				Projectile.velocity.X += Main.rand.Next(-5,6);
				Projectile.velocity.X *= 1.15f;
			}
		}
		int frame = 0;
		int counter = 0;
		int ai1 = 0;
		public override void AI() 
		{
			Projectile.netUpdate = true;
			float veloX = Projectile.velocity.X;
			float veloY = Projectile.velocity.Y;
			
			if(Math.Abs(veloX) > 20f)
			{
				Projectile.velocity.X *= 0.9f;
			}
			Player player = Main.player[Projectile.owner];
			counter++;
			#region Active check
			if (player.dead || !player.active) 
			{
				player.ClearBuff(Mod.Find<ModBuff>("SquirrelBuff").Type);
			}
			if (player.HasBuff(Mod.Find<ModBuff>("SquirrelBuff").Type))
			{
				Projectile.timeLeft = 2;
			}
			#endregion
			if(Math.Abs(veloX) > 0.3f)
			{
				int frameSpeed = 7;
				counter++;
				if (counter >= frameSpeed) {
					counter = 0;
					frame++;
					if (frame >= Main.projFrames[Projectile.type]) {
						frame = 1;
					}
				}
			}
			else
			{
				frame = 0;
			}
			
			Projectile.frame = frame;
		}	
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = 0;
			return false;
		}
	}
}