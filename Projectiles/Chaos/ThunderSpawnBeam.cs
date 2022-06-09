using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Common.GlobalNPCs;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class ThunderSpawnBeam : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thunder Beam");
		}
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24; 
            Projectile.timeLeft = 40;
            Projectile.penetrate = -1; 
            Projectile.friendly = false; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        List<Vector2> desinationList = new List<Vector2>();
        bool runOnce = true;
        public const float Speed = 6f;
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return false;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(140, 140, 140, 0);
            float endPercent = Projectile.timeLeft / 40f;
            float rotation = Projectile.velocity.ToRotation();
            int max = drawPositionList.Count;
            int progress = 0;
            for (int i = 0; i < max; i++)
            {
                float scale = Projectile.scale;
                Vector2 drawPos = drawPositionList[i];
                Color otherC = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3), false);
                otherC.A = 0;
                Vector2 sinusoid = new Vector2(0, 16 * scale * scale * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 6 + i * 2.5f))).RotatedBy(rotation);
                //spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - Projectile.alpha) / 255f), rotation, origin, new Vector2(1, scale * 1f) * Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f) * 0.5f, rotation, origin, new Vector2(2f, scale * 1f), SpriteEffects.None, 0f);
                if (i != drawPositionList.Count - 1)
                    rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
                progress++;
            }
            return false;
        }
        public void SetupLaser()
        {
            Vector2 finalDestination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 playerCenter = Main.player[(int)Projectile.knockBack].Center;
            for (int i = 0; i < 4; i++)
            {
                Vector2 destinationToPlayer = playerCenter - finalDestination;
                Vector2 startToPlayer = playerCenter - Projectile.Center;
                float r = destinationToPlayer.ToRotation();
                float x = startToPlayer.ToRotation() - r;
                x = MathHelper.WrapAngle(x);
                float lerpedAngle = MathHelper.Lerp(x, 0, (i + 1) / 5f);
                lerpedAngle += r;
                float maxLength = MathHelper.Lerp(startToPlayer.Length(), destinationToPlayer.Length(), (i + 1) / 5f);
                Vector2 circular = new Vector2(-maxLength * Main.rand.NextFloat(0.25f, 0.5f), 0).RotatedBy(lerpedAngle);
                Vector2 fromPlayer = playerCenter + circular;
                desinationList.Add(fromPlayer);
            }
            //Projectile.velocity = Vector2.Zero;
            float radians = (float)Projectile.velocity.ToRotation();
            Vector2 position = Projectile.Center;
            Vector2 velocity = Projectile.velocity.SafeNormalize(new Vector2 (0, 1)) * Speed;
            bool end = false;
            int counter = 0;
            while(!end)
            {
                position += velocity;
                drawPositionList.Add(position);
                if (Main.rand.NextBool(30))
                {
                    Dust dust2 = Dust.NewDustPerfect(position, ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f), true);
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                bool continueToFinal = true;
                if (desinationList.Count > 0)
                {
                    Vector2 goTo = desinationList[0];
                    Rectangle hitbox = new Rectangle((int)position.X - Projectile.width / 2, (int)position.Y - Projectile.height / 2, Projectile.width, Projectile.height);
                    if (hitbox.Contains(goTo.ToPoint()))
                    {
                        desinationList.RemoveAt(0);
                    }
                    else
                    {
                        redirectGrowth = 0;
                        radians = Redirect(radians, position, goTo);
                        continueToFinal = false;
                    }
                }
                if (continueToFinal)
                {
                    Rectangle hitbox = new Rectangle((int)position.X - Projectile.width / 2, (int)position.Y - Projectile.height / 2, Projectile.width, Projectile.height);
                    if (hitbox.Contains(finalDestination.ToPoint()))
                    {
                        end = true;
                    }
                    radians = Redirect(radians, position, finalDestination);
                }
                velocity = new Vector2(1, 0).RotatedBy(radians) * Speed;
                counter++;
            }
            //Projectile.velocity = velocity;
            Projectile.Center = position;
        }
        float redirectGrowth = 0.0f;
        public float Redirect(float radians, Vector2 pos, Vector2 npc)
        {
            Vector2 toNPC = npc - pos;
            float speed = Speed / 4f + redirectGrowth;
            Vector2 rnVelo = new Vector2(Speed, 0).RotatedBy(radians);
            rnVelo += toNPC.SafeNormalize(Vector2.Zero) * speed;
            float npcRad = rnVelo.ToRotation();
            redirectGrowth += 0.1f;
            return npcRad;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                for (int i = 0; i < 4; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18), true);
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                SetupLaser();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -Projectile.velocity, ModContent.ProjectileType<ThunderBall>(), Projectile.damage, 0, Main.myPlayer, 0, 0);
                //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 92, 0.6f, 0.4f);
                for (int i = 0; i < 4; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18), true);
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                runOnce = false;
            }
            float endPercent = Projectile.timeLeft / 40f;
            Projectile.alpha = (int)(255 - 235 * endPercent * endPercent);
            Projectile.scale = endPercent;
        }
	}
}
		
			