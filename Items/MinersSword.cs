using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class MinersSword : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/MinersSwordGlow").Value;
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Miner's Sword");
            Tooltip.SetDefault("Critically strikes while falling");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
		public override void SetDefaults()
		{
            Item.damage = 20; 
            Item.DamageType = DamageClass.Melee;  
            Item.width = 36;   
            Item.height = 36;
            Item.useTime = 16; 
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;    
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.useTurn = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/MinersSwordGlow").Value;
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
