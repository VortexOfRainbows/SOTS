using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MinersSword : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/MinersSwordGlow");
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Sword");
            Tooltip.SetDefault("Critically strikes while falling");
        }
		public override void SetDefaults()
		{
            item.damage = 20; 
            item.melee = true;  
            item.width = 36;   
            item.height = 36;
            item.useTime = 16; 
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/MinersSwordGlow");
            }
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (player.velocity.Y > 0)
                crit = true;
            base.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
        }
    }
}
