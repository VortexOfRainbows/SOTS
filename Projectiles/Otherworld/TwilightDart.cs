using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class TwilightDart : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TwilightDart");
		}
		
        public override void SetDefaults()
        {
			projectile.height = 22;
			projectile.width = 22;
			projectile.friendly = false;
			projectile.timeLeft = 960;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = 5;
			projectile.netImportant = true;
			projectile.tileCollide = false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(115, 115, 115, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * (0.2f - distMult);
				float y = Main.rand.Next(-10, 11) * (0.2f - distMult);
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		float soundIterater = 0;
		float distMult = 0.1f;
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(projectile.Center, 0, 0, DustID.Electric, 0, 0, projectile.alpha, default, 1.25f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 1.5f;
			}
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.55f, 0.55f, 0.75f);
			NPC owner = Main.npc[(int)projectile.ai[1]];
			if (owner.type != mod.NPCType("TwilightDevil") || !owner.active)
			{
				if (projectile.timeLeft > 480)
					projectile.Kill();
			}
			Player player = Main.player[owner.target];
			projectile.alpha -= projectile.alpha > 0 ? 2 : 0;
			
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (projectile.type == proj.type && proj.active && projectile.active && Main.npc[(int)proj.ai[1]] == owner && proj.timeLeft >= 480)
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			float mult = (360 - projectile.ai[0]) / 360;
			float addRot = 0;
			if(ofTotal % 2 == 0)
			{
				addRot = ofTotal * (180f / total) * (mult * 0.6f + 0.4f);
			}
			if (ofTotal % 2 == 1)
			{
				addRot = (ofTotal + 1) * -(180f / total) * (mult * 0.6f + 0.4f);
			}

			if(mult > 0)
				projectile.ai[0]++;

			if(projectile.timeLeft == 480)
			{
				Vector2 newVelocity = new Vector2(9.5f, 0).RotatedBy(projectile.rotation - MathHelper.ToRadians(90));
				projectile.velocity = newVelocity;
				if (Main.netMode != 1)
				{
					projectile.netUpdate = true;
				}
			}
			else if (projectile.timeLeft > 480)
			{
				if(projectile.timeLeft < 600)
				{
					distMult *= 0.98f;
				}
				soundIterater += mult * 10;
				if (ofTotal == 0 && soundIterater >= 75)
				{
					soundIterater = 0;
					Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 15, 1 - projectile.alpha / 255f);
				}
				Vector2 playerPos = new Vector2(owner.ai[2], owner.ai[3]);
				Vector2 towardsPlayer = projectile.Center - playerPos;
				towardsPlayer = -towardsPlayer;
				float rotationP = towardsPlayer.ToRotation();
				Vector2 fromNPC = new Vector2(32 + projectile.ai[0] * distMult, 0).RotatedBy(rotationP + MathHelper.ToRadians(addRot - 360 * mult * mult * 6) * ((owner.whoAmI % 2) * 2 - 1));
				float rotation = fromNPC.ToRotation();
				projectile.rotation = rotation + MathHelper.ToRadians(90);
				projectile.position = fromNPC + owner.Center - new Vector2(projectile.width / 2, projectile.height / 2);
			}
		}
	}
}
		