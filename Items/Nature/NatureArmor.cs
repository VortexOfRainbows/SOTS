using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.Projectiles.Nature;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Nature
{
	[AutoloadEquip(EquipType.Head)]
	public class NatureWreath : ModItem
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
			ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 12;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}
		private int[] Probes = new int[] {-1, -1, -1 };
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NatureShirt>() && legs.type == ModContent.ItemType<NatureLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.NatureWreath");
			if (Main.myPlayer == player.whoAmI)
			{
				SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
				int damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, 11);
				for(int i = 0; i < 3; i++)
					sPlayer.runPets(ref Probes[i], ModContent.ProjectileType<BloomingHook>(), damage, 1f, false);
			}
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 5).AddTile(TileID.WorkBenches).Register();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class NatureLeggings : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<NatureShirt>() && head.type == ModContent.ItemType<NatureWreath>();
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 16).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class NatureShirt : ModItem
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
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}
		/*public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = false;
		}*/
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<NatureWreath>() && legs.type == ModContent.ItemType<NatureLeggings>();
		}
		public override void UpdateEquip(Player player)
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.owner == player.whoAmI && proj.minion == true && proj.minionSlots > 0.01f && proj.active)
				{
					player.statDefense++;
				}
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 22).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6).AddTile(TileID.WorkBenches).Register();
		}
	}
}