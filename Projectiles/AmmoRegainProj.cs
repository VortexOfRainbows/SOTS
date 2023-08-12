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
				trueColor = new Color(100, 90, 80, 0) * (0.2f + 0.8f * (1 - (float)i / Projectile.oldPos.Length));
				if(Projectile.position != Projectile.oldPos[i])
					DrawItem(texture, Projectile.oldPos[i] + new Vector2(16, 16), frameCount, ticksPerFrame, trueColor, 1 - (float)i / Projectile.oldPos.Length);
			}
			if (Projectile.timeLeft <= 27)
				return false;
			trueColor = new Color(100, 90, 80, 0);
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i));
				Color color = trueColor;
				DrawItem(texture, Projectile.Center + circular, frameCount, ticksPerFrame, color, Projectile.scale);
			}
			trueColor = new Color(255, 255, 255);
			DrawItem(texture, Projectile.Center, frameCount, ticksPerFrame, trueColor, 1f);
			return false;
		}
		public void DrawItem(Texture2D texture, Vector2 pos, int frameCount, int ticksPerFrame, Color color, float scaleMod)
		{
			pos -= Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
			float width = texture.Width;
			float height = texture.Height / frameCount;
			float allocatedArea = 48;
			float scale = 1.0f * scaleMod;
			if (allocatedArea < (float)Math.Sqrt(width * height))
				scale *= allocatedArea / (float)Math.Sqrt(width * height);
			Main.spriteBatch.Draw(texture, pos, new Rectangle(0, texture.Height / frameCount * ((int)Main.GameUpdateCount / ticksPerFrame % frameCount), texture.Width, texture.Height / frameCount), color, Projectile.velocity.X * 0.04f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 27;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			// DisplayName.SetDefault("Ammo Regain proj");
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
			Projectile.extraUpdates = 2;
		}
		bool dead = false;
        public override bool PreAI()
        {
			Player player = Main.player[Projectile.owner];
			if(dead)
            {
				Projectile.velocity *= 0;
				if(Projectile.timeLeft > 27)
					Projectile.timeLeft = 27;
				return false;
            }
			Projectile.velocity = Vector2.Lerp(Projectile.velocity * 0.99f, (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1] * 0.04f, Projectile.ai[1] / 240);
			Vector2 toPlayer = player.Center - Projectile.Center;
			float distance = toPlayer.Length();
			if (distance < 32 || Projectile.timeLeft <= 28)
			{
				if (player.whoAmI == Main.myPlayer)
				{
					int ID = player.QuickSpawnItem(new EntitySource_Misc("SOTS:AmmoRegather"), itemType, 1);
					Main.item[ID].position = player.Center;
				}
				dead = true;
			}
			Projectile.ai[1]++;
			return true;
		}
		public override void AI()
		{
			start = MathHelper.Lerp(start, 10, 0.05f);
		}
	}
}
		