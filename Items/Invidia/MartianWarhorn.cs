using Microsoft.Xna.Framework;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Void;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class MartianWarhorn : ModItem
	{	
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 38;     
            Item.height = 20;   
            Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item90 with { Pitch = -0.9f, Volume = 1.3f};
        }
        public override void UseItemFrame(Player player)
        {
            Color c = Helpers.ColorHelper.EmeraldColor;
            c.A = 0;
            player.itemLocation.X -= 4 * player.direction;
            player.itemLocation.Y += 10 * player.gravDir;
            if (player.itemTime >= player.itemAnimationMax)
            {
                Vector2 offset = new Vector2(-Item.width / 2 * player.direction, Item.height / 2 * player.gravDir);
                Vector2 direction = new Vector2(-player.direction, player.gravDir);
                for(int j = 1; j < 4; j++)
                {
                    float count = j * 20;
                    for (int i = 0; i < count; i++)
                    {
                        Vector2 circular = new Vector2(1 + j, 0).RotatedBy(i / count * 2f * MathF.PI);
                        circular.X *= 0.5f;
                        circular = circular.RotatedBy(direction.ToRotation());
                        PixelDust.Spawn(player.itemLocation - offset - direction * 7f + new Vector2(player.direction * 8 + (player.direction == -1 ? -3 : 0), 0), 0, 0, circular - direction * (j * 2) + Main.rand.NextVector2Circular(.2f, .2f), c, 5).scale = Main.rand.NextFloat(1.0f, 1.25f) * (1.0f + 0.1f * j);
                    }
                }
            }
        }
        public override bool? UseItem(Player player)
        {
			player.AddBuff(ModContent.BuffType<Embattle>(), 600, false);
            return true;
        }
    }
}