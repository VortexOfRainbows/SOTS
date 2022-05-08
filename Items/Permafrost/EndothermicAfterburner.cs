using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Back)]
	public class EndothermicAfterburner : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Endothermic Afterburner");
			Tooltip.SetDefault("Melee attacks send a Frost Burst behind you, dealing 70% damage and inflicting Frostburn\nHitting an enemy with the Frost Burst will increase movement speed and melee speed by 10% for 10 seconds");
		}
        public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.EndothermicAfterburner = true;
		}
        public override void UpdateVanity(Player player, EquipType type)
        {
			if(player.velocity.Length() > 1 && !player.mount.Active)
			{
				Vector2 loc = new Vector2(player.Center.X - 16 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
				Dust dust = Dust.NewDustDirect(new Vector2(loc.X, loc.Y), 2, 2, ModContent.DustType<CopyDust4>());
				dust.noGravity = true;
				dust.velocity *= 0.02f;
				dust.scale = 1.0f;
				dust.fadeIn = 0.1f;
				dust.color = new Color(180, 210, 250, 0);
				dust.shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
			}
		}
	}
}