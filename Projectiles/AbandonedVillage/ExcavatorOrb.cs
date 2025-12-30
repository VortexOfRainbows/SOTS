using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Boss.Excavator;
using SOTS.Void;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using tModPorter;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class ExcavatorOrb : ModProjectile
	{
        public int Timer = 0;
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            VoidPlayer.VoidBurn(Mod, target, 3, 180);
        }
        public static Color Color => new Color(251, 129, 13, 0);
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = Projectile.width * Projectile.scale;
			float height = Projectile.width * Projectile.scale;
			width += 2;
			height += 2;
			hitbox = new Rectangle((int)(Projectile.Center.X - width/2), (int)(Projectile.Center.Y - height/2), (int)width, (int)height);
        }
        public void DrawTelegraphs()
        {
            float percent = Projectile.ai[1];
            for(int i = 0;i < 3; ++i)
            {
                DrawTelegraph(Projectile.Center + new Vector2(0, -130 * MathF.Sqrt(percent)).RotatedBy(i / 3f * MathHelper.TwoPi * 2f + Projectile.rotation), percent);
            }
        }
        public void DrawTelegraph(Vector2 destination, float completionPercent)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<ExcavatorLightning>()].Value;
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Vector2 toDestination = destination - Projectile.Center;
            float r = toDestination.ToRotation();
            Color col = new Color(200, 200, 200, 0) * completionPercent * completionPercent;
            int c = 24;
            for (int j = -1; j <= 1; j += 2)
            {
                Vector2 prev = Projectile.Center;
                for (int i = 0; i < c; ++i)
                {
                    float percent = (float)i / c;
                    float iPercent = 1 - percent;
                    Vector2 sin = new Vector2(0, MathF.Cos(MathF.PI * percent + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 8)) * 12 * j * iPercent * iPercent * completionPercent).RotatedBy(r);
                    Vector2 pos = Vector2.Lerp(Projectile.Center, destination, percent) + sin;
                    Vector2 toPrev = prev - pos;
                    Main.spriteBatch.Draw(texture, pos - Main.screenPosition, null, col * iPercent, toPrev.ToRotation(), origin, new Vector2(toPrev.Length() / texture.Width * 2f, 0.7f * iPercent), SpriteEffects.None, 0f);
                    prev = pos;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
			Color color = Color * 0.9f;
            color.A = 0;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			for(int i = 0; i < 2; i++)
			{
				SOTS.GodrayShader.Parameters["distance"].SetValue(3);
				SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
				SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
				SOTS.GodrayShader.Parameters["rotation"].SetValue(Projectile.rotation + Projectile.whoAmI + Main.GameUpdateCount * MathHelper.PiOver2 / 90f * (i % 2 * 2 - 1));
				SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f);
				SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale * 0.75f, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            DrawTelegraphs();
			return false;
		}
        public override void PostDraw(Color lightColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			Color color = new(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
            {
				Vector2 circular = new Vector2(4 * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount) );
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * 0.8f, Projectile.rotation, drawOrigin, Projectile.scale * 1.0f, SpriteEffects.None, 0f);
			}
		}
		private bool runOnce = true;
        public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				Projectile.scale = 0;
				Projectile.alpha = 0;
			}
            int target = (int)Projectile.ai[0];
            bool activated = true;
            if (target >= 0)
            {
                NPC owner = Main.npc[target];
                if(owner.ModNPC is Excavator exc && owner.active)
                {
                    Vector2 perceivedVelo = new Vector2(1, 0).RotatedBy(owner.rotation);
                    Projectile.velocity = Vector2.Zero;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + perceivedVelo * 92 * MathF.Min(Timer / 150f, 1), 0.12f);
                    Projectile.rotation += perceivedVelo.X * 0.01f;
                    if (Timer >= 200)
                    {
                        Projectile.velocity = perceivedVelo * 8f;
                        Projectile.ai[0] = -1;
                        Projectile.netUpdate = true;
                        exc.AI2 = -1;
                        owner.netUpdate = true;
                    }
                    //if (exc.InSecondPhase && Projectile.ai[0] == -1)
                    //{
                        //Player p = Main.player[owner.target];
                        //Vector2 toPlayer = p.Center - Projectile.Center;
                        //Projectile.velocity += toPlayer.SNormalize() * 0.5f;
                    //}
                    activated = false;
                }
                else
                {
                    Projectile.ai[0] = -1;
                    return;
                }
            }
            Projectile.rotation += Projectile.velocity.X * 0.01f;
            Projectile.velocity *= 0.9825f;
            if (Timer <= 150)
            {
                float scaleMult = Timer / 150f;
                if (scaleMult > 1)
                    scaleMult = 1;
                Projectile.scale = MathF.Sqrt(scaleMult) * 1.4f;
                if(Timer % 50 == 0)
                {
                    SOTSUtils.PlaySound(SoundID.Item15, Projectile.Center, 1.0f, 0.1f + 0.1f * Timer / 50f);
                }
            }
            Timer++;
            if (Timer >= 110 && activated)
            {
                if(Projectile.ai[2] == 0)
                {
                    float r = Projectile.velocity.ToRotation();
                    SOTSUtils.PlaySound(SoundID.Item92, Projectile.Center, 1.0f, 0);
                    for(int j = 1; j <= 3; ++j)
                    {
                        for (int i = 0; i < 36; ++i)
                        {
                            Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(i / 36f * 360));
                            circular.X *= 0.4f;
                            circular = circular.RotatedBy(r);
                            PixelDust.Spawn(Projectile.Center + circular * (64 - (j * 4)), 0, 0, Main.rand.NextVector2Circular(0.5f, 0.5f) + Projectile.velocity * j, Color, 6).scale = Main.rand.NextFloat(1.0f, 2.0f);
                        }
                    }
                }
                Projectile.tileCollide = true;
                if (Projectile.ai[2] % 20 == 0 && Projectile.ai[2] <= 80)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int j = -1; j <= 1; j += 2)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver2 * j), ModContent.ProjectileType<ExcavatorBolt>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
                        }
                    }
                }
                Projectile.ai[2]++;
            }
            if (Projectile.ai[2] < 270 && Main.rand.NextBool(3))
            {
                Vector2 circular = new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(MathF.PI * 2f));
                float w = Projectile.scale * Projectile.width / 2 + 24;
                PixelDust.Spawn(Projectile.Center + circular * w, 0, 0, Main.rand.NextVector2Circular(2, 2) - circular * 3 + Projectile.velocity, Color, 8).scale = Main.rand.NextFloat(1.0f, 2.0f);
            }
            else if (Main.rand.NextBool(3) && Projectile.ai[2] < 200)
            {
                PixelDust.Spawn(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextVector2Circular(2, 2), Color, 5).scale = Main.rand.NextFloat(Projectile.scale, 1.5f);
            }
            if (Projectile.ai[2] > 0)
            {
                if (Projectile.ai[2] > 270)
                {
                    float percent = (Projectile.ai[2] - 260) / 80f;
                    float sin = MathF.Sin(percent * MathF.PI) * 1.2f - percent + 1;
                    Projectile.scale = MathHelper.Lerp(Projectile.scale, sin, 0.1f);
                }
                else if (Timer > 150)
                    Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.6f, 0.005f);
                if (Projectile.ai[2] < 280)
                {
                    Projectile.ai[1] = Projectile.ai[2] / 280f;
                }
                else if (Projectile.ai[2] == 280)
                {
                    Projectile.ai[1] = 1;
                    SpawnBeams();
                }
                else
                {
                    Projectile.ai[1] *= 0.95f;
                }
                if (Projectile.scale <= 0)
                    Projectile.Kill();
            }
        }
        public void SpawnBeams()
        {
            SOTSUtils.PlaySound(SoundID.Item15, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1f, -0.2f);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int amt = 3;
                float deg = 360f / amt;
                for (int i = 0; i < amt; i++)
                {
                    Vector2 circular = new Vector2(0, -5).RotatedBy(MathHelper.ToRadians(i * deg) + Projectile.rotation);
                    Vector2 target = Projectile.Center + circular * 10;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<ExcavatorLightning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, target.X, target.Y);
                }
            }
            if(Main.expertMode)
            {
                int num = Main.masterMode ? 12 : 8;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int j = 0; j < num; j++)
                    {
                        Vector2 circular = new Vector2(0.5f, 0).RotatedBy(j * MathF.PI * 2f / num);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<ExcavatorBolt>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 2);
                    }
                }
            }
            DoDust(1.75f, -1);
        }
        public override void OnKill(int timeLeft)
        {
            DoDust(0.9f, 1);
        }
        public void DoDust(float scaleFactor = 1f, float dir = 1)
        {
            float offset = Main.rand.NextFloat(MathF.PI);
            float total = 90f;
            for (int i = 0; i < total; i++)
            {
                float percent = i / total;
                float sin = MathF.Sqrt(MathF.Sin(percent * MathHelper.TwoPi * 6) * 0.4f + 0.6f);
                float cos = MathF.Sin(percent * MathHelper.TwoPi * 12) * 0.4f + 0.6f;
                Vector2 circular = new Vector2(4 * sin + cos * 1, 0).RotatedBy(percent * MathHelper.TwoPi * dir + offset) * scaleFactor;
                PixelDust.Spawn(Projectile.Center, 0, 0, circular + Main.rand.NextVector2Circular(0.1f, 0.1f), Color, 3).scale = Main.rand.NextFloat(1.0f, 1.5f);
            }

            for (int i = 0; i < 360; i += 12)
            {
                Vector2 circularLocation = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 0, 0, SOTSUtils.TypeHelper.CopyDust4Type, newColor: Color);
                dust.velocity += circularLocation * scaleFactor;
                dust.noGravity = true;
                dust.alpha = 60;
                dust.fadeIn = 0.1f;
                dust.scale = dust.scale * 0.45f + 1.25f;
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 40;
            height = 40;
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
    public class ExcavatorLightning : FamishedLaser
    {
        public override string Texture => "SOTS/Projectiles/AbandonedVillage/ExcavatorLightning";
        public List<Vector2> offsets;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreAI()
        {
            InitializeLaser();
            if (offsets == null)
            {
                Vector2 start = Projectile.Center;
                Vector2 final = FinalPosition;
                offsets = new List<Vector2>();
                float dist = Vector2.Distance(start, final) / 16f;
                for(int i = 0; i < dist; ++i)
                {
                    offsets.Add(Main.rand.NextVector2Circular(12, 12));
                }
            }
            Projectile.ai[2] += 5;
            Projectile.ai[2] *= 1.01f;
            if (Projectile.ai[2] < 70)
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1.5f, 0.15f);
            }
            else
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.3f, 0.03f);
            if (Projectile.timeLeft < 50)
                Projectile.hostile = false;
            return true;
        }
        public override void InitializeLaser()
        {
            if (!HasInit)
            {
                Projectile.scale = 0;
                PlaySound();
            }
            int splitPower = (int)Projectile.ai[2];
            float spread = 60f;
            int totalIterations = 240;
            if (splitPower == 1)
            {
                totalIterations = 160;
                spread = 45f;
            }
            if (splitPower == 2)
                totalIterations = 120;
            float dustScaleMult = 1.5f;
            Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 startingPosition = Projectile.Center;
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
            bool ableToHitBlocks = false;
            for (int b = 0; b < totalIterations; b++)
            {
                startingPosition += Projectile.velocity * 4f;
                FinalPosition = startingPosition;
                int i = (int)startingPosition.X / 16;
                int j = (int)startingPosition.Y / 16;
                if (!ableToHitBlocks)
                    ableToHitBlocks = FinalPosition.Distance(Projectile.Center) > 80 && FinalPosition.Distance(Projectile.Center) > destination.Distance(Projectile.Center);
                if (ableToHitBlocks && SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
                    break;
                }
                if(b == 44 && !HasInit && splitPower < 2)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            Vector2 circular = Projectile.velocity.RotatedBy(MathHelper.ToRadians(k * spread));
                            Vector2 target = Projectile.Center + circular * 10;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), startingPosition, circular, ModContent.ProjectileType<ExcavatorLightning>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, target.X, target.Y, splitPower + 1);
                        }
                    }
                }
                if (!HasInit && Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(FinalPosition - new Vector2(16, 16), 24, 24, ModContent.DustType<PixelDust>(), 0, 0, 0, color * Percent * 1.5f, Main.rand.NextFloat(1.0f, 1.5f));
                    dust.noGravity = true;
                    dust.velocity = dust.velocity * 1.0f * Percent * dustScaleMult + Projectile.velocity * Main.rand.NextFloat(6f, 8f) * Percent;
                    dust.fadeIn = 7;
                }
            }
            for (int i = 2; i > 0; i--)
            {
                Dust dust = Dust.NewDustDirect(FinalPosition - new Vector2(10, 10), 12, 12, SOTSUtils.TypeHelper.CopyDust4Type, 0, 0, 0, color * Percent * Percent * 1.5f, 1.4f);
                dust.noGravity = true;
                dust.velocity = dust.velocity * 0.2f * (5 - Percent * 4) + Projectile.velocity * Main.rand.NextFloat(0.1f, 2.0f);
                if (i == 2)
                    dust.velocity += new Vector2(0, -Main.rand.NextFloat(0.1f, 0.3f));
                dust.fadeIn = 0.2f;
            }
            HasInit = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            VoidPlayer.VoidDamage(Mod, target, 5);
        }
        public override void PlaySound() => SOTSUtils.PlaySound(SoundID.Item92, Projectile.Center, 0.7f, -0.25f); // SOTSUtils.PlaySound(SoundID.Item42, Projectile.Center, 0.7f, -0.25f);
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 toEnd = FinalPosition - Projectile.Center;
            float dist = toEnd.Length();
            toEnd = toEnd.SNormalize();
            for (int i = 0; i < dist; i += 16)
            {
                Vector2 position = Projectile.Center + toEnd * i;
                Rectangle hitbox = new Rectangle((int)position.X - 6, (int)position.Y - 6, 12, 12);
                if (hitbox.Intersects(targetHitbox))
                    return true;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!HasInit)
                return false;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(0, texture.Height / 2);
            float rotation = Projectile.velocity.ToRotation();
            Vector2 start = Projectile.Center;
            Vector2 final = FinalPosition;
            Vector2 toEnd = (final - start).SNormalize();
            float dist = Vector2.Distance(start, final);
            Color color = new Color(255, 255, 80, 0);
            Vector2 prevPosition = Projectile.Center;
            float scale = 0.1f;
            float frequency = 16;
            dist /= frequency;
            int maxK = SOTS.Config.lowFidelityMode ? 1 : 2;
            for (int i = 1; i < dist; ++i)
            {
                if(scale < 1)
                    scale += 0.1f;
                Vector2 position = Projectile.Center + toEnd * i * frequency;
                if(i < dist - 2)
                {
                    position += offsets[i % offsets.Count];
                }
                Vector2 toPrev = prevPosition - position;
                float rot = toPrev.ToRotation();
                float stretch = (toPrev.Length()) / texture.Width;
                float yScale = (scale * Percent + 0.75f * Percent) * Projectile.scale;
                for(float j = 0; j < 2.4f; j += 0.75f)
                {
                    for(int k = 0; k < maxK; ++k)
                    {
                        Main.spriteBatch.Draw(texture, position - Main.screenPosition + toPrev * j, null, color * (Percent - k * 0.5f), rot, origin,
                            new Vector2(stretch * 1.1f, (yScale * 0.5f - j * 0.25f - k * 0.1f)), SpriteEffects.None, 0f);
                    }
                }
                prevPosition = position;
            }
            return false;
        }
    }
}