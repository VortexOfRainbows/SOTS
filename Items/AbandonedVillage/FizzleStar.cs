using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Items.Fragments;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.AbandonedVillage
{
	public class FizzleStar : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/FizzleStarGlow").Value;
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 15; 
            Item.DamageType = DamageClass.Magic; 
            Item.width = 36;   
            Item.height = 34;   
            Item.useTime = 17;   
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
            Item.shoot = ModContent.ProjectileType<Projectiles.AbandonedVillage.FizzleStar>(); 
            Item.shootSpeed = 10f;
			Item.mana = 6;
            Item.autoReuse = true;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/AbandonedVillage/FizzleStarGlow").Value;
                Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -4;
            }
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, 0);
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position += velocity.SNormalize() * 12;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(!Main.rand.NextBool(6))
            {
                int count = 1;
                while (Main.rand.NextBool(5))
                    count++;
                for(int i = 0; i < count; i++)
                {
                    Projectile.NewProjectile(source, position, velocity + Main.rand.NextVector2Circular(i, i), type, damage, knockback, player.whoAmI, ai2: 0);
                }
            }
            else
            {
                for(int i = -3; i <= 3; i++)
                {
                    Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(i * 4)) * Main.rand.NextFloat(0.75f, 1.25f) + Main.rand.NextVector2Circular(3, 3), type, damage * 2, knockback, player.whoAmI, ai2: Main.rand.NextFromList(-1, -2));
                }
            }
			return false; 
		}
	}
}
