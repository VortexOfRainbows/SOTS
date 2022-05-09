using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.Projectiles.Nature;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nature
{
	[AutoloadEquip(EquipType.Head)]
	public class NatureWreath : ModItem
	{
		int[] Probes = new int[] {-1, -1, -1 };
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Wreath");
			Tooltip.SetDefault("Increased max minions");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NatureShirt>() && legs.type == ModContent.ItemType<NatureLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Summons three Blooming Hooks to assist in combat";
			if (Main.myPlayer == player.whoAmI)
			{
				SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
				int damage = (int)(11 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 20);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class NatureLeggings : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Leggings");
			Tooltip.SetDefault("5% increased minion damage and movement speed");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<NatureShirt>() && head.type == ModContent.ItemType<NatureWreath>();
		}
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.05f;
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 16);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class NatureShirt : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Shirt");
			Tooltip.SetDefault("Increased defense for every active minion");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<NatureWreath>() && legs.type == ModContent.ItemType<NatureLeggings>();
		}
		public override void UpdateEquip(Player player)
		{
			for (int i = 0; i < Main.Projectile.Length; i++)
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 22);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfNature>(), 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
}