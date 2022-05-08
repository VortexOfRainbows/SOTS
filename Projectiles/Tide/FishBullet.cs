using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Prim.Trails;
using Microsoft.Xna.Framework.Graphics;

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
			projectile.width = 32;
			projectile.height = 32;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 1200;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.netImportant = true;
		}
		int tileCount = 0;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 16;
			height = 16;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(tileCount <= 2)
			{
				SoundEngine.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/DumbFishSound"), 2.0f, -0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
				for (int i = 0; i < 30; i++)
				{
					Vector2 circular = new Vector2(4, 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
					Dust dust = Dust.NewDustPerfect(projectile.Center + circular, 172);
					dust.scale = dust.scale * 0.3f + 1.0f;
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.3f + circular;
					if (Main.rand.NextBool(4))
					{
						circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
						dust = Dust.NewDustPerfect(projectile.Center + circular, DustID.Blood);
						dust.scale = dust.scale * 0.5f + 1.0f;
						dust.velocity = dust.velocity * 0.3f + circular;
					}
				}
				if (oldVelocity.X != projectile.velocity.X)
					projectile.velocity.X = -oldVelocity.X;
				if (oldVelocity.Y != projectile.velocity.Y)
					projectile.velocity.Y = -oldVelocity.Y;
			}
			tileCount++;
            return tileCount > 3;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			int type = (int)projectile.ai[0];
			int frames = 1;
			Texture2D texture = Main.itemTexture[ItemID.Salmon];
			if (type > 0 )
			{
				texture = Main.itemTexture[type];
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
				if (!Main.NPCLoaded[npc])
				{
					Main.instance.LoadNPC(npc);
				}
				texture = Main.npcTexture[npc];
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
				Vector2 circular = new Vector2(0, 4).RotatedBy(k * 1.57f + projectile.rotation + MathHelper.ToRadians(counter * 5f));
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY) + circular,
				frame, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1) + new Vector2(0, projectile.gfxOffY),
			frame, Color.White * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
        }
		int counter;
		bool runOnce = true;
		float accelerate = 0f;
		public override void AI()
		{
			if (runOnce)
			{
				if (projectile.ai[1] == 0)
				{
					for (float j = 0; j <= 1f; j += 1f)
						for (int i = 0; i < 360; i += 5)
						{
							Vector2 circular = new Vector2(8 + 4 * j, 0).RotatedBy(MathHelper.ToRadians(i));
							circular.X *= 0.5f;
							circular = circular.RotatedBy(projectile.velocity.ToRotation());
							Dust dust = Dust.NewDustDirect(projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * (28 + j * 12) + circular - new Vector2(5), 0, 0, 172);
							dust.scale = dust.scale * 0.7f + 1.1f - j * 0.2f;
							dust.noGravity = true;
							dust.velocity = Vector2.Zero + 0.6f * circular;
						}
				}
				SOTS.primitives.CreateTrail(new WaterTrail(projectile, 24));
				runOnce = false;
			}
			projectile.rotation += projectile.direction * MathHelper.ToRadians(14 + accelerate * 12);
			projectile.velocity.Y += accelerate + 0.1f;
			if(accelerate < 0.2f)
				accelerate += 0.0004f;
			for (int i = -1; i <= 1; i += 2)
			{
				if (Main.rand.NextBool(2))
				{
					Vector2 circular = new Vector2((float)Math.Sin(MathHelper.ToRadians(counter * 2.2f)) * 8 * i, 0).RotatedBy(projectile.velocity.ToRotation());
					Dust dust = Dust.NewDustPerfect(projectile.Center + circular, 172);
					dust.scale = dust.scale * 0.3f + 1.0f;
					dust.noGravity = true;
					dust.velocity = dust.velocity * 0.5f + projectile.velocity * i;
				}
			}
		}
        public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/DumbFishSound"), 2.0f, -0.6f + Main.rand.NextFloat(-0.1f, 0.1f));
			for (int i = 0; i < 30; i++)
			{
				Vector2 circular = new Vector2(4, 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
				Dust dust = Dust.NewDustPerfect(projectile.Center + circular, 172);
				dust.scale = dust.scale * 0.3f + 1.0f;
				dust.noGravity = true;
				dust.velocity = dust.velocity * 0.3f + circular;
				if(Main.rand.NextBool(2))
                {
					circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(projectile.velocity.ToRotation() + MathHelper.ToRadians(i * 12));
					dust = Dust.NewDustPerfect(projectile.Center + circular, DustID.Blood);
					dust.scale = dust.scale * 0.6f + 1.0f;
					dust.velocity = dust.velocity * 0.3f + circular;
				}
			}
		}
    }
}
		