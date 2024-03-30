using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SOTS.Common;
using System;

namespace SOTS.FakePlayer
{
    public static class FakePlayerDrawing
    {
        public static Color DrawColor(FakePlayer fakePlayer)
        {
            if(fakePlayer.FakePlayerType == FakePlayerTypeID.Tesseract)
            {
                return ColorHelpers.TesseractColor(MathHelper.TwoPi * (fakePlayer.OverrideUseSlot % 10) / 10f, 0.5f);
            }
            return Color.White;
        }
        public static Texture2D WingTexture(int type, bool outLine)
        {
            if (outLine)
            {
                if (type == FakePlayerTypeID.Hydro)
                {
                    return ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroServantWingsOutline").Value;
                }
                return ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantWingsOutline").Value;
            }
            if (type == FakePlayerTypeID.Hydro)
            {
                return ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroServantWings").Value;
            }
            return ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantWings").Value;
        }
        public static Texture2D FakePlayerTexture(int type, bool outLine, int tesseractType)
        {
            if(outLine)
            {
                if (type == FakePlayerTypeID.Tesseract)
                {
                    if(tesseractType == 2 || tesseractType == 3)
                    {
                        return ModContent.Request<Texture2D>("SOTS/FakePlayer/Tesseract/TesseractServantSheetWhite").Value;
                    }
                    return ModContent.Request<Texture2D>("SOTS/FakePlayer/Tesseract/TesseractServantSheetWhiteMale").Value;
                }
                if (type == FakePlayerTypeID.Hydro)
                {
                    return ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroServantSheetOutline").Value;
                }
                return ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheetGreen").Value;
            }
            if(type == FakePlayerTypeID.Tesseract)
            {
                return ModContent.Request<Texture2D>("SOTS/FakePlayer/Tesseract/TesseractServantSheet" + (tesseractType + 1)).Value;
            }
            if (type == FakePlayerTypeID.Hydro)
            {
                return ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroServantSheet").Value;
            }
            return ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheet").Value;
        }
        public static void DrawFrontArm(FakePlayer fakePlayer, SpriteBatch spriteBatch)
        {
            Texture2D texture = FakePlayerTexture(fakePlayer.FakePlayerType, false, fakePlayer.OverrideUseSlot % 10);
            SpriteEffects spriteEffects = fakePlayer.playerEffect;
            Vector2 vector = new Vector2((int)fakePlayer.Position.X, (int)fakePlayer.Position.Y) - Main.screenPosition + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 - 3);
            if (fakePlayer.compFrontArmFrame.X / fakePlayer.compFrontArmFrame.Width >= 7)
            {
                vector += new Vector2((!fakePlayer.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!fakePlayer.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
            }
            Vector2 origin = fakePlayer.bodyVect;
            Vector2 position = vector + GetCompositeOffset_FrontArm(fakePlayer);
            Color color = DrawColor(fakePlayer);
            Rectangle frame = fakePlayer.compFrontArmFrame;
            float rotation = fakePlayer.compositeFrontArmRotation;
            spriteBatch.Draw(texture, position, frame, color, rotation, origin + GetCompositeOffset_FrontArm(fakePlayer), 1f, spriteEffects, 0);
        }
        public static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawInfo)
        {
            return new Vector2(6 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
        }
        public static void DrawBackArm(FakePlayer fakePlayer, ref PlayerDrawSet drawInfo, bool outline)
        {
            Texture2D texture = FakePlayerTexture(fakePlayer.FakePlayerType, false, fakePlayer.OverrideUseSlot % 10);
            Texture2D textureGreen = FakePlayerTexture(fakePlayer.FakePlayerType, true, fakePlayer.OverrideUseSlot % 10);
            Vector2 vector = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y) - Main.screenPosition + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 - 3);
            Vector2 vector3 = vector;
            Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawInfo);
            vector3 += compositeOffset_BackArm;
            float rotation = drawInfo.compositeBackArmRotation;
            Color color = DrawColor(fakePlayer);
            if (outline)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                    PlayerDrawLayers.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.BackArm, new DrawData(textureGreen, vector3 + circular, drawInfo.compBackArmFrame, color, rotation, drawInfo.bodyVect + compositeOffset_BackArm, 1f, drawInfo.playerEffect, 0));
                }
            }
            else
            {
                PlayerDrawLayers.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.BackArm, new DrawData(texture, vector3, drawInfo.compBackArmFrame, color, rotation, drawInfo.bodyVect + compositeOffset_BackArm, 1f, drawInfo.playerEffect, 0)
                {
                    //shader = drawInfo.cBody
                });
            }
        }
        public static void DrawTail(FakePlayer fakePlayer, ref PlayerDrawSet drawInfo, bool outLine = false)
        {
            if (fakePlayer.FakePlayerType == FakePlayerTypeID.Subspace || fakePlayer.FakePlayerType == FakePlayerTypeID.Tesseract)
            {
                Texture2D texture = fakePlayer.FakePlayerType == FakePlayerTypeID.Tesseract ? ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractServantTail").Value : ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantTail").Value;
                Texture2D textureOutline = fakePlayer.FakePlayerType == FakePlayerTypeID.Tesseract ? ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractServantTailOutline").Value : ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantTailOutline").Value;
                Vector2 center = drawInfo.Position + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 + 2);
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                Vector2 velo = new Vector2(0, 4f);
                float scale = 1f;
                List<Vector2> positions = new List<Vector2>();
                List<float> rotations = new List<float>();
                for (int i = 0; i < 9; i++)
                {
                    Vector2 toOldPosition = fakePlayer.SecondPosition - drawInfo.Position;
                    toOldPosition.SafeNormalize(Vector2.Zero);
                    velo += toOldPosition * 0.333f;
                    velo = velo.SafeNormalize(Vector2.Zero) * scale * 4;
                    center += velo;
                    Vector2 drawPos = center - Main.screenPosition + new Vector2(0, -16 + FakePlayer.Height / 2);
                    positions.Add(drawPos);
                    rotations.Add(velo.ToRotation() - MathHelper.ToRadians(90));
                    scale -= 0.0725f;
                }
                if (outLine)
                {
                    for (int i = 8; i >= 0; i--)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                            drawInfo.DrawDataCache.Add(new DrawData(textureOutline, positions[i] + circular, new Rectangle(0, 0, texture.Width, texture.Height), DrawColor(fakePlayer), rotations[i], origin, (1 - i * 0.08f), fakePlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
                        }
                    }
                }
                else
                {
                    for (int i = 8; i >= 0; i--)
                    {
                        drawInfo.DrawDataCache.Add(new DrawData(texture, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), DrawColor(fakePlayer), rotations[i], origin, 1 - i * 0.08f, fakePlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
                    }
                    if(fakePlayer.FakePlayerType == FakePlayerTypeID.Subspace)
                    {
                        Texture2D textureInner = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantTailScales").Value;
                        for (int i = 8; i >= 0; i--)
                        {
                            drawInfo.DrawDataCache.Add(new DrawData(textureInner, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), DrawColor(fakePlayer), rotations[i], origin, 1 - i * 0.08f, fakePlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
                        }
                    }
                }
            }
        }
        public static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawInfo)
        {
            return new Vector2(-5 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        }
        public static Vector2 GetCompositeOffset_FrontArm(FakePlayer fakePlayer)
        {
            return new Vector2(-5 * ((!fakePlayer.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        }
        public static void DrawFrontArm(FakePlayer fakePlayer, ref PlayerDrawSet drawInfo, bool outline)
        {
            Texture2D texture = FakePlayerTexture(fakePlayer.FakePlayerType, false, fakePlayer.OverrideUseSlot % 10);
            Texture2D textureGreen = FakePlayerTexture(fakePlayer.FakePlayerType, true, fakePlayer.OverrideUseSlot % 10);
            SpriteEffects spriteEffects = drawInfo.playerEffect;
            Vector2 vector = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y) - Main.screenPosition + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 - 3);
            if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            {
                vector += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
            }
            Vector2 origin = drawInfo.bodyVect;
            Vector2 position = vector + GetCompositeOffset_FrontArm(ref drawInfo);
            Color color = DrawColor(fakePlayer);
            Rectangle frame = drawInfo.compFrontArmFrame;
            float rotation = drawInfo.compositeFrontArmRotation;
            if (outline)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                    DrawData drawData2 = new DrawData(textureGreen, position + circular, frame, color, rotation, origin + GetCompositeOffset_FrontArm(ref drawInfo), 1f, spriteEffects, 0);
                    drawInfo.DrawDataCache.Add(drawData2);
                }
            }
            else
            {
                DrawData drawData = new DrawData(texture, position, frame, color, rotation, origin + GetCompositeOffset_FrontArm(ref drawInfo), 1f, spriteEffects, 0);
                //drawData.shader = drawInfo.cBody;
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
        public static void DrawBody(FakePlayer fakePlayer, ref PlayerDrawSet drawInfo, bool outline)
        {
            Texture2D texture = FakePlayerTexture(fakePlayer.FakePlayerType, false, fakePlayer.OverrideUseSlot % 10);
            Texture2D textureGreen = FakePlayerTexture(fakePlayer.FakePlayerType, true, fakePlayer.OverrideUseSlot % 10);
            Player drawPlayer = drawInfo.drawPlayer;
            float drawX = (int)drawInfo.Position.X + FakePlayer.Width / 2;
            float drawY = (int)drawInfo.Position.Y + FakePlayer.Height - drawPlayer.bodyFrame.Height / 2 + 4f;
            Vector2 origin = drawInfo.bodyVect;
            Vector2 position = new Vector2(drawX, drawY) - Main.screenPosition;
            Color color = DrawColor(fakePlayer);
            Rectangle frame = new Rectangle(0, 0, 40, 56); //very first box
            SpriteEffects spriteEffects = drawInfo.playerEffect;
            if (outline)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                    DrawData drawData2 = new DrawData(textureGreen, position + circular, frame, color, 0f, origin, 1f, spriteEffects, 0);
                    drawInfo.DrawDataCache.Add(drawData2);
                }
            }
            else
            {
                DrawData drawData = new DrawData(texture, position, frame, color, 0f, origin, 1f, spriteEffects, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
        public static void DrawWings(FakePlayer fakePlayer, ref PlayerDrawSet drawInfo, int Frame, bool outline)
        {
            if (fakePlayer.FakePlayerType == FakePlayerTypeID.Tesseract)
                return;
            int Direction = drawInfo.drawPlayer.direction;
            Texture2D texture = WingTexture(fakePlayer.FakePlayerType, false);
            Texture2D textureOutline = WingTexture(fakePlayer.FakePlayerType, true);
            Vector2 drawPos = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y) + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2) - Main.screenPosition + new Vector2(-8 * Direction, -5);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
            if(fakePlayer.FakePlayerType == 1)
            {
                drawPos += new Vector2(1 * Direction, 3);
            }
            if (Frame < 0)
            {
                Frame = 0;
            }
            if (Frame > 5)
            {
                Frame = 5;
            }
            if(outline)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                    DrawData drawData2 = new DrawData(textureOutline, drawPos + circular, new Rectangle(0, Frame * texture.Height / 6, texture.Width, texture.Height / 6), DrawColor(fakePlayer), 0, origin, 1f, Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(drawData2);
                }
            }
            else
            {
                DrawData drawData = new DrawData(texture, drawPos, new Rectangle(0, Frame * texture.Height / 6, texture.Width, texture.Height / 6), DrawColor(fakePlayer), 0, origin, 1f, Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
        public static void DrawHydroFakePlayersFull()
        {
            DrawFakePlayers(1, DrawStateID.Border);
            GreenScreenManager.DrawWaterLayer(Main.spriteBatch, ref MagicWaterLayer.RenderTargetFakePlayerWings); //Draws the water player wings sprites
            DrawFakePlayers(1, DrawStateID.HeldItemAndProjectilesBeforeBackArm);
            GreenScreenManager.DrawWaterLayer(Main.spriteBatch, ref MagicWaterLayer.RenderTargetFakePlayerBody); //Draws the water player body sprites
            DrawFakePlayers(1, DrawStateID.HeldItemAndProjectilesBeforeFrontArm);
            GreenScreenManager.DrawWaterLayer(Main.spriteBatch, ref MagicWaterLayer.RenderTargetFakePlayerFrontArm); //Draws the front arm of the water player
            DrawFakePlayers(1, DrawStateID.HeldItemAndProjectilesAfterFrontArm);
        }
        public static void DrawFakePlayers(int fakePlayerType, int DrawState)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    DrawMyFakePlayers(player, fakePlayerType, DrawState); //Draws the items and projectiles from the water player
                }
            }
        }
        public static void DrawMyFakePlayers(Player player, int drawType, int drawState)
        {
            List<FakePlayer> fakePlayers = GetFakePlayers(player);
            for(int i = 0; i < fakePlayers.Count; i++)
            {
                FakePlayer fakePlayer = fakePlayers[i];
                PlayerDrawSet drawInfo = new PlayerDrawSet();
                if(fakePlayer.FakePlayerType == drawType)
                {
                    bool canIDraw = fakePlayer.PrepareDrawing(ref drawInfo, player, drawState);
                    if(fakePlayer.FakePlayerType == FakePlayerTypeID.Hydro)
                    {
                        if(drawState == DrawStateID.Border || drawState == DrawStateID.Wings)
                        {
                            if (FakePlayer.CheckItemValidityFull(player, player.HeldItem, player.HeldItem, 1))
                            {
                                Main.spriteBatch.End();
                                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                                if(drawState == DrawStateID.Border)
                                    DrawHydroConnection(player, fakePlayer, true);
                                else
                                    DrawHydroConnection(player, fakePlayer, false);
                            }
                        }
                    }
                    if (canIDraw)
                    {
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                        DrawFromCache(ref drawInfo);
                        fakePlayer.DrawFrontHandAndHeldProj(Main.spriteBatch, player, drawState);
                    }
                }
            }
        }
        public static void SetupCompositeDrawing(ref PlayerDrawSet drawInfo, FakePlayer fakePlayer, Player player)
        {
            drawInfo.drawPlayer = player;
            drawInfo.drawPlayer.compositeBackArm = fakePlayer.compositeBackArm;
            drawInfo.drawPlayer.compositeFrontArm = fakePlayer.compositeFrontArm;
            drawInfo.drawPlayer.bodyFrame = fakePlayer.bodyFrame;
            drawInfo.drawPlayer.Male = true;
            drawInfo.drawPlayer.selectedItem = fakePlayer.UseItemSlot(player);
            drawInfo.drawPlayer.lastVisualizedSelectedItem = player.inventory[drawInfo.drawPlayer.selectedItem];
            //Main.NewText(DrawInfoDummy.compFrontArmFrame.ToString() + " : " + DrawInfoDummy.usesCompositeTorso + " : " + DrawInfoDummy.drawPlayer.body);
            drawInfo.drawPlayer.heldProj = fakePlayer.HeldProj;
            drawInfo.Position = fakePlayer.Position;
            drawInfo.BoringSetup(player, new List<DrawData>(), new List<int>(), new List<int>(), Vector2.Zero, 0f, 0f, Vector2.Zero);
            //Main.NewText(DrawInfoDummy.compFrontArmFrame.ToString() + " : " + DrawInfoDummy.usesCompositeTorso + " : " + DrawInfoDummy.drawPlayer.body);
            fakePlayer.heldProjOverHand = drawInfo.heldProjOverHand;
            fakePlayer.compFrontArmFrame = drawInfo.compFrontArmFrame;
            fakePlayer.compBackArmFrame = drawInfo.compBackArmFrame;
            fakePlayer.compositeFrontArmRotation = drawInfo.compositeFrontArmRotation;
            fakePlayer.compositeBackArmRotation = drawInfo.compositeBackArmRotation;
            fakePlayer.weaponDrawOrder = (int)drawInfo.weaponDrawOrder;
            if(drawInfo.weaponOverFrontArm || Terraria.Item.claw[drawInfo.heldItem.type])
            {
                fakePlayer.weaponDrawOrder = 2;
            }
            fakePlayer.bodyVect = drawInfo.bodyVect;
        }
        private static SpriteDrawBuffer spriteBuffer;
        public static void DrawFromCache(ref PlayerDrawSet drawinfo)
        {
            //IL_0124: Unknown result type (might be due to invalid IL or missing references)
            List<DrawData> drawDataCache = drawinfo.DrawDataCache;
            if (spriteBuffer == null)
            {
                spriteBuffer = new SpriteDrawBuffer(Main.graphics.GraphicsDevice, 200);
            }
            else
            {
                spriteBuffer.CheckGraphicsDevice(Main.graphics.GraphicsDevice);
            }
            foreach (DrawData item in drawDataCache)
            {
                if (item.texture != null)
                {
                    item.Draw(spriteBuffer);
                }
            }
            spriteBuffer.UploadAndBind();
            DrawData cdd;
            int num = 0;
            for (int i = 0; i <= drawDataCache.Count; i++)
            {
                if (i != drawDataCache.Count)
                {
                    cdd = drawDataCache[i];
                    if (!cdd.sourceRect.HasValue)
                    {
                        cdd.sourceRect = cdd.texture.Frame();
                    }
                    PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
                    if (cdd.texture != null)
                    {
                        spriteBuffer.DrawSingle(num++);
                    }
                }
            }
            spriteBuffer.Unbind();
        }
        public static List<FakePlayer> GetFakePlayers(Player player)
        {
            List<FakePlayer> ret = new List<FakePlayer>();
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if(proj.active)
                {
                    if (proj.ModProjectile is FakePlayerPossessingProjectile fppp && proj.owner == player.whoAmI)
                    {
                        ret.Add(fppp.FakePlayer);
                    }
                }
            }
            return ret;
        }
        public static void DrawHydroConnection(Player player, FakePlayer fakePlayer, bool border = false)
        {
            Texture2D waterBall = ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroBall").Value;
            Texture2D waterBallOutline = ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroBallOutline").Value;
            Texture2D waterBallLine = ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroBallLightning").Value;
            Texture2D waterBallLineOutline = ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroBallLightningOutline").Value;
            Vector2 origin = waterBall.Size() / 2;
            Vector2 center = fakePlayer.Position + new Vector2(FakePlayer.Width, FakePlayer.Height) / 2;
            Vector2 playerCenter = PlayerBallCenter(player);
            float length = Vector2.Distance(center, playerCenter);
            float textureSize = waterBallLine.Width;
            float max = length / textureSize + 60;
            int totalHelixes = 4;
            Vector2 origin2 = new Vector2(waterBallLine.Width / 2, waterBallLine.Height / 2);
            if (border)
            {
                for (int a = 0; a < 4; a++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * a));
                    Main.spriteBatch.Draw(waterBallOutline, center - Main.screenPosition + circular, null, DrawColor(fakePlayer), 0f, origin, 1f, SpriteEffects.None, 0);
                }
            }
            else
            {
                Main.spriteBatch.Draw(waterBall, center - Main.screenPosition, null, DrawColor(fakePlayer), 0f, origin, 1f, SpriteEffects.None, 0);
            }
            for (float v = 0; v < max; v += 0.2f)
            {
                Vector2 drawPos = Vector2.Lerp(center, playerCenter, v / max);
                Vector2 direction = playerCenter - center;
                float rotation = direction.ToRotation();
                float heightOfHelix = (float)Math.Sin(v / max * MathHelper.Pi) * 1.01f;
                float scaleOfHelix = 1f - heightOfHelix;
                for (int b = 0; b < totalHelixes; b++)
                {
                    float scaleY = 1f - 0.1f * b;
                    if (scaleOfHelix * scaleY > 0.02f)
                    {
                        float size = 6 + b;
                        float sinusoidMod = 1 + (float)Math.Sin(MathHelper.ToRadians(scaleY * SOTSWorld.GlobalCounter * 4f));
                        float sizeOfHelix = sinusoidMod * heightOfHelix * size;
                        Vector2 sinusoid = new Vector2(0, sizeOfHelix * (float)Math.Sin(MathHelper.ToRadians(v * 6 + SOTSWorld.GlobalCounter * 3 + b * 90))).RotatedBy(rotation);
                        if (!border)
                        {
                            Main.spriteBatch.Draw(waterBallLine, drawPos - Main.screenPosition + sinusoid, null, DrawColor(fakePlayer), rotation, origin2, new Vector2(0.5f, scaleY * scaleOfHelix), SpriteEffects.None, 0f);
                        }
                        else
                        {
                            float scaleForOneBonusPixelX = 2f / waterBallLine.Width;
                            float scaleForOneBonusPixelY = 4f / waterBallLine.Height * scaleOfHelix;
                            Main.spriteBatch.Draw(waterBallLineOutline, drawPos - Main.screenPosition + sinusoid, null, DrawColor(fakePlayer), rotation, origin2, new Vector2(0.5f + scaleForOneBonusPixelX, scaleY * scaleOfHelix + scaleForOneBonusPixelY), SpriteEffects.None, 0f);
                        }
                    }
                }
            }
        }
        public static Vector2 PlayerBallCenter(Player player)
        {
            float rotation = (player.compositeFrontArm.rotation + player.compositeBackArm.rotation) / 2f;
            Vector2 rotated = new Vector2(0, 12).RotatedBy(rotation);
            rotated.Y *= 1.5f;
            return player.RotatedRelativePoint(player.MountedCenter) + new Vector2(-2 * player.direction, 12).RotatedBy(rotation) + new Vector2(0, -2);
        }
        public static void DrawHeldHydroBall(Player player)
        {
            Texture2D waterBall = ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroBallCore").Value;
            Texture2D waterBallOutline = ModContent.Request<Texture2D>("SOTS/FakePlayer/HydroBallOutlineRed").Value;
            Vector2 origin = waterBall.Size() / 2;
            Vector2 center = PlayerBallCenter(player);
            for (int k = 0; k < 4; k++)
            {
                Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                Main.spriteBatch.Draw(waterBallOutline, center - Main.screenPosition + circular, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(waterBall, center - Main.screenPosition, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0);
        }
    }
}