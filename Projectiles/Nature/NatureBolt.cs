using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Nature
{    
    public class NatureBolt : ModProjectile 
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(trueTarget.X);
			writer.Write(trueTarget.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			trueTarget.X = reader.ReadSingle();
			trueTarget.Y = reader.ReadSingle();
		}
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature Bolt");
		}
        public override void SetDefaults()
        {
			projectile.height = 18;
			projectile.width = 18;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.alpha = 50;
			projectile.timeLeft = 110;
		}
		bool reachDestination = false;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if(reachDestination)
			{
				Color color = new Color(100, 120, 100, 0);
				Player player = Main.player[(int)projectile.ai[1]];
				projectile.position = player.Center;
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Nature/NatureReticle");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 drawPos = trueTarget - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
				for (int k = 0; k < 4; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					Main.spriteBatch.Draw(texture, new Vector2(drawPos.X + x, drawPos.Y + y), null, color, projectile.rotation, drawOrigin, 0.30f - projectile.ai[0], SpriteEffects.None, 0f);
				}
				//spriteBatch.Draw(texture, drawPos, null, drawColor, projectile.rotation, drawOrigin, 0.30f - projectile.ai[0], SpriteEffects.None, 0f);
			}
			return false;
		}
		Vector2 trueTarget = Vector2.Zero;
		int counter = 0;
		public void Redirect()
		{
			counter++;
			float mult = counter / 360f;
			if (mult > 1)
				mult = 1;
			Vector2 toTarget = trueTarget - projectile.Center;
			Vector2 goTo = toTarget * mult * 0.05f;
			if (toTarget.Length() <= 6)
			{
				reachDestination = true;
			}
			projectile.velocity *= 1 - mult;
			projectile.velocity += goTo;
			Vector2 center = projectile.Center;
			for(float i = 0; i < 1; i += 0.5f)
			{
				Dust dust = Dust.NewDustDirect(center - new Vector2(5) + projectile.velocity * i, 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 100, default, 1.6f);
				dust.velocity *= 0.05f;
				dust.scale = 1.2f + 1.2f * mult;
				dust.noGravity = true;
				dust.color = VoidPlayer.natureColor;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = projectile.alpha;
			}
			projectile.position += projectile.velocity;
		}
		public override void AI()
		{
			projectile.velocity *= 0.97f;
			projectile.rotation += 0.3f;
			Player player = Main.player[(int)projectile.ai[1]];
			if(projectile.ai[0] > 0 && Main.netMode != 1)
			{
				Vector2 targetPos = new Vector2(projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
				projectile.ai[0] = 0;
				trueTarget = player.Center + targetPos;
				projectile.netUpdate = true;
				return;
			}
			if (trueTarget != Vector2.Zero && !reachDestination)
			{
				for (int i = 0; i < 4; i++)
				{
					Redirect();
				}
			}
			if(reachDestination)
            {
				projectile.ai[0] = -((110 - projectile.timeLeft) / 110f) * 0.5f;
            }
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void Kill(int timeLeft)
		{
			if(Main.netMode != NetmodeID.MultiplayerClient)
				Projectile.NewProjectile(trueTarget.X, trueTarget.Y, 0, 0, ModContent.ProjectileType<NatureBeat>(), projectile.damage, 0, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item, (int)trueTarget.X, (int)trueTarget.Y, 93, 0.35f);
		}
	}
}
		