using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{    
    public class TwilightDart : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("TwilightDart");
		}
		
        public override void SetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.friendly = false;
			Projectile.timeLeft = 960;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 5;
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(115, 115, 115, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * (0.2f - distMult);
				float y = Main.rand.Next(-10, 11) * (0.2f - distMult);
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		float soundIterater = 0;
		float distMult = 0.1f;
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Electric, 0, 0, Projectile.alpha, default, 1.25f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 1.5f;
			}
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0.55f, 0.55f, 0.75f);
			NPC owner = Main.npc[(int)Projectile.ai[1]];
			if (owner.type != ModContent.NPCType<TwilightDevil>() || !owner.active)
			{
				if (Projectile.timeLeft > 480)
					Projectile.Kill();
			}
			Player player = Main.player[owner.target];
			Projectile.alpha -= Projectile.alpha > 0 ? 2 : 0;
			
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (Projectile.type == proj.type && proj.active && Projectile.active && Main.npc[(int)proj.ai[1]] == owner && proj.timeLeft >= 480)
				{
					if (proj == Projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			float mult = (360 - Projectile.ai[0]) / 360;
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
				Projectile.ai[0]++;

			if(Projectile.timeLeft == 480)
			{
				Vector2 newVelocity = new Vector2(9.5f, 0).RotatedBy(Projectile.rotation - MathHelper.ToRadians(90));
				Projectile.velocity = newVelocity;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.netUpdate = true;
				}
			}
			else if (Projectile.timeLeft > 480)
			{
				if(Projectile.timeLeft < 600)
				{
					distMult *= 0.98f;
				}
				soundIterater += mult * 10;
				if (ofTotal == 0 && soundIterater >= 75)
				{
					soundIterater = 0;
					SOTSUtils.PlaySound(SoundID.Item15, (int)(Projectile.Center.X), (int)(Projectile.Center.Y), 1 - Projectile.alpha / 255f);
				}
				Vector2 playerPos = new Vector2(owner.ai[2], owner.ai[3]);
				Vector2 towardsPlayer = Projectile.Center - playerPos;
				towardsPlayer = -towardsPlayer;
				float rotationP = towardsPlayer.ToRotation();
				Vector2 fromNPC = new Vector2(32 + Projectile.ai[0] * distMult, 0).RotatedBy(rotationP + MathHelper.ToRadians(addRot - 360 * mult * mult * 6) * ((owner.whoAmI % 2) * 2 - 1));
				float rotation = fromNPC.ToRotation();
				Projectile.rotation = rotation + MathHelper.ToRadians(90);
				Projectile.position = fromNPC + owner.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
			}
		}
	}
}
		