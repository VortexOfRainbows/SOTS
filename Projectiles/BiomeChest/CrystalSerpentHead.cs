using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using System.Collections.Generic;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.BiomeChest
{
    public class Segment
    {
        public Segment(Vector2 position, float rotation, int aliveCounter)
        {
            this.position = position;
            this.rotation = rotation;
            this.aliveCounter = aliveCounter;
        }
        public Vector2 position;
        public float rotation;
        public int aliveCounter;
    }
    public class CrystalSerpentHead : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(rotate);
            writer.Write(accelerate);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            rotate = reader.ReadSingle();
            accelerate = reader.ReadSingle();
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Serpent");
			Main.projPet[Projectile.type] = true;
            //ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
		}
        public const float BaseAttackRate = 54;
        public const float framePerAttack = 11f;
        public void UpdateCounter(Vector2 toCenter, float numberOfSegments = 1)
        {
            int rate = (int)(BaseAttackRate / numberOfSegments / framePerAttack * 60f);
            Projectile.ai[0] += 60;
            while(Projectile.ai[0] > rate)
            {
                Projectile.ai[0] -= rate;
                Projectile.ai[1] = (int)(Projectile.ai[1] + 1) % 22;
                if(Main.myPlayer == Projectile.owner && (int)Projectile.ai[1] % 11 == 0 && toCenter != Vector2.Zero)
                {
                    Vector2 position = segments[segments.Count - 1].position;
                    int color = 1;
                    if ((int)Projectile.ai[1] == 11)
                        color = 0;
                    Vector2 to = toCenter - position;
                    to = to.SafeNormalize(Vector2.Zero);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, to * 13, ModContent.ProjectileType<StarBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner, color);
                }
            }
        }
        List<Segment> segments = new List<Segment>();
        Vector2 centerLocation;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            target.immune[Projectile.owner] = 6;
		}
		public sealed override void SetDefaults()
        {
            Projectile.width = 30;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			//Projectile.minion = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.netImportant = true;
            Projectile.minionSlots = 0f;
		}
        public sealed override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
            Vector2 first = Projectile.Center;
            Main.spriteBatch.Draw(texture, first - Main.screenPosition, null, (Color)GetAlpha(lightColor), Projectile.rotation, origin, 1.0f * generalScale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            for (int i = 0; i < segments.Count; i++)
            {
                float alpha = 0.16f * (segments[i].aliveCounter + 1);
                if (alpha > 1 || Projectile.alpha > 0)
                {
                    alpha = 1;
                }
                Vector2 toOther = first - segments[i].position;
                if(segments[i].rotation != 0)
                {
                    var num1090 = MathHelper.WrapAngle(segments[i].rotation);
                    var spinningpoint60 = toOther;
                    var radians64 = (double)(num1090 * 0.1f);
                    Vector2 vector = default(Vector2);
                    toOther = spinningpoint60.RotatedBy(radians64, vector);
                }
                Rectangle frame = new Rectangle(0, 0, 72, 120);
                if (i != segments.Count - 1)
                {
                    texture = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/CrystalSerpentBody").Value;
                }
                else
                {
                    texture = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/CrystalSerpentTail").Value;
                    frame = new Rectangle(0, 120 * (int)Projectile.ai[1], 72, 120);
                }
                float rotation = segments[i].rotation;
                int spriteDirection = toOther.X > 0f ? 1 : -1;
                Main.spriteBatch.Draw(texture, segments[i].position + Projectile.velocity - Main.screenPosition, frame, (Color)GetAlpha(lightColor) * alpha, rotation, origin, 1.0f * generalScale, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                first = segments[i].position;
            }
            //Main.spriteBatch.Draw(texture, Projectile.Center + Vector2.UnitY.RotatedBy((double)num2 * 6.28318548202515 / 4.0, new Vector2()) * num1, new Microsoft.Xna.Framework.Rectangle?(r), alpha * 0.1f, Projectile.rotation, origin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        public float generalScale = 0.75f;
        int totalSegments = 1;
        bool runOnce = true;
        public sealed override bool PreAI()
        {
            if(runOnce)
            {
                centerLocation = Projectile.Center;
                runOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active) //active check
            {
                player.ClearBuff(ModContent.BuffType<StarlightSerpent>());
            }
            if (player.HasBuff(ModContent.BuffType<StarlightSerpent>()))
            {
                Projectile.timeLeft = 2;
            }
            else
            {
                if(Projectile.owner == Main.myPlayer)
                    Projectile.Kill();
                return false;
            }
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            int ownedCounter = 1;
            int targetLength = 2;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI)
                {
                    if(proj.type == ModContent.ProjectileType<CrystalSerpentBody>())
                        targetLength++;
                    if (proj.type == ModContent.ProjectileType<CrystalSerpentHead>() && proj.whoAmI != Projectile.whoAmI)
                        ownedCounter++;
                }
            }
            while(segments.Count < targetLength)
            {
                segments.Add(new Segment(Projectile.Center, 0, 0));
            }
            while (segments.Count > targetLength)
            {
                segments.RemoveAt(0);
                if (segments.Count <= 2)
                {
                    Projectile.Kill();
                }
            }
            if(segments.Count <= 2)
            {
                Projectile.Kill();
                return false;
            }
            totalSegments = targetLength;
            return true;
        }
        float rotate = 0;
        float accelerate = 0;
        public sealed override void AI()
        {
            Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if ((int)Main.time % 120 == 0)
            {
                Projectile.netUpdate = true;
            }
            if (!player.active)
            {
                Projectile.active = false;
            }

            Vector2 first = Projectile.Center;
            float firstRot = Projectile.rotation;
            for (int i = 0; i < segments.Count; i++)
            {
                Vector2 pos = segments[i].position;
                float rotation = segments[i].rotation;
                BodyTailMovement(ref pos, first, ref rotation, firstRot, i);
                segments[i].position = pos;
                segments[i].rotation = rotation;
                first = pos;
                firstRot = segments[i].rotation;
                if(segments[i].aliveCounter < 7)
                {
                    for (int a = 0; a < 3; a++)
                    {
                        Dust dust = Dust.NewDustDirect(segments[i].position - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                        dust.velocity *= 1.2f;
                        dust.velocity += 5 * Projectile.velocity.SafeNormalize(Vector2.Zero);
                        dust.scale *= 2;
                        dust.noGravity = true;
                        dust.fadeIn = 0.2f;
                        dust.color = Main.rand.NextBool(2) ? new Color(170, 100, 190, 0) : new Color(150, 100, 200, 0);
                        dust.alpha = 100;
                    }
                }
                segments[i].aliveCounter++;
            }
            Vector2 pCenter = player.Center;
            float minDist = 700f;
            float minDist2 = 1000f;
            int npcID = -1;
            if (Projectile.Distance(pCenter) > 2000f)
            {
                Projectile.Center = pCenter;
                Projectile.netUpdate = true;
            }

            var ownerMinionAttackTargetNPC5 = Projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC5 != null && ownerMinionAttackTargetNPC5.CanBeChasedBy(this, false))
            {
                float between = Vector2.Distance(centerLocation, ownerMinionAttackTargetNPC5.Center);
                if (between < minDist * 2f)
                {
                    npcID = ownerMinionAttackTargetNPC5.whoAmI;
                }
            }

            if (npcID < 0)
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy(this, false) && player.Distance(npc.Center) < minDist2)
                    {
                        float between = Vector2.Distance(centerLocation, npc.Center);
                        if (between < minDist)
                        {
                            minDist = between;
                            npcID = i;
                        }
                    }
                }
            }

            if (npcID != -1)
            {
                NPC npc = Main.npc[npcID];
                if (accelerate < 1)
                    accelerate += 0.1f;
                else
                    accelerate = 1;
                rotate += MathHelper.ToRadians((1.6f + 0.4f * (float)Math.Sqrt(totalSegments)) * (npc.whoAmI % 2 * 2 - 1) * accelerate);
                Vector2 position = npc.Center + new Vector2(0, npc.Size.Length() * 0.6f + 256 + 68 * (float)Math.Sqrt(totalSegments)).RotatedBy(rotate);
                centerLocation = Vector2.Lerp(centerLocation, position, 0.05f);
                Vector2 toNPC = centerLocation - Projectile.Center;
                float dist = toNPC.Length();
                float maxSpeed = (7f + totalSegments * 1.7f) * accelerate;
                if (dist < maxSpeed)
                {
                    maxSpeed = dist;
                }
                Projectile.velocity = toNPC.SafeNormalize(Vector2.Zero) * maxSpeed;
                UpdateCounter(npc.Center, totalSegments - 1);
            } //move towards enemy
            else
            {
                centerLocation = Vector2.Lerp(centerLocation, player.Center, 0.05f);
                if (accelerate > 0)
                    accelerate *= 0.987f;
                UpdateCounter(Vector2.Zero, totalSegments - 1);
                float scaleFactor = 0.2f;
                Vector2 toPlayer = pCenter - Projectile.Center;
                if (toPlayer.Length() < 200f)
                {
                    scaleFactor = 0.12f;
                }
                if (toPlayer.Length() < 140f)
                {
                    scaleFactor = 0.6f;
                }
                if (toPlayer.Length() > 100f)
                {
                    if (Math.Abs(pCenter.X - Projectile.Center.X) > 20f)
                    {
                        Projectile.velocity.X += scaleFactor * Math.Sign(pCenter.X - Projectile.Center.X);
                    }

                    if (Math.Abs(pCenter.Y - Projectile.Center.Y) > 10f)
                    {
                        Projectile.velocity.Y += scaleFactor * Math.Sign(pCenter.Y - Projectile.Center.Y);
                    }
                }
                else if (Projectile.velocity.Length() > 2f)
                {
                    Projectile.velocity *= 0.95f;
                }
                float maxSpeed = 13f;
                if (Projectile.velocity.Length() > maxSpeed)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
                }
            } //idle movement

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            int direction = Projectile.direction;
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            if (direction != Projectile.direction)
            {
                Projectile.netUpdate = true;
            }
            if (Projectile.alpha > 0) //more visuals
            {
                Projectile.alpha -= 42;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
        }
        public void BodyTailMovement(ref Vector2 position, Vector2 prevPosition, ref float segmentsRotation, float segmentsRotation2, int index)
        {
            float scaleFactor15 = 36f * generalScale;
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
            return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
        }
        public override void Kill(int timeLeft)
        {
            base.Kill(timeLeft);
        }
    }
}