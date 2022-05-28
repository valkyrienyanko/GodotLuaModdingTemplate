namespace GodotModules 
{
    public class InventoryItem 
    {
        public string Name { get; set; }
        public int StackSize { get; set; }
        public InventoryItemType Type { get; set; }
        public int X, Y;

        private ControlInventory _controlInventory;
        private ControlInventoryItem _controlInventoryItem;

        public InventoryItem(ControlInventory controlInventory, int c, int r)
        {
            X = c;
            Y = r;
            _controlInventory = controlInventory;
            _controlInventoryItem = Prefabs.InventoryItem.Instance<ControlInventoryItem>();
            _controlInventoryItem.Init(controlInventory);
            _controlInventory.GridContainer.AddChild(_controlInventoryItem);
        }

        public void SetItem(string name, int stackSize = 1) 
        {
            Name = name;
            StackSize = stackSize;

            if (Items.Sprites.ContainsKey(name))
                Type = InventoryItemType.Static;
            else if (Items.AnimatedSprites.ContainsKey(name))
                Type = InventoryItemType.Animated;
            else
                throw new InvalidOperationException($"The item '{name}' does not exist as a sprite or animated sprite");
            
            _controlInventoryItem.SetItem(this);
        }

        public void RemoveItem() => _controlInventoryItem.RemoveItem();

        public override bool Equals(object obj)
        {
            var otherItem = (InventoryItem)obj;

            if (otherItem == null)
                return false;

            return X == otherItem.X && Y == otherItem.Y;
        }

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    }

    public enum InventoryItemType 
    {
        Static,
        Animated
    }
}