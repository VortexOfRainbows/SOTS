using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class RedNatureBolt : ModProjectile
	{
		public override string Texture => "SOTS/Projectiles/Nature/NatureBolt";
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
			// DisplayName.SetDefault("Pulverizer Bolt");
		}
        public override void SetDefaults()
        {
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.alpha = 50;
			Projectile.timeLeft = 70;
		}
		bool reachDestination = false;
		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			if (reachDestination)
			{
				Color color = new Color(120, 100, 100, 0);
				Projectile.position = player.Center;
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Crushers/RedNatureReticle");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 drawPos = trueTarget - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				for (int k = 0; k < 4; k++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					Main.spriteBatch.Draw(texture, new Vector2(drawPos.X + x, drawPos.Y + y), null, color, Projectile.rotation, drawOrigin, 0.30f - Projectile.ai[0], SpriteEffects.None, 0f);
				}
				//spriteBatch.Draw(texture, drawPos, null, drawColor, Projectile.rotation, drawOrigin, 0.30f - Projectile.ai[0], SpriteEffects.None, 0f);
			}
			return false;
		}
		Vector2 trueTarget = Vector2.Zero;
		int counter = 0;
		public void Redirect()
		{
			counter++;
			float mult = counter / 210f;
			if (mult > 1)
				mult = 1;
			Vector2 toTarget = trueTarget - Projectile.Center;
			Vector2 goTo = toTarget * mult * 0.09f;
			if (toTarget.Length() <= 6)
			{
				reachDestination = true;
			}
			Projectile.velocity *= 1 - mult;
			Projectile.velocity += goTo;
			Vector2 center = Projectile.Center;
			Dust dust = Dust.NewDustDirect(center - new Vector2(5) + Projectile.velocity, 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 100, default, 1.6f);
			dust.velocity *= 0.05f;
			dust.scale = 1.2f + 1.2f * mult;
			dust.noGravity = true;
			dust.color = Color.Lerp(Color.Red, Color.Maroon, Main.rand.NextFloat(1));
			dust.noGravity = true;
			dust.fadeIn = 0.2f;
			dust.alpha = Projectile.alpha;
			Projectile.position += Projectile.velocity;
		}
		public override void AI()
		{
			//Player player = Main.player[Projectile.owner];
			Projectile.velocity *= 0.97f;
			Projectile.rotation += 0.3f;
			if(!reachDestination)
				trueTarget = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			if (trueTarget != Vector2.Zero && !reachDestination)
			{
				for (int i = 0; i < 4; i++)
				{
					Redirect();
				}
			}
			if(reachDestination)
            {
				if (Projectile.timeLeft > 25)
					Projectile.timeLeft = 25;
				Projectile.ai[0] = -((70 - Projectile.timeLeft) / 70f) * 0.6f;
            }
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
		{
			if(Projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), trueTarget.X, trueTarget.Y, 0, 0, ModContent.ProjectileType<RedNatureBeat>(), Projectile.damage, 0, Main.myPlayer);
			SOTSUtils.PlaySound(SoundID.Item93, (int)trueTarget.X, (int)trueTarget.Y, 0.35f);
		}
	}
}
		