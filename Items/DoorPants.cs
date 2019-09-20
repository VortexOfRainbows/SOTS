using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
 

namespace SOTS.Items
{
	[AutoloadEquip(EquipType.Legs)]
	
	public class DoorPants : ModItem
	{	float speedDuration = 0;
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 26;

			item.value = 1250;
			item.rare = 1;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Door Pants");
			Tooltip.SetDefault("Accelerates horizontal movement when going through vanilla doors");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == 727 && body.type == 728;
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Acceleration slightly lasts longer";
			if(speedDuration > 1)
			{
				speedDuration += 0.05f;
			}
		}
		public override void UpdateEquip(Player player)
		{
			int i = (int)(player.position.X / 16);
			int j = (int)(player.position.Y / 16);
			if(speedDuration > 0)
			{
				  if(player.controlLeft) 
				  {
					  if(player.velocity.X > 0)
					  {
						  player.velocity.X = -1;
						 }
				  player.velocity.X -= 0.4f;
				  }
				  if(player.controlRight)
				  {
					  if(player.velocity.X < 0)
					  {
						  player.velocity.X = 1;
					  }
				  player.velocity.X += 0.4f;
				  }
			}
			if(Main.tile[i,j].type == 11)
			{
				speedDuration = 3;
			}
			
			if(Main.tile[i + 1,j].type == 11)
			{
				speedDuration = 3;
			}
			
			if(Main.tile[i,j + 1].type == 11)
			{
				speedDuration = 3;
			}
			
			if(Main.tile[i + 1,j + 1].type == 11)
			{
				speedDuration = 3;
			}
			if(Main.tile[i,j + 2].type == 11)
			{
				speedDuration = 3;
			}
			
			if(Main.tile[i + 1,j + 2].type == 11)
			{
				speedDuration = 3;
			}
			
			if(speedDuration > 0)
			{
				speedDuration -= .15f;
			}
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
}