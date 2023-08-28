
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SOTS.FakePlayer;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.GameContent.UI.Elements;

namespace SOTS.FakePlayer
{
    public static class FakePlayerDrawing
    {
        public static void DrawFrontArm(FakePlayer fakePlayer, SpriteBatch spriteBatch)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheet").Value;
            SpriteEffects spriteEffects = fakePlayer.playerEffect;
            Vector2 vector = new Vector2((int)fakePlayer.Position.X, (int)fakePlayer.Position.Y) - Main.screenPosition + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 - 3);
            if (fakePlayer.compFrontArmFrame.X / fakePlayer.compFrontArmFrame.Width >= 7)
            {
                vector += new Vector2((!fakePlayer.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!fakePlayer.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
            }
            Vector2 origin = fakePlayer.bodyVect;
            Vector2 position = vector + GetCompositeOffset_FrontArm(fakePlayer);
            Color color = Color.White;
            Rectangle frame = fakePlayer.compFrontArmFrame;
            float rotation = fakePlayer.compositeFrontArmRotation;
            spriteBatch.Draw(texture, position, frame, color, rotation, origin + GetCompositeOffset_FrontArm(fakePlayer), 1f, spriteEffects, 0);
        }
        public static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawInfo)
        {
            return new Vector2(6 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
        }
        public static void DrawBackArm(ref PlayerDrawSet drawInfo, bool green)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheet").Value;
            Texture2D textureGreen = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheetGreen").Value;
            Vector2 vector = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y) - Main.screenPosition + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 - 3);
            Vector2 vector3 = vector;
            Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawInfo);
            vector3 += compositeOffset_BackArm;
            float rotation = drawInfo.compositeBackArmRotation;
            Color color = Color.White;
            if (green)
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
        public static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawInfo)
        {
            return new Vector2(-5 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        }
        public static Vector2 GetCompositeOffset_FrontArm(FakePlayer fakePlayer)
        {
            return new Vector2(-5 * ((!fakePlayer.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        }
        public static void DrawFrontArm(ref PlayerDrawSet drawInfo, bool green)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheet").Value;
            Texture2D textureGreen = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheetGreen").Value;
            SpriteEffects spriteEffects = drawInfo.playerEffect;
            Vector2 vector = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y) - Main.screenPosition + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2 - 3);
            if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            {
                vector += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
            }
            Vector2 origin = drawInfo.bodyVect;
            Vector2 position = vector + GetCompositeOffset_FrontArm(ref drawInfo);
            Color color = Color.White;
            Rectangle frame = drawInfo.compFrontArmFrame;
            float rotation = drawInfo.compositeFrontArmRotation;
            if (green)
            {
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                    DrawData drawData2 = new DrawData(textureGreen, position + circular, frame, color, rotation, origin + GetCompositeOffset_FrontArm(ref drawInfo), 1f, spriteEffects, 0);
                    //drawData.shader = drawInfo.cBody;
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
        public static void DrawBody(ref PlayerDrawSet drawInfo, bool green)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheet").Value;
            Texture2D textureGreen = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantSheetGreen").Value;
            Player drawPlayer = drawInfo.drawPlayer;
            float drawX = (int)drawInfo.Position.X + FakePlayer.Width / 2;
            float drawY = (int)drawInfo.Position.Y + FakePlayer.Height - drawPlayer.bodyFrame.Height / 2 + 4f;
            Vector2 origin = drawInfo.bodyVect;
            Vector2 position = new Vector2(drawX, drawY) - Main.screenPosition;
            Color color = Color.White;
            Rectangle frame = new Rectangle(0, 0, 40, 56); //very first box
            SpriteEffects spriteEffects = drawInfo.playerEffect;
            if (green)
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
        public static void DrawWings(ref PlayerDrawSet drawInfo, int Frame)
        {
            int Direction = drawInfo.drawPlayer.direction;
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantWings").Value;
            Texture2D textureOutline = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantWingsOutline").Value;
            Vector2 drawPos = new Vector2((int)drawInfo.Position.X, (int)drawInfo.Position.Y) + new Vector2(FakePlayer.Width / 2, FakePlayer.Height / 2) - Main.screenPosition + new Vector2(-8 * Direction, -5);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 6 / 2);
            if (Frame < 0)
            {
                Frame = 0;
            }
            if (Frame > 5)
            {
                Frame = 5;
            }
            for (int k = 0; k < 4; k++)
            {
                Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                DrawData drawData2 = new DrawData(textureOutline, drawPos + circular, new Rectangle(0, Frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, 0, origin, 1f, Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                drawInfo.DrawDataCache.Add(drawData2);
            }
            DrawData drawData = new DrawData(texture, drawPos, new Rectangle(0, Frame * texture.Height / 6, texture.Width, texture.Height / 6), Color.White, 0, origin, 1f, Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            drawInfo.DrawDataCache.Add(drawData);
        }
        public static void DrawMyFakePlayers(Player player)
        {
            List<FakePlayer> fakePlayers = GetFakePlayers(player);
            for(int i = 0; i < fakePlayers.Count; i++)
            {
                FakePlayer fakePlayer = fakePlayers[i];
                PlayerDrawSet drawInfo = new PlayerDrawSet();
                bool MayCommitToDraw = fakePlayer.DrawFakePlayer(ref drawInfo, player);
                if(MayCommitToDraw)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                    DrawFakePlayer(ref drawInfo);
                    fakePlayer.SecondaryFakePlayerDrawing(Main.spriteBatch, player);
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
            drawInfo.drawPlayer.selectedItem = fakePlayer.UseItemSlot;
            drawInfo.drawPlayer.lastVisualizedSelectedItem = player.inventory[fakePlayer.UseItemSlot];
            //Main.NewText(DrawInfoDummy.compFrontArmFrame.ToString() + " : " + DrawInfoDummy.usesCompositeTorso + " : " + DrawInfoDummy.drawPlayer.body);
            drawInfo.drawPlayer.heldProj = fakePlayer.HeldProj;
            drawInfo.BoringSetup(player, new List<DrawData>(), new List<int>(), new List<int>(), Vector2.Zero, 0f, 0f, Vector2.Zero);
            //Main.NewText(DrawInfoDummy.compFrontArmFrame.ToString() + " : " + DrawInfoDummy.usesCompositeTorso + " : " + DrawInfoDummy.drawPlayer.body);
            fakePlayer.heldProjOverHand = drawInfo.heldProjOverHand;
            fakePlayer.compFrontArmFrame = drawInfo.compFrontArmFrame;
            fakePlayer.compBackArmFrame = drawInfo.compBackArmFrame;
            fakePlayer.compositeFrontArmRotation = drawInfo.compositeFrontArmRotation;
            fakePlayer.compositeBackArmRotation = drawInfo.compositeBackArmRotation;
            fakePlayer.weaponDrawOrder = (int)drawInfo.weaponDrawOrder;
            fakePlayer.bodyVect = drawInfo.bodyVect;
        }
        private static SpriteDrawBuffer spriteBuffer;
        public static void DrawFakePlayer(ref PlayerDrawSet drawinfo)
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
    }
}