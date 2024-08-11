using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using System.IO.Pipelines;
using Terraria.GameContent.LootSimulation;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;

namespace SOTS.Projectiles.BiomeChest
{
	public class FreshGreeny : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D stabby = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/FreshGreenySpike").Value;
            Texture2D leaf = ModContent.Request<Texture2D>("SOTS/Projectiles/BiomeChest/FreshGreenyLeaf").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 5 * 0.75f - 4);
            Vector2 stabOrigin = new Vector2(stabby.Width / 2, stabby.Height);
            Vector2 leafOrigin = new Vector2(0, leaf.Height / 2);
            Color color = lightColor;
			Rectangle frame = new Rectangle(0, texture.Height / 5 * Projectile.frame, texture.Width, texture.Height / 5);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Main.spriteBatch.Draw(texture, center - Main.screenPosition, frame, new Color(30, 40, 35, 0) * perc * perc, Projectile.oldRot[i], drawOrigin, Projectile.scale, Projectile.oldSpriteDirection[i] == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            foreach(Vector2 s in Stabbies)
            {
                Main.spriteBatch.Draw(leaf, Projectile.Center - Main.screenPosition, null, color * 0.3f, s.ToRotation(), leafOrigin, new Vector2(s.Length() / leaf.Width, 1), 0, 0f);
                Main.spriteBatch.Draw(stabby, Projectile.Center + s - Main.screenPosition, null, color, s.ToRotation() + MathHelper.PiOver2, stabOrigin, Projectile.scale, 0, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public sealed override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 34;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 40;
            Projectile.usesLocalNPCImmunity = true;
		}
		public override bool? CanCutTiles() => false;
		public override bool MinionContactDamage() => true;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = Math.Sign(target.Center.X - Projectile.Center.X);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            foreach (Vector2 s in Stabbies)
            {
                Rectangle stabbyHB = new Rectangle((int)(Projectile.Center.X + s.X - 8), (int)(Projectile.Center.Y + s.Y - 16), 16, 16);
                if(targetHitbox.Intersects(stabbyHB))
                {
                    return true;
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }
        private float counter = 0;
        private int frame = 0;
        private bool RunOnce = true;
        private List<Vector2> Stabbies;
        private float FoundTargetCounter = 0;
        public override void AI()
        {
            if(RunOnce)
            {
                Stabbies = [Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero];
                RunOnce = false;
            }
            Player player = Main.player[Projectile.owner];
            if (player.dead || !player.active)
                player.ClearBuff(ModContent.BuffType<Buffs.MinionBuffs.FreshGreenyBuff>());
            if (player.HasBuff(ModContent.BuffType<Buffs.MinionBuffs.FreshGreenyBuff>()))
                Projectile.timeLeft = 2;
            SlimeAI();
            FrameUpdate();
            if (counter % 5 == 0 && Main.myPlayer == Projectile.owner)
            {
                Projectile.netUpdate = true;
            }
            Projectile.ai[2] += 1 + FoundTargetCounter / 90f;
            Projectile.tileCollide = Projectile.ai[1] != 1; //If not flying
		}	
        private void UpdateStabbies(Vector2 Target)
        {
            float c = Stabbies.Count;
            if (c < 1)
                return;
            float varianceFlower = Math.Clamp(c - 1, 1, 4);
            float radians = MathHelper.ToRadians(90 + c * 2);
            float attackSpeed = 30f / c; // This number will have to change for balancing
            Projectile.localNPCHitCooldown = (int)attackSpeed;
            //float bonusAttackSpeed = attackSpeed - (int)attackSpeed; 
            //Main.NewText(bonusAttackSpeed);
            float lerpSpeed = 0.0525f;
            if (Target != Vector2.Zero)
                lerpSpeed += 0.0155f + 0.01f * MathF.Sqrt(c) * FoundTargetCounter / 70f;
            float stabCompression = (5 + c) / (10 + c);
            for (int i = 0; i < c; i++)
            {
                float percent = (i + 0.5f) / c;
                float bonusLerpSpeed = 0f;
                Vector2 idlePos;
                float variance = 6 * MathF.Sin((Projectile.ai[2] / 180f + percent * varianceFlower) * MathHelper.TwoPi);
                if (Target == Vector2.Zero)
                {
                    idlePos = new Vector2(0, -24 + variance).RotatedBy(Projectile.rotation + MathHelper.Lerp(-radians, radians, percent));
                }
                else
                {
                    float l = Target.Length();
                    Vector2 between = Target * 0.5f;
                    Vector2 cycle = new Vector2(l * 0.55f + variance, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[2] / 2f * (6 + MathF.Sqrt(c)) * Projectile.spriteDirection) + MathHelper.TwoPi * percent);
                    cycle.Y *= stabCompression;
                    cycle = cycle.RotatedBy(Target.ToRotation());
                    idlePos = between + cycle;
                    bonusLerpSpeed += l / 1600f;
                }
                Stabbies[i] = Vector2.Lerp(Stabbies[i], idlePos, lerpSpeed + bonusLerpSpeed);
            }
        }
		public void SlimeAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 idlePos = player.Center - new Vector2(60 * (Projectile.minionPos + 1) * player.direction, 0); //Maybe don't use minionPos
            int PursuitWindow = 32; //The distance at which it is considered close enough to not pursue further.
            float seekEnemyRange = 1280;
            bool flying = Projectile.ai[1] == 1;
            bool foundTarget = false;
            int target = -1;
            if(player.Distance(Projectile.Center) < 2000)
            {
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    if (between < seekEnemyRange && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                    {
                        seekEnemyRange = between;
                        target = npc.whoAmI;
                        Projectile.netUpdate = true;
                    }
                }
                if (target == -1)
                {
                    target = SOTSNPCs.FindTarget_Basic(Projectile.Center, seekEnemyRange, this, true);
                }
                if (target == -1)
                {
                    target = SOTSNPCs.FindTarget_Basic(player.Center, seekEnemyRange, this, true);
                }
                if (target != -1)
                {
                    NPC npc = Main.npc[target];
                    idlePos = npc.Center;
                    PursuitWindow = npc.width + 80;
                    foundTarget = true;
                }
            }

            if(!flying)
                Projectile.velocity.Y += 0.4f; //Gravity is normally 0.09f, but this should make it slightly snappier
            bool touchingGround = Collision.SolidTiles(Projectile.position, Projectile.width, Projectile.height + 2) ||
                Projectile.velocity.Y != Collision.TileCollision(Projectile.position + new Vector2(8, 0), new Vector2(0, Projectile.velocity.Y), Projectile.width - 16, Projectile.height).Y;
            if(touchingGround)
            {
                Projectile.ai[0]++;
            }
            else
            {
                Projectile.ai[0] = 0;
            }
            touchingGround = Projectile.ai[0] > 1;
            Vector2 toIdlePos = idlePos - Projectile.Center;
            Vector2 toIdleNorm = toIdlePos.SNormalize();
            float toIdleLen = toIdlePos.Length();
            float travelDir = MathF.Sign(toIdlePos.X);
            if(toIdleLen > 2400)
            {
                Projectile.Center = idlePos;
                toIdlePos = Vector2.Zero;
            }
            if (MathF.Abs(idlePos.X - Projectile.Center.X) > PursuitWindow)
            {
                float speedM = 0.3f;
                if(touchingGround)
                {
                    speedM = 1.5f;
                    Projectile.velocity.X *= 0.6f;
                }
                else
                {
                    Projectile.velocity.X *= 0.95f;
                }
                Projectile.velocity.X += (travelDir * speedM + toIdlePos.X * 0.0001f);
            }
            else
            {
                Projectile.velocity.X *= 0.95f;
            }

            bool FlyDueToDistance = toIdleLen > 1200;
            if (toIdlePos.Y < -128 || FlyDueToDistance) //8 blocks up vertically
            {
                if(FlyDueToDistance || !Collision.SolidTiles(idlePos - new Vector2(48, 0), 96, 96))
                    Projectile.ai[1] = 1;
            }
            else
            {
                bool hasTiles = Collision.SolidTiles(Projectile.Center - new Vector2(32, 0), 48, 28);
                if (hasTiles || touchingGround)
                    Projectile.ai[1] = 0;
            }
            if (flying)
            {
                Projectile.velocity += new Vector2(0, MathF.Sin(MathHelper.ToRadians(Projectile.ai[2] * 3))) / 120f;
                if (toIdleLen > PursuitWindow)
                {
                    Projectile.velocity *= 0.93f;
                    Projectile.velocity += toIdleNorm * 0.4f + toIdlePos * 0.003f;
                }
                else
                    Projectile.velocity *= 0.9775f;
                Projectile.rotation = Projectile.velocity.X * 0.04f;
            }
            else
            {
                Projectile.rotation = (Projectile.velocity.ToRotation() - MathHelper.ToRadians(90)) * 0.05f * Projectile.direction;
                if (touchingGround && MathF.Abs(Projectile.velocity.X) > 0.2f) //Allow jumping if touching the ground
                {
                    Projectile.velocity.Y *= 0.5f;
                    Projectile.velocity.Y -= MathF.Sqrt(27f + 10 * MathF.Abs(Projectile.velocity.X)); //Jump height varies by velocity
                }
            }
            if(MathF.Abs(Projectile.velocity.X) > 0.5f)
                Projectile.spriteDirection = Projectile.direction;

            Vector2 targetPos = Vector2.Zero;
            if(foundTarget && toIdleLen < PursuitWindow + 100 + Projectile.velocity.Length())
            {
                FoundTargetCounter++;
                targetPos = toIdlePos;
                Projectile.spriteDirection = MathF.Sign(toIdlePos.X);
                Projectile.velocity.X -= travelDir * 0.05f;
            }
            else
            {
                FoundTargetCounter--;
            }
            FoundTargetCounter = Math.Clamp(FoundTargetCounter, 0, 90);
            UpdateStabbies(targetPos);
        }
        public override void PostAI()
        {
            Projectile.frame = frame;
        }
        public void FrameUpdate()
        {
            counter += 0.5f;
			counter += MathF.Sqrt(Math.Abs(Projectile.velocity.X));
            int frameSpeed = 10;
            if (counter >= frameSpeed)
            {
				counter -= frameSpeed;
                frame++;
                if (frame >= Main.projFrames[Projectile.type] - 1)
                {
                    frame = 0;
                }
            }
			if(Projectile.ai[1] == 1) //This is when the slime is flying
			{
				counter = 0;
                frame = 4;
			}
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}