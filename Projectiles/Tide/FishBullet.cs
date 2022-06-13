using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Prim.Trails;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace SOTS.Projectiles.Tide
{    
    public class FishBullet : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fish");
		}
        public override void SetDefaults()
        {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.netImportant = true;
		}
		int tileCount = 0;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 16;
			height = 16;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(tileCount <= 2)
			{
				SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/DumbFishSound"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 2.0f, -0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				for (int i = 0; i < 30; i++)
				{
					Vector2 circular = new Vector2(4, 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
					Dust dust = Dust.NewDustPerfect(Projectile.Center + circular, 172);
					dust.scale = dust.scale * 0.3f + 1.0f;
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.3f + circular;
					if (Main.rand.NextBool(4))
					{
						circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
						dust = Dust.NewDustPerfect(Projectile.Center + circular, DustID.Blood);
						dust.scale = dust.scale * 0.5f + 1.0f;
						dust.velocity = dust.velocity * 0.3f + circular;
					}
				}
				if (oldVelocity.X != Projectile.velocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (oldVelocity.Y != Projectile.velocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
			}
			tileCount++;
            return tileCount > 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
			int type = (int)Projectile.ai[0];
			int frames = 1;
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[ItemID.Salmon].Value;
			if (type > 0 )
			{
				texture = Terraria.GameContent.TextureAssets.Item[type].Value;
			}
			else
			{
				int npc = 0;
				if (type == -6)
					npc = NPCID.GreenJellyfish;
				if (type == -5)
					npc = NPCID.PinkJellyfish;
				if (type == -4)
					npc = NPCID.BlueJellyfish;
				if (type == -3)
					npc = NPCID.Shark;
				if (type == -2)
					npc = NPCID.Crab;
				if (type == -1)
					npc = NPCID.Squid;
				if (!TextureAssets.Npc[npc].IsLoaded)
				{
					Main.instance.LoadNPC(npc);
				}
				texture = Terraria.GameContent.TextureAssets.Npc[npc].Value;
				frames = Main.npcFrameCount[npc];
			}
			float scale = 1.1f;
			if (type == -3)
				scale = 0.9f;
			Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / frames);
			Color color = new Color(0, 0, 255, 0);
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / frames / 2);
			for (int k = 0; k < 4; k++)
			{
				Vector2 circular = new Vector2(0, 4).RotatedBy(k * 1.57f + Projectile.rotation + MathHelper.ToRadians(counter * 5f));
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) + circular,
				frame, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1) + new Vector2(0, Projectile.gfxOffY),
			frame, Color.White * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
        }
		int counter;
		bool runOnce = true;
		float accelerate = 0f;
		public override void AI()
		{
			if (runOnce)
			{
				if (Projectile.ai[1] == 0)
				{
					for (float j = 0; j <= 1f; j += 1f)
						for (int i = 0; i < 360; i += 5)
						{
							Vector2 circular = new Vector2(8 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
							circular.X *= 0.5f;
							circular = circular.RotatedBy(Projectile.velocity.ToRotation());
							Dust dust = Dust.NewDustDirect(Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (28 + j * 12) + circular - new Vector2(5), 0, 0, 172);
							dust.scale = dust.scale * 0.7f + 1.1f - j * 0.2f;
							dust.noGravity = true;
							dust.velocity = Vector2.Zero + 0.6f * circular;
						}
				}
				SOTS.primitives.CreateTrail(new WaterTrail(Projectile, 24));
				runOnce = false;
			}
			Projectile.rotation += Projectile.direction * MathHelper.ToRadians(14 + accelerate * 12);
			Projectile.velocity.Y += accelerate + 0.1f;
			if(accelerate < 0.2f)
				accelerate += 0.0004f;
			for (int i = -1; i <= 1; i += 2)
			{
				if (Main.rand.NextBool(2))
				{
					Vector2 circular = new Vector2((float)Math.Sin(MathHelper.ToRadians(counter * 2.2f)) * 8 * i, 0).RotatedBy(Projectile.velocity.ToRotation());
					Dust dust = Dust.NewDustPerfect(Projectile.Center + circular, 172);
					dust.scale = dust.scale * 0.3f + 1.0f;
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.5f + Projectile.velocity * i;
				}
			}
		}
        public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/DumbFishSound"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 2.0f, -0.6f + Main.rand.NextFloat(-0.1f, 0.1f));
			for (int i = 0; i < 30; i++)
			{
				Vector2 circular = new Vector2(4, 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
				Dust dust = Dust.NewDustPerfect(Projectile.Center + circular, 172);
				dust.scale = dust.scale * 0.3f + 1.0f;
				dust.noGravity = true;
				dust.velocity = dust.velocity * 0.3f + circular;
				if(Main.rand.NextBool(2))
                {
					circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
					dust = Dust.NewDustPerfect(Projectile.Center + circular, DustID.Blood);
					dust.scale = dust.scale * 0.6f + 1.0f;
					dust.velocity = dust.velocity * 0.3f + circular;
				}
			}
		}
    }
}
		