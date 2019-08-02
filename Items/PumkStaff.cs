using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Items
{  
    public class PumkStaff : ModItem
    {
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pumkin Prayer");
			Tooltip.SetDefault("Summons a Phankin to fight for you\nThe Phankin takes 3 minion slots and is op as hell");
		}
        public override void SetDefaults()
        {
           
            item.damage = 45;
            item.summon = true;
            item.mana = 10;
            item.width = 26;
            item.height = 28;
            item.expert = true;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 0;
            item.rare = 11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("Phankin");
            item.shootSpeed = 7f;
			item.buffType = mod.BuffType("Phantasmal");
            item.buffTime = 3600;
        }
    }
}