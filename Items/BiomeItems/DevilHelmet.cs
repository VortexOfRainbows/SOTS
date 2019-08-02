using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using System.Linq;
 

namespace SOTS.Items.BiomeItems
{
	[AutoloadEquip(EquipType.Head)]
	
	public class DevilHelmet : ModItem
	{	int Probe = -1;
		int up = 0;
		public override void SetDefaults()
		{

			item.width = 28;
			item.height = 26;

			item.value = 125000;
			item.rare = 6;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devil Helmet");
			Tooltip.SetDefault("30% increased melee speed\n15% decreased melee damage\n33% decrease to all other damage types");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("DevilChestplate") && legs.type == mod.ItemType("DevilLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Grants a movement speed boost and quick breaking\nHold down to pass through walls";
			
			  if(player.controlLeft) 
			  {
				  if(player.velocity.X > 0)
				  {
					  player.velocity.X = -1;
					  }
			  player.velocity.X -= 0.22f;
			  
			  if(player.controlDown)
			  player.position.X -= 1.2f;
		  
			  }
			  if(player.controlRight)
			  {
				  if(player.velocity.X < 0)
				  {
					  player.velocity.X = 1;
					  }
			  player.velocity.X += 0.22f;
			  
			  if(player.controlDown)
			  player.position.X += 1.2f;
		  
			  }
			  if(player.controlJump && up <= 9 && up >= 1 && player.controlDown) 
			  {
				  up = 0;
				  player.velocity.Y -= 1.2f;
				  player.position.Y -= 16;
		
			  }
			  if(up > 0)
			  {
			  up--;
			  }
			if(player.controlJump) 
			  {
				  up = 10;
			  }
			  
			
			
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
			player.armorEffectDrawShadow = true;
}

		public override void UpdateEquip(Player player)
		{
		
			player.meleeSpeed += 0.33f;
			player.meleeDamage -= 0.15f;
			player.magicDamage -= 0.33f;
			player.rangedDamage -= 0.33f;
			player.thrownDamage -= 0.33f;
			player.minionDamage -= 0.33f;
			
		}


	}
}