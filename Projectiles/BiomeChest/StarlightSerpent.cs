using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using System.Collections.Generic;

namespace SOTS.Projectiles.BiomeChest
{
	public class StarlightSerpentHead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Serpent");
			Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = false;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}
        List<Vector2> segments = new List<Vector2>();
        List<float> segmentsRotation = new List<float>();
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            target.immune[projectile.owner] = 6;
		}
		public sealed override void SetDefaults()
        {
            projectile.width = 24;
			projectile.height = 24;
			projectile.penetrate = -1;
			projectile.timeLeft *= 5;
			projectile.minion = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
            projectile.hide = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1f;
		}
        public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 first = projectile.Center;
            spriteBatch.Draw(texture, first - Main.screenPosition, null, (Color)GetAlpha(lightColor), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.05f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            for (int i = 0; i < segments.Count; i++)
            {
                Vector2 toOther = first - segments[i];
                if(segmentsRotation[i] != 0)
                {
                    var num1090 = MathHelper.WrapAngle(segmentsRotation[i]);
                    var spinningpoint60 = toOther;
                    var radians64 = (double)(num1090 * 0.1f);
                    Vector2 vector = default(Vector2);
                    toOther = spinningpoint60.RotatedBy(radians64, vector);
                }
                if (i % 2 == 0 && i != segments.Count - 1)
                    texture = mod.GetTexture("Projectiles/BiomeChest/StarlightSerpentBody1");
                else if (i != segments.Count - 1)
                    texture = mod.GetTexture("Projectiles/BiomeChest/StarlightSerpentBody2");
                else
                    texture = mod.GetTexture("Projectiles/BiomeChest/StarlightSerpentTail");
                float rotation = segmentsRotation[i];
                int spriteDirection = toOther.X > 0f ? 1 : -1;
                spriteBatch.Draw(texture, segments[i] + projectile.velocity - Main.screenPosition, null, (Color)GetAlpha(lightColor), rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.05f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                first = segments[i];
            }
                //Main.spriteBatch.Draw(texture, projectile.Center + Vector2.UnitY.RotatedBy((double)num2 * 6.28318548202515 / 4.0, new Vector2()) * num1, new Microsoft.Xna.Framework.Rectangle?(r), alpha * 0.1f, projectile.rotation, origin1, projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
            return false;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
            base.DrawBehind(index, drawCacheProjsBehindNPCsAndTiles, drawCacheProjsBehindNPCs, drawCacheProjsBehindProjectiles, drawCacheProjsOverWiresUI);
        }
        bool runOnce = true;
        public sealed override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (runOnce)
            {
                modPlayer.lightDragon = projectile.whoAmI;
                for(int i = 0; i < 3; i++)
                    segments.Add(projectile.Center);
                runOnce = false;
            }
            while(projectile.ai[0] > 0)
            {
                projectile.ai[0]--;
                segments.Add(projectile.Center);
            }
            while (projectile.ai[0] < 0)
            {
                projectile.ai[0]++;
                segments.RemoveAt(0);
                if (segments.Count <= 1)
                {
                    projectile.Kill();
                }
            }
            if(segments.Count <= 1)
            {
                projectile.Kill();
            }
            while (segmentsRotation.Count < segments.Count)
            {
                segmentsRotation.Add(0f);
            }
            int total = segments.Count - 1;
            projectile.minionSlots = total * 0.5f;
            return true;
        }
        public sealed override void AI()
        {
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if ((int)Main.time % 120 == 0)
            {
                projectile.netUpdate = true;
            }
            if (!player.active)
            {
                projectile.active = false;
            }
            if (player.dead || !player.active) //active check
            {
                player.ClearBuff(mod.BuffType("StarlightSerpent"));
            }
            if (player.HasBuff(mod.BuffType("StarlightSerpent")))
            {
                projectile.timeLeft = 2;
            }

            Vector2 first = projectile.Center;
            float firstRot = projectile.rotation;
            for (int i = 0; i < segments.Count; i++)
            {
                Vector2 pos = segments[i];
                float rotation = segmentsRotation[i];
                BodyTailMovement(ref pos, first, ref rotation, firstRot);
                segments[i] = pos;
                segmentsRotation[i] = rotation;
                first = pos;
                firstRot = segmentsRotation[i];
            }
            Vector2 pCenter = player.Center;
            float minDist = 700f;
            float minDist2 = 1000f;
            int npcID = -1;
            if (projectile.Distance(pCenter) > 2000f)
            {
                projectile.Center = pCenter;
                projectile.netUpdate = true;
            }

            var ownerMinionAttackTargetNPC5 = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC5 != null && ownerMinionAttackTargetNPC5.CanBeChasedBy(this, false))
            {
                float between = projectile.Distance(ownerMinionAttackTargetNPC5.Center);
                if (between < minDist * 2f)
                {
                    npcID = ownerMinionAttackTargetNPC5.whoAmI;
                }
            }

            if (npcID < 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(this, false) && player.Distance(npc.Center) < minDist2)
                    {
                        float between = projectile.Distance(npc.Center);
                        if (between < minDist)
                        {
                            npcID = i;
                        }
                    }
                }
            }

            if (npcID != -1)
            {
                NPC npc = Main.npc[npcID];
                Vector2 toNPC = npc.Center - projectile.Center;
                (toNPC.X > 0f).ToDirectionInt();
                (toNPC.Y > 0f).ToDirectionInt();
                float scaleFactor = 0.4f;
                if (toNPC.Length() < 600f)
                {
                    scaleFactor = 0.6f;
                }
                if (toNPC.Length() < 300f)
                {
                    scaleFactor = 0.8f;
                }

                float dist = toNPC.Length();
                Vector2 vector = npc.Size;
                if (dist > vector.Length() * 0.75f)
                {
                    projectile.velocity += Vector2.Normalize(toNPC) * scaleFactor * 1.5f;
                    if (Vector2.Dot(projectile.velocity, toNPC) < 0.25f)
                    {
                        projectile.velocity *= 0.8f;
                    }
                }
                float maxSpeed = 30f;
                if (projectile.velocity.Length() > maxSpeed)
                {
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * maxSpeed;
                }
            } //move towards enemy
            else
            {
                float scaleFactor = 0.2f;
                Vector2 toPlayer = pCenter - projectile.Center;
                if (toPlayer.Length() < 200f)
                {
                    scaleFactor = 0.12f;
                }
                if (toPlayer.Length() < 140f)
                {
                    scaleFactor = 0.06f;
                }
                if (toPlayer.Length() > 100f)
                {
                    if (Math.Abs(pCenter.X - projectile.Center.X) > 20f)
                    {
                        projectile.velocity.X += scaleFactor * Math.Sign(pCenter.X - projectile.Center.X);
                    }

                    if (Math.Abs(pCenter.Y - projectile.Center.Y) > 10f)
                    {
                        projectile.velocity.Y += scaleFactor * Math.Sign(pCenter.Y - projectile.Center.Y);
                    }
                }
                else if (projectile.velocity.Length() > 2f)
                {
                    projectile.velocity *= 0.96f;
                }

                if (Math.Abs(projectile.velocity.Y) < 1f)
                {
                    projectile.velocity.Y -= 0.1f;
                }

                float maxSpeed = 15f;
                if (projectile.velocity.Length() > maxSpeed)
                {
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * maxSpeed;
                }
            } //idle movement

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            int direction = projectile.direction;
            projectile.direction = projectile.spriteDirection = projectile.velocity.X > 0f ? 1 : -1;
            if (direction != projectile.direction)
            {
                projectile.netUpdate = true;
            }

            projectile.position = projectile.Center;
            projectile.scale = 1f;
            projectile.width = projectile.height = (int)(30 * projectile.scale);
            projectile.Center = projectile.position;
            if (projectile.alpha > 0) //more visuals
            {
                projectile.alpha -= 42;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
        }
        public void BodyTailMovement(ref Vector2 position, Vector2 prevPosition, ref float segmentsRotation, float segmentsRotation2)
        {
            var scaleFactor15 = 16f;
            Vector2 toOther = prevPosition - position;
            if(segmentsRotation != segmentsRotation2)
            {
                var num1090 = MathHelper.WrapAngle(segmentsRotation2 - segmentsRotation);
                var radians64 = (double)(num1090 * 0.1f);
                toOther = toOther.RotatedBy(radians64);
            }
            segmentsRotation = toOther.ToRotation() + MathHelper.ToRadians(90);
            position = prevPosition - toOther.SafeNormalize(new Vector2(1, 0)) * scaleFactor15;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha, 255 - projectile.alpha);
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}