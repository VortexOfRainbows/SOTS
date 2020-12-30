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
			ItemID.Sets.CanFishInLava[item.type] = false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightFishingPoleGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, Main.itemTexture[item.type].Height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) - 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodFishingPole);
			//item.width = 46;
			//item.height = 36;
			//Sets the poles fishing power
			item.fishingPole = 20;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			//Sets the speed in which the bobbers are launched, Wooden Fishing Pole is 9f and Golden Fishing Rod is 17f
			item.shootSpeed = 14f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Otherworld/FromChests/TwilightFishingPoleGlow");
			}

			//The Bobber projectile
			item.shoot = ModContent.ProjectileType<TwilightBobber>();
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ReinforcedFishingPole, 1);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}