using System;
using System.Collections.ObjectModel;

namespace Tulum.Models
{
    public static class Board
    {

        public readonly static ResourceQuantity[] BRIDGE_PRICE = new[] {
             new ResourceQuantity(ResourceType.Brick,1),
             new ResourceQuantity (ResourceType.Wood,1)
         };

        public readonly static ResourceQuantity[] TOWN_PRICE = new[] {
             new ResourceQuantity(ResourceType.Brick,1),
             new ResourceQuantity (ResourceType.Grain, 1),
             new ResourceQuantity (ResourceType.Wood, 1),
             new ResourceQuantity (ResourceType.Wool,1)
         };

        public readonly static ResourceQuantity[] CITY_PRICE = new[] {
             new ResourceQuantity(ResourceType.Stone,3),
             new ResourceQuantity (ResourceType.Grain, 2),
         };

    }
}
