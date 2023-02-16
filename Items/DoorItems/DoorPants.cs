using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.DoorItems
{[AutoloadEquip(EquipType.Legs)]
	public class DoorPants : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemID.WoodHelmet && body.type == ItemID.WoodBreastplate;
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.DoorItems");
			DoorPlayer doorPlayer = DoorPlayer.ModPlayer(player);
			doorPlayer.doorPants++;
		}
		public override void UpdateEquip(Player player)
		{
			DoorPlayer doorPlayer = DoorPlayer.ModPlayer(player);
			doorPlayer.doorPants++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WoodenDoor, 2).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class BandOfDoor : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			DoorPlayer doorPlayer = DoorPlayer.ModPlayer(player);
			doorPlayer.doorPants++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Shackle, 1).AddIngredient<DoorPants>(20).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class DoorPlayer : ModPlayer
    {
		public static DoorPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<DoorPlayer>();
		}
		public int doorPants = 0;
		float speedDuration = 0;
		public override void ResetEffects()
		{
			Player player = this.Player;
			if(doorPants > 0)
			{
				int i = (int)(player.Center.X / 16);
				int j = (int)(player.Center.Y / 16);
				if (speedDuration > 0)
				{
					if (player.controlLeft)
					{
						if (player.velocity.X > 12)
						{
							player.velocity.X = 12f;
						}
						player.velocity.X -= 0.4f;
					}
					if (player.controlRight)
					{
						if (player.velocity.X < -12)
						{
							player.velocity.X = -12f;
						}
						player.velocity.X += 0.4f;
					}
				}
				bool inDoor = false;
				for (int i2 = -2; i2 < 3; i2++)
				{
					for (int j2 = -2; j2 < 3; j2++)
					{
						Tile tile = Main.tile[i + i2, j + j2];
						ModTile mTile = TileLoader.GetTile(tile.TileType);
						bool isDoor = false;
						if (mTile != null)
						{
							List<int> checkArray = new List<int>(mTile.AdjTiles);
							isDoor = checkArray.Contains(TileID.OpenDoor) || tile.TileType == TileID.OpenDoor;
						}
						else
							isDoor = tile.TileType == TileID.OpenDoor;
						if (isDoor && tile.HasTile)
						{
							inDoor = true;
							break;
						}
					}
				}
				if (inDoor)
				{
					if (speedDuration < 3)
						speedDuration = 3;
					else if (speedDuration < 10)
					{
						speedDuration += 0.01f * doorPants;
					}
				}
				else if (speedDuration > 0)
				{
					if(doorPants > 2)
					{
						speedDuration -= 0.075f;
					}
					else if (doorPants > 1)
						speedDuration -= 0.10f;
					else
						speedDuration -= 0.15f;
				}
				else
					speedDuration = 0;
			}
			doorPants = 0;
		}
    }
}