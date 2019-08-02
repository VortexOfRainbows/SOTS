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
	[AutoloadEquip(EquipType.Legs)]
	
	public class DevilLeggings : ModItem
	{	int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = 125000;
			item.rare = 6;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Devil Leggings");
			Tooltip.SetDefault("15% increased melee speed\n10% decreased melee damage\n34% decrease to all other damage types\nGrants water walking and lava wading effects");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("DevilHelmet") && body.type == mod.ItemType("DevilChestplate");
        }
		public override void UpdateEquip(Player player)
		{
		
			player.lavaImmune = true; 
			player.meleeSpeed += 0.15f;
			player.meleeDamage -= 0.10f;
			player.magicDamage -= 0.33f;
			player.rangedDamage -= 0.33f;
			player.thrownDamage -= 0.33f;
			player.minionDamage -= 0.33f;
			player.waterWalk = true; 
			player.fireWalk = true; 
		}


	}
}