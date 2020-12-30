using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SOTS.Items.Otherworld.FromChests
{
	public class StarcoreAssaultRifle : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starcore Assault Rifle");
			Tooltip.SetDefault("Fires a stream of bouncy, colorful projectiles");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 19;
            item.ranged = true;
            item.width = 58;
            item.height = 24;
            item.useTime = 5; 
            item.useAnimation = 20;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 2f;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("StarcoreBullet"); 
            item.shootSpeed = 3.5f;
			item.reuseDelay = 10;
			item.noUseGraphic = true;
		}
		int projectileNum = 0;
		int highestProjectileNum = 0;
		public override bool BeforeUseItem(Player player)
		{
			projectileNum = 0;
			return base.BeforeUseItem(player);
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.player[Main.myPlayer];
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect");
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, position, frame, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), 0f, origin, scale, SpriteEffects.None, 0f); 
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/StarcoreAssaultRifleEffect");
			for (int k = 0; k < 2; k++)
				Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition + new Vector2(0, 2), null, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), rotation, new Vector2(texture.Width/2, texture.Height/2), scale, SpriteEffects.None, 0f);
			
			texture = mod.GetTexture("Items/Otherworld/FromChests/StarcoreAssaultRifleGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }
        public override void GetVoid(Player player)
		{
			voidMana = 6;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SoundID.Item11, position);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians((speedX < 0 ? -1 : 1) * (-3f + 1.75f * projectileNum)));
			Projectile.NewProjectile(position, Vector2.Zero, mod.ProjectileType("StarcoreRifle"), item.useTime + (projectileNum >= highestProjectileNum - 1 && highestProjectileNum > 3 ? item.reuseDelay - 1 : 0) + 1, 0, player.whoAmI, perturbedSpeed.ToRotation() - new Vector2(speedX, speedY).ToRotation());
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			position += new Vector2(speedX, speedY) * 6;

			projectileNum++;
			if(highestProjectileNum < projectileNum)
				highestProjectileNum = projectileNum;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VibrantPistol", 1);
			recipe.AddIngredient(null, "VibrancyModule", 1);
			recipe.AddIngredient(null, "StarlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
