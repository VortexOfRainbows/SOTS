using Microsoft.Xna.Framework;
using SOTS;
using SOTS.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Projectiles.Otherworld;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Otherworld.FromChests
{
	public class TwilightFishingPole : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Fishing Pole");
			Tooltip.SetDefault("Casts two lines at once");
			//Allows the pole to fish in lava
			ItemID.Sets.CanFishInLava[Item.type] = false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightFishingPoleGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Terraria.GameContent.TextureAssets.Item[Item.type].Value.Height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) - 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodFishingPole);
			//Item.width = 46;
			//Item.height = 36;
			//Sets the poles fishing power
			Item.fishingPole = 20;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			//Sets the speed in which the bobbers are launched, Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f
			Item.shootSpeed = 14f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightFishingPoleGlow").Value;
			}

			//The Bobber projectile
			Item.shoot = ModContent.ProjectileType<TwilightBobber>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2 rotation = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(5 * (i * 2 - 1)));
				Projectile.NewProjectile(position.X, position.Y, rotation.X, rotation.Y, type, 0, 0f, player.whoAmI);
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ItemID.ReinforcedFishingPole, 1);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}