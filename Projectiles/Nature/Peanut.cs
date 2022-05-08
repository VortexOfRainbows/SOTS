using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Nature
{    
    public class Peanut : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut");
		}
        public override void SetDefaults()
        {
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.width = 26;
			projectile.height = 26;
			projectile.timeLeft = 3000;
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.ignoreWater = false;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 12;
			height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool PreAI()
		{
			if (projectile.velocity.X > 0)
			{
				projectile.rotation -= MathHelper.Pi;
				projectile.spriteDirection = -1;
			}
			else
			{
				projectile.spriteDirection = 1;
			}
			projectile.rotation = projectile.velocity.ToRotation() + (float)MathHelper.Pi / 4 * projectile.spriteDirection;
			if (projectile.velocity.X < 0)
			{
				projectile.rotation -= MathHelper.Pi;
			}
			return base.PreAI();
        }
        public override void AI()
		{
			projectile.velocity.Y += 0.09f;
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
				SoundEngine.PlaySound(SoundID.Dig, projectile.Center);
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), 26, 26, 0);
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), 26, 26, 7);
			}
			if(Main.myPlayer == projectile.owner)
			{
				Vector2 location = new Vector2(projectile.Center.X, projectile.Center.Y - 480);
				Vector2 fromLocation = location + new Vector2(1920 * (Main.rand.Next(2) * 2 - 1), Main.rand.NextFloat(-128, 128));
				Projectile.NewProjectile(fromLocation, Vector2.Zero, ModContent.ProjectileType<PinkyBomber>(), projectile.damage, projectile.knockBack, Main.myPlayer, location.X, location.Y);
			}
		}
	}
}
		