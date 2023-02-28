using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.Projectiles.Nature;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Invidia
{
	[AutoloadEquip(EquipType.Head)]
	public class VesperaMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
		int[] Probes = new int[] {-1, -1, -1 };
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VesperaBreastplate>() && legs.type == ModContent.ItemType<VesperaLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Vespera");
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 25;
			voidPlayer.voidGainMultiplier += 0.5f;
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 1f;
			voidPlayer.voidMeterMax2 += 25;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Evostone>(20).AddIngredient(ItemID.StoneBlock, 60).AddIngredient<FragmentOfEarth>(1).AddTile(TileID.Furnaces).Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VesperaLeggings : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<VesperaBreastplate>() && head.type == ModContent.ItemType<VesperaMask>();
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 25;
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Evostone>(25).AddIngredient(ItemID.StoneBlock, 75).AddIngredient<FragmentOfEarth>(1).AddTile(TileID.Furnaces).Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class VesperaBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[equipSlotBody] = false;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = false;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VesperaMask>() && legs.type == ModContent.ItemType<VesperaLeggings>();
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 25;
			player.GetDamage<VoidGeneric>() += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<Evostone>(30).AddIngredient(ItemID.StoneBlock, 90).AddIngredient<FragmentOfEarth>(1).AddTile(TileID.Furnaces).Register();
		}
	}
}