using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.FakePlayer;
using SOTS.Projectiles.Tide;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Common
{
    public class GreenScreenSetup : ModSystem
    {
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                GreenScreenManager.Initialize(Main.graphics.GraphicsDevice);
                GreenScreenManager.LoadContent();
            }
        }
        public static Vector2 lastScreenSize;
        public static Vector2 lastViewSize;
        public override void PreUpdateEntities()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                /// Something in this region is causing the memory leak. probably calling UPDATEWINDOWSIZE too fast
                if (lastScreenSize != new Vector2(Main.screenWidth, Main.screenHeight))
                {
                    GreenScreenManager.UpdateWindowSize(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                }
                /// Something in this region is causing the memory leak. probably calling UPDATEWINDOWSIZE too fast
                lastScreenSize = new Vector2(Main.screenWidth, Main.screenHeight);
                lastViewSize = Main.ViewSize;
            }
        }
    }
    public static class GreenScreenManager
    {
        public static RenderTarget2D TempTarget;
        public static Asset<Effect> greenScreenColor;
        public static Asset<Effect> magicWater;
        public static Asset<Effect> nebula;
        public static Asset<Texture2D> WaterNoise;
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            UpdateWindowSize(graphicsDevice, Main.screenWidth, Main.screenHeight);
            MagicWaterLayer.Initialize();
        }
        public static void UpdateWindowSize(GraphicsDevice graphicsDevice, int width, int height)
        {
            MagicWaterLayer.UpdateWindowSize(graphicsDevice, width, height);
            Main.QueueMainThreadAction(() => ResetRenderTarget2D(ref TempTarget, graphicsDevice, width, height));
        }
        public static void ResetRenderTarget2D(ref RenderTarget2D target, GraphicsDevice graphicsDevice, int width, int height)
        {
            if(target != null)
                target.Dispose();
            target = new RenderTarget2D(graphicsDevice, width, height);
        }
        public static void LoadContent()
        {
            WaterNoise = ModContent.Request<Texture2D>("SOTS/TrailTextures/Trail_4", AssetRequestMode.ImmediateLoad);
            greenScreenColor = ModContent.Request<Effect>("SOTS/Effects/GreenScreen");
            nebula = ModContent.Request<Effect>("SOTS/Effects/NebulaSky");
            magicWater = ModContent.Request<Effect>("SOTS/Effects/MagicWater");
        }
        public static void SetupGreenscreens(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            RenderTargetBinding[] prevTarget = graphicsDevice.GetRenderTargets();
            MagicWaterLayer.DrawOntoRenderTarget(spriteBatch, graphicsDevice, ref MagicWaterLayer.RenderTargetFakePlayerWings, DrawStateID.Wings);
            MagicWaterLayer.DrawOntoRenderTarget(spriteBatch, graphicsDevice, ref MagicWaterLayer.RenderTargetFakePlayerBody, DrawStateID.Body);
            MagicWaterLayer.DrawOntoRenderTarget(spriteBatch, graphicsDevice, ref MagicWaterLayer.RenderTargetFakePlayerFrontArm, DrawStateID.FrontArm);
            MagicWaterLayer.DrawOntoRenderTarget(spriteBatch, graphicsDevice, ref MagicWaterLayer.RenderTargetPlayerHoldsWaterBall, -1);
            graphicsDevice.SetRenderTargets(prevTarget);
        }
        public static void DrawWaterLayer(SpriteBatch spriteBatch, ref RenderTarget2D RenderTarget, bool returnToGameZoomMatrix = false)
        {
            spriteBatch.End();
            MagicWaterLayer.DrawLayer(spriteBatch, ref RenderTarget);
            if(!returnToGameZoomMatrix)
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            else
            {
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            }
        }
    }
    public interface IWaterSprite
    {
        /// <summary>
        /// Draw parts of sprite that are color coded, and should be drawn with the metaball layer (green screened). Water shader is activated on this sprite later.
        /// </summary>
        /// <param name="spriteBatch"></param>
        void DrawMappedSprite(SpriteBatch spriteBatch);
    }
    public static class MagicWaterLayer
    {
        public static Color BorderColor;
        public static List<IWaterSprite> MaskedEntities;
        public static RenderTarget2D RenderTargetFakePlayerWings;
        public static RenderTarget2D RenderTargetFakePlayerBody;
        public static RenderTarget2D RenderTargetFakePlayerFrontArm;
        public static RenderTarget2D RenderTargetPlayerHoldsWaterBall;
        public static Texture2D LiquidNoise;
        public static void Initialize()
        {
            MaskedEntities = new List<IWaterSprite>();
            BorderColor = new Color(255, 233, 20);
        }
        public static void UpdateWindowSize(GraphicsDevice graphicsDevice, int width, int height)
        {
            Main.QueueMainThreadAction(() => GreenScreenManager.ResetRenderTarget2D(ref RenderTargetFakePlayerWings, graphicsDevice, width, height));
            Main.QueueMainThreadAction(() => GreenScreenManager.ResetRenderTarget2D(ref RenderTargetFakePlayerBody, graphicsDevice, width, height));
            Main.QueueMainThreadAction(() => GreenScreenManager.ResetRenderTarget2D(ref RenderTargetFakePlayerFrontArm, graphicsDevice, width, height));
            Main.QueueMainThreadAction(() => GreenScreenManager.ResetRenderTarget2D(ref RenderTargetPlayerHoldsWaterBall, graphicsDevice, width, height));
        }
        public static void DrawOntoRenderTargetProjectile(bool drawBorder = false)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if(proj.active && proj.type == ModContent.ProjectileType<WaterShark>() && proj.ModProjectile is WaterShark ws)
                {
                    Color white = Color.White;
                    if(drawBorder)
                        ws.Draw(true);
                    else 
                        ws.Draw(false);
                }
            }
        }
        public static void DrawOntoRenderTarget(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, ref RenderTarget2D RenderTarget, int DrawState)
        {
            if (RenderTarget == null)
            {
                return;
            }
            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            if (DrawState == DrawStateID.Wings)
            {
                DrawOntoRenderTargetProjectile(true);
                ParticleHelper.DrawWaterParticles();
                DrawOntoRenderTargetProjectile(false);
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    if (DrawState != -1)
                    {
                        FakePlayerDrawing.DrawMyFakePlayers(player, 1, DrawState);
                    }
                    else if(FakePlayer.FakePlayer.CheckItemValidityFull(player, player.HeldItem, player.HeldItem, 1) && FakeModPlayer.ModPlayer(player).hasHydroFakePlayer) //This may cause repeated draws in multiplayer due to where the ball is rendered, but this should ultimately be more optimal than the solution (making each player have a seperate render target for the held ball)
                    {
                        FakePlayerDrawing.DrawHeldHydroBall(player);
                    }
                }
            }
            spriteBatch.End();
            AddEffect(spriteBatch, graphicsDevice, RenderTarget, GreenScreenManager.greenScreenColor.Value);
        }
        public static void DrawLayer(SpriteBatch spriteBatch, ref RenderTarget2D RenderTarget)
        {
            if (RenderTarget == null)
                return;
            LiquidNoise = GreenScreenManager.WaterNoise.Value;
            Effect WaterParallax = GreenScreenManager.magicWater.Value;
            WaterParallax.Parameters["screenWidth"].SetValue((float)Main.screenWidth / Main.GameViewMatrix.Zoom.X);
            WaterParallax.Parameters["screenHeight"].SetValue((float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y);
            WaterParallax.Parameters["width"].SetValue(256);
            WaterParallax.Parameters["height"].SetValue(256);
            WaterParallax.Parameters["scale"].SetValue(new Vector2(Main.screenHeight / 4, Main.screenHeight / 4));
            WaterParallax.Parameters["NoiseTexture0"].SetValue(LiquidNoise);
            WaterParallax.Parameters["NoiseTexture1"].SetValue(LiquidNoise);
            Vector2 circular = new Vector2(256, 0).RotatedBy(SOTSWorld.GlobalCounter / 1200f * MathHelper.TwoPi);
            circular.Y *= 0.25f;
            WaterParallax.Parameters["offset"].SetValue(Main.player[Main.myPlayer].Center * 0.5f + circular);
            WaterParallax.Parameters["twoPi"].SetValue(MathHelper.TwoPi);
            WaterParallax.Parameters["time"].SetValue(SOTSWorld.GlobalCounter / 1200f * MathHelper.TwoPi);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);

            WaterParallax.CurrentTechnique.Passes[0].Apply();
            Vector2 offset = new Vector2((float)Main.screenWidth / 2, (float)Main.screenHeight / 2);
            spriteBatch.Draw(RenderTarget, offset, null, Color.White, 0, offset, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        public static void AddEffect(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, RenderTarget2D target, Effect effect)
        {
            graphicsDevice.SetRenderTarget(GreenScreenManager.TempTarget);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            effect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(target, position: Vector2.Zero, color: Color.White);

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            spriteBatch.Draw(GreenScreenManager.TempTarget, position: Vector2.Zero, color: Color.White);

            spriteBatch.End();
        }
    }
}