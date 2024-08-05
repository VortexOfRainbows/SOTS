using Microsoft.Xna.Framework;
using SOTS.Projectiles.Blades;
using SOTS.Projectiles.Camera;
using System;
using Terraria;

namespace SOTS
{
    public static class ColorHelpers
    {
		public static Color AVDustColor => new Color(117, 120, 132, 200);
        public static Color ToothAcheLime => ToothAcheSlash.toothAcheLime;
        public static Vector3 AVIchorLight = new Vector3(0.85f, 0.81f, .22f);
        public static Vector3 AVCursedLight = new Vector3(1.28f, 0.93f, .4f);
        public static Color AcediaColor = new Color(213, 68, 255);
        public static Color GulaColor = new Color(255, 143, 128);
        public static Color DreamLampColor = DreamingFrame.Green1;
		public static Color soulLootingColor = new Color(66, 56, 111);
		public static Color destabilizeColor = new Color(80, 190, 80);
		public static Color pastelRainbow = Color.White;
		public static Color NatureColor = new Color(180, 240, 180);
		public static Color TideColor = new Color(64, 72, 178);
		public static Color PermafrostColor = new Color(200, 250, 250);
		public static Color EarthColor = new Color(230, 220, 145);
		public static Color OtherworldColor = new Color(167, 45, 225, 0);
        public static Color PurpleOtherworldColor = new Color(167, 45, 225, 0);
        public static Color VibrantColor = new Color(85, 125, 215, 0);
		public static Color LemegetonColor = new Color(255, 82, 97, 0);
		public static Color ChaosPink = new Color(231, 95, 203);
		public static Color EvilColor = new Color(100, 15, 0, 0);
        public static Color RedEvilColor = new Color(100, 15, 0, 0);
        public static Color Inferno1 = new Color(213, 68, 13);
		public static Color Inferno2 = new Color(255, 210, 155);
		public static int soulColorCounter = 0;
		public static Color VoidAnomaly = new Color(160, 120, 180);
		public static void ColorUpdate()
        {
			soulColorCounter++;
			float toRadians = MathHelper.ToRadians(soulColorCounter);
			destabilizeColor = Color.Lerp(new Color(80, 190, 80), new Color(64, 178, 172), 0.5f + (float)Math.Sin(toRadians * 1.25f) * 0.5f);
			soulLootingColor = Color.Lerp(new Color(66, 56, 111), new Color(171, 3, 35), 0.5f + (float)Math.Sin(toRadians * 1.5f) * 0.5f);
			float newAi = soulColorCounter * 3 / 13f;
			double frequency = 0.3;
			double center = 200;
			double width = 55;
			double red = Math.Sin(frequency * newAi) * width + center;
			double grn = Math.Sin(frequency * newAi + 2.0) * width + center;
			double blu = Math.Sin(frequency * newAi + 4.0) * width + center;
			pastelRainbow = new Color((int)red, (int)grn, (int)blu);
			NatureColor = Color.Lerp(new Color(192, 222, 143), new Color(45, 102, 46), 0.5f + (float)Math.Sin(toRadians) * 0.5f);
			EarthColor = Color.Lerp(new Color(253, 234, 157), new Color(142, 118, 43), 0.5f + (float)Math.Sin(toRadians * 2.5f) * 0.5f);
			TideColor = Color.Lerp(new Color(177, 187, 238), new Color(64, 72, 178), 0.5f + (float)Math.Sin(toRadians * 0.5f) * 0.5f);
			PermafrostColor = Color.Lerp(new Color(188, 217, 245), new Color(106, 148, 234), 0.5f + (float)Math.Sin(toRadians * 0.5f) * 0.5f);
			OtherworldColor = Color.Lerp(new Color(167, 45, 225, 0), new Color(64, 178, 172, 0), 0.5f + (float)Math.Sin(toRadians * 0.5f) * 0.5f);
			PurpleOtherworldColor = Color.Lerp(OtherworldColor, new Color(167, 45, 225, 0), 0.5f);
            EvilColor = Color.Lerp(new Color(55, 7, 0, 0), new Color(38, 18, 61, 0), 0.5f + (float)Math.Sin(toRadians * 1.25f) * 0.5f);
			RedEvilColor = Color.Lerp(EvilColor, new Color(255, 30, 0, 0), 0.5f) * 1.5f;
			VibrantColor = Color.Lerp(new Color(80, 120, 220, 0), new Color(180, 230, 100, 0), 0.5f + (float)Math.Sin(toRadians * 2.5f) * 0.5f);

			Color LemegetonRed = new Color(255, 82, 97);
			Color LemegetonGreen = new Color(104, 229, 101);
			Color LemegetonPurple = new Color(200, 119, 247);
			float lerpAmt = 1.5f + 3 * (float)Math.Sin(MathHelper.ToRadians(soulColorCounter * 0.33f)) * 0.5f;
			if (lerpAmt < 1)
			{
				LemegetonColor = Color.Lerp(LemegetonRed, LemegetonGreen, lerpAmt);
			}
			else if (lerpAmt < 2)
			{
				lerpAmt -= 1;
				LemegetonColor = Color.Lerp(LemegetonGreen, LemegetonPurple, lerpAmt);
			}
			else
			{
				lerpAmt -= 2;
				LemegetonColor = Color.Lerp(LemegetonPurple, LemegetonRed, lerpAmt);
			}
		}
		public static Color InfernoColorAttempt(float lerp)
		{
			return Color.Lerp(Inferno1, Inferno2, lerp);
		}
		public static Color ShrimpColorAttempt(float lerp)
		{
			return Color.Lerp(new Color(165, 68, 68), TideColor, lerp);
		}
		public static Color InfernoColorAttemptDegrees(float degrees)
		{
			return InfernoColorAttempt(0.5f * (float)Math.Sin(MathHelper.ToRadians(soulColorCounter * 3f + degrees)));
		}
		public static Color VibrantColorAttempt(float degrees, bool avoidColorCycling = false)
		{
			return Color.Lerp(new Color(80, 120, 220, 0), new Color(180, 230, 100, 0), 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians((!avoidColorCycling ? soulColorCounter * 2.5f : 0) + degrees)));
		}
		public static Color pastelAttempt(float radians, bool pinkify = false)
		{
			Color color = Color.White;
			if (pinkify)
				color = new Color(253, 198, 234);
			return pastelAttempt(radians, color);
		}
		public static Color pastelAttempt(float radians, Color overrideColor)
		{
			float newAi = radians;
			double center = 190;
			Vector2 circlePalette = new Vector2(1, 0).RotatedBy(newAi);
			double width = 65 * circlePalette.Y;
			int red = (int)(center + width);
			circlePalette = new Vector2(1, 0).RotatedBy(newAi + MathHelper.ToRadians(120));
			width = 65 * circlePalette.Y;
			int grn = (int)(center + width);
			circlePalette = new Vector2(1, 0).RotatedBy(newAi + MathHelper.ToRadians(240));
			width = 65 * circlePalette.Y;
			int blu = (int)(center + width);
			if (overrideColor == Color.White)
				return new Color(red, grn, blu);
			else
				return new Color(red, grn, blu).MultiplyRGB(overrideColor);
		}
		public static Color DiamondColor
		{
			get
			{
				Color color = new Color(97, 112, 172);
				Color other = new Color(227, 157, 206);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
		}
		public static Color RubyColor
		{
			get
			{
				Color color = new Color(151, 15, 24);
				Color other = new Color(212, 37, 24);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
		}
		public static Color EmeraldColor
		{
			get
			{
				Color color = new Color(10, 143, 93);
				Color other = new Color(33, 184, 115);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
		}
		public static Color SapphireColor
		{
			get
			{
				Color color = new Color(0, 70, 174);
				Color other = new Color(18, 116, 211);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
		}
		public static Color TopazColor
		{
			get
			{
				Color color = new Color(171, 97, 5);
				Color other = new Color(239, 167, 10);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
		}
		public static Color AmethystColor
		{
			get
			{
				Color color = new Color(133, 13, 191);
				Color other = new Color(158, 0, 244);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
		}
		public static Color AmberColor
		{
			get
			{
				Color color = new Color(159, 67, 18);
				Color other = new Color(225, 124, 30);
				return Color.Lerp(color, other, (float)(0.5f + 0.5f * Math.Sin(MathHelper.ToRadians(soulColorCounter))));
			}
        }
        public static Color AtlantisColor
        {
            get
            {
                Color color = new Color(84, 161, 192);
                Color other = new Color(213, 121, 135);
                return Color.Lerp(color, other, Main.rand.NextFloat(1) * Main.rand.NextFloat(1));
            }
        }
        public static Color AtlantisColorInverse
        {
            get
            {
                Color color = new Color(213, 121, 135);
                Color other = new Color(84, 161, 192);
                return Color.Lerp(color, other, Main.rand.NextFloat(1) * Main.rand.NextFloat(1));
            }
        }
		public static Color PolarisColor(float lerp = 0)
		{
			return Color.Lerp(new Color(100, 100, 250), new Color(250, 100, 100), lerp);

        }
		public static Color TesseractColor(float radians, float lerp = 0.5f)
		{
			return Color.Lerp(ColorHelpers.pastelAttempt(radians + soulColorCounter * 9 / 130f), ColorHelpers.AmethystColor, lerp);
        }
    }
}