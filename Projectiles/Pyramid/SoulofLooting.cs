using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid 
{    
    public class SoulofLooting : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sool of Looting");
		}
        public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Main.projFrames[Projectile.type] = 8;
			Projectile.netImportant = true;
			Projectile.timeLeft = 900;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture1 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/SoulofLooting").Value;
			Vector2 drawOrigin1 = new Vector2(texture1.Width * 0.5f, texture1.Height * 0.5f * 0.125f);
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture1, drawPos2 + new Vector2(0, Projectile.gfxOffY), new Rectangle(0, 22 * Projectile.frame, 22, 22), Projectile.GetAlpha(VoidPlayer.soulLootingColor), Projectile.rotation, drawOrigin1, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			base.PostDraw(spriteBatch, lightColor);
        }
		int owner = 0;
		public override void AI()
		{
			owner = (int)Projectile.ai[0];
			Lighting.AddLight(Projectile.Center, new Vector3(VoidPlayer.soulLootingColor.R, VoidPlayer.soulLootingColor.G, VoidPlayer.soulLootingColor.B) * 1f / 255f);
			Projectile.frameCounter++;																																						
			if (Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 8;
			}
			Projectile.ai[1]++;
			if (Projectile.ai[1] % 6 == 0)
				Projectile.alpha++;
			Projectile.velocity *= 0.95f;
			Player player = Main.player[owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			int pepperID = -1;
			int count = 0;
			for(int i = 0; i < Main.projectile.Length; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.type == ModContent.ProjectileType<GhostPepper>() && proj.active && proj.owner == owner)
                {
					pepperID = proj.whoAmI;
                }
				if (proj.type == Projectile.type && proj.active && (int)proj.ai[0] == owner)
					count++;
            }
			if(count > 40 || voidPlayer.lootingSouls >= voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption)
            {
				Projectile.Kill();
            }
			Collect(player, pepperID);
		}
		public void Collect(Player player, int proj)
        {
			if(proj != -1)
			{
				Projectile pepper = Main.projectile[proj];
				if (pepper.Hitbox.Intersects(Projectile.Hitbox))
                {
					Projectile.Kill();
                }
			}
			if (player.Hitbox.Intersects(Projectile.Hitbox))
			{
				Projectile.Kill();
			}
		}
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void Kill(int timeLeft)
		{
			int particlesR = 60;
			owner = (int)Projectile.ai[0];
			Player player = Main.player[owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.lootingSouls < voidPlayer.voidMeterMax2 - voidPlayer.VoidMinionConsumption)
			{
				Terraria.Audio.SoundEngine.PlaySound(7, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 1.3f);
				voidPlayer.lootingSouls++;
				voidPlayer.SendClientChanges(voidPlayer);
				var index = CombatText.NewText(Projectile.Hitbox, VoidPlayer.soulLootingColor.MultiplyRGB(Color.White), 1);
				if (Main.netMode == 2 && index != 100)
				{
					var combatText = Main.combatText[index];
					NetMessage.SendData(81, -1, -1, null, (int)combatText.color.PackedValue, combatText.position.X, combatText.position.Y, (float)1, 0, 0, 0);
				}
				particlesR = 20;
			}
			for (int i = 0; i < 360; i += particlesR)
			{
				Vector2 rotationalPos = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), 22, 22, Mod.Find<ModDust>("CopyDust4").Type);
				Dust dust = Main.dust[num1];
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.position += rotationalPos;
				dust.velocity += rotationalPos * 0.3f;
				dust.scale *= 2f;
				dust.fadeIn = 0.1f;
				dust.alpha = Projectile.alpha;
				dust.color = VoidPlayer.soulLootingColor;
			}
			base.Kill(timeLeft);
        }
    }
}
		
			