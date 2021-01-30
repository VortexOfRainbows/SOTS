using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace SOTS.Items.Celestial
{
	public class CelestialTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Torch");
			Tooltip.SetDefault("'As you gaze into the flame, the stars in the sky become dimmer'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
		}
		public override void SetDefaults()
		{

			item.width = 36;
			item.height = 40;
			item.value = 0;
			item.rare = 8;
			item.maxStack = 30;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = 4;
			item.consumable = true;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PixieDust, 5);
			recipe.AddIngredient(ItemID.Ectoplasm, 3);
			recipe.AddIngredient(null,"SoulResidue", 3);
			recipe.AddIngredient(ItemID.SoulofLight, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
		}
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("CelestialSerpentHead")) && !Main.dayTime;
		}
		public override bool UseItem(Player player)
		{
		NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CelestialSerpentHead"));
		Main.PlaySound(0, (int)player.position.X, (int)player.position.Y, 0);
		if(!NPC.AnyNPCs(mod.NPCType("CelestialSerpentHead")))
		{
		//		 NPC.NewNPC((int)player.Center.X, (int)player.Center.Y - 600, mod.NPCType("PutridPinky1"));	
		}
		return true;
		
		}
	}
}