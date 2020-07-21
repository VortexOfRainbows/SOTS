using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Body)]
	public class NatureShirt : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 22;
            item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = 1;
			item.defense = 3;
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
            return head.type == mod.ItemType("NatureWreath") && legs.type == mod.ItemType("NatureLeggings");
        }
		public override void UpdateEquip(Player player)
		{
			for(int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if(proj.owner == player.whoAmI && proj.minion == true && proj.minionSlots > 0.01f && proj.active)
				{
					player.statDefense++;
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Wormwood", 22);
			recipe.AddIngredient(null, "FragmentOfNature", 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
}