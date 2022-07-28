using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	[AutoloadEquip(EquipType.Waist)]
	public class Starbelt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starbelt");
			Tooltip.SetDefault("Critical strikes recover mana\nIncreased max mana by 40\n10% increased magic crit chance");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 34;     
            Item.height = 26;
            Item.value = Item.sellPrice(0, 3, 75, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarbeltGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Magic) += 10;
			player.statManaMax2 += 40;
			modPlayer.CritManasteal += 6 + Main.rand.Next(5);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.ManaCrystal, 1).AddIngredient<DissolvingAether>(1).AddIngredient<StarlightAlloy>(8).AddTile(ModContent.TileType<Furniture.HardlightFabricatorTile>()).Register();
		}
	}
}