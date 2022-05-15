using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class AcornsOfJustice : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acorns Of Justice");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 1;
			Projectile.width = 30;
			Projectile.height = 30;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
		}
		public override void AI()
        {
			Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
			Projectile.spriteDirection = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				for (int i = -2; i <= 2; i++)
				{
					int Probe = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<GrowTree>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 1, MathHelper.ToRadians(i * 4.4f));
					Main.projectile[Probe].rotation = oldVelocity.ToRotation() + MathHelper.ToRadians(90 - i * 8f);
					Main.projectile[Probe].spriteDirection = 1;
					Main.projectile[Probe].frame = 3;
				}
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				for (int i = -2; i <= 2; i++)
				{
					int Probe = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<GrowTree>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 1, MathHelper.ToRadians(i * 4.4f));
					Main.projectile[Probe].rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90 - i * 8f);
					Main.projectile[Probe].spriteDirection = 1;
					Main.projectile[Probe].frame = 3;
				}
			}
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 24; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 2);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}