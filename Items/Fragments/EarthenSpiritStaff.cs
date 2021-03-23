using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class EarthenSpiritStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Earthen Spirit Staff");
			Tooltip.SetDefault("Summons an Earthen Spirit to fight for you");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; 
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}
		/*
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Fragments/EarthenSpiritStaffEffect");
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Fragments/EarthenSpiritStaffEffect");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2),
				null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		} 
		*/
		public override void SetDefaults() 
		{
			item.damage = 21;
			item.knockBack = 4f;
			item.mana = 18;
			item.width = 38;
			item.height = 36;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item44;
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("SpiritAid");
			item.shoot = mod.ProjectileType("EarthenSpirit");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) 
		{
			player.AddBuff(item.buffType, 2);

			position = Main.MouseWorld;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingEarth", 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}