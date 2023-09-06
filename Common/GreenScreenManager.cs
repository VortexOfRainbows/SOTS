using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.FakePlayer;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Common
{
    public class GreenScreenSetup : ModSystem
    {
        public override void Load()
        {
            GreenScreenManager.Initialize(Main.graphics.GraphicsDevice);
            GreenScreenManager.LoadContent();
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
            MagicWaterLayer.DrawGreenscreenOnInterfaces(spriteBatch, graphicsDevice);
            graphicsDevice.SetRenderTargets(prevTarget);
        }
        public static void DrawWaterLayer(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            MagicWaterLayer.DrawLayer(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
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
        public static RenderTarget2D RenderTarget;
        public static Texture2D LiquidNoise;
        public static void Initialize()
        {
            MaskedEntities = new List<IWaterSprite>();
            BorderColor = new Color(255, 233, 20);
        }
        public static void UpdateWindowSize(GraphicsDevice graphicsDevice, int width, int height)
        {
            Main.QueueMainThreadAction(() => GreenScreenManager.ResetRenderTarget2D(ref RenderTarget, graphicsDevice, width, height));
        }
        public static void DrawGreenscreenOnInterfaces(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (RenderTarget == null)
            {
                return;
            }
            graphicsDevice.SetRenderTarget(RenderTarget);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null);
            /*foreach (var s in MaskedEntities)
            {
                s.DrawMappedSprite(spriteBatch);
                if (s is Entity)
                {
                    if (s is Projectile a && !a.active)
                    {
                        MaskedEntities.Remove(s);
                    }
                }
                else
                {
                    MaskedEntities.Remove(s);
                }
            }*/
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    FakePlayerDrawing.DrawMyFakePlayers(player, 1, 0);
                }
            }
            spriteBatch.End();
            AddEffect(spriteBatch, graphicsDevice, RenderTarget, GreenScreenManager.greenScreenColor.Value);
        }
        public static void DrawLayer(SpriteBatch spriteBatch)
        {
            if (RenderTarget == null)
                return;
            LiquidNoise = GreenScreenManager.WaterNoise.Value;
            Effect WaterParallax = GreenScreenManager.magicWater.Value;
            WaterParallax.Parameters["screenWidth"].SetValue((float)Main.screenWidth / 2);
            WaterParallax.Parameters["screenHeight"].SetValue((float)Main.screenHeight / 2);
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
            Vector2 offset = Vector2.Zero;
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