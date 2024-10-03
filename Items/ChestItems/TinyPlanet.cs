using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Fishing;

namespace SOTS.Items.ChestItems
{
	public class TinyPlanet : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.damage = 10;
            Item.width = 34;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TinyPlanetFish>(), 1).AddIngredient(ItemID.StoneBlock, 100).AddTile(TileID.TinkerersWorkbench).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Main.myPlayer == player.whoAmI)
			{
				int damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Default, Item.damage);
				modPlayer.tPlanetDamage += damage;
				modPlayer.tPlanetNum += 2;
			}
        }
        public override bool WeaponPrefix() => false;
        public override bool MagicPrefix() => false;
        public override bool MeleePrefix() => false;
        public override bool RangedPrefix() => false;
    }
}