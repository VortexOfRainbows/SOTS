using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.MinionBuffs;
using SOTS.Common.GlobalNPCs;
using SOTS.WorldgenHelpers;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
	public class TesseractServant : FakePlayerPossessingProjectile
	{
        public override void SafeSetStaticDefaults()
        {
            Main.projPet[Type] = false;
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
        private bool runOnce = true;
        private float TotalTesseracts = 1;
        public int MyUniqueID => (int)Projectile.ai[1] % 10;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			if (runOnce)
			{
                if (Main.myPlayer == Projectile.owner)
                {
                    FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(player);
                    Projectile.ai[1] = fPlayer.tesseractPlayerCount;
                    Projectile.netUpdate = true;
                }
				runOnce = false;
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
            return true;
		}
        private int target = -1;
        private bool validItem;
        private bool lastSkipDraw = false;
        private int TrailingType = 0;
        public override void AI()
        {
            if (FakePlayer == null)
                return;
            Player player = Main.player[Projectile.owner];
            FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(player);
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
                else if (FakePlayer.itemAnimation <= 1)
                {
                    target = -1;
                } 
                /*else if(needsLOS && !Collision.CanHitLine(Projectile.Center - new Vector2(16, 16), 32, 32, prevTarget.position, prevTarget.width, prevTarget.height))
                {
                    target = -1;
                }*/
            }
            if (target == -1 && validItem)
            {
                target = SOTSNPCs.FindTarget_Basic(Projectile.Center, maxPursuitRange, this, true);
                if(target == -1)
                    target = SOTSNPCs.FindTarget_Basic(player.Center, maxPursuitRange, this, true);
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
                if (FakePlayer.itemAnimation <= 1 && Main.myPlayer == player.whoAmI)
                {
                    cursorArea = Main.MouseWorld;
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
                float appropriateMeleeDistance = -4;
                if (FakePlayer.heldItem != null && !FakePlayer.heldItem.IsAir)
                {
                    appropriateMeleeDistance += FakePlayer.heldItem.Size.Length() * FakePlayer.heldItem.scale;
                    if (FakePlayer.heldItem.CountsAsClass(DamageClass.SummonMeleeSpeed))
                    {
                        appropriateMeleeDistance += 128f;
                    }
                }
                if (appropriateMeleeDistance < 50)
                    appropriateMeleeDistance = 50;
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
                    if (length > maxPursuitRange + appropriateMeleeDistance)
                        length = maxPursuitRange + appropriateMeleeDistance;
                    float lengthToCursor = length;
                    toCursor = toCursor.SafeNormalize(Vector2.Zero) * lengthToCursor;
                    idlePosition += toCursor;
                    idlePosition.Y = cursorArea.Y;
                    idlePosition.X -= appropriateMeleeDistance * Math.Sign(Direction);
                }
                bool move = TrailingType != 0 || FakePlayer.itemAnimation <= 0 || !itemDataRegistered;
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
                        speed += 1;
                    }
                    if (lastSkipDraw != FakePlayer.SkipDrawing)
                    {
                        Projectile.position = idlePosition;
                        Projectile.velocity = Vector2.Zero;
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
                /*if (SinusoidCounter >= 24)
                {
                    SinusoidCounter -= 24f;
                }
                Vector2 circular = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(15 * SinusoidCounter));
                SinusoidCounter += 0.75f;
                if (circular.Y > 0)
                    circular.Y *= 0.5f;
                Projectile.velocity.Y += circular.Y;*/

                if(foundTarget)
                {
                    FakePlayer.ForceItemUse = hasLOS || FakePlayer.itemAnimation > 0;
                }
                lastSkipDraw = FakePlayer.SkipDrawing;
                if(needsLOS && TrailingType != TrailingID.IDLE)
                {
                    Projectile.velocity = Collision.TileCollision(Projectile.Center - new Vector2(16, 16), Projectile.velocity, 32, 32, true, true);
                }
                CheckNoCollisionNearby();
                FakePlayer.CheckTesseractShouldDraw(player, TrailingType);
                UpdateItems(player);
                //Projectile.velocity = FakePlayer.Velocity;
            }

            Main.mouseX = oldMouseX;
            Main.mouseY = oldMouseY;
            if (Main.myPlayer == player.whoAmI) //might be excessive but is the easiest way to sync everything
                Projectile.netUpdate = true;
        }
        private bool CollisionLineOfSight(NPC npc, Player player)
        {
            return Collision.CanHitLine(Projectile.Center - new Vector2(8, 8), 16, 16, npc.position, npc.width, npc.height) ||
                Collision.CanHitLine(player.Center - new Vector2(8, 8), 16, 16, npc.position, npc.width, npc.height);
        }
        private void CheckNoCollisionNearby()
        {
            float collideRange = 40;
            for(int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile other = Main.projectile[i];
                if(Projectile.whoAmI != i)
                {
                    if(other.type == Type)
                    {
                        Vector2 nudge = Projectile.Center - other.Center;
                        float dist = nudge.Length();
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
            bool drawItem = false;
            if (runOnce)
                return false;
            if (FakePlayer == null || !FakePlayer.SkipDrawing || !lastSkipDraw || TrailingType != TrailingID.IDLE)
                return false;
            else if(FakePlayer != null && FakePlayer.heldItem != null && !FakePlayer.heldItem.IsAir && validItem)
            {
                drawItem = true;
            }
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float bonus = MathHelper.TwoPi * Projectile.ai[1] / TotalTesseracts;
            Color drawColor = ColorHelpers.TesseractColor(bonus, 0.5f);
            Texture2D planetariumTexture = PlayerInventorySlotsManager.planetariumTextures;
            //drawPosition += new Vector2(0, -64);
            int frameY = (int)Projectile.ai[1] % 10;
            int height = planetariumTexture.Height / 10;
            DrawTesseract(drawColor * 0.8f, 3f);
            drawColor *= 0.5f;
            Rectangle frame = new Rectangle(0, height * frameY, planetariumTexture.Width, height);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(1f, 0).RotatedBy(i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(planetariumTexture, drawPosition + circular, frame, drawColor * 0.5f, 0f, new Vector2(planetariumTexture.Width, height) * 0.5f, 1f, SpriteEffects.None, 0f);
            }
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
                    Main.spriteBatch.Draw(whitePixel, points[i], null, color * 0.5f, 0f, Vector2.One, depths[i] * innerScale, SpriteEffects.None, 0f);
                    Vector2 toOtherPoint = previousPoint - points[i];
                    Main.spriteBatch.Draw(whitePixel, points[i], null, color, toOtherPoint.ToRotation(), new Vector2(0, 1f), new Vector2(toOtherPoint.Length() / 2f, innerScale * (depths[i] /2f + previousDepth / 2f)), SpriteEffects.None, 0f);
                }
                previousPoint = points[i];
                previousDepth = depths[i];
            }
        }
    }
}