using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SOTS.Projectiles
{    
    public class AmmoRegainProj : ModProjectile 
    {
		public int itemType => (int)Projectile.ai[0];
		public float start = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				Projectile.oldPos[i] = Projectile.oldPos[i - 1] + (Projectile.oldPos[i] - Projectile.oldPos[i - 1]).SafeNormalize(Vector2.Zero) * MathHelper.Min(Vector2.Distance(Projectile.oldPos[i - 1], Projectile.oldPos[i]), start);
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[itemType].Value;
            if (!Terraria.GameContent.TextureAssets.Item[itemType].IsLoaded)
            {
				Main.instance.LoadItem(itemType);
			}
			DrawAnimation anim = Main.itemAnimations[itemType];
			int frameCount = 1;
			int ticksPerFrame = 1;
			if (anim != null)
			{
				frameCount = anim.FrameCount;
				ticksPerFrame = anim.TicksPerFrame;
			}
			Color trueColor;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				trueColor = Color.BlueViolet * (0.05f + 0.95f * (1 - (float)i / Projectile.oldPos.Length) * (1 - Projectile.ai[0] / 100));
				trueColor.A = 0;
				DrawItem(texture, Projectile.oldPos[i] + new Vector2(16, 16), frameCount, ticksPerFrame, trueColor, 1 - (float)i / Projectile.oldPos.Length);
			}
			trueColor = new Color(130, 140, 150, 0) * (0.3f + 0.7f * (1 - Projectile.ai[0] / 100));
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2f, 3f), 0).RotatedBy(MathHelper.ToRadians(i));
				Color color = trueColor * 0.7f;
				DrawItem(texture, Projectile.Center + circular, frameCount, ticksPerFrame, color, Projectile.scale);
			}
			trueColor = new Color(140, 140, 140); //settingto black
			float alphaMult = 1f;
			if (Projectile.ai[0] > 70)
			{
				alphaMult = (1 - 0.33f * (Projectile.ai[0] - 70) / 30);
			}
			DrawItem(texture, Projectile.Center, frameCount, ticksPerFrame, trueColor * alphaMult, 1f);
			return false;
		}
		public void DrawItem(Texture2D texture, Vector2 pos, int frameCount, int ticksPerFrame, Color color, float scaleMod)
		{
			pos -= Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
			float width = texture.Width;
			float height = texture.Height / frameCount;
			float allocatedArea = 48;
			float scale = 0.8f * scaleMod;
			if (allocatedArea < (float)Math.Sqrt(width * height))
				scale = 0.8f * allocatedArea / (float)Math.Sqrt(width * height);
			Main.spriteBatch.Draw(texture, pos, new Rectangle(0, texture.Height / frameCount * ((int)Main.GameUpdateCount / ticksPerFrame % frameCount), texture.Width, texture.Height / frameCount), color, 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ammo Regain proj");
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = false;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 780;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}
		bool runOnce = false;
        public override bool PreAI()
        {
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.1f);
				for (int i = 0; i < 40; i++)
				{
					Vector2 circularLocation = new Vector2(Main.rand.NextFloat(6f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, 60);
					Main.dust[dust].velocity = circularLocation;
					Main.dust[dust].scale *= 1.5f;
					Main.dust[dust].noGravity = true;
				}
				runOnce = false;
			}
			Player player = Main.player[Projectile.owner];
			if (Projectile.timeLeft < 720)
			{
				Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1] * 0.25f;

				Vector2 toPlayer = player.Center - Projectile.Center;
				float distance = toPlayer.Length();
				if (distance < 32)
				{
					if (player.whoAmI == Main.myPlayer)
						player.QuickSpawnItem(new EntitySource_Misc("SOTS:AmmoRegather"), itemType, 1);
					Projectile.Kill();
				}
				Projectile.ai[1]++;
			}
			else
            {
				Projectile.velocity *= 0.975f;
            }
			return true;
		}
		public override void AI()
		{
			start = MathHelper.Lerp(start, 10, 0.05f);
		}
	}
}
		