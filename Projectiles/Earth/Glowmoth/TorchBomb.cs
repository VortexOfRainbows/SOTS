using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Earth.Glowmoth 
{    
    public class TorchBomb : ModProjectile 
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22; 
            Projectile.timeLeft = 90;
            Projectile.penetrate = 1; 
            Projectile.friendly = false; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false; 
			Projectile.alpha = 0;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			fallThrough = false;
			return true;
		}
		public override void AI()
		{
			if (!Main.rand.NextBool(3))
			{
				Vector2 fuse = new Vector2(12, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4) + fuse, 0, 0, ModContent.DustType<AlphaDrainDust>());
				Color color2 = Color.Lerp(Color.Orange, Color.Blue, Main.rand.NextFloat(1f));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.4f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.2f;
			}
			Projectile.velocity.Y += 0.08f;
			Projectile.velocity.X *= 0.99f;
			Projectile.rotation += Projectile.velocity.X * 0.03f;
		}
		public override void Kill(int timeLeft)
		{
			if(Projectile.owner == Main.myPlayer)
			{
				for (int i = 0; i < 16; i++)
				{
					Vector2 circular = new Vector2(5 + i % 2, 0).RotatedBy(MathHelper.ToRadians(i * 360f / 16));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<TorchEmber>(), 20, 2, Main.myPlayer);
				}
			}
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.3f, -0.1f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if(Projectile.oldVelocity.Y != Projectile.velocity.Y)
            {
				Projectile.velocity.Y = Projectile.oldVelocity.Y * -0.7f;
            }
			return false;
		}
	}
	public class TorchBombWood : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 20;
			Projectile.timeLeft = 35;
			Projectile.penetrate = 1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.alpha = 0;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 16;
			height = 16;
			fallThrough = false;
			return true;
		}
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item61, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, -0.1f);
				runOnce = false;
			}
			if (!Main.rand.NextBool(3))
			{
				Vector2 fuse = new Vector2(0, -10).RotatedBy(Projectile.rotation);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4) + fuse, 0, 0, ModContent.DustType<AlphaDrainDust>());
				Color color2 = Color.Lerp(Color.Orange, Color.Blue, Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.4f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.2f;
			}
			Projectile.velocity.Y += 0.07f;
			Projectile.velocity.X *= 0.99f;
			Projectile.rotation += Projectile.velocity.X * 0.03f;
		}
		public override void Kill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = -2; i <= 2; i++)
				{
					Vector2 circular = new Vector2(5.5f, 0).RotatedBy(MathHelper.ToRadians(i * 10) + Projectile.velocity.ToRotation());
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<TorchEmber>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
				}
			}
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.3f, -0.1f);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.oldVelocity.Y != Projectile.velocity.Y)
			{
				Projectile.velocity.Y = Projectile.oldVelocity.Y * -0.7f;
			}
			return false;
		}
	}
	public class TorchEmber : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.timeLeft = 360;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
			Projectile.alpha = 0;
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
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
			Projectile.ai[0]++;
            if (Projectile.ai[0] >= 30)
            {
				Projectile.ai[0] -= 50;
				PlaceTile();
			}
			Projectile.velocity.Y += 0.018f;
			if (!Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<AlphaDrainDust>());
				Color color2 = Color.Lerp(Color.Orange, Color.Blue, Main.rand.NextFloat(1f));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.4f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.05f;
			}
		}
		public void PlaceTile()
		{
			int i = (int)Projectile.Center.X / 16;
			int j = (int)Projectile.Center.Y / 16;
			if (Projectile.owner == Main.myPlayer)
			{
				Player player = Main.player[Projectile.owner];
				if(!Framing.GetTileSafely(i, j).HasTile)
				{
					bool hasPlaceTorch = WorldGen.PlaceTile(i, j, TileID.Torches, false, false, Projectile.owner, player.BiomeTorchPlaceStyle(0));
					if(!hasPlaceTorch)
					{
						for (int k = -1; k <= 1; k++)
						{
							for (int h = -1; h <= 1; h++)
							{
								int i2 = i + k;
								int j2 = j + h; 
								if (!Framing.GetTileSafely(i2, j2).HasTile)
									hasPlaceTorch = WorldGen.PlaceTile(i2, j2, TileID.Torches, false, false, Projectile.owner, player.BiomeTorchPlaceStyle(0));
								if(hasPlaceTorch)
								{
									NetMessage.SendTileSquare(Projectile.owner, i2, j2);
									break;
								}
							}
							if (hasPlaceTorch)
								break;
						}
					}
					else
						NetMessage.SendTileSquare(Projectile.owner, i, j);
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 14; i++)
			{
				Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.TwoPi * i / 14);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<AlphaDrainDust>());
				Color color2 = Color.Lerp(Color.Orange, Color.Blue, Main.rand.NextFloat(1f));
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.6f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.3f;
				dust.velocity += circular * Main.rand.NextFloat(0.5f, 1.1f);
			}
			PlaceTile();
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(BuffID.OnFire, 600); //adds 10 seconds of fire
        }
    }
}
		
			