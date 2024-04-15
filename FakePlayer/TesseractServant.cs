using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.MinionBuffs;
using SOTS.Common.GlobalNPCs;
using SOTS.Void;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
	public class TesseractServant : FakePlayerPossessingProjectile
	{
        public override void SafeSetStaticDefaults()
        {
            Main.projPet[Type] = false;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }
        public override string Texture => "SOTS/FakePlayer/SubspaceServant";
        public sealed override void SetDefaults()
        {
            Projectile.width = 20;
			Projectile.height = 42;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;
			Projectile.hide = false;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        private int aliveCounter = 0;
        private int timeSinceLastTP = 0;
        private bool runOnce = true;
        private float TotalTesseracts = 1;
        public int MyUniqueID => (int)Projectile.ai[1] % 10;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(player);
            if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			if (runOnce)
			{
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.ai[1] = fPlayer.tesseractPlayerCount;
                    Projectile.netUpdate = true;
                }
				runOnce = false;
			}
            else if (Projectile.ai[1] > fPlayer.tesseractPlayerCount)
            {
                Projectile.Kill();
            }
			if (FakePlayer == null)
				FakePlayer = new FakePlayer(FakePlayerTypeID.Tesseract, Projectile.identity);
            int useItemSlot = 40 + MyUniqueID;
            FakePlayer.OverrideUseSlot = useItemSlot;
            if (!player.HasBuff<TesseractBuff>())
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.timeLeft = 6;
            }
            //if (Main.myPlayer == Projectile.owner)
            //{
            //    FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(player);
            //    if(fPlayer.tesseractPlayerCount < Projectile.ai[1])
            //    {
            //        Projectile.Kill();
            //    }
            //}
            timeSinceLastTP--;
            aliveCounter++;
            return true;
		}
        private int lastTarget = -1;
        private int target = -1;
        private bool validItem;
        private bool lastSkipDraw = false;
        private int TrailingType = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(player);
            if (FakePlayer == null || fPlayer.tesseractPlayerCount <= 0)
                return;
            NPC npc = null;
            int oldMouseX = Main.mouseX;
            int oldMouseY = Main.mouseY;
            TotalTesseracts = fPlayer.tesseractPlayerCount;
            if (TotalTesseracts < 1)
                TotalTesseracts = 1;
			Vector2 idlePosition = player.Center;
            float maxPursuitRange = 1200;
            bool itemDataRegistered = fPlayer.tesseractData[MyUniqueID].ChargeFrames >= 0;
            bool foundTarget = false;
            validItem = false;
            if(FakePlayer.heldItem != null)
                validItem = FakePlayer.CheckItemValidityFull(player, FakePlayer.heldItem, FakePlayer.heldItem, FakePlayerTypeID.Tesseract);
            TrailingType = FakePlayer.ItemTrailingType(player.inventory[FakePlayer.UseItemSlot(player)]);
            bool needsLOS = TrailingType == TrailingID.RANGED || TrailingType == TrailingID.MAGIC;
            if (target != -1)
            {
                NPC prevTarget = Main.npc[target];
                if(!prevTarget.active || prevTarget.life <= 0 || prevTarget.dontTakeDamage || !prevTarget.CanBeChasedBy(this))
                {
                    target = -1;
                }
                else if (FakePlayer.BonusItemAnimationTime <= 1 && FakePlayer.itemAnimation <= 1 && lastTarget != -1)
                {
                    target = -1;
                }
            }
            lastTarget = target;
            if (validItem)
            {
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC attackNPC = Main.npc[player.MinionAttackTargetNPC];
                    if (attackNPC.Distance(player.Center) < maxPursuitRange)
                    {
                        target = player.MinionAttackTargetNPC;
                    }
                }
                if (target != -1)
                {
                    NPC attackNPC = Main.npc[target];
                    if (attackNPC.Distance(player.Center) > maxPursuitRange + 100)
                    {
                        target = -1;
                    }
                }
                if (target == -1)
                {
                    target = SOTSNPCs.FindTarget_Basic(Projectile.Center, maxPursuitRange, this, true);
                    if(target != -1)
                    {
                        NPC attackNPC = Main.npc[target];
                        if (attackNPC.Distance(player.Center) > maxPursuitRange + 100)
                        {
                            target = -1;
                        }
                    }
                    if (target == -1)
                        target = SOTSNPCs.FindTarget_Basic(player.Center, maxPursuitRange, this, true);
                }
            }
            if (target != -1 && itemDataRegistered)
            {
                foundTarget = true;
                npc = Main.npc[target];
                cursorArea = npc.Center;
                Direction = Math.Sign(npc.Center.X - Projectile.Center.X);
            }
            else if (!itemDataRegistered)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    cursorArea = Main.MouseWorld;
                    if(FakePlayer.BonusItemAnimationTime <= 1 && FakePlayer.itemAnimation <= 1)
                        Direction = Math.Sign(cursorArea.X - player.Center.X);
                }
            }
            else
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    Direction = player.direction;
                }
            }
            bool readyToUseItem = validItem && itemDataRegistered && foundTarget;
            if (!readyToUseItem)
            {
                TrailingType = 0;
            }
            if (cursorArea != Vector2.Zero || TrailingType == 0)
            {
                Vector2 result = cursorArea - Main.screenPosition;
                Main.mouseX = FakePlayer.UniqueMouseX = (int)result.X;
                Main.mouseY = FakePlayer.UniqueMouseY = (int)result.Y;
                float appropriateMeleeDistance = -7;
                if (FakePlayer.heldItem != null && !FakePlayer.heldItem.IsAir)
                {
                    appropriateMeleeDistance += FakePlayer.heldItem.Size.Length() * FakePlayer.heldItem.scale;
                    if (FakePlayer.heldItem.CountsAsClass(DamageClass.SummonMeleeSpeed))
                    {
                        appropriateMeleeDistance += 128f;
                    }
                }
                if (appropriateMeleeDistance < 44)
                    appropriateMeleeDistance = 44;
                bool hasLOS = false;
                if (TrailingType == 0)
                {
                    if(fPlayer.tesseractPlayerCount > 0)
                    {
                        float distance = 54f + fPlayer.tesseractPlayerCount * 10;
                        float bonus = MathHelper.TwoPi * Projectile.ai[1] / TotalTesseracts;
                        Vector2 circular = new Vector2(distance, 0).RotatedBy(SOTSWorld.GlobalCounter * MathHelper.Pi / 360f + bonus);
                        idlePosition += circular;
                    }
                }
                if (TrailingType == TrailingID.RANGED || TrailingType == TrailingID.MAGIC || TrailingType == TrailingID.CLOSERANGE)
                {
                    float appropriateRangedDistance = TrailingType == TrailingID.CLOSERANGE ? appropriateMeleeDistance : 96;
                    if (npc != null && foundTarget && needsLOS)
                    {
                        hasLOS = CollisionLineOfSight(npc, player);
                        appropriateRangedDistance += npc.Size.Length() / 2f + (float)Math.Sqrt(npc.width * npc.height) / 2f;
                    }
                    else if (!needsLOS)
                    {
                        hasLOS = true;
                    }
                    Vector2 toCursor = cursorArea - Projectile.Center;
                    Vector2 fromNPC = cursorArea;
                    for(float distM = 0.1f; distM < 1; distM += 0.05f)
                    {
                        if (SOTSWorldgenHelper.TrueTileSolid((int)fromNPC.X / 16, (int)fromNPC.Y / 16, false))
                        {
                            break;
                        }
                        fromNPC = cursorArea - toCursor.SafeNormalize(Vector2.Zero) * appropriateRangedDistance * distM;
                    }
                    idlePosition = fromNPC;
                }
                if (TrailingType == TrailingID.MELEE)
                {
                    hasLOS = true;
                    Vector2 toCursor = cursorArea - player.Center;
                    float length = toCursor.Length();
                    if (length > maxPursuitRange + appropriateMeleeDistance + 320)
                        length = maxPursuitRange + appropriateMeleeDistance + 320;
                    float lengthToCursor = length;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
                    idlePosition += toCursor;
                    idlePosition.Y = cursorArea.Y;
                    idlePosition.X -= appropriateMeleeDistance * Math.Sign(Direction);
                }
                bool move = TrailingType != 0 || (FakePlayer.BonusItemAnimationTime <= 0 && FakePlayer.itemAnimation <= 0) || !itemDataRegistered;
                if(move)
                {
                    Vector2 toIdle = idlePosition - Projectile.Center;
                    float dist = toIdle.Length();
                    float speed = 6f + dist * 0.01f;
                    if(TrailingType == 0)
                    {
                        speed += (float)Math.Pow(dist, 1.5f) * 0.0025f;
                    }
                    else
                    {
                        speed += 2 + dist * 0.0075f;
                    }
                    if ((lastSkipDraw != FakePlayer.SkipDrawing || (dist > 320 && timeSinceLastTP <= 0) || aliveCounter == 4) && aliveCounter > 3)
                    {
                        if (Main.myPlayer == player.whoAmI)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TesseractLaser>(), 0, 0, Main.myPlayer, idlePosition.X, idlePosition.Y, Projectile.ai[1]);
                        }
                        Projectile.Center = idlePosition;
                        Projectile.velocity = Vector2.Zero;
                        timeSinceLastTP = 10;
                    }
                    else
                    {
                        if (dist < speed)
                        {
                            speed = toIdle.Length();
                        }
                        Projectile.velocity = toIdle.SafeNormalize(Vector2.Zero) * speed;
                    }
                }
                else
                {
                    Projectile.velocity *= 0.75f;
                }
                if (Direction == 1)
                {
                    if (Projectile.ai[0] < Direction)
                        Projectile.ai[0] += 0.1f;
                }
                else
                {
                    if (Projectile.ai[0] > Direction)
                        Projectile.ai[0] -= 0.1f;
                }
                if(foundTarget)
                {
                    FakePlayer.ForceItemUse = (hasLOS || FakePlayer.itemAnimation > 0) && aliveCounter > 10;
                }
                lastSkipDraw = FakePlayer.SkipDrawing;
                if(needsLOS && TrailingType != TrailingID.IDLE && Projectile.Distance(cursorArea) < 320)
                {
                    Projectile.velocity = Collision.TileCollision(Projectile.Center - new Vector2(16, 16), Projectile.velocity, 32, 32, true, true);
                }
                if(aliveCounter < 4)
                {
                    Projectile.velocity *= 0;
                }
                CheckNoCollisionNearby();
                FakePlayer.OverrideTrailingType = TrailingType;
                if(aliveCounter > 3)
                    UpdateItems(player);
                //Projectile.velocity = FakePlayer.Velocity;
            }

            Main.mouseX = oldMouseX;
            Main.mouseY = oldMouseY;
            if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
            {
                Projectile.netUpdate = true;
            }
        }
        private bool CollisionLineOfSight(NPC npc, Player player)
        {
            return Collision.CanHitLine(Projectile.Center - new Vector2(16, 16), 32, 32, npc.position, npc.width, npc.height) ||
                Collision.CanHitLine(player.Center - new Vector2(16, 16), 32, 32, npc.position, npc.width, npc.height);
        }
        private void CheckNoCollisionNearby()
        {
            float collideRange = 48;
            for(int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile other = Main.projectile[i];
                if(Projectile.whoAmI != i)
                {
                    if(other.type == Type && other.ModProjectile is TesseractServant ts && ts.TrailingType != TrailingID.IDLE)
                    {
                        Vector2 nudge = Projectile.Center - other.Center;
                        float dist = nudge.Length();
                        nudge = nudge.RotatedBy(Projectile.ai[1] % 10 * 0.02f);
                        if (dist < collideRange)
                        {
                            Projectile.Center += nudge.SafeNormalize(Vector2.Zero) * (collideRange - dist);
                        }
                    }
                }
            }
        }
        private List<Vector2> outerPoints;
        private List<float> outerDepths;
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce || FakePlayer == null || aliveCounter <= 3)
                return false;
            Color drawColor = FakePlayer.MyBorderColor();
            bool skipDrawing = !FakePlayer.SkipDrawing || !lastSkipDraw || TrailingType != TrailingID.IDLE;
            bool drawItem = false;
            if (skipDrawing)
            {
                if(!lastSkipDraw && !FakePlayer.SkipDrawing)
                {
                    DrawWings(drawColor * 0.8f, 3f);
                    DrawWings(Color.Black, 1f);
                }
                return false;
            }
            else if(FakePlayer != null && FakePlayer.heldItem != null && !FakePlayer.heldItem.IsAir && validItem)
            {
                drawItem = true;
            }
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Texture2D planetariumTexture = PlayerInventorySlotsManager.planetariumTextures;
            //drawPosition += new Vector2(0, -64);
            int frameY = (int)Projectile.ai[1] % 10;
            int height = planetariumTexture.Height / 10;
            //DrawWings(drawColor * 0.8f, 3f);
            DrawTesseract(drawColor * 0.8f, 3f);
            drawColor *= 0.5f;
            Rectangle frame = new Rectangle(0, height * frameY, planetariumTexture.Width, height);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(1f, 0).RotatedBy(i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(planetariumTexture, drawPosition + circular, frame, drawColor * 0.5f, 0f, new Vector2(planetariumTexture.Width, height) * 0.5f, 1f, SpriteEffects.None, 0f);
            }
            //DrawWings(Color.Black, 1f);
            DrawTesseract(Color.Black, 1f);
            Main.spriteBatch.Draw(planetariumTexture, drawPosition, frame, drawColor * 1.5f, 0f, new Vector2(planetariumTexture.Width, height) * 0.5f, 1f, SpriteEffects.None, 0f);
            if(drawItem)
            {
                int heldItemType = FakePlayer.heldItem.type;
                DrawAnimation anim = Main.itemAnimations[heldItemType];
                int frameCount = 1;
                int ticksPerFrame = 1;
                if (anim != null)
                {
                    frameCount = anim.FrameCount;
                    ticksPerFrame = anim.TicksPerFrame;
                }
                Texture2D texture = Terraria.GameContent.TextureAssets.Item[heldItemType].Value;
                Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
                Rectangle rectangleFrame = new Rectangle(0, texture.Height / frameCount * (SOTSWorld.GlobalCounter / ticksPerFrame % frameCount), texture.Width, texture.Height / frameCount);
                int topSize = Math.Max(rectangleFrame.Width, rectangleFrame.Height);
                float scale = Math.Clamp(42f / topSize, 0, 1f);
                float sinusoid = (float)Math.Sin(SOTSWorld.GlobalCounter * Math.PI / 180f + Projectile.ai[1]) * 0.3f;
                float cosinusoid = MathF.Cos(SOTSWorld.GlobalCounter * MathHelper.Pi / 180f + Projectile.ai[1]) * 4f;
                Main.spriteBatch.Draw(texture, drawPosition + new Vector2(0, cosinusoid), rectangleFrame, Color.White * 1.0f, sinusoid, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public void DrawWings(Color color, float innerScale)
        {
            for(int j = -1; j <= 1; j += 2)
            {
                for (int i = 1; i <= 3; i++)
                {
                    float sinusoid = 15 * MathF.Sin(4 * MathHelper.ToRadians(SOTSWorld.GlobalCounter + i * 15));
                    DrawCrystal(color, j * (sinusoid + 45f * i - 5), 9 - i, innerScale);
                }
            }
        }
        public void DrawCrystal(Color color, float degreesOffset, float size = 8f, float innerScale = 2f)
        {
            Texture2D whitePixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float radians = MathHelper.ToRadians(degreesOffset);
            drawPosition += new Vector2(0, -20 - size).RotatedBy(radians);
            float root2 = 3.5f;
            List<Vector2> points = new List<Vector2>();
            List<float> depths = new List<float>();
            float bonus = 0f;
            float northDepth = 0;
            float southDepth = 0;
            for (int a = 0; a < 6; a++)
            {
                int i = a;
                int j = 1;
                int k = 0;
                if(a > 3)
                {
                    j = 0;
                    k = a == 4 ? 1 : -1;
                }
                float cos = j * (float)Math.Cos(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2 + degreesOffset) + i * MathHelper.PiOver2 + bonus);
                float sin = j * (float)Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2 + degreesOffset) + (2 + i) * MathHelper.PiOver2 + bonus);
                float depth = 0.8f - 0.2f * sin;
                Vector2 offset = new Vector2(size, root2 * size * k);
                offset.X *= cos;
                offset *= depth;
                if (a == 4)
                    southDepth = depth;
                if (a == 5)
                    northDepth = depth;
                offset = offset.RotatedBy(radians);
                points.Add(drawPosition + offset);
                depths.Add(depth);
            }
            Vector2 northPoint = points[5];
            Vector2 southPoint = points[4];
            for (int i = 0; i < 6; i++)
            {
                Main.spriteBatch.Draw(whitePixel, points[i], null, color, 0f, Vector2.One, depths[i] * innerScale, SpriteEffects.None, 0f);
                if(i < 4)
                {
                    Vector2 toOtherPoint = northPoint - points[i];
                    Main.spriteBatch.Draw(whitePixel, points[i], null, color, toOtherPoint.ToRotation(), new Vector2(0, 1f), new Vector2(toOtherPoint.Length() / 2f, innerScale * (depths[i] / 2f + northDepth / 2f)), SpriteEffects.None, 0f);
                    toOtherPoint = southPoint - points[i];
                    Main.spriteBatch.Draw(whitePixel, points[i], null, color, toOtherPoint.ToRotation(), new Vector2(0, 1f), new Vector2(toOtherPoint.Length() / 2f, innerScale * (depths[i] / 2f + southDepth / 2f)), SpriteEffects.None, 0f);
                }
            }
        }
        public void DrawTesseract(Color color, float innerScale)
        {
            outerPoints = new List<Vector2>();
            outerDepths = new List<float>();
            float scale = 14f;
            DrawFace(color, 0, scale, innerScale);
            DrawFace(color, 90, scale, innerScale);
            DrawFace(color, 180, scale, innerScale);
            DrawFace(color, 270, scale, innerScale);

            DrawFace(color, 0, 2 * scale, innerScale);
            DrawFace(color, 90, 2 * scale, innerScale);
            DrawFace(color, 180, 2 * scale, innerScale);
            DrawFace(color, 270, 2 * scale, innerScale);

            Texture2D whitePixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            int halfWay = outerPoints.Count / 2;
            for (int i = 0; i < outerPoints.Count / 2; i++)
            {
                Vector2 previousPoint = outerPoints[i + halfWay];
                float previousDepth = outerDepths[i + halfWay];
                Vector2 toOtherPoint = previousPoint - outerPoints[i];
                Main.spriteBatch.Draw(whitePixel, outerPoints[i], null, color, toOtherPoint.ToRotation(), new Vector2(0, 1f), new Vector2(toOtherPoint.Length() / 2f, innerScale * (outerDepths[i] * 0.5f + previousDepth * 0.5f)), SpriteEffects.None, 0f);
            }
        }
        public void DrawFace(Color color, float degreesOffset, float size = 8f, float innerScale = 2f)
        {
            Texture2D whitePixel = ModContent.Request<Texture2D>("SOTS/Items/Secrets/WhitePixel").Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            //drawPosition += new Vector2(0, -64);
            float root2 = (float)Math.Sqrt(2);
            List<Vector2> points = new List<Vector2>();
            List<float> depths = new List<float>();
            float bonus = MathHelper.TwoPi * Projectile.ai[1] / TotalTesseracts;
            for (int a = 0; a < 4; a++)
            {
                int i = 0;
                int j = 1;
                if (a == 0)
                {
                    i = 0;
                    j = 1;
                }
                if (a == 1)
                {
                    i = 1;
                    j = 1;
                }
                if (a == 2)
                {
                    i = 1;
                    j = -1;
                }
                if (a == 3)
                {
                    i = 0;
                    j = -1;
                }
                float cos = (float)Math.Cos(MathHelper.ToRadians(SOTSWorld.GlobalCounter + degreesOffset) + i * MathHelper.PiOver2 + bonus);
                float sin = (float)Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter + degreesOffset) + i * MathHelper.PiOver2 + bonus);
                float depth = 0.8f - 0.2f * sin;
                Vector2 offset = new Vector2(size * root2, size * j);
                offset.X *= cos;
                offset *= depth;
                offset = offset.RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter) + bonus / 2f);
                points.Add(drawPosition + offset);
                depths.Add(depth);
                if (degreesOffset % 180 == 0)
                {
                    outerPoints.Add(drawPosition + offset);
                    outerDepths.Add(depth);
                }
            }
            Vector2 previousPoint = points[points.Count - 1];
            float previousDepth = depths[points.Count - 1];
            for (int i = 0; i < points.Count; i++)
            {
                if (i != 2)
                {
                    Main.spriteBatch.Draw(whitePixel, points[i], null, color * 1f, 0f, Vector2.One, depths[i] * innerScale, SpriteEffects.None, 0f);
                    Vector2 toOtherPoint = previousPoint - points[i];
                    Main.spriteBatch.Draw(whitePixel, points[i], null, color, toOtherPoint.ToRotation(), new Vector2(0, 1f), new Vector2(toOtherPoint.Length() / 2f, innerScale * (depths[i] /2f + previousDepth / 2f)), SpriteEffects.None, 0f);
                }
                previousPoint = points[i];
                previousDepth = depths[i];
            }
        }
    }
    public class TesseractLaser : ModProjectile
    {
        public override string Texture => "SOTS/Items/Secrets/WhitePixel";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.hostile = Projectile.friendly = Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<VoidSummon>();
            Projectile.ignoreWater = true;
        }
        public void DustScatter(Vector2 position, float mult = 1f)
        {
            Vector2 start = Projectile.Center;
            Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 toDestination = destination - start;
            float rand = Main.rand.NextFloat(MathHelper.TwoPi);
            for (int i = 0; i < 18; i++)
            {
                Vector2 circular = new Vector2(1, 0).RotatedBy(i / 18f * MathHelper.TwoPi + rand);
                Dust dust = Dust.NewDustDirect(position - new Vector2(5) + circular * 16, 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
                dust.scale *= 0.2f;
                dust.scale += 1.0f;
                dust.velocity *= 0.5f;
                dust.velocity += Main.rand.NextFloat() * mult * toDestination.SafeNormalize(Vector2.Zero);
                dust.velocity += circular * Main.rand.NextFloat(2.4f, 3.2f);
                dust.noGravity = true;
                dust.color = Color.Lerp(coreColor, Color.White, Main.rand.NextFloat(0.5f));
                dust.fadeIn = 0.2f;
            }
        }
        public override void AI()
        {
            if ((int)Projectile.localAI[0] == 0)
            {
                Vector2 start = Projectile.Center;
                Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                DustScatter(start, -0.5f);
                int length = (int)start.Distance(destination);
                if(length > 40)
                    DustScatter(destination, 0.5f);
                for (int f = 0; f < Math.Min(length, 45); f ++)
                {
                    Vector2 position = Vector2.Lerp(start, destination, Main.rand.NextFloat());
                    Dust dust = Dust.NewDustDirect(position - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.0f);
                    dust.scale *= 0.2f;
                    dust.scale += 0.75f;
                    dust.velocity *= 0.3f;
                    dust.noGravity = true;
                    dust.color = Color.Lerp(coreColor, Color.White, Main.rand.NextFloat(0.6f));
                    dust.fadeIn = 0.2f;
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 24f)
            {
                Projectile.Kill();
            }
        }
        public Color coreColor => ColorHelpers.TesseractColor(MathHelper.TwoPi * (Projectile.ai[2] % 10) / 10f, 0.5f);
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(0, 1);
            Vector2 start = Projectile.Center;
            Vector2 end = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 toEnd = end - start;
            float length = toEnd.Length() / 2;
            float progress = Projectile.localAI[0] / 24f;
            Main.spriteBatch.Draw(texture, start - Main.screenPosition, null, coreColor * (1 - progress), toEnd.ToRotation(), origin, new Vector2(length, 3f * (1 - progress)), SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, start - Main.screenPosition, null, Color.Black * (1 - progress), toEnd.ToRotation(), origin, new Vector2(length, 1f * (1 - progress)), SpriteEffects.None, 0f);
            return false;
        }
    }
}