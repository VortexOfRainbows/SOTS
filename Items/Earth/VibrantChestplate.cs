using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Earth
{
	[AutoloadEquip(EquipType.Body)]
	public class VibrantChestplate : ModItem
	{
		public Texture2D glowTexture => Mod.Assets.Request<Texture2D>("Items/Earth/VibrantChestplateGlow").Value;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(glowTexture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 5;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Chestplate");
			Tooltip.SetDefault("Increases void damage by 10% and ranged damage by 5%");
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<VibrantHelmet>() && legs.type == ModContent.ItemType<VibrantLeggings>();
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			player.GetDamage<VoidGeneric>() += 0.10f;
			player.GetDamage(DamageClass.Ranged) += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VibrantBar>(), 12).AddTile(TileID.Anvils).Register();
		}

	}
}