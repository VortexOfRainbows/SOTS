using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Wings;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class MachinaBooster : PlayerDrawLayer
	{
		public Texture2D[] wingAssets = null;
        public bool HasBooster(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, "MachinaBooster", EquipType.Wings);
        public bool HasBladeWings(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, "GildedBladeWings", EquipType.Wings);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return HasBooster(drawInfo) || HasBladeWings(drawInfo);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);
		public static Color changeColorBasedOnStealth(Color color, PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			var shadow = drawInfo.shadow;
			if (drawPlayer.inventory[drawPlayer.selectedItem].type == ItemID.PsychoKnife)
			{
				var num23 = drawPlayer.stealth;
				if (num23 < 0.03)
					num23 = 0.03f;
				var num24 = (float)((1.0 + num23 * 10.0) / 11.0);
				if (num23 < 0.0)
					num23 = 0.0f;
				if (num23 >= 1.0 - shadow && shadow > 0.0)
					num23 = shadow * 0.5f;
				color = new Color((int)(byte)(color.R * num23),
					(byte)(color.G * num23),
					(byte)(color.B * num24),
					(byte)(color.A * num23));
			}
			else if (drawPlayer.shroomiteStealth)
			{
				var num23 = drawPlayer.stealth;
				if (num23 < 0.03)
					num23 = 0.03f;
				var num24 = (float)((1.0 + num23 * 10.0) / 11.0);
				if (num23 < 0.0)
					num23 = 0.0f;
				if (num23 >= 1.0 - shadow && shadow > 0.0)
					num23 = shadow * 0.5f;
				color = new Color((byte)(color.R * (double)num23),
					(byte)(color.G * num23),
					(byte)(color.B * num24),
					(byte)(color.A * num23));
			}
			else if (drawPlayer.setVortex)
			{
				var num23 = drawPlayer.stealth;
				if (num23 < 0.03)
					num23 = 0.03f;
				if (num23 < 0.0)
					num23 = 0.0f;
				if (num23 >= 1.0 - shadow && shadow > 0.0)
					num23 = shadow * 0.5f;
				Color secondColor = new Color(Vector4.Lerp(Vector4.One, new Vector4(0.0f, 0.12f, 0.16f, 0.0f), 1f - num23));
				color = color.MultiplyRGBA(secondColor);
			}
			return color;
		}
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (Main.dresserInterfaceDummy == drawInfo.drawPlayer)
				return;
			if (drawInfo.drawPlayer.dead || drawInfo.drawPlayer.mount.Active)
			{
				return;
			}
			if(HasBladeWings(drawInfo))
            {
                DrawBladeWings(ref drawInfo);
            }
            if(HasBooster(drawInfo))
            {
                DrawMachinaWings(ref drawInfo);
            }
		}
		private void DrawMachinaWings(ref PlayerDrawSet drawInfo)
		{
            if (wingAssets == null)
            {
                wingAssets = new Texture2D[11];
                wingAssets[0] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[1] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart4Base", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[2] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart4Base2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[3] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart5", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[4] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart5_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[5] = Mod.Assets.Request<Texture2D>("Items/Wings/WingBooster2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[6] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart4EffectFill", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[7] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart4EffectOutline", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[8] = Mod.Assets.Request<Texture2D>("Items/Wings/WingBooster2Effect", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[9] = Mod.Assets.Request<Texture2D>("Items/Wings/WingBooster2Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                wingAssets[10] = Mod.Assets.Request<Texture2D>("Items/Wings/WingPart4Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            MachinaBoosterPlayer MachinaBoosterPlayer = drawPlayer.GetModPlayer<MachinaBoosterPlayer>();
            //Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/MachinaBooster_Wings_Glow").Value;
            Texture2D smallPiece = wingAssets[0];
            Texture2D bigPiece = wingAssets[1];
            Texture2D bigPieceAlt = wingAssets[2];
            Texture2D mediumPiece = wingAssets[3];
            Texture2D mediumPieceAlt = wingAssets[4];
            Texture2D boosterPiece = wingAssets[5];
            Texture2D bonusPiece = wingAssets[6];
            Texture2D bonusPiece2 = wingAssets[7];
            Texture2D bonusPiece3 = wingAssets[8];
            Texture2D boosterPieceGlow = wingAssets[9];
            Texture2D wingPieceGlow = wingAssets[10];

            float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
            //drawX -= 9 * drawPlayer.direction;
            float drawY = (int)drawInfo.Position.Y + drawPlayer.height / 2;
            //drawY += 2 * drawPlayer.gravDir;


            Vector2 position = new Vector2(drawX, drawY) - Main.screenPosition;
            float alpha = 1 - drawInfo.shadow;
            float dustAlpha = 1 - drawInfo.shadow;
            dustAlpha *= drawPlayer.stealth;
            alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
            Color color = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX / 16, (int)drawY / 16)); //apply lighting to wings
            color = changeColorBasedOnStealth(color, drawInfo);
            //Rectangle frame = new Rectangle(0, Main.wingsTexture[drawPlayer.wings].Height / 4 * drawPlayer.wingFrame, Main.wingsTexture[drawPlayer.wings].Width, Main.wingsTexture[drawPlayer.wings].Height / 4);
            float rotation = drawPlayer.bodyRotation;
            SpriteEffects spriteEffects = drawInfo.playerEffect;

            int mode = drawPlayer.wingFrame;
            List<DrawData> drawData = new List<DrawData>();
            List<DrawData> drawData2 = new List<DrawData>();
            List<DrawData> drawData3 = new List<DrawData>();
            List<DrawData> drawData4 = new List<DrawData>();
            int wingNum = 0;
            for (int j = 0; j < 2; j++)
            {
                int direction = (j * 2) - 1;
                direction *= -drawPlayer.direction;
                Vector2 currentPos = position;
                currentPos.X -= direction + drawPlayer.direction;
                currentPos.Y -= 4 * drawPlayer.gravDir;
                if (mode == 0)
                {
                    currentPos.X -= direction * 4;
                }
                for (int i = 0; i < 10; i++)
                {
                    float scale = 7.5f - (i == 0 ? 0 : j);
                    scale /= 7.5f;
                    if (mode == 0 && i != 0)
                        scale *= 0.85f;
                    float rotationI = MathHelper.ToRadians((i - 2) * -(7.5f + (j == 1 ? 0.5f : 0)) * direction * drawPlayer.gravDir);
                    if (mode == 0)
                    {
                        rotationI = MathHelper.ToRadians((64 + (j == 1 ? 4f : 0)) * direction * drawPlayer.gravDir);
                    }
                    Vector2 fromPlayer = new Vector2(-6 * direction * scale, 0).RotatedBy(rotation - rotationI);
                    if (mode == 0)
                    {
                        fromPlayer = new Vector2((-1f - 0.75f * scale) * direction, 0).RotatedBy(rotation - rotationI);
                    }
                    currentPos += fromPlayer;
                    Texture2D currentTexture = smallPiece;

                    if (i == 0)
                    {
                        currentTexture = j % 2 == 0 ? mediumPiece : mediumPieceAlt;
                        if (j == 1)
                            currentPos.X += 2 * direction;
                        if (mode != 0)
                            currentPos.X -= 4 * direction;
                    }
                    else if (i == 1)
                    {
                        if (j == 1 && mode == 0)
                            currentPos.X -= direction * 2;
                        if (mode == 0)
                            currentPos.X -= 1 * direction;
                        if (mode != 0)
                            currentPos.X += 3 * direction;
                    }
                    else if (i % 3 == 0)
                        currentTexture = j % 2 == 0 ? bigPiece : bigPieceAlt;

                    Vector2 origin = new Vector2(currentTexture.Width / 2, currentTexture.Height / 2);
                    if (currentTexture == bigPiece || currentTexture == bigPieceAlt)
                    {
                        origin = new Vector2(currentTexture.Width / 2, 8);
                        if (drawPlayer.gravDir == -1)
                        {
                            origin = new Vector2(currentTexture.Width / 2, currentTexture.Height - 8);
                        }
                    }
                    /*if (type == (int)EpicWingType.Blocky && i % 3 == 0 && mode == 0 && i != 0)
					{
						rotationI += MathHelper.ToRadians(90 * direction * drawPlayer.gravDir);
					}*/
                    if (/*type == (int)EpicWingType.Default && */i % 3 == 0 && mode == 0 && i != 0)
                    {
                        rotationI += MathHelper.ToRadians(26 * direction * drawPlayer.gravDir);
                    }
                    bool booster = i != 0 && i % 3 == 0 && drawPlayer.wingFrame == 2;
                    if (booster)
                    {
                        wingNum++;
                        currentTexture = boosterPiece;
                        Vector2 rotationOrigin = new Vector2(-2.75f * direction, 6f) - drawPlayer.velocity;
                        Vector2 currentPos2 = currentPos + Main.screenPosition;
                        float overrideRotation = rotationOrigin.ToRotation();
                        Vector2 dustVelo = new Vector2(7.2f, 0).RotatedBy(overrideRotation);
                        /*if (type == (int)EpicWingType.Default)
						{*/
                        Color color2 = new Color(110, 110, 110, 0);
                        color2 = changeColorBasedOnStealth(color2, drawInfo);
                        for (int k = 0; k < 6; k++)
                        {
                            float x = Main.rand.Next(-10, 11) * 0.1f;
                            float y = Main.rand.Next(-10, 11) * 0.1f;
                            DrawData data2 = (new DrawData(bonusPiece3, currentPos + new Vector2(x, y), null, color2 * alpha * alpha, rotation + overrideRotation - MathHelper.ToRadians(90), origin, scale, spriteEffects, 0));
                            data2.shader = drawInfo.cWings;
                            drawData3.Add(data2);
                        }
                        if (drawInfo.shadow == 0f)
                        {
                            Color colorDust = changeColorBasedOnStealth(Color.White, drawInfo);
                            int dustType = ModContent.DustType<CopyDust>();
                            if (j == 0)
                                dustType = ModContent.DustType<CopyDust3>();
                            int index = Dust.NewDust(currentPos2 + dustVelo * 1.5f * scale + new Vector2(-4, -4), 0, 0, dustType, 0, 0, 0, colorDust);
                            Dust dust = Main.dust[index];
                            dust.noGravity = true;
                            dust.fadeIn = 0.1f;
                            dust.velocity = dustVelo;
                            dust.scale += 0.3f;
                            dust.scale *= scale;
                            dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
                            dust.alpha = (int)(0.7f * (int)(255 - colorDust.A));
                            drawInfo.DustCache.Add(index);
                        }
                        /*}
						else
						{
							if (drawInfo.shadow == 0f)
							{
								int index = Dust.NewDust(currentPos2 + dustVelo * 1.5f + new Vector2(-5, -5), 0, 0, ModContent.DustType("CubeDust").Type);
								Dust dust = Main.dust[index];
								dust.noGravity = true;
								dust.fadeIn = 0.5f;
								dust.velocity *= 0.8f;
								dust.velocity += dustVelo;
								dust.scale *= scale;
								dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
								dust.alpha = (int)(0.7f * (255 - (255 * dustAlpha)));
								Main.playerDrawDust.Add(index);
							}
						}*/
                        DrawData data = (new DrawData(currentTexture, currentPos, null, color * alpha, rotation + overrideRotation - MathHelper.ToRadians(90), origin, scale, spriteEffects, 0));
                        data.shader = drawInfo.cWings;
                        drawData.Add(data);
                        data = (new DrawData(boosterPieceGlow, currentPos, null, changeColorBasedOnStealth(Color.White, drawInfo) * alpha, rotation + overrideRotation - MathHelper.ToRadians(90), origin, scale, spriteEffects, 0));
                        data.shader = drawInfo.cWings;
                        drawData4.Add(data);
                        Color color3 = new Color(60, 70, 80, 0) * 0.4f;
                        for (int i2 = 0; i2 < 360; i2 += 30)
                        {
                            Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i2));
                            data = (new DrawData(boosterPieceGlow, currentPos + addition, null, changeColorBasedOnStealth(color3, drawInfo) * alpha, rotation + overrideRotation - MathHelper.ToRadians(90), origin, scale, spriteEffects, 0));
                            data.shader = drawInfo.cWings;
                            drawData4.Add(data);
                        }
                    }
                    else
                    {
                        Vector2 vector2_1 = Vector2.Zero;
                        if (i == 0)
                        {
                            vector2_1 = fromPlayer;
                            rotationI = 0f;
                        }
                        DrawData data = (new DrawData(currentTexture, currentPos - vector2_1, null, color * alpha, rotation - rotationI, origin, scale, spriteEffects, 0));
                        data.shader = drawInfo.cWings;
                        drawData.Add(data);
                        if (/*type == (int)EpicWingType.Default && */i % 3 == 0 && i != 0)
                        {
                            data = (new DrawData(wingPieceGlow, currentPos - vector2_1, null, changeColorBasedOnStealth(Color.White, drawInfo) * alpha, rotation - rotationI, origin, scale, spriteEffects, 0));
                            data.shader = drawInfo.cWings;
                            drawData4.Add(data);
                        }
                        if (/*type == (int)EpicWingType.Default && */mode == 1 && (i % 3 == 0 && i != 0))
                        {
                            if (drawPlayer.controlJump)
                            {
                                Color color3 = new Color(60, 70, 80, 0) * 0.4f;
                                for (int i2 = 0; i2 < 360; i2 += 30)
                                {
                                    Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i2));
                                    data = (new DrawData(wingPieceGlow, currentPos + addition - vector2_1, null, changeColorBasedOnStealth(color3, drawInfo) * alpha, rotation - rotationI, origin, scale, spriteEffects, 0));
                                    data.shader = drawInfo.cWings;
                                    drawData4.Add(data);
                                }
                            }
                            Color color2 = new Color(110, 110, 110, 0);
                            color2 = changeColorBasedOnStealth(color2, drawInfo);
                            if (!drawPlayer.controlJump)
                                color2 *= 0.5f;
                            int amt = drawPlayer.controlJump ? 6 : 2;
                            for (int k = 0; k < amt; k++)
                            {
                                float x = Main.rand.Next(-10, 11) * 0.1f;
                                float y = Main.rand.Next(-10, 11) * 0.1f;
                                if (k == 0)
                                {
                                    DrawData data3 = (new DrawData(bonusPiece, currentPos, null, color2 * alpha, rotation - rotationI, origin, scale, spriteEffects, 0));
                                    data3.shader = drawInfo.cWings;
                                    drawData2.Add(data3);
                                }
                                if (k < 4)
                                {
                                    x = 0;
                                    y = 0;
                                }
                                Vector2 tilt2 = new Vector2(0, 4.5f).RotatedBy(MathHelper.ToRadians(MachinaBoosterPlayer.randCounter * 20));
                                Vector2 tilt3 = new Vector2(tilt2.X, 0).RotatedBy(rotation - rotationI);
                                Vector2 tilt = new Vector2(0, 1).RotatedBy(rotation - rotationI) * drawPlayer.gravDir;
                                Vector2 currentPos2 = currentPos - vector2_1 + Main.screenPosition;
                                if (/*type == (int)EpicWingType.Default &&*/Main.rand.NextBool(18) && drawPlayer.controlJump)
                                {
                                    if (drawInfo.shadow == 0f)
                                    {
                                        Color colorDust = changeColorBasedOnStealth(Color.White, drawInfo);
                                        int dustType = ModContent.DustType<CopyDust>();
                                        if (j == 0)
                                            dustType = ModContent.DustType<CopyDust3>();
                                        int index = Dust.NewDust(currentPos2 + tilt3 * scale + tilt2 * scale + (tilt * Main.rand.Next(16, 34) * scale) + new Vector2(-4, -4), 0, 0, dustType, 0, 0, 0, colorDust);
                                        Dust dust = Main.dust[index];
                                        dust.noGravity = true;
                                        dust.fadeIn = 0.6f;
                                        dust.velocity *= 0;
                                        dust.scale *= 0.45f;
                                        dust.scale *= scale;
                                        dust.shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
                                        dust.alpha = (int)(0.7f * (int)(255 - colorDust.A));
                                        drawInfo.DustCache.Add(index);
                                    }
                                }
                                DrawData data2 = (new DrawData(bonusPiece2, currentPos + new Vector2(x, y), null, color2 * alpha * alpha, rotation - rotationI, origin, scale, spriteEffects, 0));
                                data2.shader = drawInfo.cWings;
                                drawData2.Add(data2);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < drawData2.Count; i++)
                drawInfo.DrawDataCache.Add(drawData2[i]);
            for (int i = 0; i < drawData.Count; i++)
                if ((i % 10) % 3 != 0 && mode != 0)
                    drawInfo.DrawDataCache.Add(drawData[i]);
            for (int i = 0; i < drawData3.Count; i++)
                drawInfo.DrawDataCache.Add(drawData3[i]);
            for (int i = 0; i < drawData.Count; i++) //add the more important layers on so they don't have weired overlap
                if ((i % 10) % 3 == 0 || mode == 0)
                    drawInfo.DrawDataCache.Add(drawData[i]);
            for (int i = 0; i < drawData4.Count; i++)
                drawInfo.DrawDataCache.Add(drawData4[i]);
        }
        private void DrawBladeWings(ref PlayerDrawSet drawInfo)
        {
            //if(drawInfo.shadow != 0f)
            //{
            //    return;
            //}a
            Player drawPlayer = drawInfo.drawPlayer;
            MachinaBoosterPlayer mbPlayer = drawPlayer.GetModPlayer<MachinaBoosterPlayer>();
            List<DrawData> dataTrails = new List<DrawData>();
            List<DrawData> dataBack = new List<DrawData>();
            List<DrawData> dataMiddle = new List<DrawData>();
            List<DrawData> dataFront = new List<DrawData>();
            Texture2D blade = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWing", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeF = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingFlipped", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeOutline = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingOutline", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeOutlineF = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingOutlineFlipped", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeHandle = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingHandle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeHandleF = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingHandleFlipped", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D pixel = Mod.Assets.Request<Texture2D>("Items/Secrets/WhitePixel", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
            float drawY = (int)drawInfo.Position.Y + drawPlayer.height / 2;
            drawX -= 2 * drawPlayer.direction;
            float alpha = 1 - drawInfo.shadow;
            alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
            Color color = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX / 16, (int)drawY / 16)); //apply lighting to wings
            color = changeColorBasedOnStealth(color, drawInfo);
            //Rectangle frame = new Rectangle(0, Main.wingsTexture[drawPlayer.wings].Height / 4 * drawPlayer.wingFrame, Main.wingsTexture[drawPlayer.wings].Width, Main.wingsTexture[drawPlayer.wings].Height / 4);
            float rotation = drawPlayer.bodyRotation;
            SpriteEffects spriteEffects = drawInfo.playerEffect;
            Vector2 bladeOrigin = blade.Size() / 2;
            float counter = mbPlayer.FlightCounter + 40;
            int bladeID = drawPlayer.direction == 1 ? 9 : 0;
            for (int i = -1; i <= 1; i += 2)
            {
                bool Front = i * drawPlayer.gravDir == 1;
                float direction = drawPlayer.direction * i;
                for (int j = -4; j <= 4; j++)
                {
                    Vector2 position = new Vector2(drawX, drawY) - Main.screenPosition;
                    int proper = j * i * (int)drawPlayer.direction;
                    float scale = (Front ? 0.7f : 0.625f) + 0.05f * j;
                    float bonusDist = 0;
                    if (Math.Abs(j) % 2 == 1)
                    {
                        scale *= 0.75f;
                        bonusDist = 20;
                    }

                    //Creative Flight
                    float sinusoid = MathF.Sin(MathHelper.ToRadians(counter * 2 + 10 * j)) * 5.5f * direction;
                    Vector2 creativeOffset = new Vector2(-58 * direction, 0).RotatedBy(MathHelper.ToRadians(proper * 18.5f + sinusoid + 2 * direction)) * scale;
                    float creativeRotation = -MathHelper.ToRadians(45 * direction - 21 * proper - sinusoid);
                    creativeOffset.Y += 2;

                    //Normal Flight
                    sinusoid = MathF.Sin(MathHelper.ToRadians(counter * 2 + 12 * j));
                    sinusoid *= (34 - j) * direction;
                    sinusoid += 12 * direction;
                    Vector2 normalOffset = new Vector2(-(54 + bonusDist) * direction, 4).RotatedBy(MathHelper.ToRadians(proper * 6.5f + sinusoid)) * scale;
                    float normalRotation = -MathHelper.ToRadians(76 * direction - 13 * proper - sinusoid);

                    //Grounded
                    proper -= 6 * (int)direction;
                    sinusoid = MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2 + 12 * j)) * (16 - j) * direction;
                    Vector2 groundedOffset = new Vector2(-(64 + bonusDist * scale) * direction, 4).RotatedBy(MathHelper.ToRadians(proper * 6 + sinusoid * 0.2f)) * scale;
                    groundedOffset.X *= 0.4f;
                    if (!Front)
                    {
                        groundedOffset.Y *= 1.1f;
                    }
                    float groundedRotation = -MathHelper.ToRadians(72 * direction - 6 * proper - sinusoid);
                    float groundedScale = scale * 0.75f;

                    Vector2 finalOffset;
                    float finalRotation;
                    float finalScale;

                    if(mbPlayer.FlightModeFloat > 1)
                    {
                        float lerpAmt = mbPlayer.FlightModeFloat - 1;
                        finalOffset = Vector2.Lerp(normalOffset, creativeOffset, lerpAmt);
                        finalRotation = MathHelper.Lerp(normalRotation, creativeRotation, lerpAmt);
                        finalScale = scale;
                        position.Y += 6 * lerpAmt * drawPlayer.gravDir;
                        finalScale += lerpAmt * 0.05f; 
                    }
                    else
                    {
                        float lerpAmt = mbPlayer.FlightModeFloat;
                        finalOffset = Vector2.Lerp(groundedOffset, normalOffset, lerpAmt);
                        finalRotation = MathHelper.Lerp(groundedRotation, normalRotation, lerpAmt);
                        finalScale = MathHelper.Lerp(groundedScale, scale, lerpAmt);
                        position.Y -= 24 * (1 - lerpAmt) * drawPlayer.gravDir;
                    }
                    Color finalColor1 = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter + j * 20), true), 0.85f);
                    Color finalColor2 = finalColor1;
                    finalColor2.A = 0;
                    Vector2 bladePosition = position + finalOffset * drawPlayer.gravDir;
                    dataBack.Add(new DrawData(Front ? blade : bladeF, bladePosition, null, finalColor1 * alpha * alpha * 0.8f, rotation + finalRotation, bladeOrigin, finalScale, spriteEffects, 0));
                    dataMiddle.Add(new DrawData(Front ? bladeOutline : bladeOutlineF, bladePosition, null, finalColor2 * alpha * alpha * 1.2f, rotation + finalRotation, bladeOrigin, finalScale, spriteEffects, 0));
                    dataFront.Add(new DrawData(Front ? bladeHandle : bladeHandleF, bladePosition, null, color * alpha, rotation + finalRotation, bladeOrigin, finalScale, spriteEffects, 0));

                    if(!Main.gamePaused && !Main.gameMenu)
                    {
                        mbPlayer.WingsBeingVisualized = true;
                        if (drawInfo.shadow == 0f && mbPlayer.BladeWingTrails != null && mbPlayer.BladeWingTrails[bladeID] != null) // Add dust to end of blades
                        {
                            Vector2 offset = ((-bladeOrigin + new Vector2(2, 2)) * finalScale * drawPlayer.gravDir).RotatedBy(rotation + finalRotation + (direction == -1 ? MathHelper.ToRadians(76) : 0));
                            Vector2 dustPosition = bladePosition + offset + Main.screenPosition;
                            mbPlayer.BladeWingTrails[bladeID].Insert(0, dustPosition);
                        }
                    }
                    bladeID++;
                    bladeID %= 18;
                }
            }
            if(mbPlayer.BladeWingTrails != null && !Main.gameMenu && drawInfo.shadow == 0f)
            {
                Vector2 trailOrigin = new Vector2(0, 1);
                float trailSpriteLength = pixel.Width;
                for (int i = 0; i < mbPlayer.BladeWingTrails.Length; i++)
                {
                    int bladeNum = i % 9;
                    float scaleF = 0.5f + bladeNum * 0.0625f;
                    List<Vector2> list = mbPlayer.BladeWingTrails[i];
                    if(list.Count > 0)
                    {
                        Vector2 previous = list[0];
                        for (int j = 1; j < list.Count; j++)
                        {
                            float alpha2 = 1 - (j / (float)list.Count);
                            Vector2 current = list[j];
                            Vector2 toPrevious = previous - current;
                            Color finalColor1 = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter + (bladeNum - 4) * 20 + j * 6), true), 0.85f);
                            finalColor1.A = 0;
                            dataTrails.Add(new DrawData(pixel, current - Main.screenPosition, null, finalColor1 * alpha * alpha2 * scaleF * 0.8f, toPrevious.ToRotation(), trailOrigin, new Vector2(toPrevious.Length() / trailSpriteLength, 0.5f * scaleF + 1f * alpha2), SpriteEffects.None, 0));
                            previous = current;
                        }
                    }
                }
            }
            for (int i = 0; i < dataTrails.Count; i++)
            {
                DrawData data = dataTrails[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
            }
            for (int i = 0; i < dataBack.Count; i++)
            {
                DrawData data = dataBack[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
                data = dataMiddle[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
                if(dataFront.Count > i)
                {
                    data = dataFront[i];
                    data.shader = drawInfo.cWings;
                    drawInfo.DrawDataCache.Add(data);
                }
            }
        }
    }
    public class Halo : PlayerDrawLayer
    {
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return HasBladeWings(drawInfo);
        }
        public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (Main.dresserInterfaceDummy == drawInfo.drawPlayer)
                return;
            if (drawInfo.drawPlayer.dead || drawInfo.drawPlayer.mount.Active)
            {
                return;
            }
            if (HasBladeWings(drawInfo))
            {
                DrawHalo(ref drawInfo);
            }
        }
        public bool HasBladeWings(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, "GildedBladeWings", EquipType.Wings);
        private void DrawHalo(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0)
                return;
            Player drawPlayer = drawInfo.drawPlayer;
            MachinaBoosterPlayer mbPlayer = drawPlayer.GetModPlayer<MachinaBoosterPlayer>();
            List<DrawData> drawData0 = new List<DrawData>();
            List<DrawData> drawData1 = new List<DrawData>();
            List<DrawData> drawData2 = new List<DrawData>();
            Texture2D pixel = Mod.Assets.Request<Texture2D>("Items/Secrets/WhitePixel", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int repeats = 40;
            Vector2 center = new Vector2((int)drawPlayer.Center.X, (int)drawPlayer.Center.Y + drawPlayer.gfxOffY) + new Vector2(-10 * drawPlayer.direction, -(drawPlayer.height / 2 + 6) * drawPlayer.gravDir);
            Vector2[] points = new Vector2[repeats];
            float[] scales = new float[repeats];
            Vector2[] points2 = new Vector2[repeats / 2];
            float[] scales2 = new float[repeats / 2];
            Vector2 bonusPoint = Vector2.Zero;
            float bonusScale = 0f;
            float tilt = 35f + MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 1.5f)) * 5f;
            float zTilt = 1f + MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.75f)) * 0.2f;
            for (int i = 0; i < repeats; i++)
            {
                float rotation = i / (float)repeats * MathHelper.TwoPi + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.75f);
                float offset = 11;
                int pointN = i % repeats;
                float offsetBonus = 0;
                if (pointN == 39)
                    offsetBonus += 18;
                if (pointN == 2)
                    offsetBonus += 14;
                if (pointN == 5)
                    offsetBonus += 14;
                if (pointN == 10)
                    offsetBonus += 10;
                if (pointN == 14)
                    offsetBonus += 20;
                if (pointN == 17)
                    offsetBonus += 14;
                if (pointN == 20)
                    offsetBonus += 8;
                if (pointN == 27)
                    offsetBonus += 10;
                if (pointN == 31)
                    offsetBonus += 16;
                offset += offsetBonus * (0.85f + 0.15f * MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * (2 + i / 40f) + i * 24))) * 0.95f;

                if(i % 2 == 0)
                {
                    Vector2 innerCircular = new Vector2(7, 0).RotatedBy(rotation);
                    innerCircular.Y *= 0.45f * drawPlayer.gravDir * zTilt;
                    innerCircular.X *= drawPlayer.direction;
                    innerCircular = innerCircular.RotatedBy(MathHelper.ToRadians(tilt * -drawPlayer.direction * drawPlayer.gravDir));
                    points2[i / 2] = center + innerCircular;
                    scales2[i / 2] = 0.7f + MathF.Sin(rotation) * 0.1f / zTilt;
                }

                Vector2 circular = new Vector2(offset, 0).RotatedBy(rotation);
                circular.Y *= 0.55f * drawPlayer.gravDir * zTilt;
                circular.X *= drawPlayer.direction;
                circular = circular.RotatedBy(MathHelper.ToRadians(tilt * -drawPlayer.direction * drawPlayer.gravDir));
                points[i] = center + circular;
                scales[i] = 0.8f + MathF.Sin(rotation) * 0.2f / zTilt;
                if (pointN == 37)
                {
                    Vector2 bonusPos = new Vector2(5, 0).RotatedBy(rotation);
                    bonusPos.Y *= 0.55f * drawPlayer.gravDir * zTilt;
                    bonusPos.X *= drawPlayer.direction;
                    bonusPos = bonusPos.RotatedBy(MathHelper.ToRadians(tilt * -drawPlayer.direction * drawPlayer.gravDir));
                    bonusPoint = points[i] + bonusPos;
                    bonusScale = scales[i];
                }
            }
            Vector2 previous = points[points.Length - 1];
            for (int i = 0; i < points.Length; i++)
            {
                Color color = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2) + i / (float)repeats * MathHelper.TwoPi, true), 0.8f);
                Color color2 = color * 1.1f;
                color.A = 0;
                Vector2 current = points[i];
                Vector2 toPrevious = previous - current;
                drawData0.Add(new DrawData(pixel, points[i] - Main.screenPosition, null, color, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, scales[i] * 0.8f), SpriteEffects.None, 0));
                drawData2.Add(new DrawData(pixel, points[i] - Main.screenPosition, null, color2, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, scales[i]) + Vector2.One * 0.6f, SpriteEffects.None, 0));
                previous = current;
            }
            previous = points2[points2.Length - 1];
            for (int i = 0; i < points2.Length; i++)
            {
                Color color = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * -1) + i / (float)repeats * MathHelper.TwoPi * 2, true), 0.8f);
                Color color2 = color * 1.1f;
                color.A = 0;
                Vector2 current = points2[i];
                Vector2 toPrevious = previous - current;
                drawData1.Add(new DrawData(pixel, points2[i] - Main.screenPosition, null, color, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, scales2[i] * 0.8f), SpriteEffects.None, 0));
                drawData2.Add(new DrawData(pixel, points2[i] - Main.screenPosition, null, color2, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, scales2[i]) + Vector2.One * 0.6f, SpriteEffects.None, 0));
                previous = current;
            }
            Color finalColor2 = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2) + 37f / (float)repeats * MathHelper.TwoPi, true), 0.8f);
            Color finalColor3 = finalColor2 * 1.1f;
            finalColor2.A = 0;
            drawData0.Add(new DrawData(pixel, bonusPoint - Main.screenPosition, null, finalColor2, 0f, new Vector2(1, 1), bonusScale + 0.2f, SpriteEffects.None, 0));
            drawData2.Add(new DrawData(pixel, bonusPoint - Main.screenPosition, null, finalColor3, 0f, new Vector2(1, 1), bonusScale + 0.8f, SpriteEffects.None, 0));
            for (int i = 0; i < drawData2.Count; i++)
            {
                DrawData data = drawData2[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
            }
            for (int i = 0; i < drawData1.Count; i++)
            {
                DrawData data = drawData1[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
            }
            for (int i = 0; i < drawData0.Count; i++)
            {
                DrawData data = drawData0[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
}