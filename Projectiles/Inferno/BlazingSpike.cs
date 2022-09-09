using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using System.Collections.Generic;
using SOTS.Dusts;

namespace SOTS.Projectiles.Inferno
{    
    public class BlazingSpike : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Spike");
		}
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 22; 
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false; 
            Projectile.DamageType = DamageClass.Melee; 
			Projectile.alpha = 0;
			Projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCsAndTiles.Add(index);
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			fallThrough = false;
			return true;
		}
		public override void AI()
		{
			if (Projectile.velocity.Length() != 0 && Projectile.ai[0] == -1)
			{
				Projectile.velocity *= 0.98f;
				Projectile.velocity.Y += 0.1f;
			}
			else
				Projectile.velocity *= 0.96f;
			if (Projectile.velocity.Length() > 1)
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, VoidPlayer.InfernoColorAttemptDegrees(SOTSWorld.GlobalCounter * 2).ToVector3() * 0.3f);
			Projectile.alpha = 220 - (int)(220f * Projectile.timeLeft / 300f);
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 5; i > 0; i--)
			{
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				int dust2 = Dust.NewDust(Projectile.Center - new Vector2(8, 8), 8, 8, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(0.5f));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.1f;
				dust.velocity = dust.velocity * 0.1f - circular * 0.1f * Main.rand.NextFloat(2);
				dust.alpha = 125;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.velocity.X = 0;
			Projectile.velocity.Y = 0;
			return false;
		}
	}
}
		
			