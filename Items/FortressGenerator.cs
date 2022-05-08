using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using System.Runtime.Remoting.Messaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items
{
	//[AutoloadEquip(EquipType.Shield)]
	public class FortressGenerator : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fortress Generator");
			Tooltip.SetDefault("Increases max minions and max sentries by 1\nIncreases damage by 10% and life regeneration by 2\nGenerates 4 platforms to the left and right of you\nYou can right click to drag the platforms, but they will always remain symmetrical\nSentries can be summoned on top of the platforms\nAbsorbs 25% of damage done to players on your team when above 25% life and grants immunity to knockback");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 32;     
            Item.height = 40;
            Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			Item.defense = 6;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.lifeRegen += 2;
			player.noKnockback = true;
			player.hasPaladinShield = true;
			player.maxMinions += 1;
			player.maxTurrets += 1;
			player.allDamage += 0.1f;
			PlatformPlayer modPlayer = player.GetModPlayer<PlatformPlayer>();
			modPlayer.platformPairs += 2;
			modPlayer.fortress = true;
			if (hideVisual)
				modPlayer.hideChains = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PlatformGenerator>(), 1);
			recipe.AddIngredient(ItemID.PaladinsShield, 1);
			recipe.AddIngredient(ItemID.PygmyNecklace, 1);
			recipe.AddRecipeGroup("SOTS:T2DD2Armor", 1);
			recipe.AddRecipeGroup("SOTS:T2DD2Accessory", 1);
			recipe.AddIngredient(ItemID.SpectreBar, 10);
			recipe.AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}