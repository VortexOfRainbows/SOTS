using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.DoorItems
{[AutoloadEquip(EquipType.Legs)]
	public class DoorPants : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.rare = 1;
			item.defense = 1;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Door Pants");
			Tooltip.SetDefault("Accelerates horizontal movement when going through doors\nBuilds up speed while standing in doors");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ItemID.WoodHelmet && body.type == ItemID.WoodBreastplate;
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Acceleration lasts longer\nBuild up speed faster";
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenDoor, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class BandOfDoor : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 28;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
			item.defense = 1;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Band of Door");
			Tooltip.SetDefault("'Open the door to the ultimate form of travel'");
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			DoorPlayer doorPlayer = DoorPlayer.ModPlayer(player);
			doorPlayer.doorPants++;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Shackle, 1);
			recipe.AddIngredient(ModContent.ItemType<DoorPants>(), 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
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
						ModTile mTile = TileLoader.GetTile(tile.type);
						bool isDoor = false;
						if (mTile != null)
						{
							List<int> checkArray = new List<int>(mTile.adjTiles);
							isDoor = checkArray.Contains(TileID.OpenDoor) || tile.type == TileID.OpenDoor;
						}
						else
							isDoor = tile.type == TileID.OpenDoor;
						if (isDoor && tile.active())
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