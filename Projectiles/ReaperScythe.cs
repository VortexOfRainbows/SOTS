using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using SOTS.Dusts;

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
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.aiStyle = -1;
			projectile.penetrate = 6;      //this is how many enemy this projectile penetrate before disappear
			projectile.extraUpdates = 1;
			projectile.timeLeft = 100;
			Main.projFrames[projectile.type] = 6;
			projectile.tileCollide = false;
			projectile.ignoreWater = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10 * (projectile.extraUpdates + 1); //nerf immunity ignoring to make it less overpowered on single target
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			target.immune[player.whoAmI] = 0;
			if (projectile.penetrate <= 2)
			{
				projectile.ai[1] = 25;
				projectile.netUpdate = true; //make sure to sync the AI change
			}
		}
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
        public override void AI()
		{
			start = MathHelper.Lerp(start, 10, 0.05f);
			projectile.ai[0]++;
			if (projectile.ai[0] == 20) //server syncing module since Main.MouseWorld is not based on players mouse but instead local player's mouse
			{
				if(projectile.owner == Main.myPlayer)
				{
					cen = projectile.DirectionTo(Main.MouseWorld) * 0.5f;
					projectile.netUpdate = true;
				}
			}
			if (projectile.ai[0] > 20 && projectile.ai[0] < 50)
            {
				if(projectile.ai[0] == 28)
					SoundEngine.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 71, 0.55f, 0.2f);
				projectile.velocity += cen * 1.8f;
            }
            else if (projectile.ai[0] < 20)
            {
				projectile.velocity *= 0.95f;
			}
			else if (projectile.ai[0] > 50)
            {
				projectile.velocity *= 0.97f;
            }
			projectile.rotation += MathHelper.ToRadians(projectile.velocity.X * 1.75f);
		}
        public override bool CanDamage()
        {
			return projectile.ai[1] != 25;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.66f;
				dust.velocity += circularLocation * 0.15f;
				dust.scale *= 1.75f;
				dust.fadeIn = 0.1f;
				dust.color = new Color(200, 100, 200);
				dust.alpha = 100;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 1; i < projectile.oldPos.Length; i++)
			{
				projectile.oldPos[i] = projectile.oldPos[i - 1] + (projectile.oldPos[i] - projectile.oldPos[i - 1]).SafeNormalize(Vector2.Zero) * MathHelper.Min(Vector2.Distance(projectile.oldPos[i - 1], projectile.oldPos[i]), start);
			}
			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			Texture2D texture = Main.projectileTexture[projectile.type];
			Rectangle frame = new Rectangle(0, 0, 26, 28);
			Vector2 drawOrigin = new Vector2(26 * 0.5f, 28 * 0.5f);
			Color trueColor;
			for (int i = 0; i < projectile.oldPos.Length; i++)
			{
				trueColor = Color.BlueViolet * (0.05f + 0.95f * (1 - (float)i / projectile.oldPos.Length) * (1 - projectile.ai[0] / 100));
				trueColor.A = 0;
				spriteBatch.Draw(texture, projectile.oldPos[i] + drawOrigin - Main.screenPosition, frame, trueColor, projectile.oldRot[i], drawOrigin, 1 - (float)i / projectile.oldPos.Length, SpriteEffects.None, 0f);
			}
			trueColor = new Color(130, 75, 150, 0) * (0.3f + 0.7f * (1 - projectile.ai[0] / 100));
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2f, 3f), 0).RotatedBy(MathHelper.ToRadians(i));
				Color color = trueColor * 0.7f;
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			trueColor = new Color(40, 40, 40); //settingto black
			float alphaMult = 1f;
			if(projectile.ai[0] > 70)
            {
				alphaMult = (1 - 0.33f * (projectile.ai[0] - 70) / 30);
			}
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, frame, trueColor * alphaMult, projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			//spriteBatch.End();
			//spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}