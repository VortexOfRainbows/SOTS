using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	[AutoloadEquip(EquipType.Head)]
	public class MeguminHat : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 22;
			item.value = 125000;
			item.rare = ItemRarityID.Red;
			item.defense = 1;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawAltHair = true;
			drawHair = false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archmage's Hat");
			Tooltip.SetDefault("hi");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MeguminShirt") && legs.type == mod.ItemType("MeguminLeggings");
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Fatal hits will now return your health to your current mana\nEach time this happens, your max mana will be cut down depending on how much damage you take\nIf you take more damage than your max mana can handle, the fatal hit will not be cancelled\nYour max mana will return to normal after death or 5 minutes";
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            modPlayer.megSet = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.manaCost += 0.33f;
			player.meleeDamage -= 0.33f;
			player.rangedDamage -= 0.33f;
			player.minionDamage -= 0.33f;
			player.thrownDamage -= 0.33f;
                
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            modPlayer.megHat = true;
				
			if(player.statMana < (player.statManaMax2 + player.statManaMax) * 0.25f)
			{
				player.AddBuff(mod.BuffType("FrozenThroughTime"), 30, false);
				
			}
		}
	}
}