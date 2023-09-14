using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Curse;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace SOTS.Projectiles.Tide
{    
    public class HydroBubble : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{

		}
        public override void SetDefaults()
        {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 0; 
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 3000;
			Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	

		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		public bool RunOnce = true;
		public float AiCounter = 0f;
        public float TargetWidth = -1f;
        public float ChargeTime = 36f;
        public override void AI()
        {
            if (AiCounter > ChargeTime - 12f)
            {
                if (RunOnce)
                {
                    SOTSUtils.PlaySound(SoundID.Item80, Projectile.Center, 1.0f, 0.3f);
                    RunOnce = false;
                }
            }
            else if (AiCounter == 0)
            {
                SOTSUtils.PlaySound(SoundID.Item21, Projectile.Center, 1.2f, -0.5f);
            }
			if((int)Projectile.ai[1] != -1 && Projectile.owner == Main.myPlayer)
			{
				NPC target = Main.npc[(int)Projectile.ai[1]];
				if(target.active && !target.friendly)
				{
					Projectile.Center = target.Center;

                    TargetWidth = target.width;
                    if (target.height > target.width)
                        TargetWidth = target.height;
                    TargetWidth *= 0.95f;
                    if (AiCounter > ChargeTime)
                    {
                        Projectile.friendly = false;
                        if (AiCounter % 15 == 0)
                        {
                            Projectile.friendly = true;
                            if (Projectile.ai[0] > 0)
                            {
                                Vector2 circular = new Vector2(0, (int)Math.Sqrt(target.width * target.height) * 0.36f + 56).RotatedBy(Main.rand.NextFloat(-2.6f, 2.6f));
                                Vector2 spawnLoc = circular + Projectile.Center + target.velocity * 0.5f;
                                Vector2 velocity = -circular * 0.21f + target.velocity * 0.1f;
                                for (float i = 0; i < 1; i += 0.02f)
                                {
                                    float sin = (float)Math.Sin(i * MathHelper.Pi);
                                    float scaleMult = 0.5f + 0.8f * sin;
                                    WaterParticle.NewWaterParticle(spawnLoc + velocity * 0.1f * i, new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.06f, 0.06f)) + velocity * i, 1.1f * scaleMult);
                                }
                                Projectile.ai[0]--;
                            }
                            else
                            {
                                Projectile.Kill();
                            }
                        }
                    }
                    else
                    {
                        float encirclementProgress = AiCounter / ChargeTime;
                        encirclementProgress *= encirclementProgress * encirclementProgress * encirclementProgress;
                        float progress = encirclementProgress * 1f;
                        if (progress > 1f)
                        {
                            progress = 1f;
                        }
                        for (int j = -1; j <= 1; j += 2)
                        {
                            Vector2 circular = new Vector2(0, TargetWidth * 1.0f).RotatedBy(progress / 2f * MathHelper.TwoPi * j);
                            WaterParticle.NewWaterParticle(Projectile.Center + circular, Main.rand.NextVector2Circular(2, 2), Main.rand.NextFloat(1.0f, 1.2f));
                        }
                    }
                }
				else
				{
					Projectile.Kill();
				}
			}
			if(Projectile.ai[1] == -1)
            {
                Projectile.Kill();
            }
			AiCounter++;

		}
		public override void Kill(int timeLeft)
        {
            float popMultiplier = 2.5f;
            for (int j = 0; j < TargetWidth * popMultiplier; j++)
            {
                Vector2 circular = new Vector2(0, TargetWidth * 1.0f).RotatedBy(j / TargetWidth / popMultiplier * MathHelper.TwoPi);
                WaterParticle.NewWaterParticle(Projectile.Center + circular, circular.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.2f, 3.4f) + Main.rand.NextVector2Circular(0.4f, 0.4f), Main.rand.NextFloat(1.4f, 1.6f));
            }
            SOTSUtils.PlaySound(SoundID.Item89, Projectile.Center, 1.15f, 0.45f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public void Draw(bool outLine)
        {
            if(TargetWidth <= 0f)
            {
                return;
            }
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D outline = ModContent.Request<Texture2D>("SOTS/Projectiles/Tide/HydroBubbleOutline").Value;
            Vector2 origin = outline.Size() / 2;
            float encirclementProgress = AiCounter / ChargeTime;
            encirclementProgress *= encirclementProgress * encirclementProgress * encirclementProgress;
            float progress = encirclementProgress * 1f;
            if(progress > 1f)
            {
                progress = 1f;
            }
            float iterateAmount = 0.8f / TargetWidth;
            for(int j = -1; j <= 1; j += 2)
            {
                float start = 0;
                if(j == -1)
                {
                    start = -iterateAmount;
                }
                bool terminate = false;
                float i = start;
                while (!terminate)
                {
                    float sinusoid = (float)Math.Sin(SOTSWorld.GlobalCounter / 90f * MathHelper.TwoPi);
                    float cosinusoid = (float)Math.Cos(i * MathHelper.TwoPi * 6f + SOTSWorld.GlobalCounter / 240f * MathHelper.TwoPi) * sinusoid;
                    float pickSmaller = TargetWidth * 0.1f;
                    if(pickSmaller > 6f)
                    {
                        pickSmaller = 6;
                    }
                    Vector2 circular = new Vector2(0, TargetWidth + pickSmaller * cosinusoid).RotatedBy(i / 2f * MathHelper.TwoPi);
                    if (outLine)
                    {
                        Main.spriteBatch.Draw(outline, Projectile.Center - Main.screenPosition + circular, null, Color.White, Projectile.rotation, origin, 0.75f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + circular, null, Color.White, Projectile.rotation, origin, 0.6f, SpriteEffects.None, 0f);
                    }
                    if (j == -1)
                    {
                        if(i < -progress)
                            terminate = true;
                    }
                    else if(i > progress)
                        terminate = true;
                    i += iterateAmount * j;
                }
            }
        }
    }
}