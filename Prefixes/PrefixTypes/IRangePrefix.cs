using PoEBridgeMod.Items;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace PoEBridgeMod.Prefixes
{
    public interface IRangePrefix
    {
        void CustomShoot(Item item, Player player, Vector2 position, float speedX, float speedY, int damage, float knockBack); 
    }
}
