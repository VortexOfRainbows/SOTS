using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Wings;
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
            Player drawPlayer = drawInfo.drawPlayer;
            List<DrawData> drawDataFront = new List<DrawData>();
            List<DrawData> drawDataBack = new List<DrawData>();
            Texture2D blade = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWing", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeF = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingFlipped", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeBase = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingBase", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Texture2D bladeBaseF = Mod.Assets.Request<Texture2D>("Items/Wings/BladeWingBaseFlipped", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
            float drawY = (int)drawInfo.Position.Y + drawPlayer.height / 2;
            drawX -= 1 * drawPlayer.direction;
            drawY -= 6 * drawPlayer.gravDir;

            int mode = drawPlayer.wingFrame;
            bool InCreativeFlight = mode == 2;
            bool InNormalFlight = mode == 1;
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
            Vector2 bladeOrigin = blade.Size() / 2;
            Vector2 bladeBaseOrigin = bladeBase.Size() / 2;
            for (int i = -1; i <= 1; i += 2)
            {
                bool Front = i == 1;
                float scale = Front ? 1f : 0.95f;
                float direction = drawPlayer.direction * i;
                for (int j = -1; j <= 1; j++)
                {
                    int proper = j * i * (int)drawPlayer.direction;
                    Vector2 circularOffset1 = Vector2.Zero; 
                    Vector2 circularOffset2 = Vector2.Zero;
                    float rotation1 = -MathHelper.ToRadians(45 * direction * drawPlayer.gravDir - 35 * proper);
                    float rotation2 = MathHelper.ToRadians(90 * direction * drawPlayer.gravDir + 35 * proper);
                    if (InCreativeFlight)
                    {
                        circularOffset1 = new Vector2(-66 * direction, 6 * drawPlayer.gravDir).RotatedBy(MathHelper.ToRadians(proper * 35)) * scale;
                        circularOffset2 = new Vector2(-52 * direction, 0).RotatedBy(MathHelper.ToRadians(proper * 35)) * scale;
                    }
                    else
                    {
                        circularOffset1 = new Vector2(-50 * direction, 6 * drawPlayer.gravDir).RotatedBy(MathHelper.ToRadians(proper * 15)) * scale;
                        circularOffset2 = new Vector2(-(36 - 4 * proper * direction) * direction, 0).RotatedBy(MathHelper.ToRadians(proper * 15)) * scale;
                        rotation1 = -MathHelper.ToRadians(72 * direction * drawPlayer.gravDir - 24 * proper);
                        rotation2 = MathHelper.ToRadians(45 * direction * drawPlayer.gravDir + 24 * proper);
                    }
                    drawDataBack.Add(new DrawData(Front ? blade : bladeF, position + circularOffset1, null, color * alpha, rotation + rotation1, bladeOrigin, scale, spriteEffects, 0));
                    drawDataFront.Add(new DrawData(Front ? bladeBase : bladeBaseF, position + circularOffset2 * 0.5f, null, color * alpha, rotation + rotation2, bladeBaseOrigin, scale, spriteEffects, 0));
                }
            }
            for (int i = 0; i < drawDataBack.Count; i++)
            {
                DrawData data = drawDataBack[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
            }
            for (int i = 0; i < drawDataFront.Count; i++)
            {
                DrawData data = drawDataFront[i];
                data.shader = drawInfo.cWings;
                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
}