using SOTS.Projectiles.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Head)]
	
	public class PatchLeatherHat : ModItem
	{	
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 4;
		}
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
			ArmorIDs.Head.Sets.DrawHatHair[equipSlotHead] = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PatchLeatherTunic>() && legs.type == ModContent.ItemType<PatchLeatherPants>();
        }
		private int[] SnakeProbes = new int[] { -1, -1, -1, -1, -1, -1 };
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.PatchLeather");
			if(Main.myPlayer == player.whoAmI)
            {
                SOTSPlayer sPlayer = SOTSPlayer.ModPlayer(player);
                int damage = SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, 15);
                for (int i = 0; i < 3; i++)
                    sPlayer.runPets(ref SnakeProbes[i], ModContent.ProjectileType<FlyingSnake>(), damage, 1f, false, i);
			}
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snakeskin>(), 24).AddRecipeGroup("SOTS:EvilMaterial", 8).AddTile(TileID.Anvils).Register();
		}
	}
}