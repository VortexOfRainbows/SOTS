using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using System.Collections.Generic;
using SOTS.Dusts;

namespace SOTS.Projectiles.Inferno
{    
    public class BlazingMine : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Trap");
		}
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34; 
            Projectile.timeLeft = 1200;
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
			width = 12;
			height = 12;
			fallThrough = false;
			return true;
		}
		public override void AI()
        {
			if (Projectile.velocity.Length() > 1)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				if (Main.rand.NextBool(4))
				{
					Dust dust = Main.dust[Dust.NewDust(Projectile.Center - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>())];
					dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.2f;
					dust.velocity *= 0.3f;
					dust.alpha = 125;
				}
			}
			if (Projectile.velocity.Length() != 0 && Projectile.ai[0] == -1)
			{
				Projectile.velocity *= 0.97f;
				Projectile.velocity.Y += 0.1f;
			}
			else
				Projectile.velocity *= 0.95f;
			Lighting.AddLight(Projectile.Center, VoidPlayer.InfernoColorAttemptDegrees(SOTSWorld.GlobalCounter * 2).ToVector3());
			Projectile.alpha = 200 - (int)(200f * Projectile.timeLeft / 1200f);
        }
		public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, 0.5f);
			for (int i = 20; i > 0; i--)
			{
				Vector2 circular = new Vector2(16, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				int dust2 = Dust.NewDust(Projectile.Center - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(0.5f));
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.8f;
				dust.velocity = dust.velocity * 0.1f - circular * 0.1f * Main.rand.NextFloat(2);
				dust.alpha = 125;
			}
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = 8; i > 0; i--)
                {
					Vector2 circular = new Vector2(4 + Projectile.velocity.Length() + (Projectile.ai[0] == -1 ? 2.5f : 0), 0).RotatedBy(MathHelper.PiOver4 * i);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + circular.SafeNormalize(Vector2.Zero) * 12, circular, ModContent.ProjectileType<BlazingSpike>(), (int)(Projectile.damage * 0.1f), Projectile.knockBack / 2, Main.myPlayer, Projectile.ai[0]);
				}
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
		
			