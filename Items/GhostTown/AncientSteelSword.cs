using Microsoft.Xna.Framework;
using SOTS.Projectiles.Evil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class AncientSteelSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Sword");
            Tooltip.SetDefault("Does area damage on hit for 80% damage\nCritical strikes deal 50% more damage and may also apply a stacking, permanent bleed for 5 damage per second");
        }
		public override void SetDefaults()
		{
            item.damage = 16; 
            item.melee = true;  
            item.width = 60;   
            item.height = 60;
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;    
            item.knockBack = 5.4f;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.crit = 4;
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if(player.whoAmI == Main.myPlayer)
            {
                float ToPlayer = (player.Center - target.Center).ToRotation();
                Vector2 position = target.Hitbox.TopLeft() + EdgeOfRect(target.Hitbox, ToPlayer);                
                Projectile.NewProjectile(position, new Vector2(player.direction, 0), ModContent.ProjectileType<BloodSpark>(), (int)(damage * 0.8f), knockBack, Main.myPlayer, (ToPlayer + MathHelper.PiOver2) * 180f / MathHelper.Pi, crit ? -1 : 0);
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 18);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }
        public Vector2 EdgeOfRect(Rectangle rect, float rad)
        {
            float theta = MathHelper.WrapAngle(rad);
            float rectAtan = (float)Math.Atan2(rect.Width, rect.Height);
            float tanTheta = (float)Math.Tan(theta);
            Vector2 edgePoint = new Vector2(rect.Width / 2, rect.Height / 2);
            int xFactor = 1;
            int yFactor = -1;
            bool flip = false; 
            if ((theta > -rectAtan) && (theta <= rectAtan))
            {
                flip = true;
                yFactor = 1;
            }
            else if ((theta > rectAtan) && (theta <= (Math.PI - rectAtan)))
            {
                yFactor = 1;
            }
            else if ((theta > (Math.PI - rectAtan)) || (theta <= -(Math.PI - rectAtan)))
            {
                flip = true;
                xFactor = -1;
            }
            else
            {
                xFactor = -1;
            }
            if (flip) 
            {
                edgePoint.X += xFactor* (rect.Width / 2);
                edgePoint.Y += yFactor* (rect.Width / 2) * tanTheta;
            } 
            else
            {
                edgePoint.X += xFactor * (rect.Height / (2 * tanTheta));
                edgePoint.Y += yFactor * (rect.Height / 2);
            }
            return edgePoint;
        }
    }
}
