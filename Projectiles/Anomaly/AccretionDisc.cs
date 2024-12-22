using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Void;
using System;
using SOTS.Helpers;
using SOTS.Dusts;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using SOTS.Projectiles.AbandonedVillage;
using SOTS.Projectiles.Tide;
using SOTS.Common.GlobalNPCs;
using Microsoft.Build.Construction;

namespace SOTS.Projectiles.Anomaly
{    
    public class AccretionDisc : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
		public override void SendExtraAI(BinaryWriter writer)
		{

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{

		}
        public override void SetDefaults()
        {
			Projectile.height = 50;
            Projectile.width = 50;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
			Projectile.timeLeft = 1400;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			//Projectile.netUpdate = true;
			//Projectile.tileCollide = false;
			//Projectile.ai[0] = -1;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 28;
			height = 28;
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			modifiers.HitDirectionOverride = SOTSUtils.SignNoZero(Projectile.Center.X - target.Center.X);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{

		}
		private bool runOnce = true;
		private int initialDirection;
		private Vector2 initialVelo;
		private float rotateSpeed = 11f;
        public override bool PreAI()
        {
			Player player = Main.player[Projectile.owner];
			if (runOnce)
			{
				runOnce = false;
				initialVelo = Projectile.velocity;
				initialDirection = SOTSUtils.SignNoZero(Projectile.velocity.X);
			}
            if (Projectile.ai[0] < 0)
            {
                Projectile.tileCollide = false;
                Projectile.friendly = false;
                if (Projectile.timeLeft > 50)
                    Projectile.timeLeft = 50;
                return true;
            }
            else
            {
                Color c = Color.Lerp(ColorHelper.VoidAnomalyBlue, ColorHelper.VoidAnomalyPink, Main.rand.NextFloat()) * 0.75f;
                c.A = 0;
                for (int j = -1; j <= 1; j += 2)
                {
                    if(Main.rand.NextBool(4))
                    {
                        Vector2 offset = new Vector2(j * initialDirection, j).RotatedBy(Projectile.rotation);
                        Dust dust = PixelDust.Spawn(Projectile.Center + offset * 16, 0, 0, offset * Main.rand.NextFloat(4.5f, 6.5f) + Main.rand.NextVector2Circular(0.1f, 0.1f) + Projectile.velocity * Main.rand.NextFloat(0.1f, 0.2f), c, 8);
                        dust.scale = Main.rand.NextFloat(1, 2);
                    }
                }
                if (Main.rand.NextBool(2))
                {
                    if (Projectile.ai[1] < 70 || Projectile.ai[1] > 430)
                    {
                        Dust dust = PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Circular(1.2f, 1.2f) + Projectile.velocity * Main.rand.NextFloat(), c, 7);
                        dust.scale = Main.rand.NextFloat(1.5f, 2.5f);
                    }
                }
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] < 70)
            {
                Projectile.velocity *= 0.976f;
                if (Projectile.ai[1] >= 30 && Projectile.owner == Main.myPlayer)
                {
                    if (Projectile.ai[2] >= 0)
                    {
                        Projectile.ai[2] = -1;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AccretionSingularity>(), Projectile.damage, Projectile.knockBack * 0.5f, Main.myPlayer, ai2: Projectile.identity);
                    }
                }
            }
			else if (Projectile.ai[1] < 430)
			{
				Projectile.velocity *= 0.88f;
			}
			else
			{
                Projectile.tileCollide = false;
				Vector2 toPlayer = player.Center - Projectile.Center;
				Projectile.velocity *= 0.78f;
				float speed = 2 + (Projectile.ai[1] - 210f) / 30f;
				float dist = toPlayer.Length();
                if (speed > dist)
				{
					speed = dist;
                }
				if (dist < 16)
				{
					Projectile.ai[0] = -1;
					Projectile.netUpdate = true;
				}
                Projectile.velocity += toPlayer.SNormalize() * 0.7f * speed;
            }
            return true;
        }
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.rotation += initialDirection * MathHelper.ToRadians(rotateSpeed);
            Projectile.spriteDirection = initialDirection;
			Projectile.ai[1]++;
			if(Projectile.timeLeft > 50)
				Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 1.8f / 255f);
        }
        public override void OnKill(int timeLeft)
		{
			base.OnKill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return true;
            Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture2.Width / 2, texture2.Height / 4);
            Rectangle first = new Rectangle(0, 0, texture2.Width, texture2.Height / 2);
            Rectangle second = new Rectangle(0, texture2.Height / 2, texture2.Width, texture2.Height / 2);
            float timeLeftMult = MathF.Min(Projectile.timeLeft / 50f, 1);
            int min = Math.Max(0, 50 - Projectile.timeLeft);
            float expansion = Math.Min(1, Projectile.ai[1] / 70f);
            //for (int i = min; i < Projectile.oldPos.Length; i++)
            //{
            //    if (Projectile.oldPos[i] == Vector2.Zero)
            //        break;
            //    float perc = 1 - i / (float)Projectile.oldPos.Length;
            //    Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
            //    Main.spriteBatch.Draw(texture2, center - Main.screenPosition, second, Projectile.GetAlpha(Color.Gray) * perc, Projectile.velocity.X * 0.05f, origin, Projectile.scale * perc,
            //        Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            //}
            for (int a = 0; a <= 2; a++)
            {
                int j = a == 0 ? 0 : a == 1 ? 1 : -1;
                Vector2 previous = Projectile.Center;
                for (int i = min; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero)
                        break;
                    float dist = 16 + (i - min) * 0.75f * expansion;
                    float perc = 1 - i / (float)Projectile.oldPos.Length;
                    Vector2 offset = new Vector2(dist * j * initialDirection, dist * j).RotatedBy(Projectile.rotation + MathHelper.ToRadians(rotateSpeed * -i * initialDirection));
                    Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2 + offset;
                    if (i != min)
                    {
                        float scale = 17f * perc;
                        Vector2 toPrev = previous - center;
                        Color c = Color.Lerp(ColorHelper.VoidAnomalyBlue, ColorHelper.VoidAnomalyPink, perc);
                        if (a != 0)
                        {
                            c.A = 0;
                            scale = 1f + perc;
                        }
                        float length = toPrev.Length() / 2f;
                        Main.spriteBatch.Draw(SOTSUtils.WhitePixel, center - Main.screenPosition, null, c * perc * timeLeftMult * 0.9f, toPrev.ToRotation(), 
							new Vector2(0, 1), new Vector2(length, scale), SpriteEffects.None, 0f);
                        if(a == 0)
                        {
                            c = Color.Black;
                            Main.spriteBatch.Draw(SOTSUtils.WhitePixel, center - Main.screenPosition, null, c * perc * timeLeftMult * 1.2f, toPrev.ToRotation(),
                                new Vector2(0, 1), new Vector2(length, scale * 0.8f), SpriteEffects.None, 0f);
                        }
                    }
                    previous = center;
                }
            }
			if(Projectile.timeLeft > 50)
            {
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, second, Projectile.GetAlpha(Color.White), Projectile.velocity.X * 0.05f, origin, Projectile.scale,
                    Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, first, Projectile.GetAlpha(Color.White), Projectile.rotation, origin, Projectile.scale,
                    Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return null;
        }
    }
    public class AccretionSingularity : ModProjectile
    {
        public static Texture2D Blue = null;
        public static Texture2D Pink = null;
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            DrawSingularity(0);
            DrawSingularity(1);
            float scale = MathF.Min(1, Projectile.ai[1] / 40f);
            for(int i = 0; i < 6; i++)
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, i / 12f * MathF.PI, texture.Size() / 2, scale, SpriteEffects.None, 0f);
            DrawSingularity(2);
            return false;
        }
        public void DrawSingularity(int style)
        {
            Texture2D Black = style == 3 ? null : Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (Blue == null && style != 3)
                Blue = ModContent.Request<Texture2D>("SOTS/Projectiles/Anomaly/AccretionSingularityBlue", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            if (Pink == null && style != 3)
                Pink = ModContent.Request<Texture2D>("SOTS/Projectiles/Anomaly/AccretionSingularityPink", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Color c = style == 3 ? ColorHelper.VoidAnomalyPink : new Color(100, 100, 100, 0);
            c.A = 0;
            Vector2 origin = style == 3 ? Vector2.Zero : style == 0 ? new Vector2(0, Black.Height / 2) : new Vector2(0, Blue.Height / 2);
            Vector2 one = new Vector2(1, 0);
            Vector2 prev = Projectile.Center;
            float cutoff = -0.1f;
            float scaleB = MathF.Min(1, Projectile.ai[1] / 40f);
            int count = style == 3 ? 30 : SOTS.Config.lowFidelityMode ? 46 : 61;
            for (int i = 0; i < count; i++)
            {
                float rad = i / ((float)count - 1) * 2 * MathF.PI + Projectile.ai[0] * .075f;
                rad *= Projectile.direction;
                float sin = MathF.Sin(MathHelper.ToRadians(i / (float)(count - 1) * 2700f));
                Vector2 circular = one.RotatedBy(rad);
                float yCompress = 0.6f + Projectile.ai[0] / 600f;
                circular.Y *= yCompress;
                float scale = 1 + circular.Y * 0.5f;
                bool skip = (circular.Y < cutoff && style == 2) || (circular.Y > cutoff && style == 1);
                circular = circular.RotatedBy(MathHelper.ToRadians(15 * -Projectile.direction * MathF.Sin(MathHelper.ToRadians(Projectile.ai[0] * 3))));
                circular *= Projectile.ai[1] + (style == 0 ? 4 : 0) + sin * 3;
                if(style == 3)
                {
                    if(Main.rand.NextBool(14))
                    {
                        float expansionRate = (1 - Projectile.ai[0] / 50f);
                        Vector2 spin = circular - prev;
                        Dust d = PixelDust.Spawn(Projectile.Center + circular * 1.05f, 0, 0, spin * 0.5f + 1.5f * circular.SNormalize() * expansionRate, c, Main.rand.Next(4, 7));
                        d.scale = Main.rand.NextFloat(1, 2);
                    }
                }
                else if (!skip || style == 0)
                {
                    Vector2 toPrev = prev - circular;
                    if (i != 0)
                    {
                        Vector2 toCenter = -circular;
                        if(style == 0)
                        {
                            Main.spriteBatch.Draw(Black, Projectile.Center + circular - Main.screenPosition, null, Color.White * scaleB, (-circular).ToRotation(), origin, new Vector2(circular.Length() / Black.Width, 1 * scale), SpriteEffects.None, 0f);
                        }
                        else
                        {
                            Main.spriteBatch.Draw(Pink, Projectile.Center + circular - Main.screenPosition, null, c * 1.2f, toPrev.ToRotation(), origin, new Vector2(toPrev.Length() / Blue.Width * 3, scale), SpriteEffects.None, 0f);
                            Main.spriteBatch.Draw(Blue, Projectile.Center + circular - Main.screenPosition, null, c * 0.5f, (-circular).ToRotation(), origin, new Vector2((circular.Length() - 20f * (1 - yCompress * MathF.Sin(rad))) / Pink.Width, 1 * scale), SpriteEffects.None, 0f);

                        }
                    }
                }
                prev = circular;
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.timeLeft = 2000618;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<VoidRanged>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
        }
        public override bool PreAI()
        {
            Projectile parent = null;
            for (short i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Projectile.owner && proj.identity == (int)Projectile.ai[2])
                {
                    parent = proj;
                    break;
                }
            }
            if (parent != null)
            {
                Projectile.Center = parent.Center + new Vector2(0, 8);
            }
            DrawSingularity(3);
            Projectile.velocity *= 0.94f;
            if (Projectile.ai[0] == 0)
                SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1.5f, -0.3f);
            if (Projectile.ai[0] == 29)
                SOTSUtils.PlaySound(SoundID.Item67, Projectile.Center, 1.2f, -0.4f);
            if (Projectile.ai[0] >= 45 && Projectile.ai[0] % 15 == 0 && Projectile.ai[0] <= 90)
                SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1.5f, 1f);
            if (Projectile.ai[0] == 100)
                SOTSUtils.PlaySound(SoundID.Item105, Projectile.Center, 1.2f, 0.4f);
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 60)
            {
                float exp = 0.6f + Projectile.ai[0] / 24f;
                Projectile.ai[1] = MathF.Min(Projectile.ai[1] + exp * exp, 120);
            }
            else
            {
                float exp = 0.2f + (Projectile.ai[0] - 80f) / 20f;
                Projectile.ai[1] -= exp * exp;
                if (Projectile.ai[1] < 0)
                    Projectile.Kill();
            }
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Item62, Projectile.Center, 1.2f, -0.3f);
            int c = SOTS.Config.lowFidelityMode ? 5 : 4;
            for (int i = 0; i < 360; i += c)
            {
                float rand = Main.rand.NextFloat();
                Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4) + Main.rand.NextFloat(4 * rand) + 8 * rand, 0).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 5, Projectile.Center.Y - 5), 0, 0, ModContent.DustType<PixelDust>());
                dust.velocity = circularLocation;
                dust.color = Color.Lerp(ColorHelper.VoidAnomalyPink, ColorHelper.VoidAnomalyBlue, Main.rand.NextFloat());
                dust.color.A = (byte)Main.rand.Next(100);
                dust.noGravity = true;
                dust.fadeIn = 3f;
                dust.scale = dust.scale * 1.5f + (1 - rand) * 2.25f;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                for (int i = 0; i < 16; i++)
                {
                    Vector2 velocity = new Vector2(1, 0).RotatedBy(i / 8f * MathF.PI);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity * (2 + i % 2) * Main.rand.NextFloat(0.9f, 1.1f), ModContent.ProjectileType<AccretionNova>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(), -1f);
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float distX = MathF.Min(MathF.Abs(targetHitbox.Right - projHitbox.Left), MathF.Abs(targetHitbox.Left - projHitbox.Right));
            float distY = MathF.Min(MathF.Abs(targetHitbox.Top - projHitbox.Bottom), MathF.Abs(targetHitbox.Bottom - projHitbox.Top)) * 1.3f;
            float dist = MathF.Sqrt(distX * distX + distY * distY);
            return dist < 10 + Projectile.ai[1] * 0.9f;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.Defense *= 0f;
        }
    }
    public class AccretionNova : ModProjectile
    {
        private Color myColor => Color.Lerp(ColorHelper.VoidAnomalyBlue, ColorHelper.VoidAnomalyPink, Projectile.ai[0] % 1f);
        public override string Texture => "SOTS/Projectiles/Anomaly/AccretionDisc";
        public override bool PreDraw(ref Color lightColor)
        {
            Color c = Projectile.GetAlpha(myColor) * 2f;
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
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, c * perc, rot, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                previous = center;
            }
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
            Projectile.timeLeft = 240;
            Projectile.penetrate = -1;
            Projectile.alpha = 0;
            Projectile.localNPCHitCooldown = 40;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 1.6f / 255f);
            int spawn = SOTS.Config.lowFidelityMode ? 5 : 4;
            if(Main.rand.NextBool(spawn))
            {
                Color c = myColor;
                c.A = (byte)Main.rand.Next(256);
                PixelDust.Spawn(Projectile.Center, 0, 0, Main.rand.NextVector2Circular(1, 1) * 0.3f, c, 6).scale = Main.rand.NextFloat(.75f, 1.25f);
            }
            Projectile.velocity.Y += 0.01f;
            Projectile.velocity *= 1.0045f;
            Projectile.alpha++;
            if (Projectile.numUpdates <= 0)
            {
                int target = SOTSNPCs.FindTarget_WithLos(Projectile.Center, out float _, 8, 640, Projectile, (int)Projectile.ai[1]);
                if (target != -1)
                {
                    NPC npc = Main.npc[target];
                    Vector2 toNPC = npc.Center - Projectile.Center;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity.SNormalize(), toNPC.SNormalize(), 0.1f).SNormalize() * Projectile.velocity.Length();
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[1] = target.whoAmI;
            Projectile.netUpdate = true;
        }
        public override void OnKill(int timeLeft)
        {
            Color c = myColor * 0.5f;
            c.A = 0;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Dust d = Dust.NewDustDirect(center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), newColor: c * perc);
                d.velocity *= 0.25f * perc;
                d.velocity += Projectile.oldVelocity * 0.75f;
                d.noGravity = true;
                d.fadeIn = 0.2f;
                d.scale = d.scale * 0.4f + 0.8f * perc;
                d.color.A = 0;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X * 0.9f;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            return false;
        }
    }
}
