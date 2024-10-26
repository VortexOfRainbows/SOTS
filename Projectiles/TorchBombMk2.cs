using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using System;
using SOTS.Projectiles.Earth.Glowmoth;
using SOTS.Void;
using SOTS.Common.GlobalNPCs;

namespace SOTS.Projectiles
{    
    public class TorchBombMk2 : ModProjectile 
    {
		public float PercentComplete => Projectile.timeLeft / (float)StartingTimeLeft;
        public override bool PreDraw(ref Color lightColor)
        {
            float scaler = Projectile.ai[2] * 0.15f + 1f;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, 1);
            Vector2 previous = Projectile.Center;
			Color glow = new Color(100, 100, 100, 0) * (1.0f - 0.95f * PercentComplete);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                float dist = toPrev.Length();
                if (dist > 1600)
                    break;
                float sinusoid = MathF.Sin(MathHelper.ToRadians(i * 12 + Projectile.ai[1] * -2f));
                float rot = toPrev.ToRotation();
                Vector2 stretch = new Vector2(dist / texture.Width * 1.05f, perc * 3f * scaler);
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector2 helix = new Vector2(0, 12 * sinusoid * MathF.Sin(perc * MathF.PI) * j * perc).RotatedBy(rot);
                    Main.EntitySpriteDraw(texture, center + helix - Main.screenPosition, null, ColorHelper.InfernoColorGradient(perc) * perc * PercentComplete, rot, drawOrigin, new Vector2(stretch.X, stretch.Y * 0.75f), SpriteEffects.FlipVertically, 0f);
                }
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, ColorHelper.InfernoColorGradient(perc) * perc * PercentComplete, rot, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                previous = center;
            }
            texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            drawOrigin = texture.Size() /  2;
            for(int j = Projectile.tileCollide ? 1 : 0; j < 2; j++)
            {
                float dir = j * 2 - 1;
                float aMult = j == 0 ? 0.5f : 1f;
                Vector2 pos = j == 0 ? new Vector2(Projectile.ai[0], Projectile.ai[1]) : Projectile.Center;
                for (int i = 0; i < 6; i++)
                {
                    float rot = i * MathHelper.Pi / 3f - MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2f * dir);
                    Vector2 circular = new Vector2(20f * PercentComplete + 4f, 0).RotatedBy(rot) * scaler;
                    Main.EntitySpriteDraw(texture, circular + pos - Main.screenPosition, null, glow * aMult, Projectile.rotation + rot, drawOrigin, Projectile.scale * scaler, SpriteEffects.None, 0f);
                }
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale * scaler, SpriteEffects.None, 0f);
            return false;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
		{
			Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
			Projectile.width = Projectile.height = 20; 
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1; 
            Projectile.friendly = false; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false; 
			Projectile.alpha = 0;
		}
		private bool RunOnce = true;
        private float Gravity = 0.1f;
        private int StartingTimeLeft = 90;
		public override void AI()
        {
			if(RunOnce)
			{
                if (Projectile.ai[2] == 1)
                {
                    StartingTimeLeft = 70;
                }
                else if (Projectile.ai[2] == 0)
                {
                    StartingTimeLeft = 50;
                }
                else
                {
                    SOTSUtils.PlaySound(SoundID.Item61, Projectile.Center, 1, -0.2f);
                }
                Projectile.timeLeft = StartingTimeLeft;
                Vector2 targetPosition = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Vector2 toTargetPosition = targetPosition - Projectile.Center;
                float dist = toTargetPosition.Length();
                Gravity = 0.04f + dist * 0.0001f;
				Projectile.velocity = toTargetPosition / StartingTimeLeft;
				Projectile.velocity.Y -= Gravity * StartingTimeLeft * 0.5f;
				RunOnce = false;
            }
            else
                Projectile.velocity.Y += Gravity;
            for (float i = 0; i < 1; i += 0.5f)
            {
                Color c = ColorHelper.InfernoColorGradient(Main.rand.NextFloat(0.5f, 1.0f)) * (1f - 0.5f * PercentComplete);
                c.A = 0;
				Vector2 fuse = new Vector2(-12 * PercentComplete, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4);
                PixelDust.Spawn(Projectile.Center + fuse + Projectile.velocity * i, 0, 0, Main.rand.NextVector2Circular(0.5f, 0.5f) * (1 - PercentComplete), c, 8).scale = 1.0f + .5f * (1 - PercentComplete);
                Projectile.rotation += Projectile.velocity.X * 0.014f;
            }
        }
        public override void OnKill(int timeLeft)
		{
			if(Projectile.owner == Main.myPlayer)
			{
                Vector2 bonusVelo = Vector2.Zero;
                float speed = 5;
				int count = 16;
				Projectile.velocity.Y -= Gravity * StartingTimeLeft * 0.5f;
                if (Projectile.ai[2] == 1)
                {
                    count = 12;
                    speed = 3f;
                    bonusVelo = Projectile.velocity.SNormalize() * 5f;
                }
                else if (Projectile.ai[2] == 0)
                {
                    count = 8;
                    speed = 3f;
                    bonusVelo = Projectile.velocity.SNormalize() * 1.5f;
                }
                for (int i = 0; i < count; i++)
                {
                    Vector2 circular = new Vector2(speed + i % 2, 0).RotatedBy(MathHelper.ToRadians(i * 360f / count));
                    if (Projectile.ai[2] != 2)
                        circular += Main.rand.NextVector2Circular(1, 1);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular + bonusVelo, ModContent.ProjectileType<TorchEmberMk2>(), Projectile.damage, 2, Main.myPlayer, 0, 3 - Projectile.ai[2]);
				}
                count = 1;
                if (Projectile.ai[2] == 2)
                    count = 3;
                else if (Projectile.ai[2] == 1)
                    count = 2;
                if (Projectile.ai[2] > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(200, 200);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Type, Projectile.damage, 2, Main.myPlayer, position.X, position.Y, Projectile.ai[2] - 1);
                    }
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Vector2 circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.TwoPi * i / 15f);
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<AlphaDrainDust>(), newColor: ColorHelper.InfernoColorGradient(Main.rand.NextFloat(0.5f, 1.0f)));
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale = 1.6f;
                dust.alpha = Projectile.alpha;
                dust.velocity *= 0.3f;
                dust.velocity += circular * Main.rand.NextFloat(0.5f, 1.1f) + Projectile.oldVelocity * 0.3f;
            }
            SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.3f, -0.1f);
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 12;
            height = 12;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
    }
	public class TorchEmberMk2 : TorchEmber
	{
		public override string Texture => "SOTS/Projectiles/TorchBombMk2";
        public override bool PreDraw(ref Color lightColor)
        {
            float scaler = 1f;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, 1);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                float dist = toPrev.Length();
                if (dist > 1600)
                    break;
                float rot = toPrev.ToRotation();
                Vector2 stretch = new Vector2(dist / texture.Width, perc * 2f * scaler);
                Color color = ColorHelper.InfernoColorGradient(perc);
                color.A = 0;
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, color * perc, rot, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                previous = center;
            }
            texture = ModContent.Request<Texture2D>("SOTS/Dusts/CopyDust4").Value;
            Color c = ColorHelper.InfernoColorGradient(.4f) * 1.0f;
            c.A = 0;
            for(int i = 0; i < 5; i++)
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 8, 8), c, MathHelper.TwoPi * i / 5f, new Vector2(4), 0.75f, SpriteEffects.FlipVertically, 0f);
            return false;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
			Projectile.width = Projectile.height = 16;
			Projectile.timeLeft = 360;
			Projectile.penetrate = 5;
			Projectile.alpha = 0;
			Projectile.localNPCHitCooldown = 40;
			Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
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

			if (Projectile.ai[1] > 0)
            {
                int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 320, Projectile);
                if (target != -1)
                {
					NPC npc = Main.npc[target];
					Vector2 toNPC = npc.Center - Projectile.Center;
					toNPC = toNPC.SNormalize() * (0.07f + 0.005f * Projectile.ai[1]);
					float prevSpeed = Projectile.velocity.Length();
					Projectile.velocity += toNPC;
					Projectile.velocity = Projectile.velocity.SNormalize() * prevSpeed;
                }
            }
            if(Main.rand.NextBool(6))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4), 0, 0, ModContent.DustType<AlphaDrainDust>(), newColor: ColorHelper.InfernoColorGradient(Main.rand.NextFloat(0.5f, 1.0f)));
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale = 1.4f;
                dust.alpha = Projectile.alpha;
                dust.velocity = dust.velocity * 0.1f + Projectile.velocity * 1.5f;
            }
		}
		public override void OnKill(int timeLeft)
		{
			int count = Math.Min(360 - timeLeft, 16);
            for (int i = 0; i < count; i++)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.TwoPi * i / count);
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<AlphaDrainDust>(), newColor: ColorHelper.InfernoColorGradient(Main.rand.NextFloat(0.5f, 1.0f)));
                dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.6f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.3f;
				dust.velocity += circular * Main.rand.NextFloat(0.5f, 1.1f) + Projectile.oldVelocity * 0.3f;
            }
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Color c = ColorHelper.InfernoColorGradient(perc) * perc;
                c.A = 0;
                PixelDust.Spawn(center, 0, 0, Main.rand.NextVector2Circular(0.3f, 0.3f) * perc + Projectile.oldVelocity * 0.4f, c, 7).scale = 0.5f + perc;
            }
            PlaceTile();
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire3, 600);
        }
    }
}
		
			