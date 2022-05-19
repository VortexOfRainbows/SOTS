using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SOTS.Dusts;
using Terraria.Audio;

namespace SOTS.Projectiles
{
	public class ReaperScythe : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cen.X);
			writer.Write(cen.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			cen.X = reader.ReadSingle();
			cen.Y = reader.ReadSingle();
		}
        public float start = 0;
		Vector2 cen = Vector2.Zero;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Main.projFrames[Projectile.type] = 6;
		}
		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 6;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 100;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10 * (Projectile.extraUpdates + 1); //nerf immunity ignoring to make it less overpowered on single target
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			target.immune[player.whoAmI] = 0;
			if (Projectile.penetrate <= 2)
			{
				Projectile.ai[1] = 25;
				Projectile.netUpdate = true; //make sure to sync the AI change
			}
		}
        public override void AI()
		{
			start = MathHelper.Lerp(start, 10, 0.05f);
			Projectile.ai[0]++;
			if (Projectile.ai[0] == 20) //server syncing module since Main.MouseWorld is not based on players mouse but instead local player's mouse
			{
				if(Projectile.owner == Main.myPlayer)
				{
					cen = Projectile.DirectionTo(Main.MouseWorld) * 0.5f;
					Projectile.netUpdate = true;
				}
			}
			if (Projectile.ai[0] > 20 && Projectile.ai[0] < 50)
            {
				if(Projectile.ai[0] == 28)
					SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 71, 0.55f, 0.2f);
				Projectile.velocity += cen * 1.8f;
            }
            else if (Projectile.ai[0] < 20)
            {
				Projectile.velocity *= 0.95f;
			}
			else if (Projectile.ai[0] > 50)
            {
				Projectile.velocity *= 0.97f;
            }
			Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 1.75f);
		}
        public override bool? CanHitNPC(NPC target)
        {
			return Projectile.ai[1] != 25;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.66f;
				dust.velocity += circularLocation * 0.15f;
				dust.scale *= 1.75f;
				dust.fadeIn = 0.1f;
				dust.color = new Color(200, 100, 200);
				dust.alpha = 100;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			for (int i = 1; i < Projectile.oldPos.Length; i++)
			{
				Projectile.oldPos[i] = Projectile.oldPos[i - 1] + (Projectile.oldPos[i] - Projectile.oldPos[i - 1]).SafeNormalize(Vector2.Zero) * MathHelper.Min(Vector2.Distance(Projectile.oldPos[i - 1], Projectile.oldPos[i]), start);
			}
			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = new Rectangle(0, 0, 26, 28);
			Vector2 drawOrigin = new Vector2(26 * 0.5f, 28 * 0.5f);
			Color trueColor;
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				trueColor = Color.BlueViolet * (0.05f + 0.95f * (1 - (float)i / Projectile.oldPos.Length) * (1 - Projectile.ai[0] / 100));
				trueColor.A = 0;
				Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + drawOrigin - Main.screenPosition, frame, trueColor, Projectile.oldRot[i], drawOrigin, 1 - (float)i / Projectile.oldPos.Length, SpriteEffects.None, 0f);
			}
			trueColor = new Color(130, 75, 150, 0) * (0.3f + 0.7f * (1 - Projectile.ai[0] / 100));
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2f, 3f), 0).RotatedBy(MathHelper.ToRadians(i));
				Color color = trueColor * 0.7f;
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			trueColor = new Color(40, 40, 40); //settingto black
			float alphaMult = 1f;
			if(Projectile.ai[0] > 70)
            {
				alphaMult = (1 - 0.33f * (Projectile.ai[0] - 70) / 30);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, trueColor * alphaMult, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}